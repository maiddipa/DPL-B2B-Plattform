import { Injectable } from '@angular/core';
import { ILoadingLocation } from './loading-location.model';
import {
  EntityState,
  ActiveState,
  EntityStore,
  StoreConfig,
} from '@datorama/akita';

export interface LoadingLocationsState
  extends EntityState<ILoadingLocation<number>, number>,
    ActiveState<number> {}

@Injectable({ providedIn: 'root' })
@StoreConfig({ name: 'loading-locations' })
export class LoadingLocationsStore extends EntityStore<LoadingLocationsState> {
  constructor() {
    super();
  }
}
