import { Injectable } from '@angular/core';
import { QueryEntity, Query } from '@datorama/akita';
import { UserStore } from './user.store';
import { IUser } from './user.model';
import { filter, map, switchMap } from 'rxjs/operators';
import { generatePermissionKey } from '../../core/services/permission.utils';
import { of } from 'rxjs';
import { UserRole } from '../../core/services/dpl-api-services';

export interface AppPermissions {
  [permission: string]: boolean;
}

@Injectable({ providedIn: 'root' })
export class UserQuery extends Query<IUser<number, number>> {
  // user$ = this.select().pipe(map((user) => (user.permissions?.length > 0 ? user : null)));
  user$ = this.select().pipe(map((user) => (user.role === UserRole.DplEmployee || user.permissions?.length > 0 ? user : null)));
  permissions$ = this.user$
    .pipe(
      filter((user) => !!user?.permissions)
    )
    .pipe(
      map((user) =>
        user.permissions.reduce((prev, perm) => {
          // detailed permission
          const permissionKey = generatePermissionKey(
            perm.action,
            perm.resource,
            perm.referenceId
          );
          prev[permissionKey] = true;

          // general permission
          const actionPermissionKey = generatePermissionKey(perm.action);
          prev[actionPermissionKey] = true;
          return prev;
        }, {} as AppPermissions)
      )
    );

  ordersCompletedToggle$ = this.select().pipe(
    map((user) =>
      user && user.settings && user.settings.viewSettings
        ? user.settings.viewSettings.ordersCompletedToggle
        : false
    )
  );

  constructor(protected store: UserStore) {
    super(store);
  }
}
