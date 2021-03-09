import {
  VoucherAggregate,
  VoucherType,
  Voucher,
  VouchersSearchRequestSortOptions,
  VoucherStatus,
  ListSortDirection,
  PartnerType,
} from 'apps/dpl-live/src/app/core/services/dpl-api-services';
import { DplApiService } from '@app/core';

export interface Summary {
  carrierTypeId: number;
  issuedCount: number;
  submittedCount: number;
  accountedCount: number;
  canceledCount: number;
  expiredCount: number;
  issuedSum: number;
  submittedSum: number;
  accountedSum: number;
  canceledSum: number;
  expiredSum: number;
  tooltip: string;
}

export enum VoucherSummaryType {
  Sum = 'Sum',
  Count = 'Count',
}

export interface VoucherRegisterSum {
  issued: number;
  submitted: number;
  accounted: number;
  canceled: number;
  expired: number;
}

export interface VoucherRow extends Voucher {
  loadCarrierTypes?: string[];
  loadCarrierQuantity?: number;
  loadCarrierQualites?: string[];
}
