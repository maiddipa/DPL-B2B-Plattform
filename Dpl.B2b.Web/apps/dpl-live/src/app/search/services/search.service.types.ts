import {
  LoadCarrierOffering,
  OrderQuantityType,
  OrderType,
} from '@app/api/dpl';
import { IGeoPoint } from '@app/shared';
import { IAccount } from 'apps/dpl-live/src/app/accounts/state/account.model';

export interface ISearchInput {
  postingAccount: IAccount;
  type: SearchInputType;
  isZipOnlySearch: boolean;
  searchText: string;
  geoPoint: IGeoPoint<google.maps.places.PlaceResult>;
  zip: string;
  // company: string;
  // street: string;
  // city: string;
  // country: string;
  routing: boolean;
  palletId: number;
  quantityType: OrderQuantityType;
  quantity: number;
  calculatedQuantity: number;
  stackHeight: number;
  baseLoadCarrierIds: number[];
  radius: number;
  calendarWeek: Date;
  daysOfWeek: number[];
  orderType: OrderType;
  position?: IMapClickPosition;
  formValue: any
}

export type IMapClickPosition = { lat: number; lng: number };

export type SearchInputType = 'form' | 'map';

export enum SearchInputOrderType {
  // Selbstabholung
  Pickup = 2,
  // Abgabe
  Dropoff = 5,
}

export type IDistance = google.maps.Distance;
export type IDuration = google.maps.Duration;
export interface IRoutingInfo {
  distance: IDistance;
  duration: IDuration;
  duration_in_traffic: IDuration;
}

export type IZipFeatureCollection = GeoJSON.FeatureCollection<
  GeoJSON.GeometryObject,
  { plz: string; note: string; center: [number, number] }
>;

export interface ISearchResponse {
  id: number;
  input: ISearchInput;
  geoJson: IZipFeatureCollection['features'][0];
  origin: IGeoPoint<any>;
  destinations: Array<IDestination>;
}

export interface IDestination {
  id: number;
  info: IGeoPoint<LoadCarrierOffering>;
  routing: IRoutingInfo;
  availableDaysOfWeek: number[];
  googleRoutingLink: string;
}
