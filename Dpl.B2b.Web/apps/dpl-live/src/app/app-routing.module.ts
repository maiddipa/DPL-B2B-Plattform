import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { MsalGuard } from '@azure/msal-angular';

import { AuthGuard } from './core/auth.guard';
import {
  PermissionResourceType,
  ResourceAction,
} from './core/services/dpl-api-services';
import { PrivacyComponent } from './shared';
import { EmployeeGuard } from './shared/guards/employee.guard';
import { PermissionGuard } from './shared/guards/permission.guard';
import { DefaultContentComponent } from './shared/layout/components/default-content/default-content.component';
import { HomeComponent } from './shared/layout/components/home/home.component';
import { ImprintComponent } from './shared/layout/components/imprint/imprint.component';

const routes: Routes = [
  {
    path: '',
    pathMatch: 'full',
    component: HomeComponent,
  },
  {
    path: 'imprint',
    pathMatch: 'full',
    component: ImprintComponent,
  },
  {
    path: 'privacy',
    pathMatch: 'full',
    component: PrivacyComponent,
  },
  {
    path: '',
    canActivate: [MsalGuard, AuthGuard],
    children: [
      {
        path: '',
        component: DefaultContentComponent,
        children: [
          {
            path: 'accounts',
            pathMatch: 'full',
            redirectTo: 'accounts',
          },
          {
            path: '',
            canActivate: [PermissionGuard],
            data: {
              action: ResourceAction.CreateOrder,
              resource: PermissionResourceType.Division,
            },
            loadChildren: () =>
              import('./availabilities/availabilities.module').then(
                (m) => m.AvailabilitiesModule
              ),
          },
          {
            path: 'orders',
            canActivate: [PermissionGuard],
            data: {
              action: ResourceAction.ReadOrder,
              resource: PermissionResourceType.Division,
            },
            loadChildren: () =>
              import('./orders/orders.module').then((m) => m.OrdersModule),
          },
          {
            path: 'order-loads',
            canActivate: [PermissionGuard],
            data: {
              action: ResourceAction.ReadOrderLoad,
              resource: PermissionResourceType.Division,
            },
            loadChildren: () =>
              import('./order-loads/order-loads.module').then(
                (m) => m.OrderLoadsModule
              ),
          },
          {
            path: 'order-process',
            canActivate: [PermissionGuard],
            data: {
              action: ResourceAction.ReadOrder,
              resource: PermissionResourceType.Division,
            },
            loadChildren: () =>
              import('./order-process/order-processes.module').then(
                (m) => m.OrderProcessesModule
              ),
          },
          {
            path: 'receipt',
            canActivate: [PermissionGuard],
            data: {
              action: ResourceAction.CreateLoadCarrierReceipt,
              resource: PermissionResourceType.Division,
            },
            loadChildren: () =>
              import('./load-carrier-receipt/load-carrier-receipt.module').then(
                (m) => m.LoadCarrierReceiptModule
              ),
          },
          {
            path: 'transfer',
            loadChildren: () =>
              import('./transfer/transfer.module').then(
                (m) => m.TransferModule
              ),
          },
          {
            path: 'transports',
            // TODO add permission guard as soon as we have permission actions for this
            loadChildren: () =>
              import('./transports/transports.module').then(
                (m) => m.TransportsModule
              ),
          },
          {
            path: 'voucher',
            canActivate: [PermissionGuard],
            data: {
              action: ResourceAction.CreateVoucher,
              resource: PermissionResourceType.Division,
            },
            loadChildren: () =>
              import('./voucher/voucher.module').then((m) => m.VoucherModule),
          },
          {
            path: 'voucher-register',
            canActivate: [PermissionGuard],
            data: {
              action: ResourceAction.ReadVoucher,
              resource: PermissionResourceType.Division,
            },
            loadChildren: () =>
              import('./voucher-register/voucher-register.module').then(
                (m) => m.VoucherRegisterModule
              ),
          },
          {
            path: 'search',
            canActivate: [PermissionGuard],
            data: {
              action: ResourceAction.CreateLivePoolingSearch,
              resource: PermissionResourceType.Division,
            },
            loadChildren: () =>
              import('./search/search.module').then((m) => m.SearchModule),
          },
          {
            path: 'load-carrier-receipts',
            canActivate: [PermissionGuard],
            data: {
              action: ResourceAction.ReadLoadCarrierReceipt,
              resource: PermissionResourceType.Division,
            },
            loadChildren: () =>
              import(
                './load-carrier-receipts/load-carrier-receipts.module'
              ).then((m) => m.LoadCarrierReceiptsModule),
          },
          {
            path: 'sorting',
            canActivate: [PermissionGuard],
            data: {
              action: ResourceAction.ReadLoadCarrierReceipt,
              resource: PermissionResourceType.Division,
            },
            loadChildren: () =>
              import('./sorting/sorting.module').then((m) => m.SortingModule),
          },
        ],
      },
    ],
  },
  {
    path: '',
    canActivate: [MsalGuard, AuthGuard,EmployeeGuard],
    children: [
      {
        path: 'customer-administration',
        loadChildren: () =>
          import(
            './customer-administration/customer-administration.module'
          ).then((m) => m.CustomerAdministrationModule),
      },
      {
        path: 'address-administration',
        loadChildren: () =>
          import('./address-administration/address-administration.module').then(
            (m) => m.AddressAdministrationModule
          ),
      },
      {
        path: 'reporting',
        loadChildren: () =>
          import('./reporting/reporting.module').then(
            (mod) => mod.ReportingModule
          ),
      },
    ],
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
