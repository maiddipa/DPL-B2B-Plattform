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
import {
  Address,
  API_BASE_URL,
  Organization,
} from '../../../core/services/dpl-api-services';
import CustomStore from 'devextreme/data/custom_store';
import * as AspNetData from 'devextreme-aspnet-data-nojquery';
import { AuthenticationService } from '../../../core/services';
import { map } from 'rxjs/operators';

@Component({
  selector: 'dpl-customer-administration-organization-form',
  templateUrl: './customer-administration-organization-form.component.html',
  styleUrls: ['./customer-administration-organization-form.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class CustomerAdministrationOrganizationFormComponent implements OnInit {
  @Input() formData: Organization;

  @Output() formSubmit = new EventEmitter<Organization>();

  buttonOptions: any = {
    text: 'Speichern',
    type: 'success',
    useSubmitBehavior: true,
  };

  addressesDataSource: CustomStore;
  baseUrl: string;

  addressDisplayExpr = (address: Address) => {
    return address
      ? `${address.city}, ${address.postalCode}, ${address.street1}`
      : '';
  };

  constructor(
    private authenticationService: AuthenticationService,
    @Optional() @Inject(API_BASE_URL) baseUrl?: string
  ) {
    this.baseUrl = baseUrl;
  }

  ngOnInit(): void {
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
