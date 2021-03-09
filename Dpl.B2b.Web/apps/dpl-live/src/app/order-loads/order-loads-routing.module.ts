import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { OrderLoadsComponent } from './order-loads.component';

const routes: Routes = [
  { path: '', pathMatch: 'full', component: OrderLoadsComponent },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class OrderLoadsRoutingModule {}
