import {
  Component,
  OnInit,
  ChangeDetectionStrategy,
  Optional,
  Inject,
} from '@angular/core';
import CustomStore from 'devextreme/data/custom_store';
import { combineLatest, NEVER, Observable } from 'rxjs';
import { filter, map, switchMap } from 'rxjs/operators';
import { AuthenticationService } from '../../../core/services/authentication.service';
import {
  API_BASE_URL,
  DocumentNumberSequence,
  DocumentTypeEnum,
} from '../../../core/services/dpl-api-services';
import {
  CustomerAdminOrganizationUnit,
  CustomerAdminScope,
} from '../../services/customer-administration.service.types';
import { UnitsQuery } from '../../state/units.query';
import * as AspNetData from 'devextreme-aspnet-data-nojquery';
import DataSource from 'devextreme/data/data_source';
import ArrayStore from 'devextreme/data/array_store';
import * as _ from 'lodash';
import { DplApiService } from '../../../core';
type ViewData = {
  unit: CustomerAdminOrganizationUnit;
  dataSource: CustomStore;
  documentTypeData: any;
  allNumberSequenzesForActiveCustomer: DocumentNumberSequence[];
};

@Component({
  selector: 'dpl-customer-administration-division-document-settings',
  templateUrl:
    './customer-administration-division-document-settings.component.html',
  styleUrls: [
    './customer-administration-division-document-settings.component.scss',
  ],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class CustomerAdministrationDivisionDocumentSettingsComponent
  implements OnInit {
  baseUrl: string;
  viewData$: Observable<ViewData>;
  readOnlyTrue: boolean = false;

  constructor(
    private dpl: DplApiService,
    private authenticationService: AuthenticationService,
    private unitsQuery: UnitsQuery,
    @Optional() @Inject(API_BASE_URL) baseUrl?: string
  ) {
    this.baseUrl = baseUrl;
  }

  ngOnInit(): void {
    const unit$ = this.unitsQuery.selectActive().pipe(filter((x) => !!x));
    const customerId$ = unit$.pipe(
      switchMap((unit) => {
        // get customer id for active division
        return this.unitsQuery.selectAll().pipe(
          map((units) => {
            return units.find((x) => x.idString === unit.parentIdString).parent;
          })
        );
      })
    );
    const allNumberSequenzesForActiveCustomer$ = customerId$.pipe(
      switchMap((customerId) => {
        return customerId
          ? this.dpl.numberSequencesApiService.getByCustomerId(customerId)
          : NEVER; // return never if customerId is undefined maybe find an other way to dispose or stop watching
      })
    );
    const documentTypeData$ = this.dpl.divisionDocumentSettingsAdministrationApiService.getDocumentTypes();

    this.viewData$ = combineLatest([
      unit$,
      allNumberSequenzesForActiveCustomer$,
      documentTypeData$,
    ]).pipe(
      map(([unit, allNumberSequenzesForActiveCustomer, documentTypeData]) => {
        const dataSource = AspNetData.createStore({
          key: 'id',
          loadUrl: this.baseUrl + '/division-document-settings',
          loadParams: {
            customerDivisionId:
              unit.scope === CustomerAdminScope.DivisionDocumentSettings
                ? unit.parent
                : undefined,
          },
          updateUrl: this.baseUrl + '/division-document-settings',
          onBeforeSend: async (method, ajaxOptions) => {
            return await this.authenticationService
              .getTokenForRequestUrl(this.baseUrl)
              .pipe(
                map((token) => {
                  ajaxOptions.xhrFields = { withCredentials: false };
                  ajaxOptions.headers = { Authorization: 'Bearer ' + token };
                  console.log(ajaxOptions.data.values);
                  if (ajaxOptions.method === 'PUT') {
                    // add custom request data to insert request
                    const payload = JSON.parse(ajaxOptions.data.values);
                    // set NumberSequenceID if changed
                    if (payload.documentNumberSequence?.id) {
                      payload.documentNumberSequenceId =
                        payload.documentNumberSequence.id;
                      payload.documentNumberSequence = undefined;
                    }
                    // set DocumentTypID if changed
                    if (payload.documentType?.id) {
                      payload.documentTypeId = payload.documentType.id;
                      payload.documentType = undefined;
                    }

                    ajaxOptions.data.values = JSON.stringify(payload);
                  }
                  if (ajaxOptions.method === 'POST') {
                    // add custom request data on insert request
                    const payload = JSON.parse(ajaxOptions.data.values);
                    payload.divisionId = unit.parent;
                    ajaxOptions.data.values = JSON.stringify(payload);
                  }
                })
              )
              .toPromise();
          },
          
        });
        const viewData: ViewData = {
          dataSource,
          unit,
          documentTypeData,
          allNumberSequenzesForActiveCustomer,
        };
        return viewData;
      })
    );
  }

  // Editor Preparing
  onEditorPreparing(e) {
    if (e.parentType === 'dataRow' && !e.row.isNewRow) {
      this.readOnlyTrue = true;
    } else {
      this.readOnlyTrue = false;
    }
  }

  getNumberSequenceDataSource(viewData: ViewData, documentTypeName) {
    console.log('documentTypeId', documentTypeName);
    return new DataSource(
      new ArrayStore({
        key: 'id',
        data: viewData.allNumberSequenzesForActiveCustomer.filter(
          (x) => x.documentType === documentTypeName
        ),
      })
    );
  }


  getCalculatedValue(item) {
    // console.log(item)
    return item.documentType.id;
  }
  getCalculatedDisplayValue(item) {
    return item.documentType.name + ' | ' + item.documentType.shortName;
  }
}
