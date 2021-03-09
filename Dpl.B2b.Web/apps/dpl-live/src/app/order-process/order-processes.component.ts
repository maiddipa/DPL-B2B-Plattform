import { Component, Inject, OnDestroy, OnInit, Optional } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';
import { Order } from '@app/api/dpl';
import { ParseParamConfig, parseParams } from '@dpl/dpl-lib';
import * as _ from 'lodash';
import { BehaviorSubject, combineLatest, iif, Observable, of } from 'rxjs';
import {
  distinctUntilChanged,
  map,
  pluck,
  publishReplay,
  refCount,
  switchMap,
  tap,
} from 'rxjs/operators';

import { OrderProcessesService } from './services/order-processes.service';
import { OrderGroupsSearchRequest } from './services/order-processes.service.types';
import { OrderProcess } from './state/order-process.model';
import { OrderProcessesQuery } from './state/order-processes.query';

export interface OrderProcessComponentQueryParams
  extends OrderGroupsSearchRequest {
  selectedOrderId: number;
}

const queryParamsConfig: ParseParamConfig<
  any,
  OrderProcessComponentQueryParams
> = {
  query: {
    id: {
      type: 'int',
      isArray: true,
    },
    orderId: {
      type: 'int',
      isArray: true,
    },
    orderLoadId: {
      type: 'int',
      isArray: true,
    },
    loadCarrierReceiptId: {
      type: 'int',
      isArray: true,
    },
    selectedOrderId: {
      type: 'int',
    },
  },
};

type ViewData = {
  mode: 'page' | 'dialog';
  showAsTable: boolean;
  orders: OrderProcess[];
  selectedOrder: Order;
  orderProcesses: OrderProcess[];
};

@Component({
  selector: 'dpl-order-process',
  templateUrl: './order-processes.component.html',
  styleUrls: ['./order-processes.component.scss'],
})
export class OrderProcessesComponent implements OnInit, OnDestroy {
  viewData$: Observable<ViewData>;
  selectedOrderId$ = new BehaviorSubject<number | null>(null);
  showAsTable$ = new BehaviorSubject<boolean>(false);

  constructor(
    private orderProcesses: OrderProcessesService,
    private route: ActivatedRoute,
    private router: Router,
    public query: OrderProcessesQuery,
    @Optional() public dialogRef: MatDialogRef<OrderProcessesComponent>,
    @Optional()
    @Inject(MAT_DIALOG_DATA)
    private dialogData: OrderProcessComponentQueryParams
  ) {}

  ngOnInit() {
    const mode: ViewData['mode'] = this.dialogData ? 'dialog' : 'page';

    const dialogParams$ = of({ query: this.dialogData });

    const routeParams$ = parseParams<any, OrderProcessComponentQueryParams>(
      this.route,
      queryParamsConfig
    );

    const params$ = iif(
      () => mode === 'dialog',
      dialogParams$,
      routeParams$
    ).pipe(publishReplay(1), refCount());

    // get data from server and load store
    const loadData$ = params$.pipe(
      map((params) => {
        const request = { ...params.query };
        delete request.selectedOrderId;
        return request as OrderGroupsSearchRequest;
      }),
      distinctUntilChanged(_.isEqual),
      tap((request) => this.orderProcesses.load(request))
    );
    loadData$.subscribe();

    const orderGroups$ = this.orderProcesses
      .getActiveOrderProcesses()
      .pipe(publishReplay(1), refCount());

    const orders$ = this.orderProcesses.getActiveOrderProcesses().pipe(
      map((processes) => {
        const orders = _(processes)
          .map((i) => i.children)
          .flatten()
          .value();

        return orders;
      }),
      tap((orders) => {
        if (!this.selectedOrderId$.value) {
          this.selectedOrderId$.next(orders[0].data.id);
        }
      })
    );

    const selectedOrderId$ = params$.pipe(
      pluck('query', 'orderId'),
      distinctUntilChanged(),
      tap((orderIds) => {
        if (!orderIds || orderIds.length === 0) {
          return;
        }

        // set inital selected order when dialog was called with orderId params
        this.selectedOrderId$.next(orderIds[0]);
      }),
      switchMap(() => {
        return this.selectedOrderId$;
      }),
      distinctUntilChanged(),
      tap((selectedOrderId) => {
        if (mode === 'dialog') {
          return;
        }

        this.router.navigate([], {
          replaceUrl: true,
          queryParams: {
            selectedOrderId,
          },
          queryParamsHandling: 'merge',
        });
      })
    );

    const selectedOrder$ = combineLatest([orders$, selectedOrderId$]).pipe(
      map(([orders, selectedOrderId]) => {
        return orders.find((i) => i.data.id === selectedOrderId) || orders[0];
      }),
      publishReplay(1),
      refCount()
    );

    const orderLoads$ = selectedOrder$.pipe(pluck('children'));

    this.viewData$ = combineLatest([
      this.showAsTable$,
      orderGroups$,
      orders$,
      selectedOrder$,
      orderLoads$,
    ]).pipe(
      map(([showAsTable, orderGroups, orders, selectedOrder, orderLoads]) => {
        const viewData: ViewData = {
          mode,
          showAsTable,
          orders,
          selectedOrder: selectedOrder ? (selectedOrder.data as Order) : null,
          orderProcesses: showAsTable ? orderGroups : orderLoads,
        };
        return viewData;
      })
    );
  }

  ngOnDestroy(): void {
    this.orderProcesses.resetActive();
  }

  onOrderChanged(orderId: number) {
    this.selectedOrderId$.next(orderId);
  }

  getRowTitle(row: any) {
    const type = row.type === 'Demand' ? 'Bedarf' : 'Verfügbarkeit';
    return `${type} #${row.referenceNumber}`;
  }

  closeDialog() {
    this.orderProcesses.resetActive();
    this.dialogRef.close();
  }
}
