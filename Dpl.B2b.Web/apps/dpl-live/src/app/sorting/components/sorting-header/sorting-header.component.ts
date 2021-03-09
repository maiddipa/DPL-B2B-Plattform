import {
  Component,
  OnInit,
  ChangeDetectionStrategy,
  Input,
} from '@angular/core';
import { LoadCarrierReceipt } from '../../../core/services/dpl-api-services';

@Component({
  selector: 'dpl-sorting-header',
  templateUrl: './sorting-header.component.html',
  styleUrls: ['./sorting-header.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class SortingHeaderComponent implements OnInit {
  @Input() receipt: LoadCarrierReceipt;

  constructor() {}

  ngOnInit(): void {
    console.log('receipt', this.receipt);
  }
}
