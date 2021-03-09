import { ID } from '@datorama/akita';

export interface ICountryState {
  id: ID;
}

/**
 * A factory function that creates CountryStates
 */
export function createCountryState(params: Partial<ICountryState>) {
  return {} as ICountryState;
}
