import { Pipe } from '@angular/core';

import {
  TranslatePipeEnumBase,
  TranslatePipeBase,
} from './translate-pipe.base';
import { LocalizationService } from '@app/core';

@Pipe({
  name: 'voucherReasonType',
})
export class VoucherReasonType extends TranslatePipeBase {
  name: string = 'VoucherReasonTypes';

  constructor(private ls: LocalizationService) {
    super(ls);
  }

  transform(value: number, format: 'short' | 'long' = 'long') {
    switch (format) {
      case 'short':
        return this.translate(value.toString(), this.name, 'ShortName');
      case 'long':
      default:
        return this.translate(value.toString(), this.name);
    }
  }
}
