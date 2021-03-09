import { ChangeDetectionStrategy, Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { ILabelValue } from '@dpl/dpl-lib';

@Component({
  selector: 'app-day-selector-dialog',
  template: `
    <mat-toolbar color="primary">
      <span
        class="fill-remaining-space"
        i18n="
          DaySelectorDialogToolbarLabel|DaySelectorDialog Toolbar
          Label@@DaySelectorDialogToolbarLabel"
        >Tag auswählen</span
      >
    </mat-toolbar>
    <mat-action-list class="scrollable">
      <button
        mat-list-item
        *ngFor="let option of dayOptions"
        (click)="onSelected(option.value)"
      >
        <h3 matLine>{{ option.label }}</h3>
      </button>
    </mat-action-list>
  `,
  styles: [],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class DaySelectorDialogComponent {
  constructor(
    public dialogRef: MatDialogRef<DaySelectorDialogComponent, number>,
    @Inject(MAT_DIALOG_DATA) public dayOptions: ILabelValue<number>[]
  ) {}

  onSelected(dayofWeek: number): void {
    this.dialogRef.close(dayofWeek);
  }
}
