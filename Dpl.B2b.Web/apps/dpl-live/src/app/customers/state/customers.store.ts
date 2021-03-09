import { Injectable } from '@angular/core';
import { ICustomer } from './customer.model';
import {
  EntityState,
  ActiveState,
  EntityStore,
  StoreConfig,
} from '@datorama/akita';

export interface CustomersState
  extends EntityState<ICustomer<number>, number>,
    ActiveState<number> {}

@Injectable({ providedIn: 'root' })
@StoreConfig({ name: 'customers' })
export class CustomersStore extends EntityStore<CustomersState> {
  constructor() {
    super();
  }
}
