import { Injectable } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { LoadCarrierOffering } from '@app/api/dpl';
import {
  IDistanceMatrixResponse,
  IGeoPoint,
  IRoutingInfoRequest,
} from '@app/shared';
import { CountryPipe } from '@app/shared/pipes/country.pipe';
import * as moment from 'moment';
import { BehaviorSubject, EMPTY, forkJoin, iif, of, Subject } from 'rxjs';
import { map, switchMap, tap } from 'rxjs/operators';

import { AccountsService } from '../../accounts/services/accounts.service';
import { DplApiService } from '../../core/services/dpl-api.service';
import { SessionService } from '../../core/services/session.service';
import { chunkArray, getOffsetSinceStartOfWeek } from '../../core/utils';
import { GeojsonService } from './geojson.service';
import { GoogleMapsService } from './google-maps.service';
import {
  IDestination,
  IMapClickPosition,
  ISearchInput,
  ISearchResponse,
} from './search.service.types';

@Injectable({
  providedIn: 'root',
})
export class SearchService {
  responses: BehaviorSubject<ISearchResponse[]>;
  selectedResponseIndex: Subject<number>;

  constructor(
    private dpl: DplApiService,
    private maps: GoogleMapsService,
    private dialog: MatDialog,
    private snagBar: MatSnackBar,
    private countryPipe: CountryPipe,
    private accountsService: AccountsService,
    private geoJson: GeojsonService,
    private session: SessionService,
  ) {
    this.responses = new BehaviorSubject<ISearchResponse[]>([]);
    this.selectedResponseIndex = new Subject();
  }

  getSearchResponses() {
    return this.responses.asObservable();
  }

  clearSearchResponses() {
    this.responses.next([]);
  }

  search(input: ISearchInput) {
    const geoPointFromZip$ = of(this.geoJson.getGeoJsonForZip(input.zip)).pipe(
      switchMap((geoJson) => {
        if (!geoJson) {
          return EMPTY;
        }

        return of(<IGeoPoint<any>>{
          lat: geoJson.properties.center[0],
          lng: geoJson.properties.center[1],
          label: `PLZ Bereich - ${input.zip}`,
          address: this.getAddress(`PLZ Bereich - ${input.zip}`), //TODO i18n
        });
      })
    );

    const geoPointFromInput$ = of(input.geoPoint);

    // use the search input data to identify potential matches via google places api
    // the let user select one GeoPoint from potential matches
    const geoPointFromForm$ = input.isZipOnlySearch
      ? geoPointFromZip$
      : geoPointFromInput$;

    // if this is a MapClick based search get point from click data
    // use reverse geocode position to get name
    const geoPointFromMapClick$ = this.getGeocodes(input.position).pipe(
      map((resp) => {
        const geocode = resp.origins.length > 0 ? resp.origins[0] : null;

        if (geocode) {
          return this.maps.mapToGeoPoint(geocode);
        }

        return <IGeoPoint<any>>{
          ...input.position,
          ...{
            label: 'Point on Map',
            address: this.getAddress(
              `lat: ${input.position.lat} | lng: ${input.position.lng}`
            ),
          },
        };
      })
    );

    // first step is to identify the geopoint
    return iif(
      // if the map was clicked to select a point
      () => input.type === 'form',
      geoPointFromForm$,
      // else
      geoPointFromMapClick$
    ).pipe(
      // if a geopoint was found
      // use geopoint + search input to get destinations from backend
      switchMap((origin?: IGeoPoint<any>) => {
        if (!origin) {
          return EMPTY;
        }
        return this.getDestinations(input, origin).pipe(
          // use destinations retrieved from backend to calculate routes for each
          // using the google maps distance matrix api
          switchMap((destinations) => {
            const responseWithoutRouting$ = of(destinations).pipe(
              map((destinations) => {
                const response = destinations.map(
                  (d) =>
                    <IDistanceMatrixResponse['response'][0]>{
                      destination: d,
                      matrixResponse: {
                        distance: {
                          text: `${(d.distance / 1000).toFixed(1)} km`,
                          value: d.distance,
                        },
                      },
                    }
                );

                return <IDistanceMatrixResponse>{
                  response,
                  status: google.maps.DistanceMatrixStatus.OK,
                };
              })
            );

            return iif(
              () => !input.routing,
              responseWithoutRouting$,
              this.getRoutingInfo(<IRoutingInfoRequest>{
                origin,
                destinations,
              })
            );
          }),
          // finally map all data to search response
          map((data) => {
            return <ISearchResponse>{
              id: this.session.getNextId(),
              input,
              geoJson: input.isZipOnlySearch
                ? this.geoJson.getGeoJsonForZip(input.zip)
                : null,
              origin,
              destinations: data.response.map((i) => {
                return <IDestination>{
                  // HACK User address id to uniquely identify a basket item across searches
                  // instead it should use the guid that is attached to details
                  // i.destination.data.details[0].guid
                  // however because they are displayed as groups in the ui
                  // and becasue we will implement proper blocking later
                  // it is currently solved this way
                  id: i.destination.data.address.id,
                  // id: this.session.getNextId(),
                  // TODO remove hardcoded days of week and replace with address specific dates
                  availableDaysOfWeek: [1, 2, 3, 4, 5, 6],
                  info: i.destination,
                  routing: i.matrixResponse,
                  googleRoutingLink: this.createGoogleRouteLink(
                    origin,
                    i.destination
                  ),
                };
              }),
            };
          })
        );
      }),
      tap((response) => {
        this.responses.next([...this.responses.value, response]);
      })
    );
  }

