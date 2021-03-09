import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { SharedModule } from '@app/shared';

import {
  LoadCarrierReceiptComponent,
  LoadCarrierReceiptFormComponent,
} from './components';
import { LoadCarrierReceiptRoutingModule } from './load-carrier-receipt-routing.module';

@NgModule({
  declarations: [LoadCarrierReceiptComponent, LoadCarrierReceiptFormComponent],
  imports: [CommonModule, SharedModule, LoadCarrierReceiptRoutingModule],
})
export class LoadCarrierReceiptModule {}
