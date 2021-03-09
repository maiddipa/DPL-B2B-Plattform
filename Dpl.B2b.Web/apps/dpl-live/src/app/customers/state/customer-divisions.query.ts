import { Injectable } from '@angular/core';
import { combineQueries, QueryEntity, selectPersistStateInit } from '@datorama/akita';
import { map, publishReplay, refCount } from 'rxjs/operators';
import { LoadingLocationsQuery } from 'apps/dpl-live/src/app/loading-locations/state/loading-locations.query';

import { ICustomerDivision } from './customer-division.model';
import {
  CustomerDivisionsState,
  CustomerDivisionsStore,
} from './customer-divisions.store';

@Injectable({ providedIn: 'root' })
export class CustomerDivisionsQuery extends QueryEntity<
  CustomerDivisionsState
> {
  divisions$ = this.selectDivisions();
  activeDivision$ = this.selectActiveDivision();

  constructor(
    protected store: CustomerDivisionsStore,
    private loadingLocationsQuery: LoadingLocationsQuery
  ) {
    super(store);
  }

  private selectDivisions() {
    return combineQueries([
      this.selectAll(),
      this.loadingLocationsQuery.loadingLocationsAsObject$,
    ]).pipe(
      map(([divisions, loadingLocations]) => {
        return divisions.map((division) => {
          if (!division) {
            return null;
          }

          return <ICustomerDivision>{
            ...division,
            loadingLocations: division.loadingLocations.map(
              (id) => loadingLocations[id]
            ),
          };
        });
      }),
      publishReplay(1),
      refCount()
    );
  }

  private selectActiveDivision() {    
    return combineQueries([
      this.selectActive(),
      this.loadingLocationsQuery.loadingLocationsAsObject$,
      selectPersistStateInit()
    ]).pipe(
      map(([division, loadingLocations]) => {
        if (!division) {
          return null;
        }

        return <ICustomerDivision>{
          ...division,
          loadingLocations: division.loadingLocations.map(
            (id) => loadingLocations[id]
          ),
        };
      }),
      publishReplay(1),
      refCount()
    );
  }
}
