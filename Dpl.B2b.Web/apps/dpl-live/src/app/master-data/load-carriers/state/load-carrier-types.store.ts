import { Injectable } from '@angular/core';
import { ILoadCarrierType } from './load-carrier-type.model';
import {
  EntityState,
  ActiveState,
  EntityStore,
  StoreConfig,
} from '@datorama/akita';

export interface LoadCarrierTypesState
  extends EntityState<ILoadCarrierType, number>,
    ActiveState<number> {}

@Injectable({ providedIn: 'root' })
@StoreConfig({ name: 'load-carrier-types' })
export class LoadCarrierTypesStore extends EntityStore<LoadCarrierTypesState> {
  constructor() {
    super();
  }
}
