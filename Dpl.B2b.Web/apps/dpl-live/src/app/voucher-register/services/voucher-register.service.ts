import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, combineLatest, iif, Observable, of } from 'rxjs';

import { DplApiService } from 'apps/dpl-live/src/app/core/services/dpl-api.service';
import {
  VoucherSummary,
  VouchersSearchRequestSortOptions,
  ListSortDirection,
  VoucherStatus,
  VoucherType,
  PartnerType,
  Voucher,
} from 'apps/dpl-live/src/app/core/services/dpl-api-services';
import {
  Filter,
  FilterValueDateRange,
  FilterValueNumberRange,
} from 'apps/dpl-live/src/app/filters/services/filter.service.types';
import * as moment from 'moment';
import * as _ from 'lodash';
import { ILoadCarrierType } from 'apps/dpl-live/src/app/master-data/load-carriers/state/load-carrier-type.model';
import { DplApiSort } from '../../core/utils';
import { LoadCarrierTypesQuery } from '../../master-data/load-carriers/state/load-carrier-types.query';
import { PaginationResponse } from '@datorama/akita';
import { first, map } from 'rxjs/operators';
import { LocalizationService } from '../../core/services';
import { VoucherRow } from './voucher-register.service.types';
import { LoadCarriersService } from '../../master-data/load-carriers/services/load-carriers.service';
import { ILoadCarrierQuality } from '../../master-data/load-carriers/state/load-carrier-quality.model';
import { ILoadCarrier } from '../../master-data/load-carriers/state/load-carrier.model';
import { CustomersService } from '../../customers/services/customers.service';

type VouchersSearchRequest = Parameters<DplApiService['vouchers']['get']>[0];

export type GetVouchersRequest = {
  filter: ExtendedVoucherFilter;
  page: number;
  limit: number;
  sort: DplApiSort<VouchersSearchRequestSortOptions>;
};

export type VoucherFilter = {
  Type: VoucherType;
  DocumentNumber: string;
  Recipient: string;
  RecipientType: PartnerType;
  IssueDate: FilterValueDateRange;
  States: VoucherStatus[];
  Reasons: number[];
  Shipper: string;
  ValidUntil: FilterValueDateRange;
  Quantity: FilterValueNumberRange;
  Supplier: string;
  DplNote: boolean;
  CustomerReference: string;
};

export interface ExtendedVoucherFilter extends Partial<VoucherFilter> {
  loadCarrierTypeId: number;
  customerId?: number;
}

@Injectable({
  providedIn: 'root',
})
export class VoucherRegisterService {
  constructor(
    private dpl: DplApiService,
    private localizationService: LocalizationService,
    private customersService: CustomersService
  ) {}

  forceRefreshSubject = new BehaviorSubject<boolean>(true);

  getVouchers(
    request: GetVouchersRequest,
    loadCarriers: ILoadCarrier<ILoadCarrierType, ILoadCarrierQuality>[]
  ) {
    const { filter, page, limit, sort } = request;
    const filterConvert = this.convertFilterToSearch(filter);

    const searchRequest: VouchersSearchRequest = {
      ...sort,
      page,
      limit,
      ...filterConvert,
    };
    return this.dpl.vouchers.get(searchRequest).pipe(
      map((data) => {
        const rows = data.data.map((voucher) => {
          const row: VoucherRow = { ...voucher };

          // show only data for active load carrier type in row
          const possibleloadCarrierIdsForType = loadCarriers.filter(
            (x) => x.type.id === filter.loadCarrierTypeId //active loadcarrier type
          );

          // row.positions = row.positions.filter(x =>
          //   possibleloadCarrierIdsForType.find(y => y.id === x.loadCarrierId)
          // );

          row.loadCarrierQualites = _.uniq(
            row.positions
              // positions filtered on active load carrier type before
              .filter((x) =>
                possibleloadCarrierIdsForType.find(
                  (y) => y.id === x.loadCarrierId
                )
              )
              .map(
                (y) =>
                  // only return type - wit #4259
                  `${this.localizationService.getTranslation(
                    'LoadCarrierTypes',
                    filter.loadCarrierTypeId
                  )}`
                // `${this.localizationService.getTranslation(
                //   'LoadCarrierTypes',
                //   result.activeType.id.toString()
                // )} ${this.localizationService.getTranslation(
                //   'LoadCarrierQualities',
                //   result.loadCarriers
                //     .find(x => x.id === y.loadCarrierId)
                //     .quality.id.toString()
                // )}`
              )
          );

          row.loadCarrierQuantity = _.sum(
            row.positions
              // positions filtered on active load carrier type before
              .filter((x) =>
                possibleloadCarrierIdsForType.find(
                  (y) => y.id === x.loadCarrierId
                )
              )
              // tslint:disable-next-line:radix
              .map((p) => (p.quantity ? p.quantity : 0))
          );

          return row;
        });
        return {
          ...data,
          data: rows,
        };
      })
    ) as Observable<PaginationResponse<Voucher>>;
  }

