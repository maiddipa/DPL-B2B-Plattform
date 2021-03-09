import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { VoucherRegisterComponent } from './voucher-register.component';
import { AuthGuard } from '../core/auth.guard';

const routes: Routes = [
  {
    path: '',
    pathMatch: 'full',
    redirectTo: 'start',
  },
  {
    path: 'start',
    pathMatch: 'full',
    component: VoucherRegisterComponent,
  },
  {
    path: 'start/:carrierType',
    component: VoucherRegisterComponent,
  },
  {
    path: 'start/:carrierType:highlightId',
    component: VoucherRegisterComponent,
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class VoucherRegisterRoutingModule {}
