import { Inject, Injectable, LOCALE_ID } from '@angular/core';
import { loadMessages } from 'devextreme/localization';
import * as _ from 'lodash';
import { getLanguageFromLocale } from '../utils';

@Injectable({
  providedIn: 'root',
})
export class DevExtremeLocalizationService {
  private language: string;
  constructor(@Inject(LOCALE_ID) locale: string) {
    this.language = getLanguageFromLocale(locale || 'de');
  }

  loadMessage(key: string, value: string) {
    const messages = {};
    messages[key] = value;
    const messagesWithLanguage = this.getMessagesWithLanguage(messages);
    loadMessages(messagesWithLanguage);
  }

  loadMessages(messages: { key: string; value: string }[]) {
    const messagesDict = _(messages)
      .keyBy((i) => i.key)
      .mapValues((i) => i.value)
      .value();

    const messagesWithLanguage = this.getMessagesWithLanguage(messagesDict);
    loadMessages(messagesWithLanguage);
  }  

  private getMessagesWithLanguage(messages: { [key: string]: string }) {
    const messagesWithLanguage = {};
    messagesWithLanguage[this.language] = messages;
    return messagesWithLanguage;
  }
}
