import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { ReportDesignerComponent } from './components/reportdesigner/report-designer.component';
import { ReportViewerComponent } from './components/reportviewer/report-viewer.component';
import { ReportingAuthGuard } from './reporting-auth.guard';

const routes: Routes = [
  {
    path: '',
    canActivate: [ReportingAuthGuard],
    canActivateChild: [ReportingAuthGuard],
    children: [
      {
        path: 'designer',
        component: ReportDesignerComponent,
      },
      {
        path: 'viewer',
        component: ReportViewerComponent,
      },
    ],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class ReportingRoutingModule {}
