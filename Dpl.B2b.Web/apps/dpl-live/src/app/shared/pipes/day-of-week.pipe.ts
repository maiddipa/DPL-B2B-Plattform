import { Pipe, PipeTransform } from '@angular/core';
import * as moment from 'moment';
import { DayOfWeek } from '@app/api/dpl';
import { LocalizationService } from 'apps/dpl-live/src/app/core/services/localization.service';

// TODO i18n => user localized enum entries
export const daysOfWeekOptionDict = {
  0: { label: 'Mo', value: 0 },
  1: { label: 'Di', value: 1 },
  2: { label: 'Mi', value: 2 },
  3: { label: 'Do', value: 3 },
  4: { label: 'Fr', value: 4 },
  5: { label: 'Sa', value: 5 },
};

@Pipe({
  name: 'dayOfWeek',
})
export class DayOfWeekPipe implements PipeTransform {
  constructor(private localizationService: LocalizationService) {}

  transform(
    dayOfWeek: DayOfWeek | string | number,
    weekStart: Date = null
  ): any {
    if (typeof dayOfWeek !== 'number') {
      if (!dayOfWeek) {
        throw new Error("dayOfWeek can't be null");
      }
      return this.localizationService.getTranslation('DayOfWeek', dayOfWeek);
    }

    if (weekStart) {
      const date = (moment as any)(weekStart) as moment.Moment;
      date.add(dayOfWeek, 'day');
      return `${(daysOfWeekOptionDict[dayOfWeek] || {}).label} (${date.format(
        'DD.MM.YYYY'
      )})`;
    }

    return (daysOfWeekOptionDict[dayOfWeek] || {}).label;
  }
}
