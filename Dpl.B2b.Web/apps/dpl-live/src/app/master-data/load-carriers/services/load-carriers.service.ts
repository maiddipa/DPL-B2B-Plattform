import { Injectable } from '@angular/core';
import { switchMap, first, map } from 'rxjs/operators';

import { LoadCarriersQuery } from '../state/load-carriers.query';
import { LoadCarrierTypesQuery } from '../state/load-carrier-types.query';
import { LoadCarrierTypesStore } from '../state/load-carrier-types.store';

@Injectable({ providedIn: 'root' })
export class LoadCarriersService {
  constructor(
    private loadCarriersQuery: LoadCarriersQuery,
    private loadCarrierTypesQuery: LoadCarrierTypesQuery,
    private loadCarrierTypesStore: LoadCarrierTypesStore
  ) {}

  getLoadCarriers() {
    return this.loadCarriersQuery.loadCarriers$;
  }

  getLoadCarrierTypes() {
    return this.loadCarrierTypesQuery.selectAll();
  }

  getActiveLoadCarrierType() {
    return this.loadCarrierTypesQuery.selectActive();
  }

  setActiveLoadCarrierType(id: number) {
    this.loadCarrierTypesStore.setActive(id);
  }

  getLoadCarrierById(loadCarrierId: number) {
    return this.loadCarriersQuery.selectEntity(loadCarrierId);
  }

  getQuantityPerEur(loadCarrierId: number) {
    return this.loadCarriersQuery.quantityPerEur$.pipe(
      map((dict) => {
        if (!dict[loadCarrierId]) {
          return null;
        }
        return dict[loadCarrierId];
      })
    );
  }
}
