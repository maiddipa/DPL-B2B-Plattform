import { Component, Input, OnInit } from '@angular/core';
import { LoadCarrierPipe } from '@app/shared';
import { filterNil } from '@datorama/akita';
import { LoadingService } from '@dpl/dpl-lib';
import { LoadCarriersService } from 'apps/dpl-live/src/app/master-data/load-carriers/services/load-carriers.service';
import { sum } from 'lodash';
import { combineLatest, Observable, of } from 'rxjs';
import { map, pluck, switchMap } from 'rxjs/operators';

import { CustomersService } from '../../../customers/services/customers.service';
import { FilterService } from '../../../filters/services/filter.service';
import {
  VoucherFilter,
  VoucherRegisterService,
} from '../../services/voucher-register.service';
import {
  Summary,
  VoucherRegisterSum,
} from '../../services/voucher-register.service.types';

interface ViewData {
  aggregates: Summary[];
  total: VoucherRegisterSum;
  loadCarrierTypeIds: number[];
  activeType: number;
}
@Component({
  selector: 'voucher-summary',
  templateUrl: './voucher-summary.component.html',
  styleUrls: ['./voucher-summary.component.scss'],
})
export class VoucherSummaryComponent implements OnInit {
  @Input() isDplEmployee: boolean;

  displayedColumns2: string[] = [
    'carrierTypeId',
    'issued',
    // 'submitted',
    'accounted',
    'canceled',
    'expired',
    // "total"
  ];

  viewData$: Observable<ViewData>;

  constructor(
    private filterService: FilterService,
    private voucherRegisterService: VoucherRegisterService,
    private loadCarriersService: LoadCarriersService,
    private loadCarrierPipe: LoadCarrierPipe,
    private loadingService: LoadingService,
    private customersService: CustomersService
  ) {}

  ngOnInit() {
    const customerId$ = this.isDplEmployee
      ? this.customersService.getActiveCustomer().pipe(filterNil, pluck('id'))
      : of(undefined);

    const filter$ = combineLatest([
      customerId$,
      this.filterService.getActiveFilter<VoucherFilter>('vouchers'),
    ]).pipe(
      map(([customerId, filter]) => {
        return {
          ...filter,
          customerId,
        };
      })
    );

    const items$ = filter$.pipe(
      switchMap((filter) => {
        return this.voucherRegisterService
          .getVoucherSummary(filter)
          .pipe(this.loadingService.showLoadingWhile());
      })
    );

    const activeType$ = this.loadCarriersService.getActiveLoadCarrierType();

    this.viewData$ = combineLatest([items$, activeType$]).pipe(
      map(([items, activeType]) => {
        const aggregates = items.map((dto) => {
          const summaryDto: Summary = {
            tooltip: this.loadCarrierPipe.transform(
              dto.loadCarrierTypeId,
              'type',
              'LongName'
            ),
            carrierTypeId: dto.loadCarrierTypeId,
            issuedCount: dto.count.issued ? dto.count.issued : 0,
            submittedCount: dto.count.submitted ? dto.count.submitted : 0,
            accountedCount: dto.count.accounted ? dto.count.accounted : 0,
            canceledCount: dto.count.canceled ? dto.count.canceled : 0,
            expiredCount: dto.count.expired ? dto.count.expired : 0,
            issuedSum: dto.sum.issued ? dto.sum.issued : 0,
            submittedSum: dto.sum.submitted ? dto.sum.submitted : 0,
            accountedSum: dto.sum.accounted ? dto.sum.accounted : 0,
            canceledSum: dto.sum.canceled ? dto.sum.canceled : 0,
            expiredSum: dto.sum.expired ? dto.sum.expired : 0,
          };
          return summaryDto;
        });

        const total = {
          accounted: sum(aggregates.map((x) => x.accountedSum)),
          canceled: sum(aggregates.map((x) => x.canceledSum)),
          expired: sum(aggregates.map((x) => x.expiredSum)),
          issued: sum(aggregates.map((x) => x.issuedSum)),
          submitted: sum(aggregates.map((x) => x.submittedSum)),
        };

        const loadCarrierTypeIds = aggregates.map((x) => x.carrierTypeId);

        const viewData: ViewData = {
          aggregates,
          total,
          loadCarrierTypeIds,
          activeType: activeType ? activeType.id : null,
        };
        return viewData;
      })
    );
  }

  selectLoadCarrierType(value: number) {
    this.loadCarriersService.setActiveLoadCarrierType(value);
  }
}
