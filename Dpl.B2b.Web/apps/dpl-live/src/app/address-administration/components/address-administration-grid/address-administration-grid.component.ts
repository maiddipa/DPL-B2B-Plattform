import { Inject } from '@angular/core';
import {
  Component,
  OnInit,
  ChangeDetectionStrategy,
  Optional,
} from '@angular/core';
import CustomStore from 'devextreme/data/custom_store';
import { combineLatest, Observable, of } from 'rxjs';
import { map } from 'rxjs/operators';
import { AuthenticationService } from '../../../core/services/authentication.service';
import { API_BASE_URL, Country } from '../../../core/services/dpl-api-services';
import * as AspNetData from 'devextreme-aspnet-data-nojquery';
import DataSource from 'devextreme/data/data_source';
import { DplApiService } from '../../../core';
import ArrayStore from 'devextreme/data/array_store';

type ViewData = {
  dataSource: CustomStore;
  countries: Country[];
};
@Component({
  selector: 'dpl-address-administration-grid',
  templateUrl: './address-administration-grid.component.html',
  styleUrls: ['./address-administration-grid.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AddressAdministrationGridComponent implements OnInit {
  baseUrl: string;
  viewData$: Observable<ViewData>;

  constructor(
    private dpl: DplApiService,
    private authenticationService: AuthenticationService,
    @Optional() @Inject(API_BASE_URL) baseUrl?: string
  ) {
    this.baseUrl = baseUrl;
    this.getFilteredStates = this.getFilteredStates.bind(this);
  }

  ngOnInit(): void {
    const countries$ = this.dpl.addressesAdministrationService.getCountries();
    this.viewData$ = combineLatest([countries$]).pipe(
      map(([countries]) => {
        console.log(countries);
        const dataSource = AspNetData.createStore({
          key: 'id',
          loadUrl: this.baseUrl + '/addressesadministration',
          loadParams: {},
          onBeforeSend: async (method, ajaxOptions) => {
            return await this.authenticationService
              .getTokenForRequestUrl(this.baseUrl)
              .pipe(
                map((token) => {
                  ajaxOptions.xhrFields = { withCredentials: false };
                  ajaxOptions.headers = { Authorization: 'Bearer ' + token };
                  if (
                    ajaxOptions.method === 'PUT' ||
                    ajaxOptions.method === 'POST'
                  ) {
                    // add custom request data to insert request
                    const payload = JSON.parse(ajaxOptions.data.values);
                    // set NumberSequenceID if changed
                    if (payload.country) {
                      payload.countryId = payload.country;
                      payload.country = undefined;
                    }
                    // set DocumentTypID if changed
                    if (payload.state) {
                      payload.stateId = payload.state;
                      payload.state = undefined;
                    }

                    ajaxOptions.data.values = JSON.stringify(payload);
                  }
                })
              )
              .toPromise();
          },
          updateUrl: this.baseUrl + '/addressesadministration',
          insertUrl: this.baseUrl + '/addressesadministration',
          deleteUrl: this.baseUrl + '/addressesadministration',
        });

        const countriesDataSource = new DataSource(
          new ArrayStore({
            key: 'id',
            data: countries,
          })
        );

        const viewData: ViewData = {
          dataSource,
          countries,
        };
        return viewData;
      })
    );
  }

  getFilteredStates(options) {
    // return {
    //     store: this.cities,
    //     filter: options.data ? ["StateID", "=", options.data.StateID] : null
    // };
    if (options?.data?.country) {
      const customStore = {
        store: new CustomStore({
          key: 'id',
          loadMode: 'raw',

          load: async () => {
            return this.dpl.addressesAdministrationService
              .getStatesByCountryId(options.data.country)
              .toPromise();
          },
        }),

        sort: 'name',
      };
      customStore.store.load();
      return customStore;
    }
    return {
      store: [],
    };
  }

  setCountryValue(rowData: any, value: any): void {
    console.log('setCountryValue', rowData, value);
    rowData.state = null;
    (<any>this).defaultSetCellValue(rowData, value);
  }

  onEditorPreparing(e) {
    if (e.parentType === 'dataRow' && e.dataField === 'state') {
      e.editorOptions.disabled = typeof e.row.data.country !== 'number';
    }
  }
}
