import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { VoucherRoutingModule } from './voucher-routing.module';
import { VoucherComponent } from './voucher.component';
import { VoucherFormComponent } from './voucher-form/voucher-form.component';
import { SharedModule } from '@app/shared';

@NgModule({
  declarations: [VoucherComponent, VoucherFormComponent],
  imports: [CommonModule, VoucherRoutingModule, SharedModule],
})
export class VoucherModule {}
