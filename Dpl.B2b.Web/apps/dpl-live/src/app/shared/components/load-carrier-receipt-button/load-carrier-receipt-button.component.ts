import { Component, OnInit, Input } from '@angular/core';
import {
  OrderTransportType,
  OrderLoad,
  OrderLoadStatus,
  OrderType,
  Order, LoadCarrierReceiptType,
} from '@app/api/dpl';
import { LoadCarrierReceiptAction } from '../../../load-carrier-receipt/components';
import { Router } from '@angular/router';
import { DocumentsService } from '@app/shared/services';

@Component({
  selector: 'dpl-load-carrier-receipt-button',
  templateUrl: './load-carrier-receipt-button.component.html',
  styleUrls: ['./load-carrier-receipt-button.component.scss'],
})
export class LoadCarrierReceiptButtonComponent implements OnInit {
  transportType = OrderTransportType;
  orderLoadStatus = OrderLoadStatus;
  @Input() load: OrderLoad;
  @Input() order: Order;
  constructor(private router: Router, private document: DocumentsService) {}

  ngOnInit(): void {}

  onCreateReceipt(orderLoad: OrderLoad) {
    const getAction: (orderType: OrderType) => LoadCarrierReceiptType = (
      orderType: OrderType
    ) => {
      switch (orderType) {
        case OrderType.Supply:
          return LoadCarrierReceiptType.Pickup;
        case OrderType.Demand:
          return LoadCarrierReceiptType.Delivery;
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
