import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { LoadCarrierReceiptsComponent } from './load-carrier-receipts.component';

const routes: Routes = [{ path: '', component: LoadCarrierReceiptsComponent }];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class LoadCarrierReceiptsRoutingModule { }
