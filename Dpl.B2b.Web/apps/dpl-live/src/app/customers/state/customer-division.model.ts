import { CustomerDivisionDocumentSettings } from 'apps/dpl-live/src/app/core/services/dpl-api-services';
import { ILoadingLocation } from 'apps/dpl-live/src/app/loading-locations/state/loading-location.model';

export interface ICustomerDivision<
  TLoadingLocation extends number | ILoadingLocation = ILoadingLocation
> {
  id: number;
  name: string;
  shortName: string;
  loadingLocations: TLoadingLocation[];
  documentSettings: CustomerDivisionDocumentSettings[];
}
/**
 * A factory function that creates CustomerDivisions
 */
export function createCustomerDivision(params: Partial<ICustomerDivision>) {
  return {} as ICustomerDivision;
}
