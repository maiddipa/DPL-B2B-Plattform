import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';

import { AccountsComponent } from '../accounts/accounts.component';
import { FiltersModule } from '../filters/filters.module';
import { SharedModule } from '../shared/shared.module';
import { AccountsRoutingModule } from './accounts-routing.module';
import { AccountBookingsComponent } from './components/account-bookings/account-bookings.component';
import { AccountSelectorArticlesComponent } from './components/account-selector-articles/account-selector-articles.component';
import { AccountSummaryRowComponent } from './components/account-summary-row/account-summary-row.component';
import { AccountSummaryComponent } from './components/account-summary/account-summary.component';
import { AccountsService } from './services/accounts.service';
import { AccountsQuery } from './state/accounts.query';
import { AccountsStore } from './state/accounts.store';

@NgModule({
  declarations: [
    AccountsComponent,
    AccountSelectorArticlesComponent,
    AccountSummaryComponent,
    AccountBookingsComponent,
    AccountSummaryRowComponent,
  ],
  imports: [CommonModule, SharedModule, AccountsRoutingModule, FiltersModule],
  providers: [AccountsStore, AccountsQuery, AccountsService],
  exports: [],
})
export class AccountsModule {}
