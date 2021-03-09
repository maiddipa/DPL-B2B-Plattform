import { Injectable } from '@angular/core';
import { IVoucherReasonType } from './voucher-reason-type.model';
import { EntityState, EntityStore, StoreConfig } from '@datorama/akita';

export interface VoucherReasonTypesState
  extends EntityState<IVoucherReasonType, number> {}

@Injectable({ providedIn: 'root' })
@StoreConfig({ name: 'voucher-reason-types' })
export class VoucherReasonTypesStore extends EntityStore<
  VoucherReasonTypesState
> {
  constructor() {
    super();
  }
}
