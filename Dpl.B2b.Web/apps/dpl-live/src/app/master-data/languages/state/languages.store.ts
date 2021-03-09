import { Injectable } from '@angular/core';
import { ILanguage } from './language.model';
import {
  EntityState,
  ActiveState,
  EntityStore,
  StoreConfig,
} from '@datorama/akita';

export interface LanguagesState
  extends EntityState<ILanguage, number>,
    ActiveState<number> {}

@Injectable({ providedIn: 'root' })
@StoreConfig({ name: 'languages' })
export class LanguagesStore extends EntityStore<LanguagesState> {
  constructor() {
    super();
  }
}
