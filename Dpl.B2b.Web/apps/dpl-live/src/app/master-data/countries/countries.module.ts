import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';

import { CountriesService } from './services/countries.service';
import { CountriesQuery } from './state/countries.query';
import { CountriesStore } from './state/countries.store';
import { CountryStatesQuery } from './state/country-states.query';
import { CountryStatesStore } from './state/country-states.store';

@NgModule({
  imports: [CommonModule],
  declarations: [],
  exports: [],
  providers: [
    CountriesStore,
    CountriesQuery,
    CountryStatesStore,
    CountryStatesQuery,
    CountriesService,
  ],
})
export class CountriesModule {}
