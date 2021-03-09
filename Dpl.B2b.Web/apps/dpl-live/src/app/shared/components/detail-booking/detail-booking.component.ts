import {
  Component,
  OnInit,
  ChangeDetectionStrategy,
  Input,
} from '@angular/core';
import { AccountingRecord } from '@app/api/dpl';

@Component({
  selector: 'dpl-detail-booking',
  templateUrl: './detail-booking.component.html',
  styleUrls: ['./detail-booking.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class DetailBookingComponent implements OnInit {
  @Input() booking: AccountingRecord;

  constructor() {}

  ngOnInit(): void {}
}
