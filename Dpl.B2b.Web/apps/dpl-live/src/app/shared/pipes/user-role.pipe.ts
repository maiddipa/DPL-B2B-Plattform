import { Pipe, PipeTransform } from '@angular/core';
import { TranslatePipeEnumBase } from './translate-pipe.base';
import { LocalizationService } from '@app/core';

@Pipe({
  name: 'userRole',
})
export class UserRolePipe extends TranslatePipeEnumBase {
  name: string = 'UserRole';

  constructor(private ls: LocalizationService) {
    super(ls);
  }
}
