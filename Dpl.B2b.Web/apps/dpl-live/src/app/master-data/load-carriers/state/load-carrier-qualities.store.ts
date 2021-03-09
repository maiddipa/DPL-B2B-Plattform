import { Injectable } from '@angular/core';
import { ILoadCarrierQuality } from './load-carrier-quality.model';
import {
  EntityState,
  ActiveState,
  EntityStore,
  StoreConfig,
} from '@datorama/akita';

export interface LoadCarrierQualitiesState
  extends EntityState<ILoadCarrierQuality, number>,
    ActiveState<number> {}

@Injectable({ providedIn: 'root' })
@StoreConfig({ name: 'load-carrier-qualities' })
export class LoadCarrierQualitiesStore extends EntityStore<
  LoadCarrierQualitiesState
> {
  constructor() {
    super();
  }
}
