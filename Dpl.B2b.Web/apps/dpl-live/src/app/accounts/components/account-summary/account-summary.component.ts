import {Component, OnInit} from '@angular/core';
import {AccountsService} from '../../services/accounts.service';
import {combineLatest, EMPTY, Observable} from 'rxjs';
import {map, switchMap} from 'rxjs/operators';
import {DplApiService} from "@app/core";
import {BalanceOverview} from "@app/api/dpl";
import {LoadCarriersService} from "../../../master-data/load-carriers/services/load-carriers.service";

type ViewData = {
  balances: BalanceOverview[];
  intactBalance: BalanceOverview;
  defectBalance: BalanceOverview;
  postingRequestBalance: BalanceOverview;
}

@Component({
  selector: 'app-account-summary',
  templateUrl: './account-summary.component.html',
  styleUrls: ['./account-summary.component.css'],
})
export class AccountSummaryComponent implements OnInit {
  viewData$: Observable<ViewData>;
  condition: keyof ViewData;
  balancesLoaded : boolean = false;
  refreshBalances: boolean = false;

  constructor(
    private accountsService: AccountsService,
    private dplApiService: DplApiService,
    private loadCarrierService: LoadCarriersService
  ) {
  }

  ngOnInit() {
    this.loadBalances(this.refreshBalances);
  }

  activeAccount$ = this.accountsService.getActiveAccount();
  activeLoadCarrierType$ = this.loadCarrierService.getActiveLoadCarrierType();

  loadBalances(refreshBalances: boolean) {
    this.viewData$ = combineLatest([this.activeAccount$, this.activeLoadCarrierType$]).pipe(
      map(([activeAccount, activeLoadCarrierType]) =>
        ({account: activeAccount, loadCarrierType: activeLoadCarrierType})),
      switchMap(request => {
        if (request.account && request.loadCarrierType) {
          this.balancesLoaded = refreshBalances;
          refreshBalances = false;
          return this.dplApiService.postingAccountBalancesApiService.get(
            {
              refLtmsArticleId: request.loadCarrierType?.refLtmsArticleId,
              loadCarrierTypeId: request.loadCarrierType?.id,
              postingAccountId: request.account?.id,
              forceBalanceCalculation: this.balancesLoaded
            })
        } else {
          return EMPTY;
        }
      }),
      map((response) => {
          const viewData: ViewData = {
            balances: response.balances,
            defectBalance: response.defectBalance,
            intactBalance: response.intactBalance,
            postingRequestBalance: response.postingRequestBalance,
          };
          this.balancesLoaded = true;
          this.refreshBalances = false;
          return viewData;
        }
      )
    );
  }
}
