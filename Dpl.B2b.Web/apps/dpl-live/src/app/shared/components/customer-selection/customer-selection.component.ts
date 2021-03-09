import { Component, OnInit } from '@angular/core';
import { ICustomer } from '../../../customers/state/customer.model';
import { FormControl } from '@angular/forms';
import { of, Observable } from 'rxjs';
import { switchMap, startWith, map, first, filter } from 'rxjs/operators';
import { DplApiService } from '../../../core/services';
import {
  DplEmployeeCustomer,
  DplEmployeeCustomerSelectionEntryType,
} from '@app/api/dpl';
import { UserService } from '../../../user/services/user.service';
import { AddressPipe } from '@app/shared/pipes';

@Component({
  selector: 'dpl-customer-selection',
  templateUrl: './customer-selection.component.html',
  styleUrls: ['./customer-selection.component.scss'],
})
export class CustomerSelectionComponent implements OnInit {
  customerSelected: boolean;
  options$: Observable<DplEmployeeCustomer[]>;
  activeCustomer: DplEmployeeCustomer;
  customerControl = new FormControl();
  type = DplEmployeeCustomerSelectionEntryType;

  constructor(
    private dpl: DplApiService,
    private userService: UserService,
    private address: AddressPipe
  ) {
    this.customerSelected = false;
  }

  ngOnInit() {
    this.options$ = this.customerControl.valueChanges.pipe(
      startWith(''),
      filter((value: string) => value.length > 1),
      switchMap((value) => {
        return this.dpl.employeeService
          .searchCustomers({ searchTerm: value })
          .pipe(
            map((results) => {
              console.log(results);
              return results;
            })
          );
      })
    );
  }

  onOptionSelected(customer: DplEmployeeCustomer) {
    this.activeCustomer = customer;
    // TODO call refresh user with param
    this.userService.refreshUserData(customer).pipe(first()).subscribe();
  }

  // using a variable instead of a function prevents us from having to use bind(this)
  displayFn = (customer: DplEmployeeCustomer) => {
    return customer
      ? `customer.displayName | ${this.address.transform(
          customer.address,
          'long'
        )}`
      : '';
  };

  reset() {
    this.activeCustomer = undefined;
    this.customerControl.setValue(null);
    this.userService.refreshUserData().pipe(first()).subscribe();
  }
}

export enum CustomerSelectionEntryType {
  Customer = 'Customer',
  Division = 'Division',
  Account = 'Account',
}
