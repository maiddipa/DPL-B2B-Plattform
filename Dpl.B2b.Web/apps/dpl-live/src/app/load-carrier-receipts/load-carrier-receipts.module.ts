import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { LoadCarrierReceiptsRoutingModule } from './load-carrier-receipts-routing.module';
import { LoadCarrierReceiptsComponent } from './load-carrier-receipts.component';
import { LoadCarrierReceiptsListComponent } from './components/load-carrier-receipts-list/load-carrier-receipts-list.component';
import { SharedModule } from '../shared';


@NgModule({
  declarations: [LoadCarrierReceiptsComponent, LoadCarrierReceiptsListComponent],
  imports: [
    CommonModule,
    SharedModule,
    LoadCarrierReceiptsRoutingModule
  ]
})
export class LoadCarrierReceiptsModule { }
