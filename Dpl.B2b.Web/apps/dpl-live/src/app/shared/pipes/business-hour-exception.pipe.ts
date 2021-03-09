import { Pipe, PipeTransform } from '@angular/core';
import { TranslatePipeEnumBase } from './translate-pipe.base';
import { LocalizationService } from 'apps/dpl-live/src/app/core/services/localization.service';


@Pipe({
  name: 'businessHourExceptionType',
})
export class BusinessHourExceptionPipe extends TranslatePipeEnumBase {
  name: string = 'BusinessHourExceptionType';

  constructor(private ls: LocalizationService) {
    super(ls);
  }
}
