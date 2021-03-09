import {
  Directive,
  Input,
  OnChanges,
  OnDestroy,
  OnInit,
  SimpleChanges,
} from '@angular/core';
import { Query } from '@datorama/akita';
import { takeUntilDestroyed } from 'ngx-sub-form';
import { Observable, ReplaySubject } from 'rxjs';
import { delay, switchMap, tap } from 'rxjs/operators';

import { LoadingService } from '../services/loading.service';

@Directive({
  selector: '[dplLoading]',
})
export class LoadingDirective<TLoadingComponent, TModelState>
  implements OnInit, OnChanges, OnDestroy {
  @Input() dplLoading: Observable<boolean> | Query<TModelState>;

  private changes$ = new ReplaySubject<void>();

  constructor(private loadingService: LoadingService) {}

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['dplLoading']) {
      this.changes$.next();
    }
  }

  ngOnInit(): void {
    const loading$ = this.changes$.pipe(switchMap(() => this._getLoading()));

    loading$
      .pipe(
        takeUntilDestroyed(this),
        delay(0),
        tap((loading) => this.loadingService.setLoading(loading))
      )
      .subscribe();
  }

  ngOnDestroy(): void {
    this.loadingService.setLoading(false);
  }

  private _getLoading() {
    if (!this.dplLoading) {
      throw new Error('dplLoading or query must be provided');
    }

    return typeof this.dplLoading['selectLoading'] === 'function'
      ? (this.dplLoading as Query<TModelState>).selectLoading()
      : (this.dplLoading as Observable<boolean>);
  }
}
