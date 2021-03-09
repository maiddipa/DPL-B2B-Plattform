import {
  animate,
  state,
  style,
  transition,
  trigger,
} from '@angular/animations';
import {
  ChangeDetectionStrategy,
  ChangeDetectorRef,
  Component,
  Input,
  OnChanges,
  OnInit,
  SimpleChanges,
} from '@angular/core';
import { Sort } from '@angular/material/sort';
import { Router } from '@angular/router';
import {
  LoadCarrierReceipt,
  OrderLoad,
  OrderLoadStatus,
  OrderQuantityType,
  OrderSearchRequestSortOptions,
  OrderStatus,
  OrderTransportType,
  OrderType,
} from '@app/api/dpl';
import { DocumentsService, OrdersViewType } from '@app/shared';
import { combineLatest, EMPTY, Observable } from 'rxjs';
import { map, startWith, switchMap, tap } from 'rxjs/operators';
import {
  FilteredTableRequestData,
  FilterTableGetDataFn,
} from '../../filters/components/filtered-table/filtered-table.component';
import { FilterContext } from '../../filters/services/filter.service.types';

import { LoadCarrierReceiptAction } from '../../load-carrier-receipt/components';
import { LoadCarriersService } from '../../master-data/load-carriers/services/load-carriers.service';
import { ILoadCarrier } from '../../master-data/load-carriers/state/load-carrier.model';
import { OrderProcessesDialogService } from '../../order-process/services/order-processes-dialog.service';
import { UserService } from '../../user/services/user.service';
import { OrderData, OrderFilter, OrdersService } from '../services';
import { OrdersQuery } from '../state/orders.query';

type ViewData = {
  isDplEmployee: boolean;
};

@Component({
  selector: 'dpl-orders-list',
  templateUrl: './orders-list.component.html',
  styleUrls: ['./orders-list.component.scss'],
  animations: [
    trigger('detailExpand', [
      state('collapsed', style({ height: '0px', minHeight: '0' })),
      state('expanded', style({ height: '*' })),
      transition(
        'expanded <=> collapsed',
        animate('225ms cubic-bezier(0.4, 0.0, 0.2, 1)')
      ),
    ]),
  ],
})
export class OrdersListComponent implements OnChanges, OnInit {
  @Input() viewType: OrdersViewType;
  @Input() context: FilterContext;
  @Input() sort: Sort;

  orderType: OrderType;
  sortOption = OrderSearchRequestSortOptions;
  orderStatus = OrderStatus;
  orderLoadStatus = OrderLoadStatus;
  transportType = OrderTransportType;
  quantityType = OrderQuantityType;

  displayedColumns = ['number'];

  expandedOrder: OrderData | null;

  loadCarrierReceipt: LoadCarrierReceipt | null;

  getDataFn: FilterTableGetDataFn<
    OrderData,
    OrderFilter,
    OrderSearchRequestSortOptions
  >;

  viewData$: Observable<ViewData>;

  constructor(
    private document: DocumentsService,
    private router: Router,
    private loadCarriersService: LoadCarriersService,
    public orderService: OrdersService,
    private userService: UserService,
    public orderProcesses: OrderProcessesDialogService,
    public query: OrdersQuery,
    private cdRef: ChangeDetectorRef
  ) {
    this.getDataFn = this.getData.bind(this);
  }

  ngOnInit(): void {
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

  ngOnChanges(changes: SimpleChanges): void {
    if (changes.viewType) {
      switch (this.viewType) {
        case OrdersViewType.Journal: {
          this.displayedColumns = [
            'number',
            'dplCode',
            'timestamp',
            'earliestFulfillmentDateTime',
            'latestFulfillmentDateTime',
            //'fulfillmentDateTime',
            'abholort',
            'typ',
            'amount',
            'stapel',
            'stapelhoehe',
            'traeger',
            'traegerQuantity',
            'terminDPL',
            'status',
            'orderTypeJournal',
            'cancelOrder',
            'receipt',
            'chat',
            'hasDplNote',
            // 'orderProcess',
          ];
          break;
        }
        case OrdersViewType.LivePooling: {
          this.displayedColumns = [
            'number',
            'dplCode',
            'timestamp',
            // 'earliestFulfillmentDateTime',
            // 'latestFulfillmentDateTime',
            // 'fulfillmentDateTime',
            'abholort',
            'typ',
            'amount',
            'stapel',
            'stapelhoehe',
            'traeger',
            'traegerQuantity',
            'terminDPL',
            'status',
            'orderTypePooling',
            'cancelOrder',
            'chat',
            'hasDplNote',
            // 'orderProcess',
          ];
          break;
        }
        default: {
          this.displayedColumns = [
            'number',
            'dplCode',
            'timestamp',
            'earliestFulfillmentDateTime',
            'latestFulfillmentDateTime',
            // 'fulfillmentDateTime',
            'abholort',
            'typ',
            'amount',
            'stapel',
            'stapelhoehe',
            'traeger',
            'traegerQuantity',
            'terminDPL',
            'status',
            'cancelOrder',
            'receipt',
            'chat',
            'hasDplNote',
            // 'orderProcess',
          ];
          break;
        }
      }
    }
  }

  private getData(
    data: FilteredTableRequestData<OrderFilter, OrderSearchRequestSortOptions>
  ) {
    return this.orderService.getOrders({
      type: this.viewType,
      ...data,
    });
  }

  getStatusColor(status: OrderStatus | OrderLoadStatus, isLoad: boolean) {
    if (!isLoad) {
      switch (status) {
        case OrderStatus.Pending:
          return 'lightblue';
        // confirmed means data has been replicated
        // match means somebody inside dpl has matched these orders
        // but transport hasnt been planned yet
        // we should not expose the difference in status to customers as matches can be reverted
        case OrderStatus.Confirmed:
        case OrderStatus.Matched:
          return 'yellow';
        // HACK OrderLoad handle aggregated order status
        // case OrderStatus.TransportPlanned:
        //   return 'green';
        // case OrderStatus.Fulfilled:
        //   return 'green';
        case OrderStatus.Expired:
          return 'grey';
        case OrderStatus.CancellationRequested:
          return 'orange';
        case OrderStatus.Cancelled:
          return 'red';
      }
    } else {
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
  }

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

  // onCancel(order: OrderData) {
  //   return this.orderService.cancel(order).pipe(delay(200));
  // }

  selectedRowClick(row: OrderData): void {
    // HACK OrderLoad uncommented detail handling - loadCarrierReceipt not exist on Order!
    // this.expandedOrder = this.expandedOrder === row ? null : row;
    // this.loadCarrierReceipt = row ? row.loadCarrierReceipt : null;

    // DetailRow for DplNote
    if (row.hasDplNote) {
      this.expandedOrder = this.expandedOrder === row ? null : row;
    }
  }

  detailExpanded(order: OrderData): string {
    // return 'collapsed';
    // HACK OrderLoad uncommented detail handling - loadCarrierReceipt not exist on Order!
    // const loadCarrierReceipt =
    //   this.expandedOrder && this.expandedOrder.loadCarrierReceipt;

    // return order === this.expandedOrder &&
    //   _.isObject(loadCarrierReceipt) &&
    //   loadCarrierReceipt.type !== undefined
    //   ? 'expanded'
    //   : 'collapsed';

    // DetailRow for DplNote
    return order === this.expandedOrder ? 'expanded' : 'collapsed';
  }

  getLoadCarrierById(id): Observable<ILoadCarrier<number, number>> {
    return this.loadCarriersService.getLoadCarrierById(id);
  }
}
