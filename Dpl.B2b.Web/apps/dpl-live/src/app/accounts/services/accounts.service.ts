import { Injectable } from '@angular/core';
import { ɵINTERNAL_BROWSER_PLATFORM_PROVIDERS } from '@angular/platform-browser';
import {
  AccountingRecord,
  LoadCarrierQualityType,
  PostingAccountCondition,
} from '@app/api/dpl';
import { DplApiService, DplApiSort } from '@app/core';
import { combineQueries, PaginationResponse } from '@datorama/akita';
import * as _ from 'lodash';
import { combineLatest, Observable } from 'rxjs';
import { map } from 'rxjs/operators';

import {
  FilterValueDateRange,
  FilterValueNumberRange,
} from '../../filters/services/filter.service.types';
import { LoadCarriersService } from '../../master-data/load-carriers/services/load-carriers.service';
import { UserService } from '../../user/services/user.service';
import { AccountsQuery } from '../state/accounts.query';
import { AccountsStore } from '../state/accounts.store';
import { IBalance } from '../state/balance.model';
import { BalancesQuery } from '../state/balances.query';
import { BalancesStore } from '../state/balances.store';
import {
  AccountingRecordsSearchRequest,
  AccountOverviewType,
  ActiveAccountLoadCarrierType,
  ILoadCarrierOrderOption,
  IOrderOptions,
  ISelfServiceData,
} from './accounts.service.types';

export type GetAccountingRecordsRequest = {
  filter: ExtendedAccountingRecordFilter;
  page: number;
  limit: number;
};

export interface AccountingRecordFilter {
  AccountDate: FilterValueDateRange;
  Credit: FilterValueNumberRange;
  Charge: FilterValueNumberRange;
  Description: string;
}

export interface ExtendedAccountingRecordFilter
  extends Partial<AccountingRecordFilter> {
  postingAccountId: number;
  loadCarrierTypeId: number;
}

@Injectable({ providedIn: 'root' })
export class AccountsService {
  constructor(
    private accountsStore: AccountsStore,
    private accountsQuery: AccountsQuery,
    private balancesStore: BalancesStore,
    private balancesQuery: BalancesQuery,
    private loadCarrierService: LoadCarriersService,
    private dpl: DplApiService,
    private user: UserService
  ) {}

  getAllAccounts() {
    return this.accountsQuery.accounts$;
  }

  getAllowedDestinationAccounts() {
    return this.dpl.postingAccounts.getAllowedDestinationAccounts();
  }

  setActiveAccount(id?: number) {
    const currentActiveId = this.accountsQuery.getActiveId();

    if (currentActiveId === id) {
      return;
    }

    if (id) {
      this.user.updateAccountBalance(id).subscribe();
    }

    this.accountsStore.setActive(id);

    // select first loadCarrierType from Account
    const active = this.accountsQuery.getActive();
    if (active && active.loadCarrierTypeIds.length > 0) {
      this.loadCarrierService.setActiveLoadCarrierType(
        active.loadCarrierTypeIds[0]
      );
    } else {
      this.loadCarrierService.setActiveLoadCarrierType(null);
    }
  }

  getActiveAccount() {
    return this.accountsQuery.activeAccount$;
  }

  setActiveBalance(generatedId?: string) {
    this.balancesStore.setActive(generatedId);
  }

  getActiveBalance() {
    return this.balancesQuery.activeBalance$;
  }

  getBalanceForLoadCarrierType(loadCarrierTypeId: number) {
    return this.getActiveAccount().pipe(
      map((account) => {
        if (account && account.loadCarrierTypeBalances) {
          return account.loadCarrierTypeBalances.find(
            (x) => x.loadCarrierTypeId === loadCarrierTypeId
          );
        }
        return null;
      })
    );
  }

  getBalanceForLoadCarrier(loadCarrierId: number) {
    return this.getActiveAccount().pipe(
      map((account) => {
        if (account && account.balances) {
          return account.balances.find(
            (x) => x.loadCarrierId === loadCarrierId
          );
        }
        return null;
      })
    );
  }

  getBalanceForLoadCarrierTypes(loadCarrierTypeIds: number[]) {
    return this.getActiveAccount().pipe(
      map((account) => {
        return account.loadCarrierTypeBalances.filter((x) =>
          loadCarrierTypeIds.find((y) => y === x.loadCarrierTypeId)
        );
      })
    );
  }

