import { Component, OnInit } from '@angular/core';
import { AccountsService } from '../accounts/services/accounts.service';
import { UserService } from '../user/services/user.service';
import { Observable } from 'rxjs';
import { switchMap, map, first } from 'rxjs/operators';

@Component({
  selector: 'dpl-transfer',
  templateUrl: './transfer.component.html',
  styleUrls: ['./transfer.component.scss'],
})
export class TransferComponent implements OnInit {
  viewData$: Observable<boolean>;

  constructor(
    private accountsService: AccountsService,
    private userService: UserService
  ) {}

  ngOnInit() {
    // hack refresh active account balances
    this.viewData$ = this.accountsService.getActiveAccount().pipe(
      first(),
      switchMap((account) => {
        return this.userService.updateAccountBalance(account.id);
      }),
      map(() => true)
    );
  }
}
