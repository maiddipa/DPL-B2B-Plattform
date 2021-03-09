import { Injectable } from '@angular/core';
import { ICountryState } from './country-state.model';
import {
  EntityState,
  ActiveState,
  EntityStore,
  StoreConfig,
} from '@datorama/akita';

export interface CountryStatesState
  extends EntityState<ICountryState, number>,
    ActiveState<number> {}

@Injectable({ providedIn: 'root' })
@StoreConfig({ name: 'country-states' })
export class CountryStatesStore extends EntityStore<CountryStatesState> {
  constructor() {
    super();
  }
}
