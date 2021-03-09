import { Component, OnInit, Input } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';

@Component({
  selector: 'app-business-hour-day',
  templateUrl: './business-hour-day.component.html',
  styleUrls: ['./business-hour-day.component.scss'],
})
export class BusinessHourDayComponent implements OnInit {
  @Input() day: string;
  hourOptions: number[] = [
    6,
    7,
    8,
    9,
    10,
    11,
    12,
    13,
    14,
    15,
    16,
    17,
    18,
    19,
    20,
  ];
  minuteOptions: number[] = [0, 15, 30, 45];
  form: FormGroup;

  constructor(private formBuilder: FormBuilder) {}

  ngOnInit() {
    this.form = this.formBuilder.group({
      fromHour: [8, []],
      fromMinute: [30, []],
      toHour: [17, []],
      toMinute: [30, []],
    });
  }
}
