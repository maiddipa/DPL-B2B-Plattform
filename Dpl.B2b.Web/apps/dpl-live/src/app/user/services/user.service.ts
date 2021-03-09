import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import {
  applyTransaction,
  filterNil,
  selectPersistStateInit,
} from '@datorama/akita';
import { Omit } from '@dpl/dpl-lib';
import * as _ from 'lodash';
import {
  combineLatest,
  EMPTY,
  Observable,
  of,
  ReplaySubject,
  throwError,
} from 'rxjs';
import {
  catchError,
  distinctUntilChanged,
  filter,
  first,
  map,
  pluck,
  switchMap,
  tap,
} from 'rxjs/operators';

import { IAccount } from '../../accounts/state/account.model';
import { AccountsStore } from '../../accounts/state/accounts.store';
import { IBalance } from '../../accounts/state/balance.model';
import { BalancesStore } from '../../accounts/state/balances.store';
import { AddressesStore } from '../../addresses/state/addresses.store';
import { NotificationService } from '../../core';
import { AuthenticationService } from '../../core/services/authentication.service';
import {
  ApiException,
  Balance,
  DplEmployeeCustomer,
  LoadCarrierQualityType,
  User,
  UserRole,
} from '../../core/services/dpl-api-services';
import { DplApiService } from '../../core/services/dpl-api.service';
import { CustomerDivisionsStore } from '../../customers/state/customer-divisions.store';
import { CustomersStore } from '../../customers/state/customers.store';
import { FilterContext } from '../../filters/services/filter.service.types';
import { LoadingLocationsStore } from '../../loading-locations/state/loading-locations.store';
import { LoadCarriersService } from '../../master-data/load-carriers/services/load-carriers.service';
import {
  IUser,
  IUserSettings,
  SettingsFilterTemplate,
} from '../state/user.model';
import { UserQuery } from '../state/user.query';
import { UserStore } from '../state/user.store';
import { normalizeUserData } from './user.normalize';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  ensureUserData$: ReplaySubject<void> = new ReplaySubject(1);
  loadCarrierDict = new ReplaySubject<{}>();
  initialized: boolean;
  constructor(
    private auth: AuthenticationService,
    private dpl: DplApiService,
    private userStore: UserStore,
    private userQuery: UserQuery,
    private customersStore: CustomersStore,
    private divisionsStore: CustomerDivisionsStore,
    private postingAccountsStore: AccountsStore,
    private balancesStore: BalancesStore,
    private loadingLocationsStore: LoadingLocationsStore,
    private addressesStore: AddressesStore,
    private loadCarrierService: LoadCarriersService,
    private notification: NotificationService
  ) {}

  isLoading() {
    return this.userQuery.selectLoading();
  }

  getCurrentUser() {
    return this.userQuery.user$;
  }

  getIsDplEmployee() {
    return this.getCurrentUser().pipe(
      filterNil,
      pluck('role'),
      map((role) => role === UserRole.DplEmployee),
      distinctUntilChanged()
    );
  }

  getPermissions() {
    return this.userQuery.permissions$;
  }

  refreshUserData(customer?: DplEmployeeCustomer): Observable<void> {
    return combineLatest([
      this.auth.isLoggedIn(),
      selectPersistStateInit(),
    ]).pipe(
      switchMap(([isLoggedIn]) => {
        this.userStore.setLoading(true);
        if (!isLoggedIn) {
          return EMPTY;
        }
        // check customer
        return this.dpl.user
          .get({
            customerId: customer ? customer.customerId : undefined,
          })
          .pipe(
            catchError((error) => {
              // if user is authorized but there was a server error 
              if (ApiException.isApiException(error) && error.status === 500) {
                return throwError(error);
              }
              // on any other error => likely Unauthorized
              else {
                this.notification.showError(
                  'User hat keine Zugriffsberechtigung.'
                );
              }

              return of(null);
            })
          );
      }),
      filterNil,
      map((userData) => ({
        userData,
        entities: normalizeUserData(userData),
      })),
      switchMap((data) =>
        this.processAccountsAndBalances(data.userData).pipe(
          map((entities) => ({ ...data.entities, ...entities }))
        )
      ),
      tap((entities) => {
        applyTransaction(() => {
          console.log('REFRESH USERDATA', entities);

          this.addressesStore.set(entities.addresses ? entities.addresses : []);
          this.loadingLocationsStore.set(
            entities.loadingLocations ? entities.loadingLocations : []
          );

          this.divisionsStore.set(entities.divisions ? entities.divisions : []);
          this.balancesStore.set(entities.balances ? entities.balances : []);
          this.postingAccountsStore.set(
            entities.postingAccounts ? entities.postingAccounts : []
          );
          this.customersStore.set(entities.customers ? entities.customers : []);

          // employee - check customer divisionId is set
          if (customer && customer.divisionId) {
            this.divisionsStore.setActive(customer.divisionId);
          } else {
            const active = this.divisionsStore.getValue().active;
            if (!active) {
              // make sure there always is an active division / posting account / customer
              this.divisionsStore.setActive(
                this.divisionsStore.getValue().ids[0]
              );
            }
          }
          // employee - check customer postingAccountId is set
          if (customer && customer.postingAccountId) {
            this.postingAccountsStore.setActive(customer.postingAccountId);
          } else {
            this.postingAccountsStore.setActive(
              this.postingAccountsStore.getValue().ids[0]
            );
          }
          // employee - set active customer customerId
          if (customer && customer.customerId) {
            this.customersStore.setActive(customer.customerId);
          } else {
            this.customersStore.setActive(
              this.customersStore.getValue().ids[0]
            );
          }

          const userData: IUser<number, number> = {
            ...this.userStore.getValue(),
            ...entities.user[Object.keys(entities.user)[0]],
          };

          // this needs to be the last store we update as we rely on the loading flag of this store
          this.userStore.update(userData);
          this.userStore.setLoading(false);
        });
      }),
      map((i) => null)
    );
  }

  getLoadCarrierDict() {
    return this.loadCarrierService.getLoadCarriers().pipe(
      filter((loadCarriers) => loadCarriers && loadCarriers.length > 0),
      first(),
      map((loadCarriers) => {
        return _(loadCarriers)
          .keyBy((i) => i.id)
          .value();
      })
    );
  }

  updateAccountBalance(postingAccountId: number) {
    return this.dpl.postingAccounts.getBalances(postingAccountId).pipe(
      switchMap((balances) => {
        return this.getLoadCarrierDict().pipe(
          map((loadCarriersDict) => {
            return this.processAccountBalance(
              postingAccountId,
              balances,
              loadCarriersDict
            );
          }),
          tap((data) => {
            applyTransaction(() => {
              // clear previous balances
              if (
                this.postingAccountsStore.getValue() &&
                this.postingAccountsStore.getValue().entities &&
                this.postingAccountsStore.getValue().entities[postingAccountId]
              ) {
                const balancesToRemove = this.postingAccountsStore.getValue()
                  .entities[postingAccountId].loadCarrierTypeBalances;
                this.balancesStore.remove(balancesToRemove);
              }

              this.balancesStore.add(data.balances);
              this.postingAccountsStore.update(postingAccountId, {
                ...data.account,
                balances,
              });
            });
          })
        );
      })
    );
  }

  processAccountsAndBalances(user: User) {
    return this.loadCarrierService.getLoadCarriers().pipe(
      filter((loadCarriers) => loadCarriers && loadCarriers.length > 0),
      first(),
      map((loadCarriers) => {
        const loadCarriersDict = _(loadCarriers)
          .keyBy((i) => i.id)
          .value();

        const postingAccounts = user.postingAccounts || [];
        return postingAccounts.map((postingAccount) => {
          const { account, balances } = this.processAccountBalance(
            postingAccount.id,
            postingAccount.balances,
            loadCarriersDict
          );

          return {
            account: <IAccount<string>>{
              ...postingAccount,
              ...account,
            },
            balances,
          };
        });
      }),
      map((items) => {
        const postingAccounts = items.map((i) => i.account);
        const balances = _(items)
          .map((i) => i.balances)
          .flatten()
          .value();

        return {
          postingAccounts,
          balances,
        };
      })
    );
  }

  private processAccountBalance(
    postingAccountId: number,
    accountBalances: Balance[],
    loadCarriersDict: {} = null
  ) {
    const loadCarrierTypesForBalances = _(accountBalances)
      .groupBy((i) => loadCarriersDict[i.loadCarrierId].type.id)
      .value();

    const balances = _(Object.keys(loadCarrierTypesForBalances))
      .flatMap((key) => {
        const loadCarrierTypeId = parseInt(key);
        const loadCarrierTypeBalances =
          loadCarrierTypesForBalances[loadCarrierTypeId];

        const balancesIntact = loadCarrierTypeBalances.filter(
          (i) => loadCarriersDict[i.loadCarrierId].quality.type === 'Intact'
        );

        const balancesDefect = loadCarrierTypeBalances.filter(
          (i) => loadCarriersDict[i.loadCarrierId].quality.type === 'Defect'
        );

        const generateBalance = (
          qualityType: LoadCarrierQualityType,
          balances: Balance[]
        ) => {
          const generatedId = `${postingAccountId}|${loadCarrierTypeId}|${qualityType}`;
          return <IBalance>{
            ...{
              loadCarrierTypeId,
              qualityType,
              generatedId,
            },
            ...this.calculateLoadCarrierTypeBalance(balances),
          };
        };

        return [
          generateBalance(LoadCarrierQualityType.Intact, balancesIntact),
          generateBalance(LoadCarrierQualityType.Defect, balancesDefect),
        ];
      })
      .value();

    const account = <
      Pick<IAccount<string>, 'loadCarrierTypeBalances' | 'loadCarrierTypeIds'>
    >{
      ...{
        loadCarrierTypeBalances: balances.map((i) => i.generatedId),
        loadCarrierTypeIds: Object.keys(
          loadCarrierTypesForBalances
        ).map((key) => parseInt(key)),
      },
    };

    return {
      account,
      balances,
    };
  }

  private calculateLoadCarrierTypeBalance(balances: Balance[]) {
    const baseBalance: Omit<Balance, 'loadCarrierId'> = {
      availableBalance: 0,
      coordinatedBalance: 0,
      provisionalBalance: 0,
      provisionalCharge: 0,
      provisionalCredit: 0,
      inCoordinationCharge: 0,
      inCoordinationCredit: 0,
      uncoordinatedCharge: 0,
      uncoordinatedCredit: 0,
      postingRequestBalanceCharge: 0,
      postingRequestBalanceCredit: 0,
      latestUncoordinatedCharge: null,
      latestUncoordinatedCredit: null,
    };
    return balances.reduce((prev, current) => {
      prev.availableBalance += current.availableBalance;
      prev.coordinatedBalance += current.coordinatedBalance;
      prev.provisionalBalance += current.provisionalBalance;
      prev.provisionalCharge += current.provisionalCharge;
      prev.provisionalCredit += current.provisionalCredit;
      prev.inCoordinationCharge += current.inCoordinationCharge;
      prev.inCoordinationCredit += current.inCoordinationCredit;
      prev.uncoordinatedCharge +=
        current.uncoordinatedCharge + current.inCoordinationCharge;
      prev.uncoordinatedCredit +=
        current.uncoordinatedCredit + current.inCoordinationCredit;
      prev.postingRequestBalanceCharge += current.postingRequestBalanceCharge;
      prev.postingRequestBalanceCredit += current.postingRequestBalanceCredit;
      prev.latestUncoordinatedCharge =
        prev.latestUncoordinatedCharge > current.latestUncoordinatedCharge
          ? prev.latestUncoordinatedCharge
          : current.latestUncoordinatedCharge;
      prev.latestUncoordinatedCharge =
        prev.latestUncoordinatedCredit > current.latestUncoordinatedCredit
          ? prev.latestUncoordinatedCredit
          : current.latestUncoordinatedCredit;
      return prev;
    }, baseBalance);
  }

  persistFilterTemplates(
    context: FilterContext,
    update: SettingsFilterTemplate
  ) {
    // get user settings object from store
    return this.userQuery.select().pipe(
      first(),
      switchMap((user) => {
        const filterTemplates: IUserSettings['filterTemplates'] =
          user.settings && user.settings.filterTemplates
            ? { ...user.settings.filterTemplates }
            : ({} as IUserSettings['filterTemplates']);

        filterTemplates[context] = update;

        const settings: IUserSettings = { ...user.settings, filterTemplates };

        return this.dpl.user.updateSettings(settings).pipe(
          tap((result) => {
            const updateUser = { ...user, settings };
            //update store user settings
            this.userStore.update(updateUser);
          })
        );
      })
    );
  }

  persistOrdersCompletedToggle(check: boolean) {
    // get user settings object from store
    return this.userQuery.select().pipe(
      first(),
      switchMap((user) => {
        const viewSettings: IUserSettings['viewSettings'] =
          user.settings && user.settings.viewSettings
            ? { ...user.settings.viewSettings }
            : ({} as IUserSettings['viewSettings']);

        viewSettings.ordersCompletedToggle = check;

        const settings: IUserSettings = { ...user.settings, viewSettings };

        return this.dpl.user.updateSettings(settings).pipe(
          tap((result) => {
            const updateUser = { ...user, settings };
            //update store user settings
            this.userStore.update(updateUser);
          })
        );
      })
    );
  }

  updateBalance(postingAccountId: number) {
    this.dpl.postingAccounts.getBalances(postingAccountId).pipe();
  }
}
