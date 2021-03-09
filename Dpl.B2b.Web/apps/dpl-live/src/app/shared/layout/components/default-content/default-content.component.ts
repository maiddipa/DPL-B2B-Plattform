import { Component, OnInit } from '@angular/core';
import { combineLatest, Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { UserRole } from '../../../../core/services/dpl-api-services';
import { CustomersService } from '../../../../customers/services/customers.service';
import { UserService } from '../../../../user/services/user.service';

type ViewData = {
  userRole: UserRole;
  customerId: number | null;
};

@Component({
  selector: 'dpl-default-content',
  templateUrl: './default-content.component.html',
  styleUrls: ['./default-content.component.scss'],
})
export class DefaultContentComponent implements OnInit {
  userRole = UserRole;

  viewData$: Observable<ViewData>;
  constructor(
    private userData: UserService,
    private customers: CustomersService
  ) {}

  ngOnInit(): void {
    const user$ = this.userData.getCurrentUser();

    const customerId$ = this.customers
      .getActiveCustomer()
      .pipe(map((customer) => (customer ? customer.id : null)));

    this.viewData$ = combineLatest([user$, customerId$]).pipe(
      map(([user, customerId]) => {
        return <ViewData>{
          userRole: user ? user.role : undefined,
          customerId,
        };
      })
    );
  }
}
