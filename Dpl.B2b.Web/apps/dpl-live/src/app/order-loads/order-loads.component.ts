import { Component, OnInit } from '@angular/core';
import { Sort } from '@angular/material/sort';
import { ActivatedRoute } from '@angular/router';
import {
  ListSortDirection,
  OrderLoadSearchRequestSortOptions,
  UserRole,
} from '@app/api/dpl';
import { getMatSort, OrderLoadsViewType } from '@app/shared';
import { getTypedParams } from '@dpl/dpl-lib';
import { combineLatest, Observable, of } from 'rxjs';
import {
  filter,
  map,
  publishReplay,
  refCount,
  switchMap,
} from 'rxjs/operators';

import { FilterContext } from '../filters/services/filter.service.types';
import { RouteHighlightQueryParams } from '@app/shared/directives';
import { UserService } from '../user/services/user.service';

export interface OrderLoadsComponentQueryParams
  extends RouteHighlightQueryParams {
  type: OrderLoadsViewType;
}

type ViewData = {
  viewType: OrderLoadsViewType;
  context: FilterContext;
  matSort: Sort;
  role: UserRole;
};

@Component({
  selector: 'dpl-order-loads',
  templateUrl: './order-loads.component.html',
  styleUrls: ['./order-loads.component.scss'],
})
export class OrderLoadsComponent implements OnInit {
  viewType = OrderLoadsViewType;
  userRole = UserRole;

  viewData$: Observable<ViewData>;

  constructor(
    private route: ActivatedRoute,
    private userService: UserService
  ) {}

  ngOnInit() {
    const viewType$ = getTypedParams<any, OrderLoadsComponentQueryParams>(
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
          case OrderLoadsViewType.Journal:
            return getMatSort<OrderLoadSearchRequestSortOptions>({
              sortBy:
                OrderLoadSearchRequestSortOptions.PlannedFulfillmentDateTime,
              sortDirection: ListSortDirection.Descending,
            });
          default:
            return getMatSort<OrderLoadSearchRequestSortOptions>({
              sortBy: OrderLoadSearchRequestSortOptions.CreatedAt,
              sortDirection: ListSortDirection.Descending,
            });
        }
      })
    );

    const filterContext$ = viewType$.pipe(
      map((viewType) => {
        switch (viewType) {
          case OrderLoadsViewType.Journal:
            return <FilterContext>'journal';
          case OrderLoadsViewType.LivePooling:
            return <FilterContext>'livePoolingOrders';
          default:
            throw new Error(`Unkown order loads view type: ${viewType}`);
        }
      }),
      publishReplay(1),
      refCount()
    );

    const userRole$ = this.userService.getCurrentUser().pipe(
      filter((x) => !!x),
      map((user) => user.role)
    );

    this.viewData$ = combineLatest([
      viewType$,
      filterContext$,
      matSort$,
      userRole$,
    ]).pipe(
      map(([viewType, context, matSort, userRole]) => {
        const viewData: ViewData = {
          viewType,
          context,
          matSort,
          role: userRole,
        };
        return viewData;
      })
    );
  }
}
