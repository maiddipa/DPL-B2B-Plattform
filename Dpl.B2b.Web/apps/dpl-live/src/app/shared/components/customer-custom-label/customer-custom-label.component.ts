import { Component, OnInit, ChangeDetectionStrategy, Input } from '@angular/core';
import { CustomersService } from '../../../customers/services/customers.service';

@Component({
  selector: 'dpl-customer-custom-label',
  templateUrl: './customer-custom-label.component.html',
  styleUrls: ['./customer-custom-label.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class CustomerCustomLabelComponent implements OnInit {
  @Input() labelKey:string;

  constructor(public customersService: CustomersService) { }

  ngOnInit(): void {
  }



}
