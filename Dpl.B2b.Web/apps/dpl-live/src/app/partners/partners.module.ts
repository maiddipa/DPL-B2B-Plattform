import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';

import { PartnersRoutingModule } from './partners-routing.module';
import { PartnerService } from './services/partner.service';

@NgModule({
  declarations: [],
  imports: [CommonModule, PartnersRoutingModule],
  exports: [],
  entryComponents: [],
  providers: [PartnerService],
})
export class PartnersModule {}
