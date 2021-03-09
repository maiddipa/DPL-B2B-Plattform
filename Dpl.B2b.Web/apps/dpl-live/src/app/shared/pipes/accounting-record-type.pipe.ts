import { Pipe } from '@angular/core';
import { LocalizationService } from '@app/core';

import { TranslatePipeEnumBase } from './translate-pipe.base';

// tslint:disable-next-line: use-pipe-transform-interface
@Pipe({
  name: 'accountingRecordType',
})
export class AccountingRecordTypePipe extends TranslatePipeEnumBase {
  name = 'AccountingRecordType';

  constructor(private ls: LocalizationService) {
    super(ls);
  }

  transform(value: string, format: 'name' | 'description' = null) {
    switch (format) {
      case 'name':
        return this.translate(value, this.name, 'name');
      case 'description':
        return this.translate(value, this.name, 'description');
      default:
        return this.translate(value, this.name);
    }
  }
}
