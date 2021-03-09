import { Injectable } from '@angular/core';
import { QueryEntity } from '@datorama/akita';
import { CountryStatesStore, CountryStatesState } from './country-states.store';

@Injectable({ providedIn: 'root' })
export class CountryStatesQuery extends QueryEntity<CountryStatesState> {
  constructor(protected store: CountryStatesStore) {
    super(store);
  }
}
