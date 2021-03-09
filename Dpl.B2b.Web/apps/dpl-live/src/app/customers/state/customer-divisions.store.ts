import { Injectable } from '@angular/core';
import { ICustomerDivision } from './customer-division.model';
import {
  EntityState,
  ActiveState,
  EntityStore,
  StoreConfig,
} from '@datorama/akita';

export interface CustomerDivisionsState
  extends EntityState<ICustomerDivision<number>, number>,
    ActiveState<number> {}

@Injectable({ providedIn: 'root' })
@StoreConfig({ name: 'customer-divisions' })
export class CustomerDivisionsStore extends EntityStore<
  CustomerDivisionsState
> {
  constructor() {
    super();
  }
}
