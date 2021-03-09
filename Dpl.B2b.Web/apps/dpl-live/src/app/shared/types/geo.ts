import { LoadCarrierOffering } from '@app/api/dpl';

export interface IGeoPoint<T> {
  label: string;
  lat: number;
  lng: number;
  distance?: number;
  address: {
    street: string;
    zip: string;
    details: string;
    country: string | number; // this will contain the country id for offerings and country code for reverse geocodes
  };
  data: T;
}

export interface IRoutingInfoRequest {
  origin: IGeoPoint<any>;
  destinations: IGeoPoint<LoadCarrierOffering>[];
}

export interface IDistanceMatrixResponse {
  response: {
    destination: IGeoPoint<LoadCarrierOffering>;
    matrixResponse: google.maps.DistanceMatrixResponseElement;
  }[];
  status: google.maps.DistanceMatrixStatus;
}

export interface IGeocodeRequest {
  address?: string;
  location?: {
    lat: number;
    lng: number;
  };
}
