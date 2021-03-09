import { Injectable } from '@angular/core';
import { QueryEntity } from '@datorama/akita';
import {
  LoadCarrierQualitiesStore,
  LoadCarrierQualitiesState,
} from './load-carrier-qualities.store';

@Injectable({ providedIn: 'root' })
export class LoadCarrierQualitiesQuery extends QueryEntity<
  LoadCarrierQualitiesState
> {
  constructor(protected store: LoadCarrierQualitiesStore) {
    super(store);
  }
}
