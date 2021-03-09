import { Injectable } from '@angular/core';
import { QueryEntity } from '@datorama/akita';
import { VouchersStore, VoucherState } from './vouchers.store';

@Injectable({ providedIn: 'root' })
export class VouchersQuery extends QueryEntity<VoucherState> {
  orderLoads$ = this.selectAll();
  constructor(protected store: VouchersStore) {
    super(store);
  }
}