  getVoucherSummary(
    filter: Partial<Omit<ExtendedVoucherFilter, 'loadCarrierTypeId'>>
  ): Observable<VoucherSummary[]> {
    return this.dpl.voucherSummaries.get({
      ...this.convertFilterToSummary(filter),
    });
  }

  convertFilterToSummary(
    filter: Partial<Omit<ExtendedVoucherFilter, 'loadCarrierTypeId'>>
  ): Partial<VouchersSearchRequest> {
    return {
      customerId: filter.customerId ? [filter.customerId] : [],
      type: filter.Type,
      documentNumber: filter.DocumentNumber,
      recipient: filter.Recipient,
      recipientType: filter.RecipientType,
      fromIssueDate: filter.IssueDate ? filter.IssueDate.from : undefined,
      toIssueDate: filter.IssueDate ? filter.IssueDate.to : undefined,
      states: filter.States,
      reasonTypes: filter.Reasons,
      shipper: filter.Shipper,
      validFrom: filter.ValidUntil ? filter.ValidUntil.from : undefined,
      validTo: filter.ValidUntil ? filter.ValidUntil.to : undefined,
      quantityFrom: filter.Quantity ? filter.Quantity.from : undefined,
      quantityTo: filter.Quantity ? filter.Quantity.to : undefined,
      supplier: filter.Supplier,
      hasDplNote: filter.DplNote,
      customerReference: filter.CustomerReference,
    };
  }

  convertFilterToSearch(
    filter: ExtendedVoucherFilter
  ): Partial<VouchersSearchRequest> {
    return {
      customerId: filter.customerId ? [filter.customerId] : [],
      loadCarrierTypes: [filter.loadCarrierTypeId],
      type: filter.Type,
      documentNumber: filter.DocumentNumber,
      recipient: filter.Recipient,
      recipientType: filter.RecipientType,
      fromIssueDate: filter.IssueDate ? filter.IssueDate.from : undefined,
      toIssueDate: filter.IssueDate ? filter.IssueDate.to : undefined,
      states: filter.States,
      reasonTypes: filter.Reasons,
      shipper: filter.Shipper,
      validFrom: filter.ValidUntil ? filter.ValidUntil.from : undefined,
      validTo: filter.ValidUntil ? filter.ValidUntil.to : undefined,
      quantityFrom: filter.Quantity ? filter.Quantity.from : undefined,
      quantityTo: filter.Quantity ? filter.Quantity.to : undefined,
      supplier: filter.Supplier,
      hasDplNote: filter.DplNote,
      customerReference: filter.CustomerReference,
    };
  }

