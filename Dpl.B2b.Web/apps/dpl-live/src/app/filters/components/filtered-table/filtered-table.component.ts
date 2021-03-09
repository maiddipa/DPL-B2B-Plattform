import {
  AfterContentInit,
  AfterViewInit,
  Component,
  ContentChild,
  EventEmitter,
  Input,
  OnChanges,
  OnDestroy,
  OnInit,
  Output,
  SimpleChanges,
  ViewChild,
} from '@angular/core';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatSort, Sort } from '@angular/material/sort';
import { MatTable } from '@angular/material/table';
import { ListSortDirection } from '@app/api/dpl';
import {
  EntityState,
  getEntityType,
  PaginationResponse,
  PaginatorPlugin,
  QueryEntity,
} from '@datorama/akita';
import { takeUntilDestroyed } from 'ngx-sub-form';
import {
  BehaviorSubject,
  combineLatest,
  Observable,
  of,
  ReplaySubject,
} from 'rxjs';
import {
  debounceTime,
  distinctUntilChanged,
  first,
  map,
  pluck,
  publishReplay,
  refCount,
  startWith,
  switchMap,
  tap,
} from 'rxjs/operators';

import { DplApiSort } from '../../../core/utils';
import { FilterService } from '../../services/filter.service';
import { FilterContext } from '../../services/filter.service.types';

export interface FilteredTableRequestData<TExtendedFilter, TSortOptions> {
  filter: TExtendedFilter;
  sort: DplApiSort<TSortOptions>;
  page: number;
  limit: number;
}

export interface FilteredTableStatus {
  initialized: boolean;
  hasData: boolean;
  hasFilter: boolean;
}

export type FilterTableExtendFilterFn<
  TFilter,
  TExtendedFilter extends Partial<TFilter>
> = (data: Partial<TFilter>) => Observable<TExtendedFilter>;

export type FilterTableGetDataFn<TModel, TFilter, TSortOptions> = (
  data: FilteredTableRequestData<TFilter, TSortOptions>
) => Observable<PaginationResponse<TModel>>;

@Component({
  selector: 'dpl-filtered-table',
  templateUrl: './filtered-table.component.html',
  styleUrls: ['./filtered-table.component.scss'],
})
export class FilteredTableComponent<
  TEntityState extends EntityState,
  TSortOptions,
  TFilter,
  TExtendedFilter extends TFilter
