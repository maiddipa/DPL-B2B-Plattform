import { Injectable } from '@angular/core';
import { QueryEntity } from '@datorama/akita';
import { OrderLoadsStore, OrderLoadState } from './order-loads.store';

@Injectable({ providedIn: 'root' })
export class OrderLoadsQuery extends QueryEntity<OrderLoadState> {
  orderLoads$ = this.selectAll();
  constructor(protected store: OrderLoadsStore) {
    super(store);
  }
}
