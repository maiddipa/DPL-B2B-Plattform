import { Injectable } from '@angular/core';
import { EntityState, EntityStore, StoreConfig } from '@datorama/akita';
import { Address } from 'apps/dpl-live/src/app/core/services/dpl-api-services';

export interface AddressesState extends EntityState<Address, number> {}

@Injectable({ providedIn: 'root' })
@StoreConfig({ name: 'addresses' })
export class AddressesStore extends EntityStore<AddressesState> {
  constructor() {
    super();
  }
}
