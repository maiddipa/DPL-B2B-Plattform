import {
  Directive,
  HostBinding,
  Inject,
  Input,
  OnChanges,
  OnDestroy,
  Optional,
  SimpleChanges,
  OnInit,
} from '@angular/core';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ActivatedRoute } from '@angular/router';
import { filterNil } from '@datorama/akita';
import { getTypedParams } from '@dpl/dpl-lib';
import { takeUntilDestroyed } from 'ngx-sub-form';
import {
  iif,
  of,
  Subscription,
  Subject,
  combineLatest,
  ReplaySubject,
} from 'rxjs';
import { map, pluck, tap, switchMap } from 'rxjs/operators';

export interface RouteHighlightQueryParams {
  highlightId?: string | string[] | number | number[] | undefined;
}

@Directive({
  selector: '[dplRouteHighlight]',
})
export class RouteHighlightDirective implements OnInit, OnChanges, OnDestroy {
  @Input('dplRouteHighlight') id: number | string;
  @Input('dplRouteHighlightParamName') paramName: string;

  @HostBinding('class.highlight') private highlight = false;

  private id$ = new ReplaySubject<number | string>();
  private paramName$ = new ReplaySubject<string>();

  constructor(
    private route: ActivatedRoute,
    @Optional()
    @Inject(MAT_DIALOG_DATA)
    private dialogData: { [key: string]: number[]; highlightId: number[] }
  ) {}

  ngOnInit(): void {
    if (!this.id) {
      throw new Error('dplRouteHighlight needs to be set');
    }

    const mode = this.dialogData != null ? 'dialog' : 'route';

    const queryParams$ = getTypedParams<any, RouteHighlightQueryParams>(
      this.route
    ).pipe(pluck('query'));

    const highlightIds$ = combineLatest([
      this.paramName$,
      iif(() => mode === 'dialog', of(this.dialogData), queryParams$),
    ]).pipe(
      map(([paramName, data]) => {
        console.log(paramName, data);
        if (paramName) {
          return data[paramName];
        }

        return data.highlightId;
      }),
      filterNil,
      map((idOrArrayOfId) => {
        if (Array.isArray(idOrArrayOfId)) {
          return idOrArrayOfId as string[];
        }

        return [idOrArrayOfId as string];
      })
    );

    combineLatest([highlightIds$, this.id$])
      .pipe(
        map(([highlightIds, id]) =>
          // we want a weak equality check here as all query params are strings
          // tslint:disable-next-line: triple-equals
          highlightIds.some((highlighId) => highlighId == id)
        ),
        tap((isHighlighted) => {
          this.highlight = isHighlighted;
        }),
        takeUntilDestroyed(this)
      )
      .subscribe();
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['id']) {
      this.id$.next(this.id);
    }

    if (changes['paramName']) {
      this.paramName$.next(this.paramName);
    }
  }

  ngOnDestroy(): void {}
}
