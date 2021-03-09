import { Pipe, PipeTransform, Inject, LOCALE_ID } from '@angular/core';
import {
  DecimalPipe,
  formatCurrency,
  formatNumber,
  formatPercent,
} from '@angular/common';

function isEmpty(value: any): boolean {
  return (
    value === undefined || value == null || value === '' || value !== value
  );
}

function parseIntAutoRadix(text: string): number {
  // tslint:disable-next-line: radix
  const result: number = parseInt(text);
  if (isNaN(result)) {
    throw new Error('Invalid integer literal when parsing ' + text);
  }
  return result;
}

const NUMBER_FORMAT_REGEXP = /^(\d+)?\.((\d+)(-(\d+))?)?$/;

/**
 * Extended number pipe.
 * Use grouping allows the thousands separator to be switched off.
 * Default value true.
 */
@Pipe({
  name: 'numberEx',
})
export class NumberExPipe implements PipeTransform {
  constructor(@Inject(LOCALE_ID) private _locale: string) {}

  /**
   * Transforms number ex pipe
   * @param value The number to be formatted.
   * @param [useGrouping] Turn thousands separator on/off
   * @param digitsInfo Decimal representation options, specified by a string
   * in the following format:<br>
   * <code>{minIntegerDigits}.{minFractionDigits}-{maxFractionDigits}</code>.
   *   - `minIntegerDigits`: The minimum number of integer digits before the decimal point.
   * Default is `1`.
   *   - `minFractionDigits`: The minimum number of digits after the decimal point.
   * Default is `0`.
   *   - `maxFractionDigits`: The maximum number of digits after the decimal point.
   * Default is `3`.
   * @param locale A locale code for the locale format rules to use.
   * When not supplied, uses the value of `LOCALE_ID`, which is `en-US` by default.
   * See [Setting your app locale](guide/i18n#setting-up-the-locale-of-your-app).
   * @returns transform
   */
  transform(
    value: number,
    useGrouping: boolean = true,
    digitsInfo?: string,
    locale?: string
  ): any {
    locale = locale || this._locale;

    if (isEmpty(value)) {
      return null;
    }

    let minInt = 1;
    let minFraction = 0;
    let maxFraction = 2;

    if (digitsInfo) {
      const parts = digitsInfo.match(NUMBER_FORMAT_REGEXP);
      if (parts === null) {
        throw new Error(`${digitsInfo} is not a valid digit info`);
      }
      const minIntPart = parts[1];
      const minFractionPart = parts[3];
      const maxFractionPart = parts[5];

      if (minIntPart != null) {
        minInt = parseIntAutoRadix(minIntPart);
      }
      if (minFractionPart != null) {
        minFraction = parseIntAutoRadix(minFractionPart);
      }
      if (maxFractionPart != null) {
        maxFraction = parseIntAutoRadix(maxFractionPart);
      } else if (minFractionPart != null && minFraction > maxFraction) {
        maxFraction = minFraction;
      }
    }

    const options = <Intl.NumberFormatOptions>{
      useGrouping: useGrouping,
      minimumFractionDigits: minFraction,
      maximumFractionDigits: maxFraction,
      minimumIntegerDigits: minInt,
    };

    // const nfObject = new Intl.NumberFormat(locale, options)
    // const output= nfObject.format(value);

    // .toLocaleString benutzt intern Intl.NumberFormat
    const output = value.toLocaleString(locale, options);

    // HACK Intl. Grouping Flag not working
    // if(!useGrouping){
    //   console.log("!useGrouping", useGrouping)
    //   return output.replace(".","");
    // }

    return output;
  }
}
