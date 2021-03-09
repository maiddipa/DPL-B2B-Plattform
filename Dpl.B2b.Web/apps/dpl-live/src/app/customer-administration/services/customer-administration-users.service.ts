import { Injectable } from '@angular/core';
import { filter, first, map, switchMap } from 'rxjs/operators';
import { DplApiService } from '../../core';

import { DplApiSort } from '../../core/utils';
import { UnitsQuery } from '../state/units.query';
import { UsersStore } from '../state/users.store';
import { CustomerAdminOrganizationUnit } from './customer-administration.service.types';

export type getUsersRequest = {
  customerId?: number;
  organizationId?: number;
  filter: Partial<UserFilter>;
  page: number;
  limit: number;
  // sort: DplApiSort<UsersSearchRequestSortOptions>;
};

export type getAllUsersRequest={
  customerId?: number;
  organizationId?: number;
}

export type UserFilter = {};

@Injectable({
  providedIn: 'root',
})
export class CustomerAdministrationUsersService {
  constructor(
    private store: UsersStore,
    private unitsQuery: UnitsQuery,
    private dpl: DplApiService
  ) {
  }

  getUsers(request: getUsersRequest) {
    return this.dpl.userAdmistrationService.get(request);
  }

  private convertFilterToSearch(filter: Partial<UserFilter>) {
    return {};
  }

  setActiveUser(id?: number) {
    this.store.setActive(id ? id : null);
  }

  loadDataCustom(unit: CustomerAdminOrganizationUnit, loadOptions: any) {

    return this.dpl.userAdmistrationService
      .get({
        customerId: unit.scope === 'CustomerUsers' ? unit.parent : null,
        organizationId: unit.scope === 'OrganizationUsers' ? unit.parent : null,
      })
      .pipe(
        map((result) => {
          return {
            data: result.data,
            // totalCount: result.total,
          };
        }),
        first()
      )
      .toPromise();
  }

  getAllUsers(request:getAllUsersRequest){
    return this.dpl.userAdmistrationService.all(request);
  }
}
