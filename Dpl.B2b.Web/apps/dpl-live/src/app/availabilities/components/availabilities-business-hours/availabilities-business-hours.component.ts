import {
  Component,
  OnInit,
  Input,
  OnChanges,
  SimpleChanges,
} from '@angular/core';
import { BusinessHours } from 'apps/dpl-live/src/app/core/services/dpl-api-services';
import { BusinessHourDayComponent } from '../business-hour-day/business-hour-day.component';
import * as _ from 'lodash';
import * as moment from 'moment';

interface BusinessHoursOverview {
  day: string;
  businessHours: BusinessHours[];
}

@Component({
  selector: 'app-availabilities-business-hours',
  templateUrl: './availabilities-business-hours.component.html',
  styleUrls: ['./availabilities-business-hours.component.scss'],
})
export class AvailabilitiesBusinessHoursComponent implements OnChanges {
  @Input() businessHours: BusinessHours[];
  bh: BusinessHoursOverview[];
  constructor() {}

  ngOnInit() {}

  ngOnChanges(changes: SimpleChanges): void {
    this.bh = [];
    const group = _.mapValues(
      _.groupBy(this.businessHours, (x) =>
        this.compareBusinessHours(x.dayOfWeek)
      ),
      (v) => _.sortBy(v, 'fromTime')
    );

    _.forEach(group, (x, key) => {
      this.bh.push({
        day: moment()
          .isoWeekday(+key)
          .format('dddd'),
        businessHours: x,
      });
    });
  }

  compareBusinessHours(day: string) {
    let aDay = 0;
    switch (day) {
      case 'Monday':
        aDay = 1;
        break;
      case 'Tuesday':
        aDay = 2;
        break;
      case 'Wednesday':
        aDay = 3;
        break;
      case 'Thursday':
        aDay = 4;
        break;
      case 'Friday':
        aDay = 5;
        break;
      case 'Saturday':
        aDay = 6;
        break;
      case 'Sunday':
        aDay = 7;
        break;
    }
    return aDay;
  }
}
