import {
  Component,
  OnInit,
  ChangeDetectionStrategy,
  Optional,
  Inject,
} from '@angular/core';
import CustomStore from 'devextreme/data/custom_store';
import { combineLatest, Observable } from 'rxjs';
import { AuthenticationService } from '@app/core';
import {
  API_BASE_URL, AccountingRecordStatus,
} from '@app/api/dpl';
import * as AspNetData from 'devextreme-aspnet-data-nojquery';
import { DplApiService } from '@app/core';
import {map} from "rxjs/operators";
import {AccountsService} from "../../services/accounts.service";
import {LoadCarriersService} from "../../../master-data/load-carriers/services/load-carriers.service";
import {MatSelect, MatSelectChange} from "@angular/material/select";
import {FilterService} from "../../../filters/services/filter.service";
import {FilterChoiceItem} from "../../../filters/services/filter.service.types";

type ViewData = {
  dataSource: CustomStore;
};

@Component({
  selector: 'app-account-bookings',
  templateUrl: './account-bookings.component.html',
  styleUrls: ['./account-bookings.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AccountBookingsComponent implements OnInit {
  baseUrl: string;
  loadParams: object;
  loadUrl: string;
  viewData$: Observable<ViewData>;
  isPostingRequestsActive: false;
  selectedAccountingRecordStatus: string;
  accountingRecordStatusOptions: FilterChoiceItem[];

  ngOnInit() {
    this.loadRecords();
     this.accountingRecordStatusOptions = this.filterService.getFilterOptionsFromEnum(AccountingRecordStatus, 'AccountingRecordStatus');
  }

  constructor(
    private authenticationService: AuthenticationService,
    private filterService: FilterService,
    private accountsService: AccountsService,
    private loadCarrierService: LoadCarriersService,
    private dpl: DplApiService,
    @Optional() @Inject(API_BASE_URL) baseUrl?: string
  ) {
    this.baseUrl = baseUrl;
  }

  activeAccount$ = this.accountsService.getActiveAccount();

  activeLoadCarrierType$ = this.loadCarrierService.getActiveLoadCarrierType();

  loadRecords() {
    this.viewData$ = combineLatest([this.activeAccount$, this.activeLoadCarrierType$]).pipe(
      map(([activeAccount, activeLoadCarrierType]) => {
        if (activeAccount && activeLoadCarrierType) {
          if (this.selectedAccountingRecordStatus === AccountingRecordStatus.Pending) {
            this.loadUrl = this.baseUrl + '/postingrequests';
            this.loadParams = {
              postingAccountId: activeAccount?.id,
              LoadCarrierTypeId: activeLoadCarrierType?.id
            }
          } else {
            this.loadUrl = this.baseUrl + '/accountingrecords';
            this.loadParams = {
              refLtmsAccountId: activeAccount?.refLtmsAccountId,
              refLtmsArticleId: activeLoadCarrierType?.id,
              Status: this.selectedAccountingRecordStatus
            }
          }

          const dataSource = AspNetData.createStore({
            key: 'id',
            loadUrl: this.loadUrl,
            loadParams: this.loadParams,
            onBeforeSend: async (method, ajaxOptions) => {
              return await this.authenticationService
                .getTokenForRequestUrl(this.baseUrl)
                .pipe(
                  map((token) => {
                    ajaxOptions.xhrFields = {withCredentials: false};
                    ajaxOptions.headers = {Authorization: 'Bearer ' + token};
                  })
                )
                .toPromise();
            },
          });
          const viewData: ViewData = {
            dataSource
          };
          return viewData;
        }
      })
    );
  }
  selectLoadCarrierType(event: { source: MatSelect; value: number }) {
    this.loadCarrierService.setActiveLoadCarrierType(event.value);
  }
  selectAccountingRecordStatus(event: { source: MatSelect; value: string }) {
    this.selectedAccountingRecordStatus = event.value;
    this.loadRecords();
  }
}
