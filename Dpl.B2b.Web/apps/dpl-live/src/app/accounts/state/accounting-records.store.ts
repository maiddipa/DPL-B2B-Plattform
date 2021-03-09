import { Injectable } from '@angular/core';
import { EntityState, EntityStore, StoreConfig } from '@datorama/akita';
import { AccountingRecord } from './accounting-record.model';
import {
  AccountingRecordsSearchRequestSortOptions,
  ListSortDirection,
} from '@app/api/dpl';

export interface AccountingRecordsState
  extends EntityState<AccountingRecord, number> {
  page: number;
  limit: number;
  sortOption: AccountingRecordsSearchRequestSortOptions;
  sortDirection: ListSortDirection;
}

@Injectable({ providedIn: 'root' })
@StoreConfig({ name: 'accounting-records' })
export class AccountingRecordsStore extends EntityStore<
  AccountingRecordsState
> {
  constructor() {
    super();
  }

  akitaPreAddEntity(record: AccountingRecord) {
    record.inQuantity = record.quantity > 0 ? record.quantity : null;
    record.outQuantity = record.quantity < 0 ? Math.abs(record.quantity) : null;
    return record;
  }
}
