import { Injectable } from '@angular/core';
import { DplApiService, DplApiSort } from '@app/core';
import { TransportsStore } from '../state/transports.store';
import { tap, switchMap, catchError, map, first } from 'rxjs/operators';
import {
  TransportOfferingBidCreateRequest,
  DayOfWeek,
  TransportOfferingStatus,
  TransportBidStatus,
  TransportOfferingBid,
  TransportOfferingSortOption,
  Address,
} from '@app/api/dpl';
import { TransportsQuery } from '../state/transports.query';
import { applyTransaction, arrayAdd, arrayUpdate } from '@datorama/akita';
import { ITransport } from '../state/transport.model';
import { of, EMPTY, Observable } from 'rxjs';
import { NOT_IMPLEMENTED_EXCEPTION } from '@dpl/dpl-lib';
import { Filter } from '../../filters/services/filter.service.types';
import { GoogleMapsService } from '../../search/services/google-maps.service';
import { CountryPipe } from '@app/shared';
import * as moment from 'moment';
import { CustomersService } from '../../customers/services/customers.service';
import { CustomerDivisionsService } from '../../customers/services/customer-divisions.service';

export type TransportOfferingsSearchRequest = Parameters<
  DplApiService['offerings']['searchTransportOfferings']
>[0];

@Injectable({
  providedIn: 'root',
})
export class TransportsService {
  constructor(
    private dpl: DplApiService,
    private transportsStore: TransportsStore,
    private transportsQuery: TransportsQuery,
    private maps: GoogleMapsService,
    private countryPipe: CountryPipe,
    private division: CustomerDivisionsService
  ) {}

  getTransports(info: {
    filters: Filter[];
    page: number;
    limit: number;
    sort: DplApiSort<TransportOfferingSortOption>;
  }) {
    return this.division.getActiveDivision().pipe(
      switchMap((division) => {
        const divisionId = division.id;
        const { page, sort, limit } = info;
        const request: TransportOfferingsSearchRequest = {
          divisionId,
          ...this.convertFilterToSearchRequest(info.filters),
          page,
          limit,
          ...sort,
        };

        return this.dpl.offerings.searchTransportOfferings(request);
      })
    );
  }

  convertFilterToSearchRequest(filters: Filter[]) {
    const searchRequest = filters.reduce((request, filter) => {
      switch (filter.propertyName) {
        case 'TransportOfferingStatus':
          request.status = (filter.value || []).map(
            (value) => TransportOfferingStatus[value]
          );
          break;
        case 'LoadCarrierTypes':
          request.loadCarrierType = (filter.value || []).map((value) =>
            parseInt(value)
          );
          break;
        case 'SupplyPostalCode':
          request.supplyPostalCode = this.getSingleValue(filter.value);
          break;
        case 'DemandPostalCode':
          request.demandPostalCode = this.getSingleValue(filter.value);
          break;
        case 'SupplyEarliestFulfillmentDate': {
          const dateRange = this.getDateRange(filter.value);
          if (dateRange) {
            request.supplyEarliestFulfillmentDateFrom = dateRange.from;
            request.supplyEarliestFulfillmentDateTo = dateRange.to;
          }
          break;
        }
        case 'DemandLatestFulfillmentDate': {
          const dateRange = this.getDateRange(filter.value);
          if (dateRange) {
            request.demandLatestFulfillmentDateFrom = dateRange.from;
            request.demandLatestFulfillmentDateTo = dateRange.to;
          }
          break;
        }
        default:
          throw new Error(
            `Filter property is not handled ${filter.propertyName}`
          );
      }

      return request;
    }, {} as TransportOfferingsSearchRequest);

    return searchRequest;
  }

  getSingleValue(value: string[]) {
    return value.length > 0 ? value[0] : null;
  }

  getDateRange(value: string[]) {
    if (!value || value.length === 0) {
      return null;
    }

    return {
      from: moment(value[0]).toDate(),
      to:
        value.length > 1 && value[1]
          ? moment(value[1]).toDate()
          : moment(value[0]).toDate(),
    };
  }

  getActiveTransport() {
    return this.transportsQuery.selectActive();
  }

  getTransport(transportId: number) {
    const selectTransport$ = this.transportsQuery.selectEntity(transportId);
    return this.transportsQuery.getEntity(transportId)
      ? selectTransport$
      : this.dpl.offerings.getTransportOffering(transportId).pipe(
          tap((transport) =>
            this.transportsStore.upsert(transportId, transport)
          ),
          switchMap(() => selectTransport$)
        );
  }

  placeBid(
    transportId: number,
    request: Omit<TransportOfferingBidCreateRequest, 'divisionId'>
  ) {
    return this.division.getActiveDivision().pipe(
      first(),
      switchMap((division) => {
        const divisionId = division.id;
        return this.dpl.offerings.createTransportOfferingBid(transportId, {
          ...request,
          divisionId,
        });
      }),
      tap((bid) => {
        applyTransaction(() => {
          this.transportsStore.update(transportId, (state) => {
            // cancel existing bids in store
            const bids = state.bids.map((i) => {
              const status = TransportBidStatus.Canceled;
              return <TransportOfferingBid>{
                ...i,
                status,
              };
            });
            // add new bid to store
            return {
              bids: [bid, ...bids],
            };
          });
        });
      })
    );
  }

  cancelBid(transportId: number, bidId: number) {
    return this.dpl.offerings
      .cancelTransportOfferingBid(transportId, bidId)
      .pipe(
        tap((bid) => {
          this.transportsStore.update(transportId, (state) => {
            return {
              bids: arrayUpdate(state.bids, (i) => i.id === bid.id, bid),
            };
          });
        })
      );
  }

  acceptTransport(transportId: number) {
    return this.dpl.offerings.acceptTransportOffering(transportId).pipe(
      tap((transport) => {
        this.transportsStore.update(transportId, transport);
      })
    );
  }

  declineTransport(transportId: number) {
    return this.dpl.offerings.declineTransportOffering(transportId).pipe(
      tap((transport) => {
        this.transportsStore.update(transportId, transport);
      })
    );
  }

  getRoutingLink(fromAddress: Address, toAddress: Address) {
    return this.maps.getRouteLink({
      from: this.addressToGoogleString(fromAddress),
      to: this.addressToGoogleString(toAddress),
    });
  }

  private addressToGoogleString(address: Address) {
    return `${address.street1}, ${
      address.postalCode
    }, ${this.countryPipe.transform(address.country)}`;
  }
}
