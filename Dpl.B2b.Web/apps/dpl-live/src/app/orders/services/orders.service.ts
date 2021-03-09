import { DatePipe } from '@angular/common';
import { ChangeDetectorRef, Injectable } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import {
  EmployeeNoteCreateRequest,
  EmployeeNoteType,
  Order,
  OrderLoad,
  OrderQuantityType,
  OrderSearchRequestSortOptions,
  OrderStatus,
  OrderTransportType,
  OrderType,
  UserRole,
} from '@app/api/dpl';
import { DplApiService, DplApiSort } from '@app/core';
import {
  ConfirmActionDialogComponent,
  ConfirmActionDialogData,
  ConfirmActionDialogResult,
} from '@app/shared/components/confirm-action-dialog/confirm-action-dialog.component';
import { OnBehalfOfService } from '@app/shared/services/on-behalf-of.service';
import { OrdersViewType } from '@app/shared/types/orders-view-type';
import { filterNil, PaginationResponse } from '@datorama/akita';
import {
  ARGUMENT_OUT_OF_RANGE,
  delayCreation,
  LoadingService,
  NOT_IMPLEMENTED_EXCEPTION,
} from '@dpl/dpl-lib';
import * as _ from 'lodash';
import {
  BehaviorSubject,
  combineLatest,
  forkJoin,
  iif,
  Observable,
  of,
} from 'rxjs';
import { first, map, pluck, switchMap, tap } from 'rxjs/operators';
import { CustomerDivisionsService } from '../../customers/services/customer-divisions.service';

import { FilterValueDateRange } from '../../filters/services/filter.service.types';
import { LoadCarriersService } from '../../master-data/load-carriers/services/load-carriers.service';
import { OrderLoadsStore } from '../../order-loads/state/state/order-loads.store';
import { OrderProcessesStore } from '../../order-process/state/order-processes.store';
import { UserService } from '../../user/services/user.service';
import { OrdersStore } from '../state/orders.store';
import {
  OrderData,
  OrderMatchType,
  OrderSearchRequest,
} from './orders.service.types';

type OrdersSearchRequest = Parameters<DplApiService['orders']['search']>[0];

export type GetOrdersRequest = {
  divisionIds?: number[] | undefined;
  type: OrdersViewType;
  filter: Partial<OrderFilter>;
  page: number;
  limit: number;
  sort: DplApiSort<OrderSearchRequestSortOptions>;
};

export type OrderFilter = {
  AppointedDay: FilterValueDateRange;
  BaseLoadCarriers: number[];
  CreatedAt: FilterValueDateRange;
  DplNote: boolean;
  LoadCarriers: number[];
  Status: OrderStatus[];
};

@Injectable({
  providedIn: 'root',
})
export class OrdersService {
  constructor(
    private dpl: DplApiService,
    private loadCarriers: LoadCarriersService,
    private datePipe: DatePipe,
    private store: OrdersStore,
    private orderProcessStore: OrderProcessesStore,
    private dialog: MatDialog,
    private userService: UserService,
    private onBehalfOfService: OnBehalfOfService,
    private divisionsService: CustomerDivisionsService,
    private orderLoadsStore: OrderLoadsStore,
    private loadingService: LoadingService
  ) {}

  forceRefreshSubject = new BehaviorSubject<boolean>(true);

