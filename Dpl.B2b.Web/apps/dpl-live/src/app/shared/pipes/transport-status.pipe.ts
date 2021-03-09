import { Pipe, PipeTransform } from '@angular/core';
import { TranslatePipeEnumBase } from './translate-pipe.base';
import { LocalizationService } from '@app/core';

@Pipe({
  name: 'transportStatus',
})
export class TransportStatusPipe extends TranslatePipeEnumBase {
  name: string = 'TransportOfferingStatus';

  constructor(private ls: LocalizationService) {
    super(ls);
  }
}
