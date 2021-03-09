import {
  Directive,
  Input,
  TemplateRef,
  ViewContainerRef,
  OnDestroy,
  OnInit,
} from '@angular/core';
import { PermissionResourceType, ResourceAction, UserRole } from '@app/api/dpl';
import * as _ from 'lodash';
import { untilDestroyed } from 'ngx-take-until-destroy';
import { combineLatest, of } from 'rxjs';
import { map, switchMap, tap } from 'rxjs/operators';

import { PermissionsService } from '../../core/services/permissions.service';
import { UserService } from '../../user/services/user.service';

interface PermissionData {
  actions: ResourceAction[];
  resource?: PermissionResourceType;
  referenceId?: number;
}

@Directive({
  selector: '[hasPermission]',
})
export class HasPermissionDirective implements OnInit, OnDestroy {
  @Input('hasPermission') hasPermission:
    | ResourceAction
    | [ResourceAction, PermissionResourceType, number]
    | {
        actions: ResourceAction[];
        resource?: PermissionResourceType;
        referenceId?: number;
      };

  @Input('hasPermissionFeature') feature: AppFeature;

  constructor(
    private templateRef: TemplateRef<any>,
    private viewContainer: ViewContainerRef,
    private permissionsService: PermissionsService,
    private userService: UserService
  ) {}

  ngOnInit(): void {
    let hidden = true;

    this.userService
      .getCurrentUser()
      .pipe(
        untilDestroyed(this),
        map((user) => user.role),
        switchMap((role) => {
          if (role === UserRole.DplEmployee) {
            return of(true);
          }

          if (this.feature) {
            const hasAccessToFeature = this.getHasRoleAccessToFeature(role);
            if (!hasAccessToFeature) {
              return of(false);
            }
          }

          const { actions, resource, referenceId } = this.getPermissionData();

          const hasPermissionObsArray = resource
            ? _(this.permissionsService.getResourceTypesInHierarchy(resource))
                .map((resource) => {
                  return actions.map((action) => {
                    return this.permissionsService.hasPermission(
                      action,
                      resource,
                      referenceId
                    );
                  });
                })
                .flatten()
                .value()
            : actions.map((action) => {
                return this.permissionsService.hasPermission(
                  action,
                  resource,
                  referenceId
                );
              });

          return combineLatest(hasPermissionObsArray).pipe(
            map((hasPermissionArray) => {
              // dpl employee
              return hasPermissionArray.some((hasPermission) => hasPermission);
            })
          );
        }),
        tap((hasPermission) => {
          if (hasPermission) {
            if (hidden) {
              this.viewContainer.createEmbeddedView(this.templateRef);
              hidden = false;
            }
          } else {
            this.viewContainer.clear();
            hidden = true;
          }
        })
      )
      .subscribe({
        complete() {
          if (!hidden) {
            this.viewContainer.clear();
            hidden = true;
          }
        },
        error() {
          if (!hidden) {
            this.viewContainer.clear();
            hidden = true;
          }
        },
      });
  }

  ngOnDestroy() {}

  getPermissionData(): PermissionData {
    if (HasPermissionDirective.isPermissionDataObject(this.hasPermission)) {
      return this.hasPermission;
    }

    if (!Array.isArray(this.hasPermission)) {
      const action = this.hasPermission;
      return {
        actions: [action],
      };
    }

    const [action, resource, referenceId] = this.hasPermission;

    return {
      actions: [action],
      resource,
      referenceId,
    };
  }

  getHasRoleAccessToFeature(userRole: UserRole): boolean {
    switch (this.feature) {
      case 'create-voucher-form':
        return (
          userRole === UserRole.Retailer || userRole === UserRole.DplEmployee
        );
      case 'create-availability-form':
        return (
          userRole === UserRole.Retailer ||
          userRole === UserRole.Shipper ||
          userRole === UserRole.DplEmployee
        );
      case 'create-delivery-form':
        return (
          userRole === UserRole.Shipper || userRole === UserRole.DplEmployee
        );
      case 'create-load-carrier-receipt-delivery-form':
        return (
          userRole === UserRole.Shipper ||
          userRole === UserRole.Warehouse ||
          userRole === UserRole.DplEmployee
        );
      case 'create-load-carrier-receipt-delivery-sorting-form':
        return (
          userRole === UserRole.Warehouse ||
          userRole === UserRole.DplEmployee
        );
      case 'create-load-carrier-receipt-pickup-form':
        return (
          userRole === UserRole.Shipper ||
          userRole === UserRole.Retailer ||
          userRole === UserRole.Warehouse ||
          userRole === UserRole.DplEmployee
        );
      case 'create-load-carrier-receipt-exchange-form':
        return false;
      // return (
      //   userRole === UserRole.Shipper || userRole === UserRole.DplEmployee
      // );
      case 'create-transfer-form':
        return false;
      // return (
      //   userRole === UserRole.Shipper || userRole === UserRole.DplEmployee
      // );
      case 'voucher-list':
        return true;
      case 'accounting-record-list':
        return true;
      case 'availability-list':
        return (
          userRole === UserRole.Retailer ||
          userRole === UserRole.Shipper ||
          userRole === UserRole.DplEmployee
        );
      case 'delivery-list':
        return (
          userRole === UserRole.Shipper || userRole === UserRole.DplEmployee
        );
      case 'journal':
        return (
          userRole === UserRole.Retailer ||
          userRole === UserRole.Shipper ||
          userRole === UserRole.Warehouse ||
          userRole === UserRole.DplEmployee
        );
      case 'transports':
        return false;
      case 'load-carrier-receipt-list':
        return (
          userRole === UserRole.Retailer ||
          userRole === UserRole.Shipper ||
          userRole === UserRole.Warehouse ||
          userRole === UserRole.DplEmployee
        );
      case 'live-pooling':
        return (
          userRole === UserRole.Shipper || userRole === UserRole.DplEmployee
        );
      default:
        return false;
    }
  }

  static isPermissionDataObject(value: any): value is PermissionData {
    return typeof value === 'object' && value.actions !== undefined;
  }
}

export type AppFeature =
  | 'create-voucher-form'
  | 'create-availability-form'
  | 'create-delivery-form'
  | 'create-load-carrier-receipt-delivery-form'
  | 'create-load-carrier-receipt-pickup-form'
  | 'create-load-carrier-receipt-exchange-form'
  | 'create-load-carrier-receipt-delivery-sorting-form'
  | 'create-transfer-form'
  | 'voucher-list'
  | 'accounting-record-list'
  | 'availability-list'
  | 'delivery-list'
  | 'journal'
  | 'transports'
  | 'load-carrier-receipt-list'
  | 'live-pooling';
