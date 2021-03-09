import { Injectable } from '@angular/core';
import { QueryEntity } from '@datorama/akita';
import { LanguagesStore, LanguagesState } from './languages.store';

@Injectable({ providedIn: 'root' })
export class LanguagesQuery extends QueryEntity<LanguagesState> {
  languages$ = this.selectAll();
  activeLanguage$ = this.selectActive();
  constructor(protected store: LanguagesStore) {
    super(store);
  }
}
