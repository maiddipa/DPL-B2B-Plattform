import { Pipe, PipeTransform } from '@angular/core';
import { TranslatePipeEnumBase } from './translate-pipe.base';
import { LocalizationService } from '@app/core';

@Pipe({
  name: 'voucherStatus',
})
export class VoucherStatusPipe extends TranslatePipeEnumBase {
  name: string = 'VoucherStatus';

  constructor(private ls: LocalizationService) {
    super(ls);
  }
}
