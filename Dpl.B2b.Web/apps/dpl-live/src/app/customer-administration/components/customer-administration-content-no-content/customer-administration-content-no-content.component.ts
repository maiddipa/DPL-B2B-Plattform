import { Component, OnInit, ChangeDetectionStrategy } from '@angular/core';

@Component({
  selector: 'dpl-customer-administration-content-no-content',
  templateUrl: './customer-administration-content-no-content.component.html',
  styleUrls: ['./customer-administration-content-no-content.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class CustomerAdministrationContentNoContentComponent implements OnInit {

  constructor() { }

  ngOnInit(): void {
  }

}
