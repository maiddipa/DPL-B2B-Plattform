import {
  Component,
  OnInit,
  Input,
  OnChanges,
  SimpleChanges,
} from '@angular/core';
import { EmployeeNote } from '@app/api/dpl';
import { NoteInfoComponent } from '../note-info/note-info.component';

@Component({
  selector: 'dpl-detail-process',
  templateUrl: './detail-process.component.html',
  styleUrls: ['./detail-process.component.scss'],
})
export class DetailProcessComponent implements OnChanges {
  @Input() title: string;
  @Input() columns: 1 | 2 = 1;

  @Input() notesMode: NoteInfoComponent['mode'];
  @Input('notes') unParsedNotes: EmployeeNote | EmployeeNote[];

  notes: EmployeeNote[] = [];

  constructor() {}
  ngOnChanges(changes: SimpleChanges): void {
    if (!this.unParsedNotes) {
      return;
    }

    if (Array.isArray(this.unParsedNotes)) {
      this.notes = this.unParsedNotes;
    } else {
      this.notes = [this.unParsedNotes];
    }
  }
}
