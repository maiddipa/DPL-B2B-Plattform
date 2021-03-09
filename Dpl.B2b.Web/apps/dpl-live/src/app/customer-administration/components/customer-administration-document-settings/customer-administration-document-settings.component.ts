import {
  ChangeDetectionStrategy,
  Component,
  OnInit,
  Optional,
  Inject,
} from '@angular/core';
import CustomStore from 'devextreme/data/custom_store';
import { combineLatest, Observable } from 'rxjs';
import { filter, map, switchMap } from 'rxjs/operators';
import {
  AuthenticationService,
  DplApiService,
  LocalizationService,
} from '../../../core';
import {
  CustomerAdminOrganizationUnit,
  CustomerAdminScope,
} from '../../services/customer-administration.service.types';
import { UnitsQuery } from '../../state/units.query';
import * as AspNetData from 'devextreme-aspnet-data-nojquery';
import DataSource from 'devextreme/data/data_source';
import ArrayStore from 'devextreme/data/array_store';

import {
  API_BASE_URL,
  LoadCarrier,
  DocumentType,
} from '../../../core/services/dpl-api-services';
import { LoadCarrierPipe } from '../../../shared';
import * as _ from 'lodash';

type ViewData = {
  unit: CustomerAdminOrganizationUnit;
  dataSource: CustomStore;
  documentTypeData: any; // maybe change any
  loadCarrierTypeData: LoadCarrier[]; // maybe change any
  loadCarrierTypeDataTransformed: any;
};
@Component({
  selector: 'dpl-customer-administration-document-settings',
  templateUrl: './customer-administration-document-settings.component.html',
  styleUrls: ['./customer-administration-document-settings.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class CustomerAdministrationDocumentSettingsComponent implements OnInit {
  baseUrl: string;
  viewData$: Observable<ViewData>;
  readOnlyTrue: boolean = false;

  constructor(
    private authenticationService: AuthenticationService,
    private unitsQuery: UnitsQuery,
    private dpl: DplApiService,
    // private localizationService: LocalizationService,
    private loadCarrier: LoadCarrierPipe,
    @Optional() @Inject(API_BASE_URL) baseUrl?: string
  ) {
    this.baseUrl = baseUrl;
  }

  ngOnInit(): void {
    const unit$ = this.unitsQuery.selectActive().pipe(filter((x) => !!x));
    // get for DocumentTypes
    const documentTypeData$ = this.dpl.divisionDocumentSettingsAdministrationApiService.getDocumentTypes();
    // get for loadCarriers
    const loadCarrierData$ = this.dpl.loadCarriers.get();
    this.viewData$ = combineLatest([
      unit$,
      documentTypeData$,
      loadCarrierData$,
    ]).pipe(
      map(([unit, documentTypeData, loadCarrierData]) => {
        const loadCarrierTypeData = loadCarrierData.map((item) => {
          // map for a transformed display to show it in the select
          return {
            id: item.type.id,
            value: item.type.id,
            display: this.loadCarrier.transform(item.type.id, 'type'),
          };
        });
        // remove duplicates
        const loadCarrierTypeDataTransformed = _.uniqBy(
          loadCarrierTypeData,
          'id'
        );

        const dataSource = AspNetData.createStore({
          key: 'id',
          loadUrl: this.baseUrl + '/customer-document-settings',
          loadParams: {
            customerId:
              unit.scope === CustomerAdminScope.CustomerDocumentSettings
                ? unit.parent
                : undefined,
          },
          insertUrl: this.baseUrl + '/customer-document-settings',
          updateUrl: this.baseUrl + '/customer-document-settings',
          deleteUrl: this.baseUrl + '/customer-document-settings',
          onBeforeSend: async (method, ajaxOptions) => {
            return await this.authenticationService
              .getTokenForRequestUrl(this.baseUrl)
              .pipe(
                map((token) => {
                  ajaxOptions.xhrFields = { withCredentials: false };
                  ajaxOptions.headers = { Authorization: 'Bearer ' + token };
                  if (
                    ajaxOptions.method === 'POST' ||
                    ajaxOptions.method === 'PUT'
                  ) {
                    // add custom request data to insert request
                    const payload = JSON.parse(ajaxOptions.data.values);
                    if (
                      unit.scope === CustomerAdminScope.CustomerDocumentSettings
                    ) {
                      payload.customerId = unit.parent;
                    }
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
          loadCarrierTypeData,
          loadCarrierTypeDataTransformed,
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
  //get LoadCarrierTypeData for select
  getLoadCarrierTypeDataSource(viewData: ViewData) {
    const dataStore = new DataSource(
      new ArrayStore({
        key: 'id',
        data: viewData.loadCarrierTypeDataTransformed.filter((x) => x.value),
      })
    );
    return dataStore;
  }
  // get LoadCarrier Type Name for Select Box
  getLoadCarrierTypeName(viewData: ViewData, id: number) {
    const value = viewData.loadCarrierTypeDataTransformed.find(
      (x) => x.id === id
    );
    return value.display;
  }
  // get Document Type Name for Select Box
  getDocumentTypeName(viewData: ViewData, id: number) {
    const value = viewData.documentTypeData.find((x) => x.id === id);
    return this.documentTypeDisplayExpr(value);
  }
  // Document Type Display Expression
  documentTypeDisplayExpr = (documentType: DocumentType) => {
    return documentType
      ? `${documentType.name} | ${documentType.shortName}`
      : '';
  };
  // Custom Validaton Callback for Max & Warning Quantity
  customValidationCallbackWarningQuantity(params) {
    if (params.data.maxQuantity && params.data.thresholdForWarningQuantity) {
      if (params.data.maxQuantity >= params.data.thresholdForWarningQuantity) {
        return params;
      } else {
        return false;
      }
    } else {
      return params;
    }
  }
}