  getSelfServiceInfo() {
    const loadCarriers$ = this.loadCarrierService.getLoadCarriers();
    return combineLatest([
      loadCarriers$,
      this.getActiveAccount(),
      this.getActiveBalance(),
    ]).pipe(
      map(([loadCarriers, account, balance]) => {
        const loadCarriersDict = _(loadCarriers)
          .keyBy((i) => i.id)
          .value();
        // grab the balance for quality type 'Intakt' of current article
        const selectedBalance =
          account != null && balance != null
            ? account.loadCarrierTypeBalances.find(
                (b) =>
                  b.loadCarrierTypeId === balance.loadCarrierTypeId &&
                  b.qualityType === LoadCarrierQualityType.Intact
              )
            : null;

        if (selectedBalance) {
          const filterByLoadCarrierTypeId = (
            condition: PostingAccountCondition
          ) => {
            return (
              loadCarriersDict[condition.loadCarrierId].type.id ===
              balance.loadCarrierTypeId
            );
          };

          const pickup =
            selectedBalance.provisionalBalance > 0
              ? account.pickupConditions.filter(filterByLoadCarrierTypeId)
              : [];
          const dropoff = account.dropoffConditions.filter(
            filterByLoadCarrierTypeId
          );
          const all = _([...pickup, ...dropoff])
            .map((i) => i.loadCarrierId)
            .uniq()
            .map((id) => loadCarriersDict[id])
            .value();

          return <ISelfServiceData>{
            account,
            balance: selectedBalance,
            loadCarriers: {
              // merge unique list of pallets
              all,
              pickup,
              dropoff,
            },
          };
        }

        return <ISelfServiceData>{
          account,
          balance: null,
          loadCarriers: {
            all: [],
            pickup: [],
            dropoff: [],
          },
        };
      })
    );
  }

  getLoadCarrierOrderOptions() {
    return combineLatest([
      this.loadCarrierService.getLoadCarriers(),
      this.getAllAccounts(),
    ]).pipe(
      map(([loadCarriers, accounts]) => {
        const loadCarriersDict = _(loadCarriers)
          .keyBy((i) => i.id)
          .value();

        const supply = _(accounts)
          .map((account) => {
            const conditionsDict = _(account.dropoffConditions)
              .keyBy((i) => i.loadCarrierId)
              .value();
            return loadCarriers
              .filter((i) => !!conditionsDict[i.id])
              .map((loadCarrier) => {
                return {
                  loadCarrierId: loadCarrier.id,
                  account,
                  orderCondition: conditionsDict[loadCarrier.id],
                };
              });
          })
          .flatten()
          .groupBy((i) => i.loadCarrierId)
          .map((g, loadCarrierId) => {
            return {
              loadCarrier: loadCarriersDict[loadCarrierId],
              accountInfos: g.map(({ account, orderCondition }) => ({
                account,
                min: orderCondition.minQuantity
                  ? orderCondition.minQuantity
                  : 0,
                max: orderCondition.maxQuantity
                  ? orderCondition.maxQuantity
                  : 100000, // no max condition, for infinity 100000
              })),
            } as ILoadCarrierOrderOption;
          })
          .value();

        const demand = _(accounts)
          .map((account) => {
            const conditionsDict = _(account.pickupConditions)
              .keyBy((i) => i.loadCarrierId)
              .value();
            return account.balances
              .filter(
                (i) => conditionsDict[i.loadCarrierId] // HACK remove balance filter && i.provisionalBalance > 0
              )
              .map((balance) => {
                // console.log(
                //   balance.loadCarrierId,
                //   account,
                //   balance,
                //   conditionsDict[balance.loadCarrierId]
                // );
                return {
                  loadCarrierId: balance.loadCarrierId,
                  account,
                  balance,
                  orderCondition: conditionsDict[balance.loadCarrierId],
                };
              });
          })
          .flatten()
          .groupBy((i) => i.loadCarrierId)
          .map((g, loadCarrierId) => {
            return {
              loadCarrier: loadCarriersDict[loadCarrierId],
              accountInfos: g
                .map(({ account, balance, orderCondition }) => ({
                  account,
                  min: orderCondition.minQuantity
                    ? orderCondition.minQuantity
                    : 0,
                  // max: 1000,
                  // HACK(removed) always ensure max is 1000 not based on order condition or balance
                  max: Math.min(
                    orderCondition.maxQuantity
                      ? orderCondition.maxQuantity
                      : 100000, // no max condition, for infinity 100000
                    balance.availableBalance
                  ),
                }))
                // handle edge case when balance drives down max belwo min
                .filter((i) => i.min <= i.max),
            } as ILoadCarrierOrderOption;
          })
          // handles edge case where balances causes initially orderable load carrier to become non orderable
          .filter((i) => i.accountInfos.length > 0)
          .value();

        return {
          supply,
          demand,
        } as IOrderOptions;
      })
    );
  }

