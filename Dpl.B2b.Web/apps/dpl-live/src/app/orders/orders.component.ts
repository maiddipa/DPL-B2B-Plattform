import { Component, OnInit } from '@angular/core';
import { Sort } from '@angular/material/sort';
import { ActivatedRoute } from '@angular/router';
import { ListSortDirection, OrderSearchRequestSortOptions } from '@app/api/dpl';
import { getMatSort, OrdersViewType } from '@app/shared';
import { RouteHighlightQueryParams } from '@app/shared/directives';
import { getTypedParams } from '@dpl/dpl-lib';
import { combineLatest, Observable, of } from 'rxjs';
import { map, publishReplay, refCount } from 'rxjs/operators';
import { FilterContext } from '../filters/services/filter.service.types';

export interface OrdersComponentQueryParams extends RouteHighlightQueryParams {
  type: OrdersViewType;
  highlightIds?: number[];
}

type ViewData = {
  viewType: OrdersViewType;
  context: FilterContext;
  matSort: Sort;
};

@Component({
  selector: 'dpl-orders',
  templateUrl: './orders.component.html',
  styleUrls: ['./orders.component.scss'],
})
export class OrdersComponent implements OnInit {
  viewType = OrdersViewType;

  viewData$: Observable<ViewData>;

  constructor(private route: ActivatedRoute) {}

  ngOnInit() {
    const viewType$ = getTypedParams<any, OrdersComponentQueryParams>(
      this.route
    ).pipe(
      map((params) => {
        return params.query.type;
      }),
      publishReplay(1),
      refCount()
    );

    const matSort$ = viewType$.pipe(
      map((viewType) => {
        switch (viewType) {
          case OrdersViewType.Journal:
            return getMatSort<OrderSearchRequestSortOptions>({
              sortBy: OrderSearchRequestSortOptions.CreatedAt,
              sortDirection: ListSortDirection.Descending,
            });
          default:
            return getMatSort<OrderSearchRequestSortOptions>({
              sortBy: OrderSearchRequestSortOptions.CreatedAt,
              sortDirection: ListSortDirection.Descending,
            });
        }
      })
    );

    const filterContext$ = viewType$.pipe(
      map((viewType) => {
        switch (viewType) {
          case OrdersViewType.Demand:
            return <FilterContext>'demandOrders';
          case OrdersViewType.Supply:
            return <FilterContext>'supplyOrders';
          default:
            throw new Error(`Unkown order loads view type: ${viewType}`);
        }
      }),
      publishReplay(1),
      refCount()
    );

    this.viewData$ = combineLatest([viewType$, filterContext$, matSort$]).pipe(
      map(([viewType, context, matSort]) => {
        const viewData: ViewData = {
          viewType,
          context,
          matSort,
        };
        return viewData;
      })
    );
  }
}
