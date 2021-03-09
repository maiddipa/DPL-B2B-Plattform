import { Injectable } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';

import {
  OrderProcessesComponent,
  OrderProcessComponentQueryParams,
} from '../../order-process/order-processes.component';
import { OrderProcess } from '../../order-process/state/order-process.model';

@Injectable({
  providedIn: 'root',
})
export class OrderProcessesDialogService {
  constructor(private dialog: MatDialog) {}

  openDialog(type: OrderProcess['type'], id: number) {
    const data = this.getDialogData(type, [id]);

    this.dialog.open<
      OrderProcessesComponent,
      Partial<OrderProcessComponentQueryParams>
    >(OrderProcessesComponent, {
      width: '1200px',
      minWidth: '1000px',
      minHeight: '450px',
      data,
    });
  }

  private getDialogData(
    type: OrderProcess['type'],
    id: number[]
  ): Partial<OrderProcessComponentQueryParams> {
    switch (type) {
      case 'order':
        return {
          orderId: id,
        };
      case 'orderLoad':
        return {
          orderLoadId: id,
        };
      case 'loadCarrierReceipt':
        return {
          loadCarrierReceiptId: id,
        };
      default:
        break;
    }
  }
}
