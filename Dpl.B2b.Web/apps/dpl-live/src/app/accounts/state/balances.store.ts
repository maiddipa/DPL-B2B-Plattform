import { Injectable } from '@angular/core';
import { IBalance } from './balance.model';
import {
  EntityState,
  EntityStore,
  StoreConfig,
  ActiveState,
} from '@datorama/akita';

export interface BalancesState
  extends EntityState<IBalance, string>,
    ActiveState {}

const initialState: BalancesState = {
  active: null,
};

@Injectable({ providedIn: 'root' })
@StoreConfig({ name: 'balances', idKey: 'generatedId' })
export class BalancesStore extends EntityStore<BalancesState> {
  constructor() {
    super(initialState);
  }
}
