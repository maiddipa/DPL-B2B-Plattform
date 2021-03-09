import { AgmCoreModule, LAZY_MAPS_API_CONFIG } from '@agm/core';
import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';

import { APP_CONFIG, DplLiveConfiguration } from '../../config';
import { SharedModule } from '../shared/shared.module';
import { AddToBasketDialogComponent } from './components/add-to-basket-dialog/add-to-basket-dialog.component';
import { CalendarWeekPickerComponent } from './components/calendar-week-picker/calendar-week-picker.component';
import { DayOfWeekPickerComponent } from './components/day-of-week-picker/day-of-week-picker.component';
import { DaySelectorDialogComponent } from './components/day-selector-dialog/day-selector-dialog.component';
import { OrderConfirmationComponent } from './components/order-confirmation/order-confirmation.component';
import { PickupLocationInfoComponent } from './components/pickup-location-info/pickup-location-info.component';
import { SearchCheckoutComponent } from './components/search-checkout/search-checkout.component';
import { SearchInputComponent } from './components/search-input/search-input.component';
import { SearchListComponent } from './components/search-list/search-list.component';
import { SearchResultListComponent } from './components/search-result-list/search-result-list.component';
import { SearchResultMapComponent } from './components/search-result-map/search-result-map.component';
import { SearchRoutingModule } from './search-routing.module';
import { SearchComponent } from './search.component';
import { GeojsonService } from './services/geojson.service';
import { GoogleMapsService } from './services/google-maps.service';
import { SearchService } from './services/search.service';

@NgModule({
  declarations: [
    SearchComponent,
    SearchInputComponent,
    SearchResultListComponent,
    SearchResultMapComponent,
    SearchListComponent,
    PickupLocationInfoComponent,
    DaySelectorDialogComponent,
    SearchCheckoutComponent,
    OrderConfirmationComponent,
    DayOfWeekPickerComponent,
    CalendarWeekPickerComponent,
    AddToBasketDialogComponent,
  ],
  imports: [
    CommonModule,
    SearchRoutingModule,
    SharedModule,
    AgmCoreModule.forRoot(),
  ],
  providers: [
    {
      provide: LAZY_MAPS_API_CONFIG,
      useFactory: (config: DplLiveConfiguration) => {
        return {
          ...config.googleMaps,
          // to manage language  / region via language picker
          // we would need to store it somewhere like localStorage
          language:
            localStorage && localStorage.getItem('language')
              ? localStorage.getItem('language')
              : 'de',
          // region:
          //   localStorage && localStorage.getItem("region")
          //     ? localStorage.getItem("region")
          //     : "DE"
        };
      },
      deps: [APP_CONFIG],
    },

    GoogleMapsService,
    SearchService,
    GeojsonService,
  ],
  entryComponents: [DaySelectorDialogComponent, AddToBasketDialogComponent],
})
export class SearchModule {}
