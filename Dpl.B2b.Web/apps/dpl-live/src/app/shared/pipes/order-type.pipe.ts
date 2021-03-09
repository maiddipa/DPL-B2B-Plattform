import { Pipe } from '@angular/core';
import { OrderType } from '@app/api/dpl';

import { LocalizationService } from '../../core/services/localization.service';
import { TranslatePipeEnumBase } from './translate-pipe.base';

@Pipe({
  name: 'orderType',
})
export class OrderTypePipe extends TranslatePipeEnumBase {
  name: string = 'OrderType';

  constructor(private ls: LocalizationService) {
    super(ls);
  }

  transform(
    orderType: OrderType | any,
    format: 'default' | 'selfTransport' = 'default'
  ) {
    if (format === 'default') {
      return super.transform(orderType);
    }

    // TODO i18n
    switch (orderType) {
      case OrderType.Demand:
        return 'Selbstabholung';
      case OrderType.Supply:
        return 'Selbstanlieferung';
      default:
        throw new Error(`OrderType does not exist: ${orderType}`);
    }
  }
}
