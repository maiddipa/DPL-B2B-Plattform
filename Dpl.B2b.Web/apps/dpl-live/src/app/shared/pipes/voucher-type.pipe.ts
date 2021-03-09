import { Pipe, PipeTransform } from '@angular/core';
import { TranslatePipeEnumBase } from './translate-pipe.base';
import { LocalizationService } from '@app/core';

@Pipe({
  name: 'voucherType',
})
export class VoucherTypePipe extends TranslatePipeEnumBase {
  name: string = 'VoucherType';

  constructor(private ls: LocalizationService) {
    super(ls);
  }
}
