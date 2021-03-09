import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ContactRoutingModule } from './contact-routing.module';
import { ContactComponent } from './contact.component';
import { SharedModule } from '@app/shared';

@NgModule({
  declarations: [ContactComponent],
  imports: [CommonModule, SharedModule, ContactRoutingModule],
})
export class ContactModule {}
