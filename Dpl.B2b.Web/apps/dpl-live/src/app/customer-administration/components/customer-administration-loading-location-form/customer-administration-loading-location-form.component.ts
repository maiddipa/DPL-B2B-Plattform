import {
  Component,
  OnInit,
  ChangeDetectionStrategy,
  Input,
  Optional,
  Inject,
  Output,
  EventEmitter,
  ViewChild,
} from '@angular/core';
import CustomStore from 'devextreme/data/custom_store';
import { Observable, of } from 'rxjs';
import { map } from 'rxjs/operators';
import { AuthenticationService } from '../../../core/services/authentication.service';
import {
  Address,
  API_BASE_URL,
  LoadingLocation,
  Partner,
} from '../../../core/services/dpl-api-services';
import * as AspNetData from 'devextreme-aspnet-data-nojquery';
import { DxFormComponent } from 'devextreme-angular';

type ViewData = {
  partnersDataSource: CustomStore;
  addressesDataSource: CustomStore;
};

@Component({
  selector: 'dpl-customer-administration-loading-location-form',
  templateUrl: './customer-administration-loading-location-form.component.html',
  styleUrls: ['./customer-administration-loading-location-form.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class CustomerAdministrationLoadingLocationFormComponent
  implements OnInit {
  @Input() formData: LoadingLocation;
  @Output() formSubmit = new EventEmitter<LoadingLocation>();
  @ViewChild(DxFormComponent)
  form: DxFormComponent;
  baseUrl: string;
  viewData$: Observable<ViewData>;
  maxHeight: number = 0;
  minHeight: number = 0;

  buttonOptions: any = {
    text: 'Speichern',
    type: 'success',
    useSubmitBehavior: true,
  };

  addressDisplayExpr = (address: Address) => {
    return address
      ? `${address.city}, ${address.postalCode}, ${address.street1}`
      : '';
  };

  partnerDisplExpre = (partner: Partner) => {
    return partner ? `${partner.companyName} (${partner.id})` : '';
  };

  constructor(
    private authenticationService: AuthenticationService,
    @Optional() @Inject(API_BASE_URL) baseUrl?: string
  ) {
    this.baseUrl = baseUrl;
  }

  ngOnInit(): void {
    this.viewData$ = of([]).pipe(
      map(() => {
        console.log('Loading Location Form');
        const partnersDataSource = AspNetData.createStore({
          key: 'id',
          loadUrl: this.baseUrl + '/partnersadministration',
          onBeforeSend: async (method, ajaxOptions) => {
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

        const viewData: ViewData = {
          partnersDataSource,
          addressesDataSource,
        };
        return viewData;
      })
    );
  }

  onFormSubmit(e) {
    e.preventDefault();
    this.formSubmit.emit(this.formData);
  }

  customValidationCallbackstackHeight = (params) => {
    if (params.formItem?.dataField === 'stackHeightMin') {
      this.minHeight = parseInt(params.value, 10);

      if (this.minHeight > this.maxHeight) {
        return false;
      } else {
        return params;
      }
    } else if (params.formItem?.dataField === 'stackHeightMax') {
      this.maxHeight = parseInt(params.value, 10);

      if (this.maxHeight < this.minHeight) {
        console.log(params.value);
        return false;
      } else {
        return params;
      }
    }
  };

  fieldDataChanged(e: any) {
    console.log(e);
    this.form.instance.validate();
  }
}
