import {
  Component,
  OnInit,
  ChangeDetectionStrategy,
  Input,
} from '@angular/core';
import { OrderData, OrdersService } from '../../../orders/services';
import { Order, OrderLoad, OrderLoadStatus, OrderStatus, ResourceAction } from '@app/api/dpl';
import { delay } from 'rxjs/operators';

@Component({
  selector: 'dpl-cancel-button',
  templateUrl: './cancel-button.component.html',
  styleUrls: ['./cancel-button.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class CancelButtonComponent implements OnInit {
  @Input() type: 'Order' | 'OrderLoad';
  @Input() item: Order | OrderLoad | OrderData;
  @Input() isEmployee = false;
  resourceAction = ResourceAction;

  constructor(private ordersService: OrdersService) {}

  ngOnInit(): void {}

  isDisabled() {
    if (this.type === 'OrderLoad' || (this.item as OrderData).isLoad) {
      switch (this.item.status) {
        case OrderLoadStatus.Fulfilled:
        case OrderLoadStatus.TransportPlanned:
          return true;
        case OrderLoadStatus.Canceled:
        case OrderLoadStatus.CancellationRequested:
          return !this.isEmployee;
        default:
          return false;
      }
    } else {
      switch (this.item.status) {
        case OrderStatus.Cancelled:
        case OrderStatus.CancellationRequested:
          return !this.isEmployee;
        default:
          return false;
      }
    }
  }

  onCancel() {
    const cancel$ =
      this.type === 'OrderLoad'
        ? this.ordersService.cancel(this.item.id, true, this.type)
        : (this.item as OrderData).isLoad
        ? this.ordersService.cancel(
            (this.item as OrderData).loads[0].id,
            true,
            this.type
          )
        : this.ordersService.cancel(this.item.id, false, this.type);
    return cancel$.pipe(delay(200));
  }
}
