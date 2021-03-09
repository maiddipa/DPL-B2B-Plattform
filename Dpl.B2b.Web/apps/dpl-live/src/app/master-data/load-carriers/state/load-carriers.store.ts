import { Injectable } from '@angular/core';
import { ILoadCarrier } from './load-carrier.model';
import {
  EntityState,
  ActiveState,
  EntityStore,
  StoreConfig,
} from '@datorama/akita';

export interface LoadCarriersState
  extends EntityState<ILoadCarrier<number, number>, number>,
    ActiveState<number> {}

@Injectable({ providedIn: 'root' })
@StoreConfig({ name: 'load-carriers' })
export class LoadCarriersStore extends EntityStore<LoadCarriersState> {
  constructor() {
    super();
  }
}
