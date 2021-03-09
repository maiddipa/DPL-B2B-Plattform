import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { OrdersRoutingModule } from './orders-routing.module';
import { OrdersComponent } from './orders.component';
import { SharedModule } from '@app/shared';
import { FiltersModule } from '../filters/filters.module';
import { OrdersListComponent } from './components';

@NgModule({
  declarations: [OrdersComponent, OrdersListComponent],
  imports: [CommonModule, OrdersRoutingModule, SharedModule, FiltersModule],
})
export class OrdersModule {}
