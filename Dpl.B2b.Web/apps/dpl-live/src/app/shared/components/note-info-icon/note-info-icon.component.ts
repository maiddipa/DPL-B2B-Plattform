import {
  Component,
  OnInit,
  ChangeDetectionStrategy,
  Input,
} from '@angular/core';
import { EmployeeNoteType, EmployeeNote } from '@app/api/dpl';
import * as _ from 'lodash';

@Component({
  selector: 'dpl-note-info-icon',
  templateUrl: './note-info-icon.component.html',
  styleUrls: ['./note-info-icon.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class NoteInfoIconComponent implements OnInit {
  @Input() notes: EmployeeNote[];
  employeeNoteType = EmployeeNoteType;
  noteType?: EmployeeNoteType;
  constructor() {}

  ngOnInit() {
    if (this.notes) {
      console.log(this.notes);
      // lodash sort, last element
      this.noteType = _.orderBy(this.notes, (x) => x.createdAt, 'desc')[0].type;
    }
  }
}
