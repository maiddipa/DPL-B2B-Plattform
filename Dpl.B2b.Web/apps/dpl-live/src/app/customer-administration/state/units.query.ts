import { Injectable } from '@angular/core';
import { QueryEntity } from '@datorama/akita';
import { CustomerAdminScope } from '../services/customer-administration.service.types';
import { UnitsStore, UnitsState } from './units.store';

@Injectable({
  providedIn: 'root',
})
export class UnitsQuery extends QueryEntity<UnitsState> {
  constructor(protected store: UnitsStore) {
    super(store);
  }

  selectOnlyPhysicalUnits() {
    return this.selectAll({
      filterBy: (unit) =>
        unit.scope === CustomerAdminScope.Organization ||
        unit.scope === CustomerAdminScope.Customer ||
        unit.scope === CustomerAdminScope.Division ||
        unit.scope === CustomerAdminScope.LoadingLocation,
    });
  }
}
