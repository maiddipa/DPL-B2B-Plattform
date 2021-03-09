import { Pipe, PipeTransform } from '@angular/core';
import { TranslatePipeEnumBase } from './translate-pipe.base';
import { LocalizationService } from '@app/core';

@Pipe({
  name: 'employeeNoteType',
})
export class EmployeeNoteTypePipe extends TranslatePipeEnumBase {
  name: string = 'EmployeeNoteType';

  constructor(private ls: LocalizationService) {
    super(ls);
  }
}
