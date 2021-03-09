import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { SharedModule } from '@app/shared/shared.module';

import { CustomerDivisionSelectorComponent } from './components/customer-division-selector/customer-division-selector.component';
import { CustomersRoutingModule } from './customers-routing.module';
import { CoreModule } from '../core/core.module';
import { UserService } from '../user/services/user.service';

@NgModule({
  declarations: [CustomerDivisionSelectorComponent],
  imports: [CommonModule, CoreModule, CustomersRoutingModule],
  exports: [CustomerDivisionSelectorComponent],
  providers: [UserService],
})
export class CustomersModule {}