  getAvailableLoadCarrierTypes() {
    return this.accountsQuery.activeAccount$.pipe(
      map((account) => {
        return account ? account.loadCarrierTypeIds : [];
      })
    );
  }

  getBalancesForActiveCarrierType() {
    return combineQueries([
      this.accountsQuery.activeAccount$,
      this.loadCarrierService.getActiveLoadCarrierType(),
      this.getAccountOverviewType(), // remove if not needed
    ]).pipe(
      map(([account, activeLoadCarrierType, accountOverviewType]) => {
        // check account & activeLoadCarrierType & accountOverviewType
        if (
          account &&
          account.loadCarrierTypeBalances &&
          account.loadCarrierTypeBalances.length > 0 &&
          activeLoadCarrierType
        ) {
          return account.loadCarrierTypeBalances.filter(
            (x) => x.loadCarrierTypeId === activeLoadCarrierType.id
          );
        }
        return [] as IBalance[];
      })
    );
  }

  getActiveAccountAndLoadCarrierType(): Observable<
    ActiveAccountLoadCarrierType
  > {
    return combineQueries([
      this.accountsQuery.activeAccount$,
      this.loadCarrierService.getActiveLoadCarrierType(),
      this.getAccountOverviewType(), // remove if not needed
    ]).pipe(
      map(([account, activeLoadCarrierType, accountOverviewType]) => {
        if (account && activeLoadCarrierType) {
          const active: ActiveAccountLoadCarrierType = {
            accountId: account.id,
            loadCarrierTypeId: activeLoadCarrierType.id,
          };
          return active;
        }
        return null;
      })
    );
  }

  getSelectorExpanded() {
    return this.accountsQuery.selectorExpanded$;
  }

  setSelectorExpanded(expanded: boolean) {
    return this.accountsStore.updateSelectorExpanded(expanded);
  }

  getAccountOverviewType() {
    return this.accountsQuery.accountOverviewType$;
  }

  setAccountOverviewType(accountOverviewType: AccountOverviewType) {
    return this.accountsStore.updateAccountOverviewType(accountOverviewType);
  }

  getAccountingRecords(request: GetAccountingRecordsRequest) {
    // active posting account
    const { filter, page, limit } = request;


  }

  // private convertFilterToSearch(
  //   filter: ExtendedAccountingRecordFilter
  // ): Partial<AccountingRecordsSearchRequest> {
  //   console.log(filter);
  //   return {
  //     postingAccountIds: [filter.postingAccountId],
  //     loadCarrierTypeIds: [filter.loadCarrierTypeId],
  //     statuses:
  //       filter.Statuses &&
  //       filter.Statuses.length > 0 &&
  //       filter.Statuses.find((x) => x === AccountingRecordStatus.Provisional)
  //         ? [...filter.Statuses, AccountingRecordStatus.Pending]
  //         : filter.Statuses,
  //     chargeFrom: filter.Charge ? filter.Charge.from : undefined,
  //     chargeTo: filter.Charge ? filter.Charge.to : undefined,
  //     creditFrom: filter.Credit ? filter.Credit.from : undefined,
  //     creditTo: filter.Credit ? filter.Credit.to : undefined,
  //     description: filter.Description,
  //     fromDateTime: filter.AccountDate ? filter.AccountDate.from : undefined,
  //     toDateTime: filter.AccountDate ? filter.AccountDate.to : undefined,
  //   };
  // }
}
