import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { VoucherRegisterRoutingModule } from './voucher-register-routing.module';
import { VoucherRegisterComponent } from './voucher-register.component';
import { SharedModule } from '../shared/shared.module';
import { VoucherListComponent } from './components/voucher-list/voucher-list.component';
import { VoucherSummaryComponent } from './components/voucher-summary/voucher-summary.component';
import { FiltersModule } from '../filters/filters.module';
import { LoadCarrierTypeSelectorComponent } from './components/load-carrier-type-selector/load-carrier-type-selector.component';

@NgModule({
  declarations: [
    VoucherRegisterComponent,
    VoucherListComponent,
    VoucherSummaryComponent,
    LoadCarrierTypeSelectorComponent,
  ],
  imports: [
    CommonModule,
    VoucherRegisterRoutingModule,
    SharedModule,
    FiltersModule,
  ],
  providers: [],
})
export class VoucherRegisterModule {}
