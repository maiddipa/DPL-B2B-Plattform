import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';

import { FiltersModule } from '../filters/filters.module';
import { SharedModule } from '../shared/shared.module';
import { TransportBidFormComponent } from './components/transport-bid-form.component';
import { TransportBidsComponent } from './components/transport-bids.component';
import { TransportDetailsComponent } from './components/transport-details.component';
import { TransportLocationDetailsComponent } from './components/transport-location-details.component';
import { TransportComponent } from './components/transport.component';
import { TransportsService } from './services/transports.service';
import { TransportsRoutingModule } from './transports-routing.module';
import { TransportsComponent } from './transports.component';

@NgModule({
  declarations: [
    TransportsComponent,
    TransportComponent,
    TransportDetailsComponent,
    TransportLocationDetailsComponent,
    TransportBidFormComponent,
    TransportBidsComponent,
  ],
  imports: [CommonModule, TransportsRoutingModule, SharedModule, FiltersModule],
  providers: [TransportsService],
})
export class TransportsModule {}
