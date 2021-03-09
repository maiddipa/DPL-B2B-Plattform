import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { TransportComponent } from './components/transport.component';
import { TransportsComponent } from './transports.component';

const routes: Routes = [
  {
    path: '',
    pathMatch: 'full',
    component: TransportsComponent,
  },
  {
    path: ':id',
    component: TransportComponent,
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class TransportsRoutingModule {}
