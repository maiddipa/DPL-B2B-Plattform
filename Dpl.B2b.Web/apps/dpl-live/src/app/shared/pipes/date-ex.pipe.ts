import { Pipe, PipeTransform, Inject, LOCALE_ID } from '@angular/core';
import { DatePipe, formatDate } from '@angular/common';

function isEmpty(value: any): boolean {
  return (
    value === undefined || value == null || value === '' || value !== value
  );
}

/**
 * Transforms date pipe
 * @param value The number to be formatted.
 * @param dateFormat Date Token Format
 * @param timeFormat Time Token Format
 * @param divider Divider
 * @param locale A locale code for the locale format rules to use.
 * When not supplied, uses the value of `LOCALE_ID`, which is `en-US` by default.
 * See [Setting your app locale](guide/i18n#setting-up-the-locale-of-your-app).
 * @returns transform
 */
@Pipe({
  name: 'dateEx',
})
export class DateExPipe implements PipeTransform {
  constructor(@Inject(LOCALE_ID) private _locale: string) {}

  transform(
    value: any,
    dateFormat?: string,
    timeFormat?: string,
    divider?: string,
    locale?: string
  ): any {
    if (isEmpty(value)) {
      return null;
    }

    locale = locale || this._locale;

    dateFormat = dateFormat || 'shortDate';
    timeFormat = timeFormat || 'shortTime';

    divider = divider || '/';

    const date = formatDate(value, dateFormat, locale);
    const time = formatDate(value, timeFormat, locale);

    return `${date} ${divider} ${time}`;
  }
}
