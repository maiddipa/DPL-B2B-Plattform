import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { LoadCarrierReceiptComponent } from './components';

const routes: Routes = [
  { path: 'create', pathMatch: 'full', component: LoadCarrierReceiptComponent },
  { path: 'create/:digitalCode', component: LoadCarrierReceiptComponent },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class LoadCarrierReceiptRoutingModule {}
