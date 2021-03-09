import {
  ChangeDetectionStrategy,
  Component,
  Input,
  OnInit,
} from '@angular/core';
import { FormControl } from '@angular/forms';
import * as moment from 'moment';
import { ILabelValue } from '@dpl/dpl-lib';

@Component({
  selector: 'app-calendar-week-picker',
  templateUrl: './calendar-week-picker.component.html',
  styleUrls: ['./calendar-week-picker.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class CalendarWeekPickerComponent implements OnInit {
  @Input() form: FormControl;

  calendarWeekOptions: Date[];

  constructor() {}

  ngOnInit() {
    this.calendarWeekOptions = this.getCalendarWeeks();
    this.form.patchValue(this.calendarWeekOptions[0]);
  }

  private getCalendarWeeks() {
    moment.locale('de');
    const today = moment.utc();
    let mondayOfStartWeek = today.clone().weekday(0).startOf('day');

    // if its sunday use next week as start week
    if (today.weekday() >= 6) {
      mondayOfStartWeek.add('week', 1);
    }

    const startCalendarWeek = mondayOfStartWeek.week();

    const weekOptions = [...new Array(5)].map((v, index) => {
      const startDate = mondayOfStartWeek
        .clone()
        .week(startCalendarWeek + index);

      return startDate.toDate();
    });

    return weekOptions;
  }

  compareWith(value1: Date, value2: Date) {
    return value1.getTime() === value2.getTime();
  }
}
