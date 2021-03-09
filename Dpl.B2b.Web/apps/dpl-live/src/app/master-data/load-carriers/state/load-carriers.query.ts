import { Injectable } from '@angular/core';
import { combineQueries, QueryEntity } from '@datorama/akita';
import {
  map,
  publishReplay,
  refCount,
  filter,
  switchMap,
  tap,
} from 'rxjs/operators';

import { LoadCarrierQualitiesQuery } from './load-carrier-qualities.query';
import { LoadCarrierTypesQuery } from './load-carrier-types.query';
import { ILoadCarrier } from './load-carrier.model';
import { LoadCarriersState, LoadCarriersStore } from './load-carriers.store';
import * as _ from 'lodash';

@Injectable({ providedIn: 'root' })
export class LoadCarriersQuery extends QueryEntity<LoadCarriersState> {
  loadCarriers$ = this.getLoadCarriers();
  quantityPerEur$ = this.getQuantityPerEur();

  constructor(
    protected store: LoadCarriersStore,
    private loadCarrierQualitiesQuery: LoadCarrierQualitiesQuery,
    private loadCarrierTypesQuery: LoadCarrierTypesQuery
  ) {
    super(store);
  }

  private getLoadCarriers() {
    return this.selectLoading().pipe(
      filter((loading) => !loading),
      switchMap(() => {
        return combineQueries([
          this.selectAll(),
          this.loadCarrierTypesQuery.selectAll({ asObject: true }),
          this.loadCarrierQualitiesQuery.selectAll({ asObject: true }),
        ]);
      }),
      map(([loadCarriers, types, qualities]) => {
        return _(loadCarriers)
          .map((i) => {
            return <ILoadCarrier>{
              ...i,
              type: types[i.type],
              quality: qualities[i.quality],
            };
          })
          .orderBy([(i) => i.type.order, (i) => i.quality.order])
          .value();
      }),
      publishReplay(1),
      refCount()
    );
  }

  private getQuantityPerEur() {
    return this.loadCarriers$.pipe(
      map((loadCarriers) =>
        _(loadCarriers)
          .keyBy((i) => i.id)
          .mapValues((i) => i.type.quantityPerEur)
          .value()
      )
    );
  }
}
