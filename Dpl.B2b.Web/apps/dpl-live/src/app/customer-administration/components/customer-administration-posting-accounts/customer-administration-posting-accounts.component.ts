import {
  Component,
  OnInit,
  ChangeDetectionStrategy,
  Optional,
  Inject,
} from '@angular/core';
import CustomStore from 'devextreme/data/custom_store';
import {
  CustomerAdminOrganizationUnit,
  CustomerAdminScope,
} from '../../services/customer-administration.service.types';
import DataSource from 'devextreme/data/data_source';
import { combineLatest, Observable } from 'rxjs';
import { AuthenticationService } from '../../../core/services/authentication.service';
import { UnitsQuery } from '../../state/units.query';
import {
  Address,
  API_BASE_URL,
  LtmsAccount,
} from '../../../core/services/dpl-api-services';
import { filter, first, map, pluck, switchMap } from 'rxjs/operators';
import * as AspNetData from 'devextreme-aspnet-data-nojquery';
import { DplApiService } from '../../../core';
import ArrayStore from 'devextreme/data/array_store';

type ViewData = {
  unit: CustomerAdminOrganizationUnit;
  dataSource: CustomStore;
  addressesDataSource: CustomStore;
  customerNumber: string;
  ltmsAccountsDataSource: DataSource;
};

@Component({
  selector: 'dpl-customer-administration-posting-accounts',
  templateUrl: './customer-administration-posting-accounts.component.html',
  styleUrls: ['./customer-administration-posting-accounts.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class CustomerAdministrationPostingAccountsComponent implements OnInit {
  baseUrl: string;
  viewData$: Observable<ViewData>;

  addressDisplayExpr = (address: Address) => {
    return address
      ? `${address.city}, ${address.postalCode}, ${address.street1}`
      : '';
  };

  constructor(
    private authenticationService: AuthenticationService,
    private unitsQuery: UnitsQuery,
    private dpl: DplApiService,
    @Optional() @Inject(API_BASE_URL) baseUrl?: string
  ) {
    this.baseUrl = baseUrl;
  }

  ngOnInit(): void {
    const unit$ = this.unitsQuery.selectActive().pipe(filter((x) => !!x));
    const customerNumber$ = unit$.pipe(
      pluck('parent'),
      switchMap((customerId) => {
        return this.dpl.customersAdministrationService
          .getById(customerId)
          .pipe(pluck('refErpCustomerNumber'));
      })
    );

    const ltmsAccounts$ = customerNumber$.pipe(
      switchMap((customerNumber) => {
        return this.dpl.postingAccountsAdministrationService.getLtmsAccountsByCustomerNumber(
          customerNumber.toString()
        );
      })
    );

    this.viewData$ = combineLatest([
      unit$,
      customerNumber$,
      ltmsAccounts$,
    ]).pipe(
      map(([unit, customerNumber, ltmsAccounts]) => {
        const dataSource = AspNetData.createStore({
          key: 'id',
          loadUrl: this.baseUrl + '/postingaccountsadministration',
          loadParams: {
            customerId: unit.parent,
          },
          onBeforeSend: async (method, ajaxOptions) => {
            return await this.authenticationService
              .getTokenForRequestUrl(this.baseUrl)
              .pipe(
                map((token) => {
                  ajaxOptions.xhrFields = { withCredentials: false };
                  ajaxOptions.headers = { Authorization: 'Bearer ' + token };
                  // todo set refLtms and CustomerNumber
                  if (
                    ajaxOptions.method === 'POST' ||
                    ajaxOptions.method === 'PUT'
                  ) {
                    // add custom request data to insert request
                    const payload = JSON.parse(ajaxOptions.data.values);
                    if (payload.refLtmsAccountId) {
                      const ltmsAccount = ltmsAccounts.find(
                        (x) => x.id === payload.refLtmsAccountId
                      );
                      payload.refLtmsAccountNumber = ltmsAccount.accountNumber;
                      payload.customerNumber = ltmsAccount.customerNumber;
                    }                    

                    ajaxOptions.data.values = JSON.stringify(payload);
                  }
                  if (ajaxOptions.method === 'POST') {
                    // add custom request data to insert request
                    const payload = JSON.parse(ajaxOptions.data.values);

                    payload.customerId = unit.parent;

                    ajaxOptions.data.values = JSON.stringify(payload);
                  }
                })
              )
              .toPromise();
          },
          updateUrl: this.baseUrl + '/postingaccountsadministration',
          insertUrl: this.baseUrl + '/postingaccountsadministration',
          deleteUrl: this.baseUrl + '/postingaccountsadministration',
        });

        const addressesDataSource = AspNetData.createStore({
          key: 'id',
          loadUrl: this.baseUrl + '/addressesadministration',

          onBeforeSend: async (method, ajaxOptions) => {
            console.log(ajaxOptions);
            return await this.authenticationService
              .getTokenForRequestUrl(this.baseUrl)
              .pipe(
                map((token) => {
                  ajaxOptions.xhrFields = { withCredentials: false };
                  ajaxOptions.headers = { Authorization: 'Bearer ' + token };
                })
              )
              .toPromise();
          },
        });

        const ltmsAccountsDataSource = new DataSource(
          new ArrayStore({
            key: 'id',
            data: ltmsAccounts,
          })
        );

        const viewData: ViewData = {
          dataSource,
          unit,
          customerNumber: customerNumber.toString(),
          addressesDataSource,
          ltmsAccountsDataSource,
        };
        return viewData;
      })
    );
  }

  ltmsAccountDisplayExpression(account: LtmsAccount) {
    console.log('ltmsAccountDisplayExpression', account);
    return account
      ? `${account.name} | ${account.customerNumber} | ${account.accountNumber}`
      : '';
  }

  ltmsAccountDataSource(customerNumber: string, viewData: ViewData) {
    const customStore = {
      store: new CustomStore({
        key: 'id',
        loadMode: 'raw',

        load: async () => {
          console.log(customerNumber);
          return this.dpl.postingAccountsAdministrationService
            .getLtmsAccountsByCustomerNumber(
              customerNumber ? customerNumber : viewData.customerNumber
            )
            .toPromise();
        },
      }),

      sort: 'name',
    };
    customStore.store.load();
    return customStore;
  }
}
