import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';

import { CountriesModule } from './countries/countries.module';
import { LanguagesModule } from './languages/languages.module';
import { LoadCarriersModule } from './load-carriers/load-carriers.module';
import { MasterDataService } from './services/master-data.service';
import { VoucherReasonTypesModule } from './voucher-reason-types/voucher-reason-types.module';

@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    CountriesModule,
    LanguagesModule,
    LoadCarriersModule,
    VoucherReasonTypesModule,
  ],
  providers: [MasterDataService],
})
export class MasterDataModule {}
