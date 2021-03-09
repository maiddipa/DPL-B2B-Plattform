import {
  Component,
  OnInit,
  ChangeDetectionStrategy,
  ViewChild,
  ChangeDetectorRef,
  Optional,
  AfterViewInit,
} from '@angular/core';
import { MatColumnDef, MatTable } from '@angular/material/table';

@Component({
  selector: 'dpl-column-employee-note',
  templateUrl: './column-employee-note.component.html',
  styleUrls: ['./column-employee-note.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ColumnEmployeeNoteComponent implements OnInit, AfterViewInit {
  @ViewChild(MatColumnDef, { static: false }) columnDef: MatColumnDef;

  constructor(
    @Optional() public table: MatTable<any>,
    private cdRef: ChangeDetectorRef
  ) {}
  ngAfterViewInit(): void {
    console.log('ngAfterViewInit');
  }

  ngOnInit() {
    if (this.table) {
      console.log('ngOnInit before detectChanges');
      this.cdRef.detectChanges();
      console.log('ngOnInit after detectChanges');
      this.table.addColumnDef(this.columnDef);
    }
  }
}
