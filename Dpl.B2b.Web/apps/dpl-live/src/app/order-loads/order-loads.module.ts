import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { SharedModule } from '@app/shared';

import { FiltersModule } from '../filters/filters.module';
import { OrderLoadsListComponent } from './components/order-loads-list/order-loads-list.component';
import { OrderLoadsRoutingModule } from './order-loads-routing.module';
import { OrderLoadsComponent } from './order-loads.component';

@NgModule({
  declarations: [OrderLoadsComponent, OrderLoadsListComponent],
  imports: [CommonModule, SharedModule, OrderLoadsRoutingModule, FiltersModule],
})
export class OrderLoadsModule {}
