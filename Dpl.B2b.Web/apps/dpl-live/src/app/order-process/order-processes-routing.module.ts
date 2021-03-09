import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { OrderProcessesComponent } from './order-processes.component';

const routes: Routes = [{ path: '', component: OrderProcessesComponent }];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class OrderProcessesRoutingModule {}
