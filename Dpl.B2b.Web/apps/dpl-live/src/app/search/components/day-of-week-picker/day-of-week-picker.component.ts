import { ChangeDetectionStrategy, Component, Input } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { I18n } from '@ngx-translate/i18n-polyfill';

@Component({
  selector: 'app-day-of-week-picker',
  templateUrl: './day-of-week-picker.component.html',
  styleUrls: ['./day-of-week-picker.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class DayOfWeekPickerComponent {
  @Input() calendarWeek: Date;
  @Input() form: FormGroup;
  @Input() arrayName: string;

  daysOfWeekOptions = [
    $localize`:MondayAbr|Abkürzung Montag@@MondayAbr:Mo`,
    $localize`:TuesdayAbr|Abkürzung Dienstag@@TuesdayAbr:Di`,
    $localize`:WednesdayAbr|Abkürzung Mittwoch@@WednesdayAbr:Mi`,
    $localize`:ThursdayAbr|Abkürzung Donnerstag@@ThursdayAbr:Do`,
    $localize`:MondayAbr|Abkürzung Freitag@@FridayAbr:Fr`,
    $localize`:SaturdayAbr|Abkürzung Samstag@@SaturdayAbr:Sa`,
  ];

  constructor(private i18n: I18n) {}

}
