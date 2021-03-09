import {
  Address,
  BusinessHourException,
  BusinessHours,
  LoadingLocationDetail,
  PublicHoliday,
} from '@app/api/dpl';

export interface ILoadingLocation<TAddress extends number | Address = Address> {
  id: number;
  address: TAddress;
  businessHours?: BusinessHours[];
  businessHourExceptions?: BusinessHourException[];
  publicHolidays?: PublicHoliday[];
  detail?: LoadingLocationDetail;
}

/**
 * A factory function that creates LoadingLocations
 */
export function createLoadingLocation(params: Partial<ILoadingLocation>) {
  return {} as ILoadingLocation;
}
