import { Pipe } from '@angular/core';

import { LocalizationService } from '../../core/services';
import { TranslatePipeEnumBase } from './translate-pipe.base';

@Pipe({
  name: 'loadCarrierReceiptDepoPresetCategory',
})
export class LoadCarrierReceiptDepoPresetCategoryPipe extends TranslatePipeEnumBase {
  name: string = 'LoadCarrierReceiptDepoPresetCategory';

  constructor(ls: LocalizationService) {
    super(ls);
  }
}
