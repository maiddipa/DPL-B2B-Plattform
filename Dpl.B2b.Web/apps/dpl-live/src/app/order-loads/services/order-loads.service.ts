import { Injectable } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import {
  EmployeeNoteType,
  OrderLoad,
  OrderLoadSearchRequestSortOptions,
  OrderLoadStatus,
  OrderType,
  UserRole,
} from '@app/api/dpl';
import { DplApiService, DplApiSort } from '@app/core';
import {
  ConfirmActionDialogComponent,
  ConfirmActionDialogData,
  ConfirmActionDialogResult,
  OnBehalfOfService,
  OrderLoadsViewType,
} from '@app/shared';
import { filterNil, PaginationResponse } from '@datorama/akita';
import { combineLatest, iif, Observable, of } from 'rxjs';
import { first, map, pluck, switchMap, tap } from 'rxjs/operators';

import { CustomerDivisionsService } from '../../customers/services/customer-divisions.service';
import {
  FilterValueDateRange,
  FilterValueNumberRange,
} from '../../filters/services/filter.service.types';
import { OrderProcessesStore } from '../../order-process/state/order-processes.store';
import { UserService } from '../../user/services/user.service';
import { OrderLoadsStore } from '../state/state/order-loads.store';

type OrderLoadsSearchRequest = Parameters<
  DplApiService['orderLoads']['search']
>[0];
export type GetOrderLoadsRequest = {
  divisionIds?: number[] | undefined;
  type: OrderLoadsViewType;
  filter: Partial<OrderLoadFilter>;
  page: number;
  limit: number;
  sort: DplApiSort<OrderLoadSearchRequestSortOptions>;
};

export type OrderLoadFilter = {
  Status: OrderLoadStatus[];
  AppointedDay: FilterValueDateRange;
  FulfilmentDate: FilterValueDateRange;
  DplNote: boolean;
  LoadCarriers: number[];
  BaseLoadCarriers: number[];
  Quantity: FilterValueNumberRange;
};

@Injectable({
  providedIn: 'root',
})
export class OrderLoadsService {
  constructor(
    private dpl: DplApiService,
    private store: OrderLoadsStore,
    private orderProcessStore: OrderProcessesStore,
    private onBehalfOfService: OnBehalfOfService,
    private divisionsService: CustomerDivisionsService,
    private userService: UserService,
    private dialog: MatDialog
  ) {}

  getOrderLoads(request: GetOrderLoadsRequest) {
    const { divisionIds, filter, page, limit, sort } = request;

    const divisionIds$ = iif(
      () => !!divisionIds,
      of(divisionIds),
      this.divisionsService.getActiveDivision().pipe(
        pluck('id'),
        filterNil,
        map((divisionId) => [divisionId])
      )
    ).pipe(first());

    const searchReqeust$ = combineLatest([divisionIds$]).pipe(
      map(([divisionIds]) => {
        const searchRequest: OrderLoadsSearchRequest = {
          divisionId: divisionIds,
          type: [OrderType.Supply, OrderType.Demand],
          ...sort,
          page,
          limit,
          ...this.convertFilterToSearch(filter),
          //...this.buildOrdersSearchFilter(type, filters, completedProcess)
        };

        const filterPart: OrderLoadsSearchRequest = {};

        return searchRequest;
      })
    );

    return searchReqeust$.pipe(
      switchMap((searchRequest) => {
        return this.dpl.orderLoads.search(searchRequest) as Observable<
          PaginationResponse<OrderLoad>
        >;
      })
    );
  }

  private convertFilterToSearch(
    filter: Partial<OrderLoadFilter>
  ): Partial<OrderLoadsSearchRequest> {
    return {
      loadCarrierId: filter.LoadCarriers,
      baseLoadCarrierId: filter.BaseLoadCarriers,
      status: filter.Status,
      hasDplNote: filter.DplNote,
      loadCarrierQuantityFrom: filter.Quantity
        ? filter.Quantity.from
        : undefined,
      plannedFulfilmentDateFrom: filter.AppointedDay
        ? filter.AppointedDay.from
        : undefined,
      plannedFulfilmentDateTo: filter.AppointedDay
        ? filter.AppointedDay.to
        : undefined,
      actualFulfillmentDateFrom: filter.FulfilmentDate
        ? filter.FulfilmentDate.from
        : undefined,
      actualFulfillmentDateTo: filter.FulfilmentDate
        ? filter.FulfilmentDate.to
        : undefined,
    };
  }

  cancel(id: number): Observable<OrderLoad> {
    return this.userService.getCurrentUser().pipe(
      map((user) => {
        return user.role === UserRole.DplEmployee;
      }),
      first(),
      switchMap((dplEmployee) => {
        if (dplEmployee) {
          return this.onBehalfOfService
            .openOnBehalfofDialog(EmployeeNoteType.Cancellation)
            .pipe(
              switchMap((dplNote) => {
                if (dplNote) {
                  return this.dpl.orderLoads
                    .cancel(id, {
                      dplNote,
                    })
                    .pipe(
                      tap((data) => {
                        // only update when in store, user might have paged away already
                        this.store.update((entity) => entity.id === id, data);
                        this.orderProcessStore.updateOrderProcess(
                          'orderLoad',
                          data
                        );
                      })
                    );
                }
                // abort - do nothing
                return of(null as OrderLoad);
              })
            );
        }

        return this.dialog
          .open<
            ConfirmActionDialogComponent,
            ConfirmActionDialogData,
            ConfirmActionDialogResult
          >(ConfirmActionDialogComponent, {
            data: {
              title: 'Stornieren',
              context: 'cancel'              
            },
          })
          .afterClosed()
          .pipe(
            switchMap((result) => {
              if (!result || !result.confirmed) {
                return of(null as OrderLoad);
              }
              return this.dpl.orderLoads.cancel(id, { note: result.note }).pipe(
                tap((data) => {
                  // only update when in store, user might have paged away already
                  this.store.update((entity) => entity.id === id, data);
                })
              );
            })
          );
      })
    );
  }
}
