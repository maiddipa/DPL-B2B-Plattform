import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NG_ENTITY_SERVICE_CONFIG } from '@datorama/akita-ng-entity-service';
import { AkitaNgRouterStoreModule } from '@datorama/akita-ng-router-store';
import { AkitaNgDevtools } from '@datorama/akita-ngdevtools';
import { LOADING_MAX_DURATION } from '@dpl/dpl-lib';

import { APP_CONFIG, DplLiveConfiguration } from '../config';
import { environment } from '../environments/environment';
import { isMsalIframe } from '../utils';
import { AccountsModule } from './accounts/accounts.module';
import { AddressesModule } from './addresses/addresses.module';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { ChatModule } from './chat/chat.module';
import { ContactModule } from './contact/contact.module';
import { CoreModule } from './core/core.module';
import { API_BASE_URL } from './core/services/dpl-api-services';
import { CustomersModule } from './customers/customers.module';
import { FiltersModule } from './filters/filters.module';
import { LoadingLocationsModule } from './loading-locations/loading-locations.module';
import { MasterDataModule } from './master-data/master-data.module';
import { PartnersModule } from './partners/partners.module';
import { ProfileModule } from './profile/profile.module';
import { SharedModule } from './shared/shared.module';
import { UserSettingsModule } from './user-settings/user-settings.module';
import { UserModule } from './user/user.module';

// think about conditional loading of sub modules or creatinga parent module for app
// based on current window location window.location.pathname === "/msal-callback"

@NgModule({
  declarations: [AppComponent],
  imports: [
    BrowserModule,
    AppRoutingModule,
    CoreModule,
    SharedModule,
    BrowserAnimationsModule,
    AccountsModule,
    environment.production
      ? []
      : AkitaNgDevtools.forRoot({
          name: isMsalIframe() ? 'iFrame' : undefined,
        }),
    AkitaNgRouterStoreModule,
    MasterDataModule,
    UserModule,
    CustomersModule,
    LoadingLocationsModule,
    AddressesModule,
    PartnersModule,
    FiltersModule,
    ChatModule,
    ProfileModule,
    UserSettingsModule,
    ContactModule,
  ],
  providers: [
    // sets the base url to the api
    {
      provide: API_BASE_URL,
      useFactory: (config: DplLiveConfiguration) => config.app.apiBaseUrl,
      deps: [APP_CONFIG],
    },
    {
      provide: NG_ENTITY_SERVICE_CONFIG,
      useValue: { baseUrl: API_BASE_URL },
    },
    {
      provide: LOADING_MAX_DURATION,
      useFactory: (config: DplLiveConfiguration) => config.app.ui.loading.durationMax,
      deps: [APP_CONFIG],
    },
  ],
  bootstrap: [AppComponent],
})
export class AppModule {}
