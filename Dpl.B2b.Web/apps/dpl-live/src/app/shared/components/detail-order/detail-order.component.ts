import {
  Component,
  OnInit,
  ChangeDetectionStrategy,
  Input,
} from '@angular/core';
import { OrderData } from '../../../orders/services';
import * as moment from 'moment';

@Component({
  selector: 'dpl-detail-order',
  templateUrl: './detail-order.component.html',
  styleUrls: ['./detail-order.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class DetailOrderComponent implements OnInit {
  @Input() title: string;
  @Input() order: OrderData;

  constructor() {}

  ngOnInit(): void {}

  dateIsEqual(date1: Date, date2: Date) {
    if (date1 && date2) {
      return moment(date1).isSame(moment(date2), 'day');
    }
    return false;
  }
}
