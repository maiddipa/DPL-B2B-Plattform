import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import {
  DxReportDesignerModule,
  DxReportViewerModule,
} from 'devexpress-reporting-angular';

import { ReportDesignerComponent } from './components/reportdesigner/report-designer.component';
import { ReportViewerComponent } from './components/reportviewer/report-viewer.component';
import { ReportingRoutingModule } from './reporting-routing.module';
import { ReportingAuthGuard } from './reporting-auth.guard';

@NgModule({
  declarations: [ReportViewerComponent, ReportDesignerComponent],
  imports: [
    CommonModule,
    ReportingRoutingModule,
    DxReportViewerModule,
    DxReportDesignerModule,
  ],
  exports: [ReportViewerComponent, ReportDesignerComponent],
  providers: [ReportingAuthGuard],
})
export class ReportingModule {}
