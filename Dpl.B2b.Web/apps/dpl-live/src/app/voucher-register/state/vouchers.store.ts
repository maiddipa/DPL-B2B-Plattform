import { Injectable } from '@angular/core';

import { EntityState, EntityStore, StoreConfig } from '@datorama/akita';
import {
  ListSortDirection,
  VouchersSearchRequestSortOptions,
} from '../../core/services/dpl-api-services';
import { VoucherRow } from '../services/voucher-register.service.types';

export interface VoucherState extends EntityState<VoucherRow, number> {
  page: number;
  limit: number;
  sortOption: VouchersSearchRequestSortOptions;
  sortDirection: ListSortDirection;
}

@Injectable({ providedIn: 'root' })
@StoreConfig({ name: 'voucher' })
export class VouchersStore extends EntityStore<VoucherState> {
  constructor() {
    super();
  }
}
