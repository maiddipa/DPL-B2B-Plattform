import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { VoucherFilterComponent } from './components/voucher-filter/voucher-filter.component';
import { VoucherFilterInputComponent } from './components/voucher-filter-input/voucher-filter-input.component';
import { VoucherFilterTemplateComponent } from './components/voucher-filter-template/voucher-filter-template.component';
// tslint:disable-next-line:max-line-length
import { VoucherFilterTemplateSaveDialogComponent } from './components/voucher-filter-template-save-dialog/voucher-filter-template-save-dialog.component';
import { FilterService } from './services/filter.service';
import { FilterStore } from './state/filter.store';
import { FilterQuery } from './state/filter.query';
import { FilterTemplateService } from './services/filter-template.service';
import { FilterTemplateStore } from './state/filter-template.store';
import { FilterTemplateQuery } from './state/filter-template.query';
import { SharedModule } from '../shared/shared.module';
import { VoucherFilterTemplateRemoveDialogComponent } from './components/voucher-filter-template-remove-dialog/voucher-filter-template-remove-dialog.component';

import {
  MAT_MOMENT_DATE_FORMATS,
  MomentDateAdapter,
} from '@angular/material-moment-adapter';
import {
  DateAdapter,
  MAT_DATE_FORMATS,
  MAT_DATE_LOCALE,
} from '@angular/material/core';
import { DPL_DATE_FORMATS } from '@dpl/dpl-lib';
import { FilteredTableComponent } from './components/filtered-table/filtered-table.component';

@NgModule({
  declarations: [
    VoucherFilterComponent,
    VoucherFilterInputComponent,
    VoucherFilterTemplateComponent,
    VoucherFilterTemplateSaveDialogComponent,
    VoucherFilterTemplateRemoveDialogComponent,
    FilteredTableComponent,
  ],
  imports: [CommonModule, SharedModule],
  providers: [
    FilterService,
    FilterStore,
    FilterQuery,
    FilterTemplateService,
    FilterTemplateStore,
    FilterTemplateQuery,
    { provide: MAT_DATE_FORMATS, useValue: DPL_DATE_FORMATS },
  ],
  entryComponents: [
    VoucherFilterTemplateSaveDialogComponent,
    VoucherFilterTemplateRemoveDialogComponent,
  ],
  exports: [VoucherFilterComponent, FilteredTableComponent],
})
export class FiltersModule {}
