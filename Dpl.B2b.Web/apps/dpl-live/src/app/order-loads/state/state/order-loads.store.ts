import { Injectable } from '@angular/core';
import {
  ListSortDirection,
  OrderLoad,
  OrderLoadSearchRequestSortOptions,
} from '@app/api/dpl';
import { EntityState, EntityStore, StoreConfig } from '@datorama/akita';

export interface OrderLoadState extends EntityState<OrderLoad, number> {
  page: number;
  limit: number;
  sortOption: OrderLoadSearchRequestSortOptions;
  sortDirection: ListSortDirection;
}

@Injectable({ providedIn: 'root' })
@StoreConfig({ name: 'order-load' })
export class OrderLoadsStore extends EntityStore<OrderLoadState> {
  constructor() {
    super();
  }
}