> implements OnChanges, OnInit, AfterContentInit, AfterViewInit, OnDestroy {
  @Input() filterContext: FilterContext;
  @Input() query: QueryEntity<getEntityType<TEntityState>>;

  @Input('perPage') perPageInit = 10;
  @Input() getDataFn: FilterTableGetDataFn<
    getEntityType<TEntityState>,
    TExtendedFilter,
    TSortOptions
  >;
  @Input() extendFilterFn: FilterTableExtendFilterFn<TFilter, TExtendedFilter>;

  @Input() forceRefresh = new BehaviorSubject<boolean>(true);

  @Input() showFilterArea = true;

  @Output() statusUpdate = new EventEmitter<FilteredTableStatus>();

  @ViewChild(MatPaginator, { static: true }) public paginator: MatPaginator;

  @ContentChild(MatTable, { static: true })
  public matTableContentChild: MatTable<getEntityType<TEntityState>>;
  @ContentChild(MatSort, { static: true }) public matSortContentChild: MatSort;

  changes$ = new ReplaySubject<SimpleChanges>();
  view$ = new ReplaySubject<{ paginator: MatPaginator }>();
  content$ = new ReplaySubject<{
    table: MatTable<getEntityType<TEntityState>>;
    sort: MatSort;
  }>();

  public akitaPaginator: PaginatorPlugin<TEntityState>;

  public get status() {
    return this._status;
  }
  private _status: FilteredTableStatus = {
    initialized: false,
    hasData: undefined,
    hasFilter: undefined,
  };

  constructor(private filterService: FilterService) {}

  ngAfterViewInit(): void {
    this.view$.next({
      paginator: this.paginator,
    });
  }

  ngAfterContentInit(): void {
    this.content$.next({
      table: this.matTableContentChild,
      sort: this.matSortContentChild,
    });
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (
      changes['perPage'] ||
      changes['perPageInit'] ||
      changes['query'] ||
      changes['getDataFn'] ||
      changes['etRequestDataFn']
    ) {
      this.changes$.next(changes);
    }
  }

  ngOnInit(): void {
    this.akitaPaginator = new PaginatorPlugin<TEntityState>(
      this.query
    ).withRange();

    // ensure cache when redirecting from other routes
    this.akitaPaginator.clearCache();

    combineLatest([this.view$, this.content$])
      .pipe(
        takeUntilDestroyed(this),
        first(),
        switchMap(([view, content]) => {
          return this.changes$.pipe(map(() => ({ view, content })));
        }),
        switchMap(({ view, content }) => {
          const filter$ = this.filterService
            .getActiveFilter(this.filterContext)
            .pipe(
              switchMap((filter) => {
                if (!this.extendFilterFn) {
                  return of(filter as TExtendedFilter);
                }

                return this.extendFilterFn(filter);
              }),
              publishReplay(1),
              refCount()
            );

          const sort$ = content.sort.sortChange.asObservable().pipe(
            startWith(content.sort as Sort),
            publishReplay(1),
            refCount(),
            map((sort) => {
              return {
                sortBy: (sort.active as unknown) as TSortOptions,
                sortDirection:
                  sort.direction === 'asc'
                    ? ListSortDirection.Ascending
                    : ListSortDirection.Descending,
              } as DplApiSort<TSortOptions>;
            })
          );

          const pageEvent$ = view.paginator.page.asObservable().pipe(
            startWith({
              pageIndex: 0,
              pageSize: this.perPageInit,
            } as PageEvent),
            publishReplay(1),
            refCount()
          );

          const perPage$ = pageEvent$.pipe(
            pluck('pageSize'),
            distinctUntilChanged()
          );

          const searchInfo$ = combineLatest([filter$, sort$, perPage$]).pipe(
            publishReplay(1),
            refCount()
          );

          const page$ = searchInfo$.pipe(
            tap((_) => {
              this.akitaPaginator.clearCache();
              this.akitaPaginator.setPage(1);
            }),
            switchMap(() => {
              return this.akitaPaginator.pageChanges;
            })
          );

          const result$ = combineLatest([
            page$,
            searchInfo$,
            this.forceRefresh,
          ]).pipe(
            debounceTime(5),
            switchMap(([page, [filter, sort, limit]]) => {
              const req = () => {
                return this.getDataFn({
                  filter,
                  page,
                  sort,
                  limit,
                }).pipe(tap(() => this.akitaPaginator.setLoading()));
              };
              return this.akitaPaginator.getPage(req); // take only 1 from all$
              // return this.query
              //   .selectAll()
              //   .pipe(switchMap(() => this.akitaPaginator.getPage(req))); //Seiteneffekte beim Wechseln von Bedarf auf Verfügbarkeit
            }),
            tap((result) => {
              console.log('RESULT', result);
              content.table.dataSource = result.data;
              view.paginator.pageIndex = result.currentPage - 1;
              view.paginator.pageSize = result.perPage;
              // HACK REMOVE after tetsing complete, added 100 to result total to allow for paging test
              view.paginator.length = result.total;
            })
          );

          const updateStatus$ = result$.pipe(
            switchMap((result) => {
              return filter$.pipe(
                first(),
                map((filter) => {
                  return filter && Object.keys(filter).keys.length > 0;
                }),
                tap((hasFilter) => {
                  this._updateStatus({
                    initialized: true,
                    hasData: result.data.length > 0,
                    hasFilter,
                  });
                })
              );
            })
          );

          return updateStatus$;
        })
      )
      .subscribe();
  }

  onPage(page: PageEvent) {
    this.akitaPaginator.setPage(page.pageIndex + 1);
  }

  ngOnDestroy() {
    this.akitaPaginator.destroy();
  }

  private _updateStatus(status: FilteredTableStatus) {
    this._status = status;
    this.statusUpdate.emit(this._status);
  }
}
