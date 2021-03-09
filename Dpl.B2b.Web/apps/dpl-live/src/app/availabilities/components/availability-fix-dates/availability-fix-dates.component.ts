import { Component, OnInit, Input, EventEmitter, Output } from '@angular/core';
import * as moment from 'moment';
import { BehaviorSubject, Observable, combineLatest, of } from 'rxjs';
import { map, first, tap, filter } from 'rxjs/operators';
import * as _ from 'lodash';
import { ILoadsPerDay } from '../../services/availabilies.service.types';
import { LoadingLocationsQuery } from '../../../loading-locations/state/loading-locations.query';
import { DayOfWeek } from '@app/api/dpl';
import { ILoadingLocation } from '../../../loading-locations/state/loading-location.model';
import { DayOfWeekPickerComponent } from '../../../search/components/day-of-week-picker/day-of-week-picker.component';

interface ViewData {
  currentWeek: moment.Moment;

  selectedDates: any[];
  workWorkdays: number[];
  validWeekdays: number[];
  loads: number;
  lastWeekActive: number;
  nextWeekActive: number;
}

@Component({
  selector: 'app-availability-fix-dates',
  templateUrl: './availability-fix-dates.component.html',
  styleUrls: ['./availability-fix-dates.component.scss'],
})
export class AvailabilityFixDatesComponent implements OnInit {
  @Output() selectionChanged: EventEmitter<ILoadsPerDay[]> = new EventEmitter();
  today: moment.Moment;
  selectedDates: ILoadsPerDay[];

  viewData$: Observable<ViewData>;
  kwSubject = new BehaviorSubject(moment());
  kw$ = this.kwSubject.asObservable();

  selectedDatesSubject = new BehaviorSubject<ILoadsPerDay[]>([]);
  selectedDates$ = this.selectedDatesSubject.asObservable();

  selectedLoadingLocation$: Observable<ILoadingLocation<number>>;
  workWeekdays$ = of([1, 2, 3, 4, 5, 6]);
  constructor(private loadinglocationsquery: LoadingLocationsQuery) {
    this.selectedDates = [];
  }

  ngOnInit() {
    this.today = moment().endOf('day');
    this.selectedLoadingLocation$ = this.loadinglocationsquery
      .selectActive()
      .pipe(filter((location) => !!location));

    this.viewData$ = combineLatest(
      this.kw$,
      this.selectedLoadingLocation$,
      this.selectedDates$,
      this.workWeekdays$
    ).pipe(
      map((latest) => {
        const [kw, loadingLocation, allSelectedDates, workWorkdays] = latest;

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
        const kwCopy = moment(kw);
        const weekstart = moment(kwCopy.day(workWorkdays[0]));
        const weekend = moment(
          kwCopy.day(workWorkdays[workWorkdays.length - 1])
        );
        // find selected dates for this week
        const currentWeekSelectedDates = _.filter(allSelectedDates, (x) =>
          x.date.isBetween(weekstart, weekend, 'day', '[]')
        );
        // transform to this week's selection
        const selectedDates = [];
        currentWeekSelectedDates.forEach((loadPerDay) => {
          selectedDates[loadPerDay.date.isoWeekday()] = loadPerDay.loads;
        });

        const lastWeekActive = _.sumBy(
          _.filter(allSelectedDates, (x) => x.date.isBefore(weekstart, 'day')),
          (x) => x.loads
        );
        const nextWeekActive = _.sumBy(
          _.filter(allSelectedDates, (x) => x.date.isAfter(weekend, 'day')),
          (x) => x.loads
        );

        const viewData = {
          currentWeek: kw,
          selectedDates,
          validWeekdays,
          workWorkdays,
          lastWeekActive,
          nextWeekActive,
          loads: _.sumBy(allSelectedDates, (x) => x.loads),
        };

        return viewData;
      }),
      tap((viewData) => {
        const today = moment();
        if (viewData.currentWeek.isSame(today, 'day')) {
          let openDaysThisWeek = false;
          viewData.workWorkdays.forEach((weekday) => {
            if (viewData.validWeekdays.indexOf(weekday) !== -1) {
              openDaysThisWeek = true;
            }
          });
          if (!openDaysThisWeek) {
            this.nextWeek(viewData.currentWeek);
          }
        }
      })
    );
  }

  nextWeek(currentDate) {
    this.kwSubject.next(moment(currentDate).add(1, 'week'));
  }

  previousWeek(currentDate) {
    this.kwSubject.next(moment(currentDate).subtract(1, 'week'));
  }

  updateSelectedDate(loads: number, date: moment.Moment) {
    this.selectedDates$.pipe(first()).subscribe((selectedDates) => {
      const _selectedDates = selectedDates;
      const index = _selectedDates.findIndex((x) => x.date.isSame(date, 'day'));
      if (index >= 0) {
        _selectedDates[index].loads = loads;
      } else {
        _selectedDates.push({ date: moment(date), loads });
      }
      this.selectedDatesSubject.next(_selectedDates);
      this.selectionChanged.emit(_selectedDates);
    });
  }
}
