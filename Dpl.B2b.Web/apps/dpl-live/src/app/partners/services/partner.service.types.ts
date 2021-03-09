import { DplApiService } from '../../core/services';

export type PartnerPickerContext =
  | 'prefill'
  | 'supplier'
  | 'recipient'
  | 'shipper'
  | 'subShipper';

export type PartnersSearchRequest = Parameters<
  DplApiService['partners']['get']
>[0];
