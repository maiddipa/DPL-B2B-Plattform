import {
  Component,
  OnInit,
  ChangeDetectionStrategy,
  Optional,
  Inject,
} from '@angular/core';
import CustomStore from 'devextreme/data/custom_store';
import { AuthenticationService } from '../../../core/services/authentication.service';
import { combineLatest, Observable, of } from 'rxjs';
import {
  API_BASE_URL,
  DocumentTypeEnum,
} from '../../../core/services/dpl-api-services';
import {
  CustomerAdminOrganizationUnit,
  CustomerAdminScope,
} from '../../services/customer-administration.service.types';
import { UnitsQuery } from '../../state/units.query';
import { filter, map } from 'rxjs/operators';
import * as AspNetData from 'devextreme-aspnet-data-nojquery';
import { LocalizationService } from '../../../core';

type ViewData = {
  unit: CustomerAdminOrganizationUnit;
  dataSource: CustomStore;
  documentTypeData: any;
};

@Component({
  selector: 'dpl-customer-administration-number-sequences',
  templateUrl: './customer-administration-number-sequences.component.html',
  styleUrls: ['./customer-administration-number-sequences.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class CustomerAdministrationNumberSequencesComponent implements OnInit {
  baseUrl: string;
  viewData$: Observable<ViewData>;
  // reloadSub = new BehaviorSubject<boolean>(false);
  constructor(
    private authenticationService: AuthenticationService,
    private unitsQuery: UnitsQuery,
    private localizationService: LocalizationService,
    @Optional() @Inject(API_BASE_URL) baseUrl?: string
  ) {
    this.baseUrl = baseUrl;
  }

  ngOnInit(): void {
    const unit$ = this.unitsQuery.selectActive().pipe(filter((x) => !!x));

    // const reload$ = this.reloadSub.asObservable();
    this.viewData$ = combineLatest([
      unit$,
      this.getDocumentTypeData(),
      // reload$,
    ]).pipe(
      map(([unit, documentTypeData]) => {
        const dataSource = AspNetData.createStore({
          key: 'id',
          loadUrl: this.baseUrl + '/number-sequences',
          loadParams: {
            customerId:
              unit.scope === CustomerAdminScope.CustomerNumberSequences
                ? unit.parent
                : undefined,
          },
          updateUrl: this.baseUrl + '/number-sequences',
          deleteUrl: this.baseUrl + '/number-sequences', // didn't work
          insertUrl: this.baseUrl + '/number-sequences',
          onBeforeSend: async (method, ajaxOptions) => {
            return await this.authenticationService
              .getTokenForRequestUrl(this.baseUrl)
              .pipe(
                map((token) => {
                  ajaxOptions.xhrFields = { withCredentials: false };
                  ajaxOptions.headers = { Authorization: 'Bearer ' + token };
                  if (ajaxOptions.method === 'POST') {
                    // add custom request data to insert request
                    const payload = JSON.parse(ajaxOptions.data.values);
                    if (
                      unit.scope === CustomerAdminScope.CustomerNumberSequences
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
        };
        return viewData;
      })
    );
  }

  // DisplayExpression for Lookups
  lookUpDisplayExpression(item) {
    // DisplayExpr for UserGridLookUps
    // used for Gender & Role
    return item ? `${item.display}` : ``;
  }
  // ValueExpression for Lookups
  lookUpValueExpression(item) {
    // ValueExpr for UserGridLookUps
    // used for Gender & Role
    return item ? `${item.value}` : ``;
  }
  // CalculateValue for DocumentType
  getCalculatedValue(item) {
    // console.log(item)
    return item.documentType.id;
  }
  // CalculateDisplayValue for DocumentType
  getCalculatedDisplayValue(item) {
    console.log(item);
    return item.documentType.name + ' | ' + item.documentType.shortName;
  }

  // get Gender LookupData
  getDocumentTypeData() {
    const r = Object.keys(DocumentTypeEnum).map((value, id) => {
      value = DocumentTypeEnum[value as any];
      const display = this.localizationService.getTranslation(
        'DocumentTypeEnum',
        value
      );
      // const display = value;
      return { id, value, display };
    });

    return of(r);
  }
  // show editing of the select only when addeed a new row
  onEditorPreparing(event: any): void {
    if (
      event.dataField === 'documentType' &&
      event.parentType === 'dataRow' &&
      !event.row.isNewRow
    ) {
      //disable documentType in edit
      event.editorOptions.disabled = true;
    }
  }

  getFilterExpressionDocumentType(value) {
    console.log(value);
    let column = this as any;
    console.log(column);
    console.log(column.dataField);
    console.log(column.calculateCellValue);
    var filterExpression = [[column.dataField, 'contains', value]];
    console.log(filterExpression);
    return filterExpression;
  }
}
