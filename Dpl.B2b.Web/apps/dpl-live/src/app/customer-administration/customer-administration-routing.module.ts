import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { CustomerAdministrationComponent } from './customer-administration.component';

const routes: Routes = [{ path: '', component: CustomerAdministrationComponent }];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CustomerAdministrationRoutingModule { }
