import {
  Component,
  OnInit,
  ChangeDetectionStrategy,
  OnChanges,
  Input,
  SimpleChanges,
} from '@angular/core';
import { BusinessHours, DayOfWeek } from '@app/api/dpl';
import { Observable, of } from 'rxjs';
import { map } from 'rxjs/operators';
import * as _ from 'lodash';

type ViewData = {
  days: {
    value: DayOfWeek;
    hours: BusinessHours[];
  }[];
};

@Component({
  selector: 'dpl-business-hours',
  template: `
    <ng-container *ngIf="viewData$ | async as data">
      <div fxLayout="column" fxLayoutGap="5px">
        <div
          *ngFor="let day of data.days"
          fxLayout="row"
          fxLayoutAlign=" stretch"
          fxLayoutGap="10px"
        >
          <span fxFlex="1 0 8em">{{ day.value | dayOfWeek }}</span>
          <div fxFlex="1 0 8em" fxLayout="column">
            <span *ngFor="let hours of day.hours"
              >{{ hours.fromTime | date: 'HH:mm':'+0000' }} -
              {{ hours.toTime | date: 'HH:mm':'+0000' }}</span
            >
          </div>
        </div>
      </div>
    </ng-container>
  `,
  styles: [],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class BusinessHoursComponent implements OnChanges {
  @Input() businessHours: BusinessHours[];
  viewData$: Observable<ViewData>;
  constructor() {}

  ngOnChanges(changes: SimpleChanges) {
    this.viewData$ = of(this.businessHours).pipe(
      map((businessHours) => {
        const days = _(businessHours)
          .groupBy((i) => i.dayOfWeek)
          .mapValues((hoursOfDay) => {
            const hours = _(hoursOfDay)
              .sortBy((i) => i.fromTime)
              .value();
            return {
              value: hoursOfDay[0].dayOfWeek,
              hours,
            };
          })
          .toArray()
          .value();

        const viewData: ViewData = {
          days,
        };

        return viewData;
      })
    );
  }
}
