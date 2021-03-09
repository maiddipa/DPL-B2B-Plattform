import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { OrderProcessesRoutingModule } from './order-processes-routing.module';
import { OrderProcessesComponent } from './order-processes.component';
import { SharedModule } from '@app/shared';
import { OrderProcessesListComponent } from './components/order-processes-list/order-processes-list.component';

@NgModule({
  declarations: [OrderProcessesComponent, OrderProcessesListComponent],
  imports: [CommonModule, OrderProcessesRoutingModule, SharedModule],
})
export class OrderProcessesModule {}
