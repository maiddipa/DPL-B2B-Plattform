import { Injectable } from '@angular/core';
import { DplApiService } from '../../core';
import { GroupsStore } from '../state/groups.store';


export type getAllPermissionsRequest={
  customerId?: number;
  divisionId?: number;
  organizationId?:number;
}

export type GroupFilter = {};

@Injectable({ providedIn: 'root' })
export class CustomerAdministrationGroupsService {
  constructor(private store: GroupsStore, private dpl: DplApiService) {}

  getAllPermissions(request:getAllPermissionsRequest){
    return this.dpl.permissionAdministrationService.all(request);
  }
}
