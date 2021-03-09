import { Component, OnInit, Input } from '@angular/core';
import { LoadCarrierReceipt } from '@app/api/dpl';

@Component({
  selector: 'dpl-detail-load-carrier-receipt',
  templateUrl: './detail-load-carrier-receipt.component.html',
  styleUrls: ['./detail-load-carrier-receipt.component.scss'],
})
export class DetailLoadCarrierReceiptComponent implements OnInit {
  @Input() receipt: LoadCarrierReceipt;
  constructor() {}

  ngOnInit(): void {}
}
