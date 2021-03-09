import { Injectable } from '@angular/core';
import {
  EntityState,
  ActiveState,
  EntityStore,
  StoreConfig,
} from '@datorama/akita';
import { Order } from '@app/api/dpl';

export interface OrdersState
  extends EntityState<Order, number>,
    ActiveState<number> {}

@Injectable({ providedIn: 'root' })
@StoreConfig({ name: 'orders' })
export class OrdersStore extends EntityStore<OrdersState> {
  constructor() {
    super();
  }
}
