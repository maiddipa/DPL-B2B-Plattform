import { Injectable } from '@angular/core';
import { QueryEntity, combineQueries, EntityUIQuery } from '@datorama/akita';
import { AccountsStore, AccountsState } from './accounts.store';
import { BalancesQuery } from './balances.query';
import { map, publishReplay, refCount, switchMap } from 'rxjs/operators';
import { IAccount } from './account.model';
import { Observable, of } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class AccountsQuery extends QueryEntity<AccountsState> {
  accounts$ = this.selectAccounts();
  activeAccount$ = this.selectActiveAccount();
  selectorExpanded$ = this.select((state) => state.ui.selectorExpanded);
  // remove if not needed
  accountOverviewType$ = this.select((state) => state.ui.accountOverviewType);

  constructor(
    protected store: AccountsStore,
    private balancesQuery: BalancesQuery
  ) {
    super(store);
  }

  private selectAccounts() {
    return combineQueries([
      this.selectAll(),
      this.balancesQuery.selectAll({ asObject: true }),
    ]).pipe(
      map(([accounts, balances]) => {
        return accounts.map((account) => {
          return <IAccount>{
            ...account,
            loadCarrierTypeBalances: account.loadCarrierTypeBalances.map(
              (id) => balances[id]
            ),
          };
        });
      }),
      publishReplay(1),
      refCount()
    );
  }

  private selectActiveAccount() {
    return combineQueries([
      this.selectActive(),
      this.balancesQuery.selectAll({
        asObject: true,
      }),
    ]).pipe(
      map(([account, balances]) => {
        if (!account) {
          return null;
        }

        return <IAccount>{
          ...account,
          loadCarrierTypeBalances: account.loadCarrierTypeBalances.map(
            (id) => balances[id]
          ),
        };
      }),
      publishReplay(1),
      refCount()
    );
  }
}
