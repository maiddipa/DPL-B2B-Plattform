import { Component, OnInit } from '@angular/core';
import { AccountsService } from './services/accounts.service';
import { Observable } from 'rxjs';
import { IAccount } from './state/account.model';
import { ActiveAccountLoadCarrierType } from './services/accounts.service.types';
import { UserService } from '../user/services/user.service';
import { UserRole } from '@app/api/dpl';
import { map } from 'rxjs/operators';

@Component({
  selector: 'app-accounts',
  templateUrl: './accounts.component.html',
  styleUrls: ['./accounts.component.css'],
})
export class AccountsComponent implements OnInit {
  selectedAccount$: Observable<IAccount>;
  selectorExpanded$: Observable<boolean>;
  activeAccountLoadCarrierType$: Observable<ActiveAccountLoadCarrierType>;
  userRole$: Observable<UserRole>;
  role = UserRole;

  constructor(
    private accounstService: AccountsService,
    private userService: UserService
  ) {}

  ngOnInit() {
    this.selectedAccount$ = this.accounstService.getActiveAccount();
    this.selectorExpanded$ = this.accounstService.getSelectorExpanded();
    this.activeAccountLoadCarrierType$ = this.accounstService.getActiveAccountAndLoadCarrierType();
    this.userRole$ = this.userService
      .getCurrentUser()
      .pipe(map((user) => user.role));
  }

  expandedChange(expanded: boolean) {
    this.accounstService.setSelectorExpanded(expanded);
  }
}
