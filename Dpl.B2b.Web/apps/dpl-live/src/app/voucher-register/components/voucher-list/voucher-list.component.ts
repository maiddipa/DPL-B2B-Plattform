import {
  animate,
  state,
  style,
  transition,
  trigger,
} from '@angular/animations';
import { Component, OnInit, Input } from '@angular/core';
import {
  IPaginationResultOfVoucher,
  LoadCarrier,
  PartnerType,
  Voucher,
  VouchersSearchRequestSortOptions,
  VoucherType,
  UserRole,
  ListSortDirection,
} from '@app/api/dpl';
import {
  getMatSort,
  PrintService,
  TableColumnType,
  TextAlignment,
} from '@app/shared';
import * as _ from 'lodash';
import { combineLatest, Observable, of } from 'rxjs';

import { DplApiService } from '../../../core/services/dpl-api.service';

import { LoadCarriersService } from '../../../master-data/load-carriers/services/load-carriers.service';
import {
  ExtendedVoucherFilter,
  VoucherFilter,
  VoucherRegisterService,
} from '../../services/voucher-register.service';
import { VoucherRow } from '../../services/voucher-register.service.types';
import {
  FilteredTableRequestData,
  FilterTableExtendFilterFn,
  FilterTableGetDataFn,
} from '../../../filters/components/filtered-table/filtered-table.component';

import { VouchersQuery } from '../../state/vouchers.query';
import { ILoadCarrier } from '../../../master-data/load-carriers/state/load-carrier.model';
import { ILoadCarrierType } from '../../../master-data/load-carriers/state/load-carrier-type.model';
import { ILoadCarrierQuality } from '../../../master-data/load-carriers/state/load-carrier-quality.model';
import { map, pluck } from 'rxjs/operators';
import { filterNil } from '@datorama/akita';
import { CustomersService } from '../../../customers/services/customers.service';
import { CustomerDivisionsService } from '../../../customers/services/customer-divisions.service';

interface ViewData {
  result: IPaginationResultOfVoucher;
  rows: VoucherRow[];
  pageIndex?: number;
  length?: number;
  pageSize?: number;
  sortActive: string;
  sortDirection: string;
  error: string;
  loadCarriers: LoadCarrier[];
  listFiltered?: boolean;
  displayedColumns: string[];
}

@Component({
  selector: 'voucher-list',
  templateUrl: './voucher-list.component.html',
  styleUrls: ['./voucher-list.component.scss'],
  animations: [
    trigger('detailExpand', [
      state('collapsed', style({ height: '0px', minHeight: '0' })),
      state('expanded', style({ height: '*' })),
      transition(
        'expanded <=> collapsed',
        animate('225ms cubic-bezier(0.4, 0.0, 0.2, 1)')
      ),
    ]),
  ],
})
export class VoucherListComponent implements OnInit {
  @Input() userRole: UserRole;
  @Input() isDplEmployee: boolean;
  @Input() activeLoadCarrierType: ILoadCarrierType;
  @Input() loadCarriers: ILoadCarrier<ILoadCarrierType, ILoadCarrierQuality>[];

  public ColumnType = TableColumnType;
  public TextAlignment = TextAlignment;

  context = 'vouchers';
  sort = getMatSort<VouchersSearchRequestSortOptions>({
    sortBy: VouchersSearchRequestSortOptions.IssuanceDate,
    sortDirection: ListSortDirection.Descending,
  });
  viewData$: Observable<ViewData>;
  expandedVoucher: Voucher | null;

  sortOptions = VouchersSearchRequestSortOptions;
  recipientType = PartnerType;
  voucherType = VoucherType;

  getDataFn: FilterTableGetDataFn<
    VoucherRow,
    VoucherFilter,
    VouchersSearchRequestSortOptions
  >;

  extendFilterFn: FilterTableExtendFilterFn<
    VoucherFilter,
    ExtendedVoucherFilter
  >;

  displayedColumns = ['number'];

  status: any;

  constructor(
    public voucherRegisterService: VoucherRegisterService,
    private loadCarriersService: LoadCarriersService,
    private dpl: DplApiService,
    private printService: PrintService,
    public query: VouchersQuery,
    public customersService: CustomersService
  ) {
    this.getDataFn = this.getData.bind(this);
    this.extendFilterFn = this.extendFilter.bind(this);
  }

  ngOnInit() {
    this.displayedColumns =
      this.userRole === UserRole.Shipper
        ? [
            'issuedDate',
            'documentNumber',
            'issuerCompanyName',
            'supplier',
            'recipientType',
            'reasonType',
            'voucherType',
            'status',
            'loadCarrierQualites',
            'loadCarrierQuantity',
            'customerReference',
            'validUntil',
            'cancel',
            'chat',
            'hasDplNote',
          ]
        : [
            'issuedDate',
            'documentNumber',
            'shipper',
            'supplier',
            'recipientType',
            'reasonType',
            'voucherType',
            'status',
            'loadCarrierQualites',
            'loadCarrierQuantity',
            'customerReference',
            'validUntil',
            'cancel',
            'chat',
            'hasDplNote',
          ];
  }

  private getData(
    data: FilteredTableRequestData<
      ExtendedVoucherFilter,
      VouchersSearchRequestSortOptions
    >
  ) {
    return this.voucherRegisterService.getVouchers(
      {
        ...data,
      },
      this.loadCarriers
    );
  }

  private extendFilter(data: VoucherFilter): Observable<ExtendedVoucherFilter> {
    const customerId$ = this.isDplEmployee
      ? this.customersService.getActiveCustomer().pipe(filterNil, pluck('id'))
      : of(undefined);

    const loadCarrierTypeId$ = this.loadCarriersService
      .getActiveLoadCarrierType()
      .pipe(filterNil, pluck('id'));

    return combineLatest([customerId$, loadCarrierTypeId$]).pipe(
      map(([customerId, loadCarrierTypeId]) => {
        const extendedFilter: ExtendedVoucherFilter = {
          ...data,
          customerId,
          loadCarrierTypeId,
        };
        console.log(extendedFilter);
        return extendedFilter;
      })
    );
  }

  getLoadCarrierById(id) {
    return this.loadCarriersService.getLoadCarrierById(id);
  }

  openDocument(voucher: VoucherRow) {
    this.dpl.documents
      .getDocumentDownload(voucher.documentId)
      .subscribe((url) => {
        this.printService.printUrl(url, false);
      });
  }
}
