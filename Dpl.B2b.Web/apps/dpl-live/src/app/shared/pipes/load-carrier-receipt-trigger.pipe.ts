import { Pipe } from '@angular/core';
import { LocalizationService } from '@app/core';

import { TranslatePipeEnumBase } from './translate-pipe.base';

@Pipe({
  name: 'loadCarrierReceiptTrigger',
})
export class LoadCarrierReceiptTriggerPipe extends TranslatePipeEnumBase {
  name: string = 'LoadCarrierReceiptTrigger';

  constructor(private ls: LocalizationService) {
    super(ls);
  }
}
