import * as moment from 'moment';
import {
  OrderType,
  OrderTransportType,
  OrderStatus,
  OrderQuantityType,
  ListSortDirection,
  OrderSearchRequestSortOptions,
} from '@app/api/dpl';

export class AvailabilityRequest {
  // divisionId: string;
  sortActive?: string;
  sortDirection?: string;
  pageIndex?: number;
  pageSize?: number;
}

export interface Availability {
  carrierTypes: CarrierType[];
  rearLoading: boolean;
  sideLoading: boolean;
  jumboLoading: boolean;
  dateMin: Date;
  dateMax: Date; // dateMin = dateMax wenn Fix Termin
}

export interface CarrierType {
  id: string;
  title: string;
  qualities: CarrierQuality[];
  defaults: CarrierDefaults;
}

export interface CarrierQuality {
  id: string;
  title: string;
}

export interface CarrierDefaults {
  stackHeightMin: number;
  stackHeightMax: number;
  quantityPerStack: number;
  quantityPerEurSpace: number;
}

export interface ILoadsPerDay {
  date: moment.Moment;
  dateTo?: moment.Moment;
  loads: number;
}

export interface OrderSearchRequest {
  code?: string | null | undefined;
  type?: OrderType[] | null | undefined;
  transportType?: OrderTransportType[] | null | undefined;
  status?: OrderStatus[] | null | undefined;
  postingAccountId?: number[] | null | undefined;
  loadCarrierId?: number[] | null | undefined;
  baseLoadCarrierId?: number[] | null | undefined;
  loadingLocationId?: number[] | null | undefined;
  quantityType?: OrderQuantityType[] | null | undefined;
  numberOfLoads?: number[] | null | undefined;
  numberOfStacks?: number | null | undefined;
  stackHeightMin?: number | null | undefined;
  stackHeightMax?: number | null | undefined;
  supportsRearLoading?: boolean | null | undefined;
  supportsSideLoading?: boolean | null | undefined;
  supportsCourtLoading?: boolean | null | undefined;
  supportsRampLoading?: boolean | null | undefined;
  supportsTrailerVehicles?: boolean | null | undefined;
  supportsBoxVehicles?: boolean | null | undefined;
  supportsJumboVehicles?: boolean | null | undefined;
  earliestFulfillmentDateTime?: Date | null | undefined;
  latestFulfillmentDateTime?: Date | null | undefined;
  sortBy?: OrderSearchRequestSortOptions | undefined;
  sortDirection?: ListSortDirection | undefined;
  page?: number | undefined;
  limit?: number | undefined;
}
