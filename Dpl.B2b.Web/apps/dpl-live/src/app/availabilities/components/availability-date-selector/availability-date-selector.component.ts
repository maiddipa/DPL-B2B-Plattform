import { Component, OnInit, Input, EventEmitter, Output } from '@angular/core';
import { BehaviorSubject, Observable, combineLatest } from 'rxjs';
import { map } from 'rxjs/operators';
import { ILoadsPerDay } from '../../services/availabilies.service.types';
import { FormGroup } from '@angular/forms';

interface ViewData {
  isFixDate: boolean;
}
@Component({
  selector: 'app-availability-date-selector',
  templateUrl: './availability-date-selector.component.html',
  styleUrls: ['./availability-date-selector.component.scss'],
})
export class AvailabilityDateSelectorComponent implements OnInit {
  @Output() selectionChanged: EventEmitter<ILoadsPerDay[]> = new EventEmitter();
  @Input() form: FormGroup;
  viewData$: Observable<ViewData>;

  dateTypeSubject = new BehaviorSubject(true);
  dateType$ = this.dateTypeSubject.asObservable();

  constructor() {}

  ngOnInit() {
    this.viewData$ = combineLatest([this.dateType$]).pipe(
      map((latest) => {
        const [isFixDate] = latest;
        const viewData = {
          isFixDate,
        };
        return viewData;
      })
    );
  }

  dateTypeChange(event) {
    this.dateTypeSubject.next(event.checked);
    this.selectionChanged.emit([]);
  }

  selectedLoadsPerDay(event) {
    this.selectionChanged.emit(event);
  }
}
