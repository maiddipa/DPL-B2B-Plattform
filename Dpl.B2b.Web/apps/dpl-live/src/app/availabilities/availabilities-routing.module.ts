import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AvailabilitiesComponent } from './availabilities.component';
import { AuthGuard } from '../core/auth.guard';
import { NeedsComponent } from './components/needs/needs.component';

const routes: Routes = [
  {
    path: 'availabilities',
    children: [
      {
        path: '',
        pathMatch: 'full',
        redirectTo: 'start',
      },
      {
        path: 'start',
        component: AvailabilitiesComponent,
      },
    ],
  },
  {
    path: 'needs',
    children: [
      {
        path: '',
        pathMatch: 'full',
        redirectTo: 'start',
      },
      {
        path: 'start',
        component: NeedsComponent,
      },
    ],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class AvailabilitiesRoutingModule {}
