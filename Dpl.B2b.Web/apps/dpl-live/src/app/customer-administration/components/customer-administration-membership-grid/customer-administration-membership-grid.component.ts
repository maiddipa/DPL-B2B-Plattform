import {
  Component,
  OnInit,
  ChangeDetectionStrategy,
  Input,
  Optional,
  Inject,
} from '@angular/core';
import {
  API_BASE_URL,
  UserListItem,
} from '../../../core/services/dpl-api-services';
import {
  CustomerAdminOrganizationUnit,
  CustomerAdminScope,
} from '../../services/customer-administration.service.types';
import * as AspNetData from 'devextreme-aspnet-data-nojquery';
import CustomStore from 'devextreme/data/custom_store';
import { combineLatest, Observable, of } from 'rxjs';
import { CustomerAdministrationUsersService } from '../../services/customer-administration-users.service';
import { UnitsQuery } from '../../state/units.query';
import { filter, map, switchMap, tap } from 'rxjs/operators';
import DataSource from 'devextreme/data/data_source';
import ArrayStore from 'devextreme/data/array_store';
import { AuthenticationService } from '../../../core/services/authentication.service';
import * as _ from 'lodash';
type ViewData = {
  unit: CustomerAdminOrganizationUnit;
  dataSource: CustomStore;
  usersDataSource: DataSource;
};
@Component({
  selector: 'dpl-customer-administration-membership-grid',
  templateUrl: './customer-administration-membership-grid.component.html',
  styleUrls: ['./customer-administration-membership-grid.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class CustomerAdministrationMembershipGridComponent implements OnInit {
  baseUrl: string;
  viewData$: Observable<ViewData>;

  dataSource: any = {};

  constructor(
    private customerAdministrationUsersService: CustomerAdministrationUsersService,
    private unitsQuery: UnitsQuery,
    private authenticationService: AuthenticationService,
    @Optional() @Inject(API_BASE_URL) baseUrl?: string
  ) {
    this.baseUrl = baseUrl;
  }

  ngOnInit(): void {
    const unit$ = this.unitsQuery.selectActive().pipe(filter((x) => !!x));
    const allUsers$ = unit$.pipe(
      switchMap((unit) => {
        switch (unit.scope) {
          case CustomerAdminScope.OrganizationGroups:            
            return of(unit.parent);
          case CustomerAdminScope.CustomerGroupMembership:
            return of(unit.parent);
          case CustomerAdminScope.DivisionGroupMembership:
            return this.unitsQuery.selectAll().pipe(
              map((units) => {
                return units.find(
                  (x) => x.idString === unit.parentIdString
                ).parent;
              })
            );
          default:
            // error
            return of(0);
            break;
        }
      }),
      switchMap((customerId) => {
        return this.customerAdministrationUsersService.getAllUsers({
          customerId,
        });
      })
    );

    this.viewData$ = combineLatest([unit$, allUsers$]).pipe(
      map(([unit, allUsers]) => {
        const dataSource = AspNetData.createStore({
          key: 'userGroup.id',
          loadUrl: this.baseUrl + '/usergroupadministration/members',
          loadParams: {
            organizationId:
              unit.scope === CustomerAdminScope.OrganizationGroupMembership
                ? unit.parent
                : undefined,
            customerId:
              unit.scope === CustomerAdminScope.CustomerGroupMembership
                ? unit.parent
                : undefined,
            customerDivisionId:
              unit.scope === CustomerAdminScope.DivisionGroupMembership
                ? unit.parent
                : undefined,
          },
          // insertUrl: this.url + "/InsertOrder",
          updateUrl: this.baseUrl + '/usergroupadministration/updateMembers',
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

        const usersDataSource = new DataSource(
          new ArrayStore({
            key: 'id',
            data: allUsers,
          })
        );

        const viewData: ViewData = {
          unit,
          usersDataSource,
          dataSource,
        };
        return viewData;
      })
    );
  }

  tagBoxUserDisplayExpression(item) {
    return item ? `${item.lastName}, ${item.firstName} (${item.id})` : '';
  }

  getCalculatedCellValue(item) {
    let returnValue = _(item.users)
      .sortBy((x) => x.lastName)
      .value()
      .map((value) => `${value.lastName},${value.firstName}(${value.id})`);
    return returnValue;
  }
  getFilterExpressionUsers(value) {
    let column = this as any;
    var filterExpression = [[column.dataField, 'contains', value]];
    return filterExpression;
  }

  getUsersDisplayValue(value) {
    const users = value.data.users
      ? _(value.data.users)
          .sortBy((x) => x.lastName)
          .value()
          .map((value) => {
            return `${value.lastName},${value.firstName}(${value.id})`;
          })
      : [];
    return users;
  }
}
