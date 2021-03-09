import { Injectable } from '@angular/core';
import { QueryEntity } from '@datorama/akita';
import { BalancesStore, BalancesState } from './balances.store';

@Injectable({ providedIn: 'root' })
export class BalancesQuery extends QueryEntity<BalancesState> {
  activeBalance$ = this.selectActive();
  constructor(protected store: BalancesStore) {
    super(store);
  }
}
