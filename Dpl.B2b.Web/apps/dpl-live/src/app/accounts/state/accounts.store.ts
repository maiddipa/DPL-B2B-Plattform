import { Injectable } from '@angular/core';
import { IAccount } from './account.model';
import {
  EntityState,
  EntityStore,
  StoreConfig,
  ActiveState,
  EntityUIStore,
} from '@datorama/akita';
import { AccountOverviewType } from '../services/accounts.service.types';

export interface AccountsState
  extends EntityState<IAccount<string>, number>,
    ActiveState {
  ui: {
    selectorExpanded: boolean;
    accountOverviewType: AccountOverviewType;
  };
}

const initialState: AccountsState = {
  active: null,
  ui: {
    selectorExpanded: true,
    accountOverviewType: AccountOverviewType.Record,
  },
};

@Injectable({ providedIn: 'root' })
@StoreConfig({ name: 'accounts', idKey: 'id', cache: { ttl: 15 * 60 * 1000 } })
export class AccountsStore extends EntityStore<AccountsState> {
  constructor() {
    super(initialState);
  }

  updateSelectorExpanded(selectorExpanded: boolean) {
    this.update({ ui: { ...this.getValue().ui, selectorExpanded } });
  }

  updateAccountOverviewType(accountOverviewType: AccountOverviewType) {
    // remove if not needed
    this.update({ ui: { ...this.getValue().ui, accountOverviewType } });
  }
}
