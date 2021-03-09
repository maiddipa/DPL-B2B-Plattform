import { Pipe } from '@angular/core';
import { LocalizationService } from '@app/core';

import { TranslatePipeEnumBase } from './translate-pipe.base';

@Pipe({
  name: 'partnerType',
})
export class PartnerTypePipe extends TranslatePipeEnumBase {
  name: string = 'PartnerType';

  constructor(private ls: LocalizationService) {
    super(ls);
  }
}
