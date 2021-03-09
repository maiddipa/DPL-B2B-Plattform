import { Injectable } from '@angular/core';
import { QueryEntity } from '@datorama/akita';
import { AddressesStore, AddressesState } from './addresses.store';

@Injectable({ providedIn: 'root' })
export class AddressesQuery extends QueryEntity<AddressesState> {
  constructor(protected store: AddressesStore) {
    super(store);
  }
}
