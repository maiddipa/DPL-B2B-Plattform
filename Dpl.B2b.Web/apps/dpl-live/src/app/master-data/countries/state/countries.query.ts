import { Injectable } from '@angular/core';
import { combineQueries, QueryEntity } from '@datorama/akita';
import { map, publishReplay, refCount } from 'rxjs/operators';

import { CountriesState, CountriesStore } from './countries.store';
import { CountryStatesQuery } from './country-states.query';
import { ICountry } from './country.model';

@Injectable({ providedIn: 'root' })
export class CountriesQuery extends QueryEntity<CountriesState> {
  countries$ = this.getCountries();

  constructor(
    protected store: CountriesStore,
    private statesQuery: CountryStatesQuery
  ) {
    super(store);
  }

  private getCountries() {
    return combineQueries([
      this.selectAll(),
      this.statesQuery.selectAll({ asObject: true }),
    ]).pipe(
      map(([countries, states]) => {
        return countries.map((i) => {
          return <ICountry>{
            ...i,
            states: i.states.map((id) => states[id]),
          };
        });
      }),
      publishReplay(1),
      refCount()
    );
  }
}
