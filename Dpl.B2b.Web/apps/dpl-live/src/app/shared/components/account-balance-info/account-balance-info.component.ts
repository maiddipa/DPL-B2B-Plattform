import {
  Component,
  OnInit,
  ChangeDetectionStrategy,
  Input,
} from '@angular/core';
import { AccountsService } from '../../../accounts/services/accounts.service';
import { IBalance } from '../../../accounts/state/balance.model';
import { LoadCarriersService } from '../../../master-data/load-carriers/services/load-carriers.service';
import { combineLatest, merge, Observable, EMPTY } from 'rxjs';
import { switchMap, map, first, filter } from 'rxjs/operators';
import { UserService } from '../../../user/services/user.service';

type ViewData = {
  balances: InfoBalance[];
};

type InfoBalance = {
  carrierBalance: {
    id: number;
    balance: number;
  };
  typeBalance: {
    id: number;
    balance: number;
  };
};

@Component({
  selector: 'dpl-account-balance-info',
  templateUrl: './account-balance-info.component.html',
  styleUrls: ['./account-balance-info.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AccountBalanceInfoComponent implements OnInit {
  @Input() loadCarrierIds: number[];
  viewData$: Observable<ViewData>;

  // DD 2B - 103
  // EUR B - 2

  constructor(
    private loadCarriersService: LoadCarriersService,
    private accountsService: AccountsService
  ) {}

  ngOnInit() {
    const loadCarriers = combineLatest(
      this.loadCarrierIds.map((loadCarrierId) => {
        return this.loadCarriersService.getLoadCarrierById(loadCarrierId);
      })
    );

    const balances = loadCarriers.pipe(
      switchMap((carriers) => {
        const balancesArray = carriers.map((carrier) => {
          return combineLatest([
            this.accountsService.getBalanceForLoadCarrierType(carrier.type),
            this.accountsService.getBalanceForLoadCarrier(carrier.id),
          ]).pipe(
            filter(
              ([typeBalance, carrierBalance]) =>
                !!typeBalance && !!carrierBalance
            ),
            map(([typeBalance, carrierBalance]) => {
              console.log(typeBalance, carrierBalance);
              return {
                typeBalance,
                carrierBalance,
              };
            })
          );
        });
        return combineLatest(balancesArray);
      })
    );

    this.viewData$ = combineLatest(balances).pipe(
      map(([results]) => {
        const infoBalances = results.map((balance) => {
          const infoBalance: InfoBalance = {
            carrierBalance: {
              balance: balance.carrierBalance.availableBalance,
              id: balance.carrierBalance.loadCarrierId,
            },

            typeBalance: {
              balance: balance.typeBalance.availableBalance,
              id: balance.typeBalance.loadCarrierTypeId,
            },
          };
          return infoBalance;
        });
        const viewData: ViewData = {
          balances: infoBalances,
        };
        // hack use only LoadCarrierType balance in UI!
        return viewData;
      })
    );
  }
}
