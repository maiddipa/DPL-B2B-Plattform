import { Pipe } from '@angular/core';
import { LocalizationService } from '@app/core';

import { TranslatePipeEnumBase } from './translate-pipe.base';

@Pipe({
  name: 'loadCarrierReceiptType',
})
export class LoadCarrierReceiptTypePipe extends TranslatePipeEnumBase {
  name: string = 'LoadCarrierReceiptType';

  constructor(private ls: LocalizationService) {
    super(ls);
  }
}
