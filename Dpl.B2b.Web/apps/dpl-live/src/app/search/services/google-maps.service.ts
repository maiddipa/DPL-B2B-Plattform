import { MapsAPILoader, GeocoderAddressComponent } from '@agm/core';
import { Injectable } from '@angular/core';
import { bindCallback, from, Observable, ReplaySubject, Subject } from 'rxjs';
import { first, map, shareReplay, switchMap } from 'rxjs/operators';

import { IGeocodeRequest, IRoutingInfoRequest, IGeoPoint } from '@app/shared';
import * as _ from 'lodash';

@Injectable({
  providedIn: 'root',
})
export class GoogleMapsService {
  initialized$: Observable<void>;
  distanceMatrixService: Observable<google.maps.DistanceMatrixService>;
  geocoder: Observable<google.maps.Geocoder>;
  places: Observable<google.maps.places.PlacesService>;
  autocomplete: Observable<google.maps.places.AutocompleteService>;
  map: ReplaySubject<google.maps.Map> = new ReplaySubject();
  constructor(apiLoader: MapsAPILoader) {
    this.initialized$ = from(apiLoader.load()).pipe(shareReplay());

    this.distanceMatrixService = this.initialized$.pipe(
      map(() => new google.maps.DistanceMatrixService()),
      first()
    );

    this.geocoder = this.initialized$.pipe(
      map(() => new google.maps.Geocoder()),
      first()
    );

    this.places = this.initialized$.pipe(
      switchMap(() => {
        return this.map;
      }),
      map((mapElement) => {
        return new google.maps.places.PlacesService(mapElement as any);
      }),
      first()
    );

    this.autocomplete = this.initialized$.pipe(
      switchMap(() => {
        return this.map;
      }),
      map((mapElement) => {
        return new google.maps.places.AutocompleteService();
      }),
      first()
    );
  }

  setMap(mapElement: google.maps.Map) {
    this.map.next(mapElement);
  }

  getAutoComplete(request: {
    input: string;
    location?: { lat: number; lng: number };
  }) {
    return this.autocomplete.pipe(
      switchMap((autocomplete) => {
        const getPlacePredictions = autocomplete.getPlacePredictions.bind(
          autocomplete
        ) as google.maps.places.AutocompleteService['getPlacePredictions'];

        return bindCallback(getPlacePredictions)({
          input: '',
          sessionToken: 'TODO SET TOKEN',
        }).pipe(map(([predictions, status]) => {}));
      })
    );
  }

  getPlaces(request: {
    query: string;
    location?: { lat: number; lng: number };
  }) {
    return this.places.pipe(
      switchMap((places) => {
        return from(
          new Promise(
            (
              resolve: (value: {
                response: google.maps.places.PlaceResult[];
                status: google.maps.places.PlacesServiceStatus;
              }) => void,
              error
            ) => {
              places.textSearch(request, (response, status) => {
                resolve({
                  response,
                  status,
                });
              });
            }
          )
        );
      })
    );
  }

  getPlaceDetails(placeId: string, sessionToken?: string) {
    const request: google.maps.places.PlaceDetailsRequest = {
      placeId,
      fields: ['address_component', 'geometry'],
    };

    if (sessionToken) {
      request.sessionToken = sessionToken;
    }
    return this.places.pipe(
      switchMap((places) => {
        const getDetails = places.getDetails.bind(
          places
        ) as google.maps.places.PlacesService['getDetails'];
        return bindCallback(getDetails)(request).pipe(map(([place]) => place));
      })
    );
  }

  getGeocodes(requestData: IGeocodeRequest) {
    const request: google.maps.GeocoderRequest = {
      ...requestData,
      ...{
        // this ensure that there is a location preference
        region: 'DE',
        // typings are missing but this will ensure all geocoding results will be in english (only important for country name)
        language: 'en',
      },
    };

    return this.geocoder.pipe(
      switchMap((geocoder) => {
        return from(
          new Promise(
            (
              resolve: (value: {
                response: google.maps.GeocoderResult[];
                status: google.maps.GeocoderStatus;
              }) => void,
              error
            ) => {
              geocoder.geocode(request, (response, status) => {
                resolve({
                  response,
                  status,
                });
              });
            }
          )
        );
      })
    );
  }

  getDistanceMatrix(requestData: IRoutingInfoRequest) {
    const request: google.maps.DistanceMatrixRequest = {
      origins: [
        new google.maps.LatLng(requestData.origin.lat, requestData.origin.lng),
      ],
      destinations: requestData.destinations.map(
        (point) => new google.maps.LatLng(point.lat, point.lng)
      ),
      travelMode: google.maps.TravelMode.DRIVING,
      // transitOptions: {
      //   modes: [google.maps.TransitMode.BUS],
      //   arrivalTime: new Date(),
      //   routingPreference: google.maps.TransitRoutePreference.LESS_WALKING
      // },
      drivingOptions: {
        departureTime: new Date(),
        trafficModel: google.maps.TrafficModel.BEST_GUESS,
      },
      unitSystem: google.maps.UnitSystem.METRIC,
      avoidHighways: false,
      avoidTolls: false,
    };

    return this.distanceMatrixService.pipe(
      switchMap((distanceMatrixService) => {
        // wrap the callback interface into a promise
        return new Promise(
          (
            resolve: (value: {
              response: google.maps.DistanceMatrixResponse;
              status: google.maps.DistanceMatrixStatus;
            }) => void,
            error
          ) => {
            distanceMatrixService.getDistanceMatrix(
              request,
              (response, status) => {
                resolve({
                  response,
                  status,
                });
              }
            );
          }
        );
      })
    );
  }

  public getRouteLink(request: { from: string; to: string }) {
    return `https://www.google.com/maps/dir/?api=1&travelmode=driving&dir_action=preview&origin=${encodeURI(
      request.from
    )}&destination=${encodeURI(request.to)}`;
  }

  public mapToGeoPoint(data: {
    formatted_address?: string;
    address_components?: GeocoderAddressComponent[];
    geometry?: google.maps.places.PlaceGeometry;
  }) {
    const components = data.address_components;
    const streetNo = this.extractAddressComponentValue(components, [
      'street_number',
    ]);
    const street = this.extractAddressComponentValue(components, ['route']);
    const postalCode = this.extractAddressComponentValue(components, [
      'postal_code',
    ]);
    const city = this.extractAddressComponentValue(components, [
      'locality',
      'political',
    ]);
    const state = this.extractAddressComponentValue(components, [
      'administrative_area_level_1',
      'political',
    ]);
    const country = this.extractAddressComponentValue(
      components,
      ['country', 'political'],
      'short_name'
    );

    const streetAndNo = streetNo ? `${street} ${streetNo}` : street;

    const details = () => {
      if (postalCode && city) {
        return `${postalCode} ${city}`;
      }

      if (city) {
        return city;
      }

      if (postalCode) {
        return postalCode;
      }

      return 'Unspecified';
    };

    return <IGeoPoint<google.maps.places.PlaceResult>>{
      lat: data.geometry.location.lat(),
      lng: data.geometry.location.lng(),
      label: data.formatted_address,
      address: {
        street: streetAndNo,
        zip: postalCode,
        country,
        details: details(),
      },
      data,
    };
  }

  private extractAddressComponentValue(
    components: google.maps.GeocoderAddressComponent[],
    types: string[],
    property: Exclude<
      keyof google.maps.GeocoderAddressComponent,
      'types'
    > = 'long_name'
  ) {
    const component = components.find(
      (i) => _.difference(types, i.types).length === 0
    );
    if (!component) {
      return null;
    }

    return component[property];
  }
}
