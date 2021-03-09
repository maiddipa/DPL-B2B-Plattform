import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AddressAdministrationRoutingModule } from './address-administration-routing.module';
import { AddressAdministrationComponent } from './address-administration.component';
import { AddressAdministrationGridComponent } from './components/address-administration-grid/address-administration-grid.component';
import { SharedModule } from '../shared';


@NgModule({
  declarations: [AddressAdministrationComponent, AddressAdministrationGridComponent],
  imports: [ 
    CommonModule,
    AddressAdministrationRoutingModule,
    SharedModule,
  ]
})
export class AddressAdministrationModule { }
