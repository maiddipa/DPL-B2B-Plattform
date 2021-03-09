import { NgModule } from '@angular/core';
import {
  DxButtonModule,
  DxDataGridModule,
  DxFormModule,
  DxLookupModule,
  DxPopoverModule,
  DxPopupModule,
  DxSelectBoxModule,
  DxDateBoxModule,
  DxTagBoxModule,
  DxTemplateModule,
  DxTextBoxModule,
  DxToolbarModule,
  DxTabsModule,
  DxTabPanelModule,
  DxValidatorModule,
  DxValidationGroupModule,
  DxValidationSummaryModule
} from 'devextreme-angular';

@NgModule({
  exports: [
    DxButtonModule,
    DxDataGridModule,
    DxTagBoxModule,
    DxSelectBoxModule,
    DxDateBoxModule,
    DxPopoverModule,
    DxPopupModule,
    DxTemplateModule,
    DxLookupModule,
    DxFormModule,
    DxTextBoxModule,
    DxToolbarModule,
    DxTabsModule,
    DxTabPanelModule,
    DxValidatorModule,
    DxValidationGroupModule,
    DxValidationSummaryModule
  ],
})
export class DevExtremeModule {}
