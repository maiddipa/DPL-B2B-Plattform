import { Injectable } from '@angular/core';
import { QueryEntity } from '@datorama/akita';
import {
  AccountingRecordsStore,
  AccountingRecordsState,
} from './accounting-records.store';

@Injectable({ providedIn: 'root' })
export class AccountingRecordsQuery extends QueryEntity<
  AccountingRecordsState
> {
  accoutingRecords$ = this.selectAll();
  constructor(protected store: AccountingRecordsStore) {
    super(store);
  }
}
