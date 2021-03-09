import {
  Component,
  OnInit,
  ChangeDetectionStrategy,
  Input,
} from '@angular/core';
import { OrderLoad, OrderTransportType } from '@app/api/dpl';

@Component({
  selector: 'dpl-detail-order-load',
  templateUrl: './detail-order-load.component.html',
  styleUrls: ['./detail-order-load.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class DetailOrderLoadComponent implements OnInit {
  transportType = OrderTransportType;
  @Input() load: OrderLoad;

  constructor() {}

  ngOnInit(): void {}
}
