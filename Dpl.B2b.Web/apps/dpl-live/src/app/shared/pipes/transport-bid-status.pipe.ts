import { Pipe, PipeTransform } from '@angular/core';
import { TranslatePipeEnumBase } from './translate-pipe.base';
import { LocalizationService } from '@app/core';

@Pipe({
  name: 'transportBidStatus',
})
export class TransportBidStatusPipe extends TranslatePipeEnumBase {
  name: string = 'TransportBidStatus';

  constructor(private ls: LocalizationService) {
    super(ls);
  }
}
