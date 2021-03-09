import { Component, OnInit } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';

export type ConfirmationDialogResult = 'yes' | 'no';

@Component({
  selector: 'dpl-confirmation-dialog',
  template: `
    <div fxLayout="column" fxLayoutGap="10px">
      <mat-toolbar color="primary">
        <span>Quittung drucken?</span>
      </mat-toolbar>
      <div fxLayout="row" fxLayoutAlign="end" fxLayoutGap="10px">
        <button mat-raised-button (click)="onAnswerTapped('no')">Nein</button>
        <button
          mat-raised-button
          color="primary"
          (click)="onAnswerTapped('yes')"
        >
          Ja
        </button>
      </div>
    </div>
  `,
  styles: [],
})
export class ConfirmationDialogComponent implements OnInit {
  constructor(
    public dialogRef: MatDialogRef<
      ConfirmationDialogComponent,
      ConfirmationDialogResult
    >
  ) {}

  onAnswerTapped(result: ConfirmationDialogResult) {
    this.dialogRef.close(result);
  }

  ngOnInit() {}
}
