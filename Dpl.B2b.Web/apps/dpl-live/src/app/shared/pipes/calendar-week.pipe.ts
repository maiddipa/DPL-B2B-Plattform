import { Pipe, PipeTransform } from '@angular/core';
import * as moment from 'moment';
import { DatePipe } from '@angular/common';
moment.locale('de');

@Pipe({
  name: 'calendarWeek',
})
export class CalendarWeekPipe implements PipeTransform {
  constructor(private datePipe: DatePipe) {}
  transform(dateInWeek: Date, format: 'range' = null): any {
    const date = (moment as any)(dateInWeek) as moment.Moment;
    if (format === 'range') {
      const startDate = date.weekday(0);
      const endDate = startDate.clone().add(6, 'day');

      return `KW ${startDate.week()} (${this.datePipe.transform(
        startDate.toDate()
      )} - ${this.datePipe.transform(endDate.toDate())})`;
    }
    return `KW ${date.week()}`;
  }
}
