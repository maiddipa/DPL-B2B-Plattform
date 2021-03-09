import { Injectable } from '@angular/core';
import { QueryEntity, combineQueries } from '@datorama/akita';
import {
  LoadingLocationsStore,
  LoadingLocationsState,
} from './loading-locations.store';
import { AddressesQuery } from 'apps/dpl-live/src/app/addresses/state/addresses.query';
import { map, publishReplay, refCount } from 'rxjs/operators';
import { ILoadingLocation } from './loading-location.model';
import * as _ from 'lodash';

@Injectable({ providedIn: 'root' })
export class LoadingLocationsQuery extends QueryEntity<LoadingLocationsState> {
  loadingLocationsAsObject$ = this.selectLoadingLocationsAsObject();

  constructor(
    protected store: LoadingLocationsStore,
    private address: AddressesQuery
  ) {
    super(store);
  }

  selectLoadingLocationsAsObject() {
    return combineQueries([
      this.selectAll(),
      this.address.selectAll({
        asObject: true,
      }),
    ]).pipe(
      map(([loadingLocations, addresses]) => {
        if (!loadingLocations) {
          return null;
        }

        return loadingLocations.map((loadingLocation) => {
          return <ILoadingLocation>{
            ...loadingLocation,
            address: addresses[loadingLocation.address],
          };
        });
      }),
      map((loadingLocations) =>
        _(loadingLocations)
          .keyBy((x) => x.id)
          .value()
      ),
      publishReplay(1),
      refCount()
    );
  }
}
