import { Pipe, PipeTransform } from '@angular/core';

import { LocalizationService } from './../../core/services/localization.service';
import { LoadCarrierQualityType } from '@app/api/dpl';

@Pipe({
  name: 'loadCarrier',
})
export class LoadCarrierPipe implements PipeTransform {
  constructor(private localizationService: LocalizationService) {}
  transform(
    idOrValue: number | LoadCarrierQualityType,
    info: 'name' | 'type' | 'quality' | 'qualityType' | 'both' = 'both',
    fieldName: null | 'LongName' = null
  ): any {
    if (!idOrValue) {
      return undefined;
    }

    switch (info) {
      case 'name':
        return 'Not Implemented use of name in pallet pipe'; // not sure if we still need this
      case 'type':
        return this.localizationService.getTranslation(
          'LoadCarrierTypes',
          idOrValue.toString(),
          fieldName
        );
      case 'quality':
        return this.localizationService.getTranslation(
          'LoadCarrierQualities',
          idOrValue.toString(),
          fieldName
        );
      case 'qualityType':
        return this.localizationService.getTranslation(
          'LoadCarrierQualityType',
          idOrValue.toString(),
          fieldName
        );
      case 'both':
      default:
        return this.localizationService.getTranslation(
          'LoadCarriers',
          idOrValue.toString(),
          fieldName
        );
    }
  }
}
