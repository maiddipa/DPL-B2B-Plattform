import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'app-account-summary-row',
  templateUrl: './account-summary-row.component.html',
  styleUrls: ['./account-summary-row.component.scss'],
})
export class AccountSummaryRowComponent implements OnInit {
  @Input() displayName: string;
  @Input() additionalText: string;
  @Input() additionalQuantity: number;
  @Input() intact: number;
  @Input() defect: number;
  @Input() intactSubTotal: number;
  @Input() defectSubTotal: number;

  constructor() {}

  ngOnInit() {}
}
