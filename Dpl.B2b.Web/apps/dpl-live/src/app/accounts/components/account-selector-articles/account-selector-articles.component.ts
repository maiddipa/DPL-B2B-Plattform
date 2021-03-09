import { Component, OnInit } from '@angular/core';
import { IAccount } from '../../state/account.model';
import { MatSelectionList, MatListOption } from '@angular/material/list';
import { MatSelect } from '@angular/material/select';
import { Observable, combineLatest } from 'rxjs';
import { AccountsService } from '../../services/accounts.service';
import { map, first, debounceTime, tap } from 'rxjs/operators';
import { LoadCarriersService } from 'apps/dpl-live/src/app/master-data/load-carriers/services/load-carriers.service';
import { IBalance } from '../../state/balance.model';
import { AccountOverviewType } from '../../services/accounts.service.types';

interface IViewData {
  carrierTypeIds: number[];
  selectedCarrierTypeId?: number;
  accounts: IAccount[];
  selectedAccountId: number;
  accountOverviewType: AccountOverviewType;
}

@Component({
  selector: 'app-account-selector-articles',
  templateUrl: './account-selector-articles.component.html',
  styleUrls: ['./account-selector-articles.component.css'],
})
export class AccountSelectorArticlesComponent implements OnInit {
  viewData$: Observable<IViewData>;
  accountOverviewType = AccountOverviewType;

  constructor(
    private accountService: AccountsService,
    private loadCarrierService: LoadCarriersService
  ) {}

  ngOnInit() {
    const carrierTypeIds$ = this.accountService.getAvailableLoadCarrierTypes();
    const selectedCarrierType$ = this.loadCarrierService.getActiveLoadCarrierType();
    const accounts$ = this.accountService.getAllAccounts();
    const selectedAccount$ = this.accountService.getActiveAccount();
    const accountOverviewType$ = this.accountService.getAccountOverviewType();

    this.viewData$ = combineLatest(
      carrierTypeIds$,
      selectedCarrierType$,
      accounts$,
      selectedAccount$,
      accountOverviewType$
    ).pipe(
      map((latest) => {
        const [
          carrierTypeIds,
          selectedCarrierType,
          accounts,
          selectedAccount,
          accountOverviewType,
        ] = latest;

        const viewData: IViewData = {
          carrierTypeIds,
          selectedCarrierTypeId: selectedCarrierType
            ? selectedCarrierType.id
            : null,
          accounts,
          selectedAccountId: selectedAccount ? selectedAccount.id : null,
          accountOverviewType,
        };
        return viewData;
      }),
      tap((viewData) => {
        if (!viewData.selectedAccountId && viewData.accounts.length > 0) {
          this.accountService.setActiveAccount(viewData.accounts[0].id);
        }
      })
    );
  }

  selectLoadCarrierType(event: { source: MatSelect; value: number }) {
    this.loadCarrierService.setActiveLoadCarrierType(event.value);
  }

  selectAccount(event: { source: MatSelect; value: number }) {
    this.accountService.setActiveAccount(event.value);
  }

  selectAccountOverviewType(event: {
    source: MatSelect;
    value: AccountOverviewType;
  }) {
    // remove if not needed
    this.accountService.setAccountOverviewType(event.value);
  }
}
