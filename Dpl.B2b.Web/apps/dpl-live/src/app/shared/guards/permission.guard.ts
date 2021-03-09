import { Injectable } from '@angular/core';
import {
  ActivatedRouteSnapshot,
  CanActivate,
  Router,
  RouterStateSnapshot,
} from '@angular/router';
import { filterNil } from '@datorama/akita';
import * as _ from 'lodash';
import { combineLatest, Observable, of } from 'rxjs';
import { map, switchMap, tap } from 'rxjs/operators';

import { AccountsQuery } from '../../accounts/state/accounts.query';
import {
  PermissionResourceType,
  ResourceAction,
} from '../../core/services/dpl-api-services';
import { NotificationService } from '../../core/services/notification.service';
import { PermissionsService } from '../../core/services/permissions.service';
import { CustomerDivisionsQuery } from '../../customers/state/customer-divisions.query';
import { CustomersQuery } from '../../customers/state/customers.query';
import { UserService } from '../../user/services/user.service';
import { PermissionActionPipe } from '../pipes/permission-action.pipe';
import { PermissionResourcePipe } from '../pipes/permission-resource.pipe';

type PermissionRouteData = {
  action: ResourceAction | ResourceAction[];
  resource: PermissionResourceType;
};

@Injectable({
  providedIn: 'root',
})
export class PermissionGuard implements CanActivate {
  constructor(
    private permission: PermissionsService,
    private customer: CustomersQuery,
    private division: CustomerDivisionsQuery,
    private accounts: AccountsQuery,
    private router: Router,
    private notification: NotificationService,
    private permissionResourcePipe: PermissionResourcePipe,
    private permissionActionPipe: PermissionActionPipe,
    private userService: UserService
  ) {}

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): Observable<boolean> {
    const { actions, resource } = this.getPermissionData(
      route.data as PermissionRouteData
    );

    const obsArr = _(this.permission.getResourceTypesInHierarchy(resource))
      .filter((resource) => resource !== PermissionResourceType.Organization)
      .map((resource) => {
        return actions.map((action) => {
          return this.getActiveIdByPermissionResourceType(resource).pipe(
            switchMap((id) => {
              return this.permission.hasPermission(action, resource, id).pipe(
                map((hasPermission) => {
                  return {
                    permission: [action, resource, id],
                    hasPermission,
                  };
                })
              );
            })
          );
        });
      })
      .flatten()
      .value();

    return this.userService.getIsDplEmployee().pipe(
      switchMap((isEmployee) => {
        if (isEmployee) {
          return of(true);
        }

        return combineLatest(obsArr).pipe(
          map((permissionInfoArr) => {
            // uncomment when debugging permissions
            //console.log(permissionInfoArr);
            return permissionInfoArr.some((info) => info.hasPermission);
          }),
          tap((hasPermission) => {
            if (!hasPermission) {
              if (actions.length === 1) {
                this.notification.showError(
                  `Sie haben nicht die Berechtigung die Aktion "${this.permissionActionPipe.transform(
                    actions[0]
                  )}" für die aktuell selektierte ${this.permissionResourcePipe.transform(
                    resource
                  )} durchzuführen`
                );
              } else {
                const actionsAsString = actions
                  .map((action) => this.permissionActionPipe.transform(action))
                  .join(', ');

                `Sie haben nicht die Berechtigung die Aktionen "${actionsAsString}" für die aktuell selektierte ${this.permissionResourcePipe.transform(
                  resource
                )} durchzuführen`;
              }

              this.router.navigate(['/']);
            }
          })
        );
      })
    );
  }

  getActiveIdByPermissionResourceType(resource: PermissionResourceType) {
    const getActiveId = () => {
      switch (resource) {
        case PermissionResourceType.Customer:
          return this.customer.selectActiveId();
        case PermissionResourceType.Division:
          return this.division.selectActiveId();
        case PermissionResourceType.PostingAccount:
          return this.accounts.selectActiveId();
        default:
          throw new Error(`Argument out of range ${resource}`);
      }
    };

    return getActiveId().pipe(filterNil) as Observable<number>;
  }

  getPermissionData(
    data: PermissionRouteData
  ): { actions: ResourceAction[]; resource: PermissionResourceType } {
    const { action, resource } = data;

    if (!Array.isArray(action)) {
      return {
        actions: [action],
        resource,
      };
    }

    return {
      actions: action,
      resource,
    };
  }
}
