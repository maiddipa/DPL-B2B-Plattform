import { Injectable } from '@angular/core';

import { CountriesQuery } from '../state/countries.query';

@Injectable({
  providedIn: 'root',
})
export class CountriesService {
  constructor(private countriesQuery: CountriesQuery) {}

  getCountries() {
    return this.countriesQuery.countries$;
  }
}
