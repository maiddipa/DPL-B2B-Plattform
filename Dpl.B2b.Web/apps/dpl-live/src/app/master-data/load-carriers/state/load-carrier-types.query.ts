import { Injectable } from '@angular/core';
import { QueryEntity } from '@datorama/akita';
import {
  LoadCarrierTypesStore,
  LoadCarrierTypesState,
} from './load-carrier-types.store';

@Injectable({ providedIn: 'root' })
export class LoadCarrierTypesQuery extends QueryEntity<LoadCarrierTypesState> {
  constructor(protected store: LoadCarrierTypesStore) {
    super(store);
  }
}
