import { Injectable } from '@angular/core';
import { ICountry } from './country.model';
import {
  EntityState,
  ActiveState,
  EntityStore,
  StoreConfig,
} from '@datorama/akita';

export interface CountriesState
  extends EntityState<ICountry<number>, number>,
    ActiveState<number> {}

@Injectable({ providedIn: 'root' })
@StoreConfig({ name: 'countries' })
export class CountriesStore extends EntityStore<CountriesState> {
  constructor() {
    super();
  }
}
