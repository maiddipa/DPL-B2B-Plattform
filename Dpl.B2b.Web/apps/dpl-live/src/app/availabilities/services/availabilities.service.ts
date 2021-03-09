import { Injectable } from '@angular/core';
import {
  ListSortDirection,
  OrderGroupsCreateRequest,
  OrderSearchRequestSortOptions,
  OrderStatus,
  OrderTransportType,
  OrderType,
} from '@app/api/dpl';
import { DplApiService } from '@app/core';
import * as _ from 'lodash';
import * as moment from 'moment';

import { Filter } from '../../filters/services/filter.service.types';
import { ILoadingLocation } from '../../loading-locations/state/loading-location.model';
import { LoadingLocationsQuery } from '../../loading-locations/state/loading-locations.query';
import { LoadingLocationsStore } from '../../loading-locations/state/loading-locations.store';
import { OrderSearchRequest } from './availabilies.service.types';
import { CustomerDivisionsService } from '../../customers/services/customer-divisions.service';
import { first, pluck, switchMap } from 'rxjs/operators';

@Injectable({
  providedIn: 'root',
})
export class AvailabilitiesService {
  constructor(
    private dpl: DplApiService,
    private division: CustomerDivisionsService,
    private loadinglocationsquery: LoadingLocationsQuery,
    private loadingLocationStore: LoadingLocationsStore
  ) {}

  getAvailabilities(request: OrderSearchRequest) {
    request.type = [OrderType.Supply];
    return this.dpl.orders.search(request);
  }

  getNeeds(request: OrderSearchRequest) {
    request.type = [OrderType.Demand];
    return this.dpl.orders.search(request);
  }

  setActiveLoadingLocation(location: ILoadingLocation) {
    this.loadingLocationStore.setActive(location.id);
  }

  createAvailability(request: OrderGroupsCreateRequest) {
    const divisionId$ = this.division
      .getActiveDivision()
      .pipe(first(), pluck('id'));

    return divisionId$.pipe(
      switchMap((divisionId) => {
        return this.dpl.orderGroups.post({ ...request, divisionId });
      })
    );
  }

  createAvailabilitiesFilterRequest(
    filters: Filter[],
    page?: number,
    limit?: number,
    sortBy?: OrderSearchRequestSortOptions,
    sortDirection?: ListSortDirection
  ) {
    // LoadingLocationId
    // LoadCarrierId
    // BaseLoadCarrierId
    // todo handling load choice options from store

    // OrderTransportType

    // context specific service

    const filtersDict = _(filters).keyBy((f) => f.propertyName);

    const request: OrderSearchRequest = {};

    if (filtersDict.has('FulfilmentDate')) {
      const dates = filtersDict.get('FulfilmentDate').value;
      if (dates && dates.length > 0) {
        request.earliestFulfillmentDateTime = moment(dates[0]).toDate();
        if (dates[1]) {
          request.latestFulfillmentDateTime = moment(dates[1]).toDate();
        } else {
          request.latestFulfillmentDateTime = moment(dates[0]).toDate();
        }
      }
    }

    if (
      filtersDict.has('Status') &&
      filtersDict.get('Status').value &&
      filtersDict.get('Status').value.length > 0
    ) {
      // loop selected values an push to states enum array
      request.status = [];
      for (const value of filtersDict.get('Status').value) {
        request.status.push(OrderStatus[value]);
      }
    }
    if (
      filtersDict.has('TransportType') &&
      filtersDict.get('TransportType').value &&
      filtersDict.get('TransportType').value.length > 0
    ) {
      // loop selected values an push to states enum array
      request.transportType = [];
      for (const value of filtersDict.get('TransportType').value) {
        request.transportType.push(OrderTransportType[value]);
      }
    }

    if (
      filtersDict.has('LoadCarriers') &&
      filtersDict.get('LoadCarriers').value &&
      filtersDict.get('LoadCarriers').value.length > 0
    ) {
      // loop selected values an push to states enum array
      request.loadCarrierId = [];
      for (const value of filtersDict.get('LoadCarriers').value) {
        request.loadCarrierId.push(parseInt(value));
      }
    }

    if (
      filtersDict.has('BaseLoadCarriers') &&
      filtersDict.get('BaseLoadCarriers').value &&
      filtersDict.get('BaseLoadCarriers').value.length > 0
    ) {
      // loop selected values an push to states enum array
      request.baseLoadCarrierId = [];
      for (const value of filtersDict.get('BaseLoadCarriers').value) {
        request.baseLoadCarrierId.push(parseInt(value));
      }
    }

    // paging
    request.page = page;
    request.limit = limit;

    // sorting
    request.sortBy = sortBy;
    request.sortDirection = sortDirection;

    return request;
  }
}
