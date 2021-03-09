import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';

import { FiltersModule } from '../filters/filters.module';
import { SharedModule } from '../shared/shared.module';
import { AvailabilitiesRoutingModule } from './availabilities-routing.module';
import { AvailabilitiesComponent } from './availabilities.component';
import { AvailabilitiesBusinessHoursComponent } from './components/availabilities-business-hours/availabilities-business-hours.component';
import { AvailabilitiesContainerComponent } from './components/availabilities-container/availabilities-container.component';
import { AvailabilityDateRangeComponent } from './components/availability-date-range/availability-date-range.component';
import { AvailabilityDateSelectorComponent } from './components/availability-date-selector/availability-date-selector.component';
import { AvailabilityFixDatesComponent } from './components/availability-fix-dates/availability-fix-dates.component';
import { AvailabilityRowComponent } from './components/availability-row/availability-row.component';
import { BusinessHourDayComponent } from './components/business-hour-day/business-hour-day.component';
import { LocationBusinessHoursComponent } from './components/location-business-hours/location-business-hours.component';
import { NeedsComponent } from './components/needs/needs.component';

@NgModule({
  declarations: [
    AvailabilitiesComponent,
    BusinessHourDayComponent,
    LocationBusinessHoursComponent,
    AvailabilityDateSelectorComponent,
    AvailabilityDateRangeComponent,
    AvailabilityFixDatesComponent,
    AvailabilitiesContainerComponent,
    AvailabilityRowComponent,
    AvailabilitiesBusinessHoursComponent,
    NeedsComponent,
  ],
  imports: [
    CommonModule,
    AvailabilitiesRoutingModule,
    SharedModule,
    FiltersModule,
  ],
})
export class AvailabilitiesModule {}
