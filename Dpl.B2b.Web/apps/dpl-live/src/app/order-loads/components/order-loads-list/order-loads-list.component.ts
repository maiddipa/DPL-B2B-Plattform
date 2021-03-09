import { Component, Input, OnInit, ViewEncapsulation } from '@angular/core';
import { Sort } from '@angular/material/sort';
import { Router } from '@angular/router';
import {
  OrderLoad,
  OrderLoadSearchRequestSortOptions,
  OrderLoadStatus,
  OrderTransportType,
  OrderType,
} from '@app/api/dpl';
import { DocumentsService, OrderLoadsViewType } from '@app/shared';
import { combineLatest, Observable } from 'rxjs';
import { delay, map } from 'rxjs/operators';

import {
  FilteredTableRequestData,
  FilterTableGetDataFn,
} from '../../../filters/components/filtered-table/filtered-table.component';
import { FilterContext } from '../../../filters/services/filter.service.types';
import { LoadCarrierReceiptAction } from '../../../load-carrier-receipt/components';
import { OrderProcessesDialogService } from '../../../order-process/services/order-processes-dialog.service';
import { OrdersService } from '../../../orders/services';
import { UserService } from '../../../user/services/user.service';
import {
  OrderLoadFilter,
  OrderLoadsService,
} from '../../services/order-loads.service';
import { OrderLoadsQuery } from '../../state/state/order-loads.query';

type ViewData = {
  isDplEmployee: boolean;
};

@Component({
  selector: 'dpl-order-loads-list',
  templateUrl: './order-loads-list.component.html',
  styleUrls: ['./order-loads-list.component.scss'],
  encapsulation: ViewEncapsulation.None,
})
export class OrderLoadsListComponent implements OnInit {
  @Input() viewType: OrderLoadsViewType = OrderLoadsViewType.Journal;
  @Input() context: FilterContext;
  @Input() sort: Sort;

  orderLoadStatus = OrderLoadStatus;
  sortOption = OrderLoadSearchRequestSortOptions;
  transportType = OrderTransportType;

  displayedColumns = ['number'];
  getDataFn: FilterTableGetDataFn<
    OrderLoad,
    OrderLoadFilter,
    OrderLoadSearchRequestSortOptions
  >;

  viewData$: Observable<ViewData>;

  constructor(
    private orderLoadsService: OrderLoadsService,
    public query: OrderLoadsQuery,
    private document: DocumentsService,
    private router: Router,
    private userService: UserService,
    public orderProcesses: OrderProcessesDialogService,
    public ordersService: OrdersService
  ) {
    this.getDataFn = this.getData.bind(this);
  }

  ngOnInit() {
    switch (this.viewType) {
      case OrderLoadsViewType.Journal:
        this.displayedColumns = [
          'number',
          'dplCode',
          'timestamp',
          'plannedFulfillmentDateTime',
          'actualFulfillmentDateTime',
          'address',
          'loadCarrier',
          'loadCarrierQuantity',
          'numberOfStacks',
          'loadCarrierStackHeight',
          'baseLoadCarrier',
          'baseLoadCarrierQuantity',
          //'terminDPL', // TODO see which of if we need 3 date fields
          'status',
          'orderTypeJournal',
          'cancel',
          'receipt',
          'chat',
          'hasDplNote',
          // 'orderProcess',
        ];
        break;
      case OrderLoadsViewType.LivePooling:
        this.displayedColumns = [
          'number',
          'dplCode',
          'timestamp',
          'address',
          'loadCarrier',
          'loadCarrierQuantity',
          'numberOfStacks',
          'loadCarrierStackHeight',
          'baseLoadCarrier',
          'baseLoadCarrierQuantity',
          'plannedFulfillmentDateTime',
          'actualFulfillmentDateTime',
          'status',
          'orderTypePooling',
          'cancel',
          'chat',
          'hasDplNote',
          // 'orderProcess',
        ];
        break;

      default:
        break;
    }

    const isDplEmployee$ = this.userService.getIsDplEmployee();

    this.viewData$ = combineLatest([isDplEmployee$]).pipe(
      map(([isDplEmployee]) => {
        const viewData: ViewData = {
          isDplEmployee,
        };
        return viewData;
      })
    );
  }

  private getData(
    data: FilteredTableRequestData<
      OrderLoadFilter,
      OrderLoadSearchRequestSortOptions
    >
  ) {
    return this.orderLoadsService.getOrderLoads({
      type: this.viewType,
      ...data,
    });
  }

  getStatusColor(status: OrderLoadStatus) {
    switch (status) {
      case OrderLoadStatus.Pending:
        return 'lightblue';
      case OrderLoadStatus.TransportPlanned:
        return 'lightgreen';
      case OrderLoadStatus.Fulfilled:
        return 'green';
      case OrderLoadStatus.CancellationRequested:
        return 'orange';
      case OrderLoadStatus.Canceled:
        return 'red';
    }
  }

  // onCancel(orderLoad: OrderLoad) {
  //   return this.orderLoadsService.cancel(orderLoad.id).pipe(delay(200));
  // }

  onCreateReceipt(orderLoad: OrderLoad) {
    const getAction: (orderType: OrderType) => LoadCarrierReceiptAction = (
      orderType: OrderType
    ) => {
      switch (orderType) {
        case OrderType.Supply:
          return 'pickup';
        case OrderType.Demand:
          return 'delivery';
        default:
          throw new Error('Unkown orderType: ${type}');
      }
    };

    this.router.navigate(['/receipt/create', orderLoad.digitalCode], {
      queryParams: {
        action: getAction(orderLoad.type),
      },
    });
  }

  openDocument(orderLoad: OrderLoad) {
    this.document
      .print(orderLoad.loadCarrierReceipt.documentId, false)
      .subscribe();
  }
}
