import { Injectable } from '@angular/core';

import { CustomerDivisionsQuery } from '../state/customer-divisions.query';
import { switchMap } from 'rxjs/operators';
import { CustomerDivisionsStore } from '../state/customer-divisions.store';

@Injectable({
  providedIn: 'root',
})
export class CustomerDivisionsService {
  constructor(
    private divisionQuery: CustomerDivisionsQuery,
    private customerDivisionsStore: CustomerDivisionsStore
  ) {}

  getDivisions() {
    return this.divisionQuery.divisions$;
  }

  setActive(id: number) {
    this.customerDivisionsStore.setActive(id);
  }

  getActiveDivision() {
    return this.divisionQuery.activeDivision$;
  }
}
