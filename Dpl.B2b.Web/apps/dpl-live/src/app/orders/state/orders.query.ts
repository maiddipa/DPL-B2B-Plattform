import { Injectable } from '@angular/core';
import { QueryEntity } from '@datorama/akita';
import { OrdersStore, OrdersState } from './orders.store';

@Injectable({ providedIn: 'root' })
export class OrdersQuery extends QueryEntity<OrdersState> {
  orders$ = this.selectAll();
  constructor(protected store: OrdersStore) {
    super(store);
  }
}
