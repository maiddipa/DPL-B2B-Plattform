import { Injectable } from '@angular/core';
import {
  ActiveState,
  EntityState,
  EntityStore,
  StoreConfig,
} from '@datorama/akita';
import { CustomerAdminOrganizationUnit } from '../services/customer-administration.service.types';

export interface UnitsState
  extends EntityState<CustomerAdminOrganizationUnit, number>,
    ActiveState {}

@Injectable({ providedIn: 'root' })
@StoreConfig({ name: 'customer-administration-units', idKey: 'idString' })
export class UnitsStore extends EntityStore<UnitsState> {
  constructor() {
    super();
  }

  updateUnitName(idString, name: string) {
    this.update(idString, { name });
  }
}
