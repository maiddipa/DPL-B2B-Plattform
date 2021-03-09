import { Pipe, PipeTransform } from '@angular/core';
import { TranslatePipeEnumBase } from './translate-pipe.base';
import { LocalizationService } from '@app/core';
import { OrderLoadStatus } from '@app/api/dpl';

@Pipe({
  name: 'orderLoadStatus',
})
export class OrderLoadStatusPipe extends TranslatePipeEnumBase {
  name: string = 'OrderLoadStatus';

  constructor(private ls: LocalizationService) {
    super(ls);
  }

  transform(value: string, mode: 'text' | 'color' = 'text') {
    if (mode === 'text') {
      return super.transform(value);
    }
    switch (value) {
      case OrderLoadStatus.Canceled:
        return 'red';
      case OrderLoadStatus.CancellationRequested:
        return 'orange';
      case OrderLoadStatus.Fulfilled:
        return 'green';
      case OrderLoadStatus.Pending:
        return 'lightblue';
      case OrderLoadStatus.TransportPlanned:
        return 'lightgreen';
      default:
        return 'black';
    }
  }
}
