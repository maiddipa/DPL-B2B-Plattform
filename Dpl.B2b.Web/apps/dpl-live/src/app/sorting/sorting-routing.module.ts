import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { SortingComponent } from './sorting.component';

const routes: Routes = [{ path: ':id', component: SortingComponent }];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class SortingRoutingModule {}
