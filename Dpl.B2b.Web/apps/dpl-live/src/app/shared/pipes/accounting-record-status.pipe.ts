import { Pipe } from '@angular/core';

import { TranslatePipeEnumBase } from './translate-pipe.base';
import { LocalizationService } from '@app/core';

@Pipe({
  name: 'accountingRecordStatus',
})
export class AccountingRecordStatusPipe extends TranslatePipeEnumBase {
  name: string = 'AccountingRecordStatus';

  constructor(private ls: LocalizationService) {
    super(ls);
  }

  transform(value: string, mode: 'text' | 'color' = 'text') {
    if (mode === 'text') {
      return super.transform(value);
    }

  }
}
