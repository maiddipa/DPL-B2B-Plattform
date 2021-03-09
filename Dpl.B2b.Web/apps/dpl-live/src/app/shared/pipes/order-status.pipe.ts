import { Pipe, PipeTransform } from '@angular/core';
import { TranslatePipeEnumBase } from './translate-pipe.base';
import { LocalizationService } from '@app/core';
import { OrderStatus } from '@app/api/dpl';

@Pipe({
  name: 'orderStatus',
})
export class OrderStatusPipe extends TranslatePipeEnumBase {
  name = 'OrderStatus';

  constructor(private ls: LocalizationService) {
    super(ls);
  }

  transform(value: string, mode: 'text' | 'color' = 'text') {
    if (mode === 'text') {
      return super.transform(value);
    }
    switch (value) {
      case OrderStatus.CancellationRequested:
        return 'orange';
      case OrderStatus.Cancelled:
        return 'red';
      case OrderStatus.Confirmed:
        return 'yellow';
      case OrderStatus.Expired:
        return 'grey';
      case OrderStatus.Matched:
        return 'yellow';
      case OrderStatus.PartiallyMatched:
        return 'yellow';
      case OrderStatus.Pending:
        return 'yellow';
      default:
        return 'black';
    }
  }
}
