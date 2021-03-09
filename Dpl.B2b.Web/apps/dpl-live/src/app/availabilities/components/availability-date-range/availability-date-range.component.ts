import { Component, OnInit, Input, EventEmitter, Output } from '@angular/core';
import { FormGroup } from '@angular/forms';
import * as moment from 'moment';
import { ILoadsPerDay } from '../../services/availabilies.service.types';
import { LoadingLocationsQuery } from '../../../loading-locations/state/loading-locations.query';
import { of, Observable, combineLatest } from 'rxjs';
import { ILoadingLocation } from '../../../loading-locations/state/loading-location.model';
import { map } from 'rxjs/operators';
import { DayOfWeek } from '@app/api/dpl';

interface ViewData {
  workWorkdays: number[];
  validWeekdays: number[];
  today: any;
  isCustomDate(date): string;
}

@Component({
  selector: 'app-availability-date-range',
  templateUrl: './availability-date-range.component.html',
  styleUrls: ['./availability-date-range.component.scss'],
})
export class AvailabilityDateRangeComponent implements OnInit {
  @Input() form: FormGroup;
  @Output() selectionChanged: EventEmitter<ILoadsPerDay[]> = new EventEmitter();

  loads: number;
  startDate: moment.Moment;
  endDate: moment.Moment;
  viewData$: Observable<ViewData>;
  workWeekdays$ = of([1, 2, 3, 4, 5, 6]);
  selectedLoadingLocation$: Observable<ILoadingLocation<number>>;

  constructor(private loadinglocationsquery: LoadingLocationsQuery) {
    this.loads = 1;
  }

  ngOnInit() {
    this.selectedLoadingLocation$ = this.loadinglocationsquery.selectActive();

    this.viewData$ = combineLatest(
      this.selectedLoadingLocation$,

      this.workWeekdays$
    ).pipe(
      map((latest) => {
        const [loadingLocation, workWorkdays] = latest;

        const validWeekdays = loadingLocation.businessHours.map((x) => {
          let dayofWeek = 0;
          switch (x.dayOfWeek) {
            case DayOfWeek.Monday:
              dayofWeek = 1;
              break;
            case DayOfWeek.Tuesday:
              dayofWeek = 2;
              break;

            case DayOfWeek.Wednesday:
              dayofWeek = 3;
              break;

            case DayOfWeek.Thursday:
              dayofWeek = 4;
              break;

            case DayOfWeek.Friday:
              dayofWeek = 5;
              break;

            case DayOfWeek.Saturday:
              dayofWeek = 6;
              break;

            case DayOfWeek.Sunday:
              dayofWeek = 7;
              break;
          }
          return dayofWeek;
        });

        const viewData = {
          validWeekdays,
          workWorkdays,
          today: moment(),
          isCustomDate: (date: moment.Moment) => {
            if (validWeekdays.indexOf(date.weekday() + 1) < 0) {
              return 'disabled';
            }
            return null;
          },
        };

        return viewData;
      })
    );
  }
  choosedDateTime(event) {
    this.startDate = event.startDate;
    this.endDate = event.endDate;
    this.updateSelectedRange();
  }

  updateLoads(loads) {
    this.updateSelectedRange();
  }

  private updateSelectedRange() {
    if (this.startDate && this.endDate) {
      const selectedRange: ILoadsPerDay[] = [];
      selectedRange.push({
        date: this.startDate,
        dateTo: this.endDate,
        loads: this.loads,
      });
      this.selectionChanged.emit(selectedRange);
    }
  }
}
