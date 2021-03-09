import { Inject } from '@angular/core';
import {
  Component,
  OnInit,
  ChangeDetectionStrategy,
  Input,
  Optional,
} from '@angular/core';
import {
  API_BASE_URL,
  GroupPermission,
} from '../../../core/services/dpl-api-services';
import {
  CustomerAdminOrganizationUnit,
  CustomerAdminScope,
} from '../../services/customer-administration.service.types';
import * as AspNetData from 'devextreme-aspnet-data-nojquery';
import CustomStore from 'devextreme/data/custom_store';
import { AuthenticationService } from '../../../core/services/authentication.service';
import { filter, first, map, switchMap, tap } from 'rxjs/operators';
import { CustomerAdministrationGroupsService } from '../../services/customer-administration-groups.service';
import { UnitsQuery } from '../../state/units.query';
import DataSource from 'devextreme/data/data_source';
import { of } from 'rxjs/internal/observable/of';
import { combineLatest, Observable } from 'rxjs';
import ArrayStore from 'devextreme/data/array_store';
import * as _ from 'lodash';
type ViewData = {
  unit: CustomerAdminOrganizationUnit;
  dataSource: CustomStore;
  permissionsDataSource: DataSource;
};
@Component({
  selector: 'dpl-customer-administration-content-groups-grid',
  templateUrl: './customer-administration-content-groups-grid.component.html',
  styleUrls: ['./customer-administration-content-groups-grid.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class CustomerAdministrationContentGroupsGridComponent
  implements OnInit {
  baseUrl: string;
  viewData$: Observable<ViewData>;
  displayPermissionValues: any[];

  constructor(
    private authenticationService: AuthenticationService,
    private service: CustomerAdministrationGroupsService,
    private unitsQuery: UnitsQuery,
    @Optional() @Inject(API_BASE_URL) baseUrl?: string
  ) {
    this.baseUrl = baseUrl;
  }

  ngOnInit(): void {
    const unit$ = this.unitsQuery.selectActive().pipe(filter((x) => !!x));
    const allPermissions$ = unit$.pipe(
      switchMap((unit) => {
        // return request object {customerId: divisionId:}
        switch (unit.scope) {
          case CustomerAdminScope.OrganizationGroups:
            return this.service.getAllPermissions({
              organizationId: unit.parent,
            });
          case CustomerAdminScope.CustomerGroups:
            return this.service.getAllPermissions({
              customerId: unit.parent,
            });
          case CustomerAdminScope.DivisionGroups:
            return this.service.getAllPermissions({
              divisionId: unit.parent,
            });
          default:
            // error
            return of([] as GroupPermission[]);
            break;
        }
      })
    );

    this.viewData$ = combineLatest([unit$, allPermissions$]).pipe(
      map(([unit, allPermissions]) => {
        const dataSource = AspNetData.createStore({
          key: 'id',
          loadUrl: this.baseUrl + '/usergroupadministration',
          loadParams: {
            customerId:
              unit.scope === CustomerAdminScope.CustomerGroups
                ? unit.parent
                : undefined,
            customerDivisionId:
              unit.scope === CustomerAdminScope.DivisionGroups
                ? unit.parent
                : undefined,
            organizationId:
              unit.scope === CustomerAdminScope.OrganizationGroups
                ? unit.parent
                : undefined,
          },
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
                    console.log('PAYLOAD', payload);
                    if (unit.scope === CustomerAdminScope.CustomerGroups) {
                      payload.CustomerId = unit.parent;
                    } else if (
                      unit.scope === CustomerAdminScope.DivisionGroups
                    ) {
                      payload.DivisionId = unit.parent;
                    } else if (
                      unit.scope === CustomerAdminScope.OrganizationGroups
                    ) {
                      payload.OrganizationId = unit.parent;
                    }

                    // map permissions to permission server object

                    ajaxOptions.data.values = JSON.stringify(payload);
                  } else if (ajaxOptions.method === 'PUT') {
                    const payload = JSON.parse(ajaxOptions.data.values);
                    const mappedPermissions: GroupPermission[] = [];

                    for (const permission of payload.permissions) {
                      mappedPermissions.push(
                        allPermissions.find((x) => x.action === permission)
                      );
                    }
                    if (mappedPermissions?.length > 0 && mappedPermissions[0]) {
                      // case group permissions deleted without tag box opened
                      ajaxOptions.data.values = JSON.stringify({
                        ...payload,
                        permissions: mappedPermissions,
                      });
                    } else {
                      // case group modfified by tag box opened
                      ajaxOptions.data.values = JSON.stringify(payload);
                    }
                  }
                })
              )
              .toPromise();
          },
          updateUrl:
            this.baseUrl + '/usergroupadministration/updatePermissions',
          insertUrl: this.baseUrl + '/usergroupadministration',
        });

        const permissionsDataSource = new DataSource(
          new ArrayStore({
            key: 'action',
            data: allPermissions,
          })
        );
        const viewData: ViewData = {
          dataSource,
          permissionsDataSource,
          unit,
        };
        return viewData;
      }),
      tap((value) => {
        console.log(value.dataSource);
      })
    );
  }

  tagBoxUserDisplayExpression(item) {
    return item ? `${item.action} (${item.resource})` : '';
  }

  onEditorPreparing(e) {
    if (e.dataField === 'name' && e.parentType === 'dataRow') {
      e.editorOptions.disabled =
        e.row.data && e.row.data.isSystemGroup === true;

      // setzen von formItem.visible führt zu infinity loop
      if (e.row.data.isSystemGroup === true) {
        // e.component.columnOption('isSystemGroup', 'formItem.visible', true);
      }
    }
  }

  getFilterExpressionPermissions(value) {
    console.log(value);
    let column = this as any;
    console.log(column);
    console.log(column.dataField);
    console.log(column.calculateCellValue);
    var filterExpression = [[column.dataField, 'contains', value]];
    console.log(filterExpression);
    return filterExpression;
  }

  getCalculatedCellValue(item) {
    let returnValue = _(item.permissions)
      .sortBy((x) => x['action'])
      .value()
      .map((value) => value['action']);
    return returnValue;
  }
}
