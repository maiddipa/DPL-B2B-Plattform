import {
  Component,
  OnInit,
  ChangeDetectionStrategy,
  Input,
  Output,
  EventEmitter,
  Optional,
  Inject,
} from '@angular/core';
import CustomStore from 'devextreme/data/custom_store';
import {
  Address,
  API_BASE_URL,
  Customer,
} from '../../../core/services/dpl-api-services';
import * as AspNetData from 'devextreme-aspnet-data-nojquery';
import { AuthenticationService } from '../../../core/services/authentication.service';
import { map } from 'rxjs/operators';

@Component({
  selector: 'dpl-customer-administration-customer-form',
  templateUrl: './customer-administration-customer-form.component.html',
  styleUrls: ['./customer-administration-customer-form.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class CustomerAdministrationCustomerFormComponent implements OnInit {
  @Input() formData: Customer;
  @Output() formSubmit = new EventEmitter<Customer>();

  partnersDataSource: CustomStore;
  addressesDataSource: CustomStore;
  baseUrl: string;

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

  constructor(
    private authenticationService: AuthenticationService,
    @Optional() @Inject(API_BASE_URL) baseUrl?: string
  ) {
    this.baseUrl = baseUrl ? baseUrl : '';
  }

  ngOnInit(): void {
    this.partnersDataSource = AspNetData.createStore({
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
    this.addressesDataSource = AspNetData.createStore({
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
  }

  onFormSubmit(e) {
    e.preventDefault();
    this.formSubmit.emit(this.formData);
  }
}
