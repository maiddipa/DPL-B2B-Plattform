import { Injectable } from '@angular/core';
import { I18n } from '@ngx-translate/i18n-polyfill';

type I18nShim = (value: { id: string; value: string }) => string;

@Injectable({
  providedIn: 'root',
})
export class LocalizationService {
  private i18nShim: I18nShim;
  constructor(i18n: I18n) {
    this.i18nShim = i18n;
  }

  getLocalizationIdWithLanguage(
    itemName: string,
    valueIdOrName: string,
    fieldName: string = null
  ) {
    if (!itemName) {
      throw 'itemName cannot be null';
    }

    if (!valueIdOrName) {
      throw 'valueIdOrName cannot be null';
    }

    if (!fieldName) {
      return `${itemName}|${valueIdOrName}`;
    }

    return `${itemName}|${valueIdOrName}|${fieldName}`;
  }

  getTranslation(
    itemName: string,
    valueIdOrName: string | number,
    fieldName: string = null
  ) {
    const valueString =
      typeof valueIdOrName === 'string'
        ? valueIdOrName
        : valueIdOrName.toString();
    const generatedId = this.getLocalizationIdWithLanguage(
      itemName,
      valueString,
      fieldName
    );

    // using a shim so this cann is not picked up by the extractor
    return this.i18nShim({
      id: generatedId,
      value: generatedId, // value has to be filled with something, but is ignored when id is provided
    });
  }

  getTranslationById(id: string): string {
    return this.i18nShim({ id, value: id });
  }
}
