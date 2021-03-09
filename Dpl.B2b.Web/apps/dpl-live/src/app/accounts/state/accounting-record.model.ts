import { AccountingRecord as AccountingRecordApi } from '@app/api/dpl';

export interface AccountingRecord extends AccountingRecordApi {
  inQuantity?: number | null;
  outQuantity?: number | null;
}
