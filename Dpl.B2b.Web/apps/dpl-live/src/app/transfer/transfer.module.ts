import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { TransferRoutingModule } from './transfer-routing.module';
import { TransferComponent } from './transfer.component';
import { SharedModule } from '@app/shared';
import { TransferFormComponent } from './components/transfer-form/transfer-form.component';

@NgModule({
  declarations: [TransferComponent, TransferFormComponent],
  imports: [CommonModule, SharedModule, TransferRoutingModule],
})
export class TransferModule {}