  cancel(id: number, isLoad: boolean, context: 'Order' | 'OrderLoad') {
    const executeCancel = (
      dplNote: EmployeeNoteCreateRequest = undefined,
      userNote: string = undefined
    ) => {
      if (context === 'Order' && !isLoad) {
        return this.dpl.orders
          .cancel(id, {
            dplNote,
            note: userNote,
          })
          .pipe(
            this.loadingService.showLoadingWhile(),
            tap((data) => {
              this.store.update((entity) => entity.id === data.id, data);
              this.orderProcessStore.updateOrderProcess('order', data);
              this.forceRefreshSubject.next(true);
            })
          );
      } else {
        return this.dpl.orderLoads
          .cancel(id, {
            dplNote,
            note: userNote,
          })
          .pipe(
            switchMap((data) => {
              return context === 'OrderLoad'
                ? of(data)
                : this.dpl.orders
                    .getById(data.orderId)
                    .pipe(switchMap((order) => this.getOrderData(order)));
            }),
            this.loadingService.showLoadingWhile(),
            tap((data) => {
              if (context === 'OrderLoad') {
                this.orderLoadsStore.update(
                  (entity) => entity.id === data.id,
                  data as OrderLoad
                );
              } else {
                this.store.update(
                  (entity) => entity.id === data.id,
                  data as Order
                );
              }
              this.orderProcessStore.updateOrderProcess('orderLoad', data);
              this.forceRefreshSubject.next(true);
            })
          );
      }
    };

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
              switchMap((executionResult) => {
                if (!executionResult) {
                  return of(null as Order);
                }
                return executeCancel(executionResult);
              })
            );
        }
        return delayCreation(() =>
          this.dialog
            .open<
              ConfirmActionDialogComponent,
              ConfirmActionDialogData,
              ConfirmActionDialogResult
            >(ConfirmActionDialogComponent, {
              data: {
                title: 'Stornieren',
                context: 'cancel'
              },
              disableClose:true,
              autoFocus: false,
            })
            .afterClosed()
            .pipe(
              switchMap((result) => {
                if (!result || !result.confirmed) {
                  return of(null as Order);
                }
                return executeCancel(undefined, result.note);
              })
            )
        );
      })
    );
  }

  getOrders(request: GetOrdersRequest) {
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
        const searchRequest: OrderSearchRequest = {
          divisionId: divisionIds,
          type: this.getPossibleOrderTypes(request.type),
          ...sort,
          page,
          limit,
          ...this.convertFilterToSearch(filter),
        };

        return searchRequest;
      })
    );

    return searchReqeust$.pipe(
      switchMap((searchRequest) => {
        return this.dpl.orders.search(searchRequest).pipe(
          switchMap((response) => {
            if (response.data.length === 0) return of(response);
            const quantityData = response.data.map((order) =>
              this.getOrderData(order)
            );
            return forkJoin(quantityData).pipe(
              map((data) => {
                console.log('ForkJoin!');
                return {
                  ...response,
                  data,
                } as PaginationResponse<OrderData>;
              })
            );
          })
        );
      })
    );
  }

  private convertFilterToSearch(
    filter: Partial<OrderFilter>
  ): Partial<OrdersSearchRequest> {
    return {
      loadCarrierId: filter.LoadCarriers,
      baseLoadCarrierId: filter.BaseLoadCarriers,
      status: filter.Status,
      hasDplNote: filter.DplNote,
      confirmedFulfillmentDateFrom: filter.AppointedDay
        ? filter.AppointedDay.from
        : undefined,
      confirmedFulfillmentDateTo: filter.AppointedDay
        ? filter.AppointedDay.to
        : undefined,
    };
  }

  private getPossibleOrderTypes(viewType: OrdersViewType) {
    switch (viewType) {
      case OrdersViewType.Demand:
        return [OrderType.Demand];
      case OrdersViewType.Supply:
        return [OrderType.Supply];
      case OrdersViewType.LivePooling:
        return [OrderType.Supply, OrderType.Demand];
      default:
        return [OrderType.Supply, OrderType.Demand];
    }
  }

  // getOrders(
  //   divisionId: number,
  //   type: OrdersViewType,
  //   filters: Filter[],
  //   page: number,
  //   limit: number,
  //   sort: DplApiSort<OrderSearchRequestSortOptions>,
  //   completedProcess: boolean
  // ) {
  //   //add statuses to signature
  //   let orderType: OrderType[];
  //   switch (type) {
  //     case OrdersViewType.Demand:
  //       orderType = [OrderType.Demand];
  //       break;
  //     case OrdersViewType.Supply:
  //       orderType = [OrderType.Supply];
  //       break;
  //     case OrdersViewType.LivePooling:
  //       orderType = [OrderType.Supply, OrderType.Demand];
  //       break;
  //     default:
  //       orderType = [OrderType.Supply, OrderType.Demand];
  //       break;
  //   }

  //   let request: OrderSearchRequest = {
  //     divisionId: [divisionId],
  //     type: orderType,
  //     ...sort,
  //     page,
  //     limit,
  //   };

  //   request = {
  //     ...request,
  //     ...this.buildOrdersSearchFilter(type, filters, completedProcess),
  //   };

  //   const response$ = this.dpl.orders.search(request).pipe(
  //     tap((response) => {
  //       this.store.set(response.data);
  //     }),
  //     switchMap((response) =>
  //       this.query.orders$.pipe(
  //         map((data) => {
  //           return { ...response, data };
  //         })
  //       )
  //     )
  //   );

  //   return combineLatest(response$).pipe(
  //     switchMap(([response]) => {
  //       if (response.data.length === 0) {
  //         return of([] as OrderData[]);
  //       }
  //       const quantityData = response.data.map((order) =>
  //         this.getOrderData(order)
  //       );

  //       return forkJoin(quantityData).pipe(
  //         map((data) => {
  //           const result: PaginationResultOfOrderData = {
  //             ...response,
  //             data,
  //           };

  //           return result;
  //         })
  //       );
  //     }),
  //     map((result) => {
  //       return result as PaginationResultOfOrderData;
  //     })
  //   );
  // }

  // getOrdersSummary(
  //   type: OrdersViewType,
  //   filters: Filter[],
  //   completedProcess: boolean
  // ) {
  //   let orderType: OrderType[];
  //   switch (type) {
  //     case OrdersViewType.Demand:
  //       orderType = [OrderType.Demand];
  //       break;
  //     case OrdersViewType.Supply:
  //       orderType = [OrderType.Supply];
  //       break;
  //     case OrdersViewType.LivePooling:
  //       orderType = [OrderType.Supply, OrderType.Demand];
  //       break;
  //     default:
  //       orderType = [OrderType.Supply, OrderType.Demand];
  //       break;
  //   }
  //   const request: OrderSearchRequest = {
  //     type: orderType,
  //     ...this.buildOrdersSearchFilter(type, filters, completedProcess),
  //   };

  //   // TODO decide if we need order summary at all
  //   return of({});
  //   // const response$ = this.dpl.ordersSummary.get(request);
  //   // return response$;
  // }

  public getOrderData(order: Order) {
    return this.generateOrderDetails(order).pipe(
      map((orderInfo) => {
        const orderData: OrderData = {
          ...order,
          ...{
            //NOTE: Falls singuläre Spalten Darstellung verwendet werden soll. Kann eventuell wieder entfernt werden
            // fulfillmentDateTime: convertFulfillmentDateTime(
            //   this.datePipe,
            //   order as OrderData
            // ),
            ...orderInfo,
          },
        };

        return orderData;
      })
    );
  }

  private generateOrderDetails(
    order: Order
  ): Observable<
    Pick<
      OrderData,
      | 'address'
      | 'loadCarrierQuantity'
      | 'baseLoadCarrierQuantity'
      | 'quantityType'
      | 'numberOfStacks'
      | 'stackHeightMax'
      | 'fulfillmentDateTime'
      | 'matchType'
      | 'status'
      | 'isLoad'
    >
  > {
    return combineLatest([
      this.loadCarriers.getQuantityPerEur(order.loadCarrierId),
      this.loadCarriers.getQuantityPerEur(order.baseLoadCarrierId),
    ]).pipe(
      first(),
      map(([loadCarrierQuantityPerEur, baseLoadCarrierQuantityPerEur]) => {
        if (order.baseLoadCarrierId && !baseLoadCarrierQuantityPerEur) {
          throw new Error(
            `Quantity per EUR is required when BaseLoadCarrierId is specified: ${order.baseLoadCarrierId}`
          );
        }

        switch (order.quantityType) {
          case OrderQuantityType.LoadCarrierQuantity:
            return {
              address: order.address,
              loadCarrierQuantity: order.loadCarrierQuantity,
              baseLoadCarrierQuantity: null,
              quantityType: OrderQuantityType.LoadCarrierQuantity,
              numberOfStacks: null,
              stackHeightMax: order.stackHeightMax,
              fulfillmentDateTime: undefined,
              status: this.remapOrderStatus(order.status),
            };
          case OrderQuantityType.Load:
            return {
              address: order.address,
              loadCarrierQuantity:
                order.numberOfStacks *
                order.stackHeightMax *
                loadCarrierQuantityPerEur,
              baseLoadCarrierQuantity:
                order.numberOfStacks * baseLoadCarrierQuantityPerEur,
              quantityType: order.quantityType,
              numberOfStacks: order.numberOfStacks,
              stackHeightMax: order.stackHeightMax,
              fulfillmentDateTime: undefined,
              status: this.remapOrderStatus(order.status),
            };
          case OrderQuantityType.Stacks: {
            throw NOT_IMPLEMENTED_EXCEPTION;
          }
          default:
            throw ARGUMENT_OUT_OF_RANGE(
              `Unknown quantityType: ${order.quantityType}`
            );
        }
      }),
      map((qtyInfo) => {
        const getMatchType: () => OrderMatchType = () => {
          if (order.loads.length === 0) {
            return 'none';
          }

          if (!order.supportsPartialMatching) {
            return 'full';
          }

          if (order.loads.length > 1) {
            return 'partial';
          }

          return order.loadCarrierQuantity ===
            order.loads[0].loadCarrierQuantity
            ? 'full'
            : 'partial';
        };

        const matchType = getMatchType();

        const orderLoad =
          matchType === 'full' && order.loads.length === 1
            ? order.loads[0]
            : null;

        // only if a single load matched the entire order overwrite quantity info
        if (orderLoad) {
          return {
            address: orderLoad.address,
            loadCarrierQuantity: orderLoad.loadCarrierQuantity,
            baseLoadCarrierQuantity: orderLoad.baseLoadCarrierQuantity,
            quantityType: OrderQuantityType.LoadCarrierQuantity,
            numberOfStacks: orderLoad.numberOfStacks,
            stackHeightMax: orderLoad.loadCarrierStackHeight,
            fulfillmentDateTime: orderLoad.plannedFulfillmentDateTime
              ? this.datePipe.transform(
                  orderLoad.plannedFulfillmentDateTime,
                  'shortDate'
                )
              : undefined,
            status: orderLoad.status,
            isLoad: true,
            matchType,
          };
        }

        return { ...qtyInfo, matchType, isLoad: false };
      })
    );
  }

  remapOrderStatus(status: OrderStatus) {
    switch (status) {
      case OrderStatus.Confirmed:
        return OrderStatus.Pending;
      default:
        return status;
    }
  }
}
