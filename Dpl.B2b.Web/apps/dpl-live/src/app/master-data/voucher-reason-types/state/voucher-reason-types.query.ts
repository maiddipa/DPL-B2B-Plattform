import { Injectable } from '@angular/core';
import { QueryEntity } from '@datorama/akita';
import {
  VoucherReasonTypesStore,
  VoucherReasonTypesState,
} from './voucher-reason-types.store';

@Injectable({ providedIn: 'root' })
export class VoucherReasonTypesQuery extends QueryEntity<
  VoucherReasonTypesState
> {
  voucherReasonTypes$ = this.selectAll({
    filterBy: (i) => i.id !== -1,
  });

  constructor(protected store: VoucherReasonTypesStore) {
    super(store);
  }
}
