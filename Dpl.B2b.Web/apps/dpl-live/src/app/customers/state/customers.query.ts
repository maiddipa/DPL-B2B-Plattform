import { Injectable } from '@angular/core';
import { QueryEntity } from '@datorama/akita';
import { CustomersStore, CustomersState } from './customers.store';

@Injectable({ providedIn: 'root' })
export class CustomersQuery extends QueryEntity<CustomersState> {
  // TODO implement combine query for customer
  customers$ = this.selectAll();
  activeCustomers$ = this.selectActive();

  constructor(protected store: CustomersStore) {
    super(store);
  }
}