  getGeocodes(postion?: IMapClickPosition) {
    return this.maps
      .getGeocodes({
        location: postion ? postion : null,
      })
      .pipe(
        map((data) => {
          return {
            postion,
            origins: data.response,
          };
        })
      );
  }

  getDestinations(input: ISearchInput, origin: IGeoPoint<any>) {
    const startDate = moment(input.calendarWeek);
    const dates = input.daysOfWeek.map((daysOfWeek) =>
      startDate.clone().add(daysOfWeek, 'days').toDate()
    );
    return this.dpl.offerings
      .searchLoadCarrierOfferings({
        postingAccountId: input.postingAccount.id,
        type: input.orderType,
        lat: origin.lat,
        lng: origin.lng,
        radius: Math.trunc(input.radius),
        loadCarrierId: input.palletId,
        loadCarrierQuantity: input.quantity,
        quantityType: input.quantityType,
        baseLoadCarrierId: input.baseLoadCarrierIds,
        stackHeightMin: input.stackHeight,
        stackHeightMax: input.stackHeight,
        date: dates,
      })
      .pipe(
        map((response) =>
          response
            .map((offering) => {
              // remove business hours that can not be used for ordering
              offering.businessHours = offering.businessHours.filter((i) => {
                const dateInWeek = startDate
                  .clone()
                  .add(getOffsetSinceStartOfWeek(i.dayOfWeek), 'days')
                  .toDate();

                return offering.details.some((i) => {
                  return (
                    dateInWeek >= new Date(i.availabilityFrom) &&
                    dateInWeek <= new Date(i.availabilityTo)
                  );
                });
              });

              return offering;
            })
            .filter((offering) => offering.businessHours.length > 0)
            .map((offering) => {
              const geopPoint: IGeoPoint<LoadCarrierOffering> = {
                label: '', // TODO Remove label as we are only using address components for search + result list displays
                lat: offering.lat,
                lng: offering.lng,
                distance: offering.distance,
                address: {
                  street: `${offering.address.street1}`,
                  zip: offering.address.postalCode,
                  details: `${offering.address.postalCode} ${offering.address.city}`,
                  // HACK country on geoppoint is still a string, should be int instead
                  country: offering.address.country,
                },
                data: offering,
              };

              return geopPoint;
            })
            .sort((a, b) => {
              if (a.distance < b.distance) {
                return -1;
              } else if (a.distance === b.distance) {
                return 0;
              } else if (a.distance > b.distance) {
                return 1;
              }
            })
        )
      );
  }

  getRoutingInfo(request: IRoutingInfoRequest) {
    const batches = chunkArray(request.destinations, 25);

    const results = batches.map((destinations) => {
      const batchRequest = { ...request, ...{ destinations } };
      return this.maps
        .getDistanceMatrix(batchRequest)
        .pipe(map((resp) => resp.response.rows[0]));
    });

    return forkJoin(results).pipe(
      map((results) => {
        // merge back the batches into a single row
        const elements = results
          .map((i) => i.elements)
          .reduce((prev, current) => {
            prev.push(...current);
            return prev;
          }, []);

        // map destination info and matrix response for destination
        const response = request.destinations
          .map((destination, i) => ({
            destination,
            matrixResponse: elements[i],
          }))
          // then filter to only include valid results
          .filter(
            (destination) =>
              destination.matrixResponse.status ===
              google.maps.DistanceMatrixElementStatus.OK
          )
          .sort((a, b) => {
            if (
              a.matrixResponse.duration.value < b.matrixResponse.duration.value
            ) {
              return -1;
            } else if (
              a.matrixResponse.duration.value == b.matrixResponse.duration.value
            ) {
              return 0;
            } else if (
              a.matrixResponse.duration.value > b.matrixResponse.duration.value
            ) {
              return 1;
            }
          });

        return <IDistanceMatrixResponse>{
          response,
          status: google.maps.DistanceMatrixStatus.OK,
        };
      })
    );
  }

  private getAddress(formattedAddress: string) {
    // sample response from google: "Hanauer Landstraße 10, 60314 Frankfurt am Main, Deutschland"
    const parts = formattedAddress.split(', ');
    const street = parts[0];
    let country: string;
    if (parts.length > 1) {
      country = parts[parts.length - 1];
    }

    let zip: string;
    if (parts.length > 2) {
      zip = parts[1].trim().split(' ')[0];
    }
    const details = parts.splice(1, parts.length - 2).join(', ');
    return <IGeoPoint<any>['address']>{
      street,
      details,
      zip,
      country,
    };
  }

  public createGoogleRouteLink(
    origin: IGeoPoint<any>,
    destination: IGeoPoint<any>
  ) {
    const from = origin.address.country
      ? `${origin.address.street}, ${
          origin.address.zip
        }, ${this.countryPipe.transform(origin.address.country)}`
      : `${origin.lat.toLocaleString('en-US')},${origin.lng.toLocaleString(
          'en-US'
        )}`;

    const to = `${destination.address.street}, ${
      destination.address.zip
    }, ${this.countryPipe.transform(destination.address.country)}`;

    return this.maps.getRouteLink({
      from,
      to,
    });
  }
}
