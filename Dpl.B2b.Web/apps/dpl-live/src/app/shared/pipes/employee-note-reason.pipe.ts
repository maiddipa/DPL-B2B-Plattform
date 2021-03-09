import { Pipe, PipeTransform } from '@angular/core';
import { TranslatePipeEnumBase } from './translate-pipe.base';
import { LocalizationService } from '@app/core';

@Pipe({
  name: 'employeeNoteReason',
})
export class EmployeeNoteReasonPipe extends TranslatePipeEnumBase {
  name: string = 'EmployeeNoteReason';

  constructor(private ls: LocalizationService) {
    super(ls);
  }
}
