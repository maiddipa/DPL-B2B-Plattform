import { Injectable } from '@angular/core';
import {
  ActiveState,
  EntityState,
  EntityStore,
  StoreConfig,
} from '@datorama/akita';
import { OrganizationScopedDataSet } from '../../core/services/dpl-api-services';

export interface OrganizationsState
  extends EntityState<OrganizationScopedDataSet, number>,
    ActiveState {}

@Injectable({ providedIn: 'root' })
@StoreConfig({
  name: 'customer-administration-organizations'
})
export class OrganizationsStore extends EntityStore<OrganizationsState> {
  constructor() {
    super();
  }
}