  createVoucherFilterRequest(
    filters: Filter[],
    page?: number,
    limit?: number,
    sortBy?: VouchersSearchRequestSortOptions,
    sortDirection?: ListSortDirection,
    loadCarrierType?: ILoadCarrierType
  ) {
    const filtersDict = _(filters).keyBy((f) => f.propertyName);

    const request: VouchersSearchRequest = {};

    // summary quick filter loadCarrierType
    if (loadCarrierType) {
      request.loadCarrierTypes = [loadCarrierType.id];
    }

    if (filtersDict.has('DocumentNumber')) {
      request.documentNumber = filtersDict
        .get('DocumentNumber')
        .value.find((i) => true);
    }

    if (filtersDict.has('IssueDate')) {
      const dates = filtersDict.get('IssueDate').value;
      if (dates.length > 0) {
        request.fromIssueDate = moment(dates[0]).startOf('day').toDate();
        if (dates[1]) {
          request.toIssueDate = moment(dates[1]).endOf('day').toDate();
        } else {
          request.toIssueDate = moment(dates[0]).endOf('day').toDate();
        }
      }
    }

    if (filtersDict.has('Type') && filtersDict.get('Type').value.length > 0) {
      request.type = VoucherType[filtersDict.get('Type').value[0]];
    }

    if (
      filtersDict.has('States') &&
      filtersDict.get('States').value.length > 0
    ) {
      console.log('States', filtersDict.get('States'));
      // loop selected values an push to states enum array
      request.states = [];
      for (const value of filtersDict.get('States').value) {
        request.states.push(VoucherStatus[value]);
      }
    }

    // loadcarriertypes
    if (
      filtersDict.has('LoadCarrierTypes') &&
      filtersDict.get('LoadCarrierTypes').value.length > 0
    ) {
      console.log('LoadCarrierTypes', filtersDict.get('LoadCarrierTypes'));
      // loop selected values an push to states enum array
      request.loadCarrierTypes = [];
      for (const value of filtersDict.get('LoadCarrierTypes').value) {
        request.loadCarrierTypes.push(parseInt(value));
      }
    }
    // reasons
    if (
      filtersDict.has('Reasons') &&
      filtersDict.get('Reasons').value.length > 0
    ) {
      console.log('Reasons', filtersDict.get('Reasons'));
      // loop selected values an push to states enum array
      request.reasonTypes = [];
      for (const value of filtersDict.get('Reasons').value) {
        // tslint:disable-next-line:radix
        request.reasonTypes.push(parseInt(value));
      }
    }

    // shipper
    if (filtersDict.has('shipper')) {
      request.shipper = filtersDict.get('shipper').value.find((i) => true);
    }
    // Supplier
    if (filtersDict.has('Supplier')) {
      request.supplier = filtersDict.get('Supplier').value.find((i) => true);
    }

    // RecipientType
    if (
      filtersDict.has('RecipientType') &&
      filtersDict.get('RecipientType').value.length > 0
    ) {
      request.recipientType =
        PartnerType[filtersDict.get('RecipientType').value[0]];
    }

    if (filtersDict.has('ValidUntil')) {
      const dates = filtersDict.get('ValidUntil').value;
      if (dates.length > 0) {
        request.validFrom = moment(dates[0]).startOf('day').toDate();
        if (dates[1]) {
          request.validTo = moment(dates[1]).endOf('day').toDate();
        } else {
          request.validTo = moment(dates[0]).endOf('day').toDate();
        }
      }
    }

    if (filtersDict.has('Quantity')) {
      const quantities = filtersDict.get('Quantity').value;
      if (quantities.length > 0) {
        // tslint:disable-next-line: radix
        request.quantityFrom = parseInt(quantities[0]);
        if (quantities[1]) {
          // tslint:disable-next-line:radix
          request.quantityTo = parseInt(quantities[1]);
        } else {
          // tslint:disable-next-line:radix
          request.quantityTo = parseInt(quantities[0]);
        }
      }
    }

    if (filtersDict.has('DplNote')) {
      const dplNoteValue = filtersDict.get('DplNote').value;
      request.hasDplNote = dplNoteValue[0] === 'true';
    }

    if (filtersDict.has('CustomerReference')) {
      request.customerReference = filtersDict
        .get('CustomerReference')
        .value.find((i) => true);
    }

    //   quantityFrom?:number|null|undefined;
    // quantityTo?:number|null|undefined;

    // paging
    request.page = page;
    request.limit = limit;

    // sorting
    request.sortBy = sortBy;
    request.sortDirection = sortDirection;

    return request;
  }
}
