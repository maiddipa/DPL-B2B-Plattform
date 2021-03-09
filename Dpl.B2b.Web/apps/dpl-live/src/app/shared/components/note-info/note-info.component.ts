import {
  Component,
  OnInit,
  ChangeDetectionStrategy,
  Input,
} from '@angular/core';
import {
  EmployeeNote,
  EmployeeNoteReason,
  EmployeeNoteType,
} from '@app/api/dpl';
import * as _ from 'lodash';

@Component({
  selector: 'dpl-note-info',
  templateUrl: './note-info.component.html',
  styleUrls: ['./note-info.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class NoteInfoComponent implements OnInit {
  @Input() notes: EmployeeNote[];
  @Input() showHeading = true;
  @Input() mode: 'full' | 'tiny' = 'full';
  noteReason = EmployeeNoteReason;
  employeeNoteType = EmployeeNoteType;

  constructor() {}

  ngOnInit() {
    this.notes = _.orderBy(this.notes, (x) => x.createdAt, 'desc');
  }
}
