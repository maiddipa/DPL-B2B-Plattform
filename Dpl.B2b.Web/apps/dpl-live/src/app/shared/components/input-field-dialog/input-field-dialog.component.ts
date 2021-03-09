import { Component, OnInit, Inject } from '@angular/core';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';

@Component({
  selector: 'input-field-dialog',
  templateUrl: './input-field-dialog.component.html',
  styleUrls: ['./input-field-dialog.component.css'],
})
export class InputFieldDialogComponent implements OnInit {
  // todo dialog input type
  constructor(
    @Inject(MAT_DIALOG_DATA)
    public data: {
      title: string;
      value: string;
      property: string;
    }
  ) {}

  onNoClick() {
    console.debug('onNoClick');
  }

  ngOnInit() {}
}
