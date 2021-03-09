import { Component, OnInit, Input } from '@angular/core';
import {
  NgxSingleFieldSubFormComponent,
  WrappedControlForm,
} from '@dpl/dpl-lib';
import { AccountsService } from 'apps/dpl-live/src/app/accounts/services/accounts.service';
import { Observable, combineLatest, EMPTY, Subject, merge } from 'rxjs';
import { IAccount } from 'apps/dpl-live/src/app/accounts/state/account.model';
import { subformComponentProviders } from 'ngx-sub-form';
import { tap, switchMap, startWith, takeUntil, map } from 'rxjs/operators';

type AccountSelectorType = {
  id: number;
  name: string;
};

@Component({
  selector: 'app-posting-account-picker',
  template: `
    <mat-form-field
      fxFlex
      class="app-form-field-auto"
      *ngIf="formGroup"
      [formGroup]="formGroup"
    >
      <mat-label>
        <span *ngIf="label; else fallbackLabel">{{ label }}</span>
        <ng-template #fallbackLabel>
          <span i18n="BookingAccount|Label Buchungskonto@@BookingAccount"
            >Buchungskonto</span
          >
        </ng-template>
      </mat-label>
      <mat-select [formControl]="formGroup.controls.innerControl">
        <mat-option
          *ngFor="let account of accounts$ | async"
          [value]="account.id"
        >
          {{ account.name }}
        </mat-option>
      </mat-select>
    </mat-form-field>
  `,
  styles: [],
  providers: subformComponentProviders(PostingAccountPickerComponent),
})
export class PostingAccountPickerComponent
  extends NgxSingleFieldSubFormComponent<number>
  implements OnInit {
  @Input() context: 'Source' | 'Destination' = 'Source';
  @Input() label: string;
  accounts$: Observable<AccountSelectorType[]>;

  defaultValue: WrappedControlForm<number>;

  constructor(private accountService: AccountsService) {
    super();
  }

  ngOnInit() {
    const accountId$ = this.formGroup.controls.innerControl.valueChanges.pipe(
      tap((accountId: number) => {
        if (!accountId) {
          return;
        }
        this.defaultValue = {
          innerControl: accountId,
        };
        if (this.context === 'Source') {
          this.accountService.setActiveAccount(accountId);
        }
      })
    );

    const accountsContext$ =
      this.context === 'Source'
        ? this.accountService.getAllAccounts().pipe(
            map((accounts) => {
              return accounts.map((account) => {
                const accountSelectorType: AccountSelectorType = {
                  id: account.id,
                  name: account.name,
                };
                return accountSelectorType;
              });
            })
          )
        : this.accountService.getAllowedDestinationAccounts().pipe(
            map((accounts) => {
              return accounts.map((account) => {
                const accountSelectorType: AccountSelectorType = {
                  id: account.postingAccountId,
                  name: account.displayName,
                };
                return accountSelectorType;
              });
            })
          );

    const selectedAccount$ = this.accountService.getActiveAccount();

    const formSync$ = combineLatest([accountId$]).pipe(
      switchMap(() => EMPTY) // we do not wanne trigger new view data
      //startWith(null) // needed so viewdata outputs values
    );

    this.accounts$ = combineLatest([accountsContext$, selectedAccount$]).pipe(
      takeUntil(formSync$),
      tap(([accounts, selectedAccount]) => {
        console.log('ACCOUNT ID', accounts, selectedAccount);
        if (
          !accounts ||
          accounts.length === 0 ||
          (this.formGroup.controls.innerControl.value &&
            accounts.some(
              (x) => x.id === this.formGroup.controls.innerControl.value
            ))
        ) {
          return;
        }
        //For destination don't pre select first account
        if (this.context === 'Source') {
          this.formGroup.controls.innerControl.setValue(
            selectedAccount ? selectedAccount.id : accounts[0].id
          );
        }
      }),
      map(([accounts, selectedAccount]) => {
        return accounts;
      })
    );
  }

  protected getDefaultValues() {
    return this.defaultValue;
  }
}
