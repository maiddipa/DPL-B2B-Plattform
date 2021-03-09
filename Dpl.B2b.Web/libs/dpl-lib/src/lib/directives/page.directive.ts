import {
  Directive,
  EventEmitter,
  Host,
  Input,
  OnChanges,
  OnDestroy,
  OnInit,
  Optional,
  Output,
  Self,
  SimpleChanges,
} from '@angular/core';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatSort, Sort } from '@angular/material/sort';
import { takeUntilDestroyed } from 'ngx-sub-form';
import { combineLatest, EMPTY, Observable, ReplaySubject } from 'rxjs';
import {
  map,
  publishReplay,
  refCount,
  startWith,
  switchMap,
  tap,
  first,
} from 'rxjs/operators';

type SimplePage = Omit<PageEvent, 'previousPageIndex' | 'length'>;

export interface PaginatedTableConfig<TData> {
  getData: (
    sort: Sort,
    page: SimplePage
  ) => Observable<PaginationResult<TData>>;
  defaultSort: Sort;
  resetTrigger$?: Observable<any>;
  defaultPage?: SimplePage;
}

const defaultConfig: Partial<PaginatedTableConfig<any>> = {
  defaultPage: {
    pageIndex: 0,
    pageSize: 10,
  },
};

export interface PaginationResult<TData> {
  perPage?: number;
  lastPage?: number;
  currentPage?: number;
  total?: number;
  data?: TData | null;
}

/**
 * Add matSort + dplPage to table with inputs and outputs shown below
 * Pass reference of paginator to [dplPagePaginator]
 * Sorted and paged data will be returned for on each sort and page event via (dplPageDataChange)
 *
 * <table
 *    matSort
 *    matSortDisableClear
 *    dplPage
 *    [dplPageConfig]="data.config"
 *    [dplPagePaginator]="paginator"
 *    (dplPageDataChange)="onData($event)"
 * ></table>
 * <mat-paginator #paginator></mat-paginator>
 */
@Directive({
  selector: '[dplPage]',
})
export class PageDirective<TData> implements OnInit, OnChanges, OnDestroy {
  @Input() dplPageConfig: PaginatedTableConfig<TData>;
  @Input() dplPagePaginator: MatPaginator;

  @Output() dplPageDataChange = new EventEmitter<TData>();

  paginatorSubject$ = new ReplaySubject<MatPaginator>();

  constructor(@Optional() @Host() @Self() private sort: MatSort) {}

  ngOnInit(): void {
    const config: PaginatedTableConfig<TData> = {
      ...defaultConfig,
      ...this.dplPageConfig,
    };

    this.sort.active = config.defaultSort.active;
    this.sort.direction = config.defaultSort.direction;

    const reset$ = (config.resetTrigger$ || EMPTY).pipe(startWith());

    const sort$ = reset$.pipe(
      switchMap(() => {
        return this.sort.sortChange
          .asObservable()
          .pipe(startWith(config.defaultSort));
      }),
      publishReplay(1),
      refCount()
    );

    const pagniator$ = this.paginatorSubject$.asObservable().pipe(
      tap((paginator) => {
        paginator.pageIndex = config.defaultPage.pageIndex;
        paginator.pageSize = config.defaultPage.pageSize;
      })
    );

    const page$ = combineLatest(pagniator$, sort$).pipe(
      switchMap(([paginator]) => {
        return paginator.page
          .asObservable()
          .pipe(startWith(config.defaultPage));
      })
    );

    const response$ = combineLatest(sort$, page$).pipe(
      switchMap(([sort, page]) => {
        return config.getData(sort, page || config.defaultPage);
      }),
      switchMap((response) => {
        return pagniator$.pipe(
          first(),
          tap((paginator) => {
            paginator.pageIndex = response.currentPage - 1;
            paginator.length = response.total;
            paginator.pageSize = response.perPage;
          }),
          switchMap(() => EMPTY),
          startWith(response)
        );
      })
    );

    const data$ = response$.pipe(
      takeUntilDestroyed(this),
      map((response) => response.data),
      tap((data) => this.dplPageDataChange.emit(data))
    );

    data$.subscribe();
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes.dplPagePaginator) {
      this.paginatorSubject$.next(this.dplPagePaginator);
    }
  }

  ngOnDestroy() {}
}
