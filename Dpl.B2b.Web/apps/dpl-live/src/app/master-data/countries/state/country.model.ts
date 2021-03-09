import { Country } from 'apps/dpl-live/src/app/core/services/dpl-api-services';

import { ICountryState } from './country-state.model';

export interface ICountry<
  TCountryState extends number | ICountryState = ICountryState
> {
  id: number;
  name: string;
  iso2Code: string;
  iso3Code: string;
  licensePlateCode: string;
  states: TCountryState[];
}

/**
 * A factory function that creates Countries
 */
export function createCountry(params: Partial<Country>) {
  return {} as Country;
}
