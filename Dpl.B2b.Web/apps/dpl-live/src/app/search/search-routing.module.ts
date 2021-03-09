import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { SearchCheckoutComponent } from './components/search-checkout/search-checkout.component';
import { AuthGuard } from '../core/auth.guard';
import { SearchComponent } from './search.component';
import { OrderConfirmationComponent } from './components/order-confirmation/order-confirmation.component';
import { BasketGuard } from './guard/basket.guard';
import { PermissionGuard } from '../shared/guards/permission.guard';
import {
  PermissionResourceType,
  ResourceAction,
} from '../core/services/dpl-api-services';

const routes: Routes = [
  {
    path: '',
    pathMatch: 'full',
    redirectTo: 'start',
  },
  {
    path: 'start',
    component: SearchComponent,
  },
  {
    path: 'start/:basketId',
    component: SearchComponent,
    canActivate: [BasketGuard],
  },
  {
    path: 'checkout/:basketId',
    component: SearchCheckoutComponent,
    canActivate: [BasketGuard],
  },
  {
    path: 'confirmation/:basketId',
    component: OrderConfirmationComponent,
    canActivate: [BasketGuard],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class SearchRoutingModule {}
