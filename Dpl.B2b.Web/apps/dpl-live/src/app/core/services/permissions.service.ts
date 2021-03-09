import { Injectable } from '@angular/core';
import { ResourceAction, PermissionResourceType } from './dpl-api-services';
import { UserService } from '../../user/services/user.service';
import { first, map, pluck, switchMap } from 'rxjs/operators';
import { generatePermissionKey } from './permission.utils';
import { CustomerDivisionsService } from '../../customers/services/customer-divisions.service';

@Injectable({
  providedIn: 'root',
})
export class PermissionsService {
  constructor(
    private userService: UserService,
    private divisionsService: CustomerDivisionsService
  ) {}

  hasPermission(
    action: ResourceAction,
    resource: PermissionResourceType,
    referenceId: number
  ) {
    const permissionKey = generatePermissionKey(action, resource, referenceId);

    return this.userService.getPermissions().pipe(
      map((permissions) => {
        return permissions[permissionKey];
      })
    );
  }

  hasPermissionOnDivision(action: ResourceAction) {
    return this.divisionsService.getActiveDivision().pipe(
      first(),
      pluck('id'),
      switchMap((divisionId) => {
        return this.hasPermission(
          action,
          PermissionResourceType.Division,
          divisionId
        );
      })
    );
  }

  getResourceTypesInHierarchy(resource: PermissionResourceType) {
    switch (resource) {
      case PermissionResourceType.Organization: {
        return [PermissionResourceType.Organization];
      }
      case PermissionResourceType.Customer: {
        return [
          PermissionResourceType.Organization,
          PermissionResourceType.Customer,
        ];
      }
      case PermissionResourceType.Division: {
        return [
          PermissionResourceType.Organization,
          PermissionResourceType.Customer,
          PermissionResourceType.Division,
        ];
      }
      case PermissionResourceType.PostingAccount: {
        return [
          PermissionResourceType.Organization,
          PermissionResourceType.Customer,
          PermissionResourceType.PostingAccount,
        ];
      }
      default:
        throw new Error(`Argument out of range: ${resource}`);
    }
  }
}
