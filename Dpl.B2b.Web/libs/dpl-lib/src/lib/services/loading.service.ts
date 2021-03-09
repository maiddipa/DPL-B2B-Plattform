import { Inject, Injectable, InjectionToken, Optional } from '@angular/core';
import { BehaviorSubject, Observable, OperatorFunction, throwError, timer } from 'rxjs';
import { catchError, delay, tap } from 'rxjs/operators';

export const LOADING_MAX_DURATION = new InjectionToken<number>(
  'LOADING_MAX_DURATION'
);

@Injectable({
  providedIn: 'root',
})
export class LoadingService {
  loading$ = new BehaviorSubject<boolean>(false);

  constructor(
    @Optional() @Inject(LOADING_MAX_DURATION) private maxDuration: number
  ) {
    this.maxDuration = this.maxDuration || 0;
  }

  setLoading(loading: boolean, maxDuration?: number) {
    this._setLoading(loading);
    const duration = maxDuration || this.maxDuration;
    if (loading && duration > 0) {
      timer(duration)
        .pipe(tap(() => this.setLoading(false)))
        .subscribe();
    }
  }

  private _setLoading(loading: boolean) {
    this.loading$.next(loading);
  }

  getLoading() {
    // delay is to prevent change detection errors
    return this.loading$.pipe(delay(0));
  }

  showLoadingWhile<T>(maxDuration?: number): OperatorFunction<T, T> {
    return (source: Observable<T>) =>
      new Observable<T>((observer) => {
        // show loading when subscribe is called
        this._setLoading(true);

        let loadingWasReset = false;
        const resetLoading = () => {
          if (!loadingWasReset) {
            this._setLoading(false);
            loadingWasReset = true;
          }
        };

        // hide when unsubscribe is called
        observer.add(() => resetLoading());

        // handle max duration
        const duration = maxDuration || this.maxDuration;

        if (duration > 0) {
          const timerSub = timer(duration)
            .pipe(tap(() => resetLoading()))
            .subscribe();
          observer.add(timerSub);
        }

        source.subscribe({
          next: (data) => observer.next(data),
          error: (data) => {
            resetLoading();
            observer.error(data);
            throw data;
          },
          complete: () => {
            resetLoading();
            observer.complete();
          },
          closed: observer.closed,
        });

        return observer;
      }).pipe(
        catchError((err) => {
          return throwError(err);
        }));
  }

  private hideAfter(duration: number) {}
}
