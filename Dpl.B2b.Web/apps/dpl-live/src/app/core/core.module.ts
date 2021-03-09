import { CommonModule, registerLocaleData } from '@angular/common';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import localeDe from '@angular/common/locales/de';
import {
  ErrorHandler,
  Inject,
  InjectionToken,
  LOCALE_ID,
  NgModule,
  TRANSLATIONS,
  TRANSLATIONS_FORMAT,
} from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import {
  BroadcastService,
  MsalInterceptor,
  MsalModule,
  MsalService,
} from '@azure/msal-angular';
import { DplLibModule } from '@dpl/dpl-lib';
import {
  AppInsightsConfig,
  AppInsightsService,
  ApplicationInsightsModule,
} from '@markpieszak/ng-application-insights';
import { I18n } from '@ngx-translate/i18n-polyfill';
import { loadMessages, locale } from 'devextreme/localization';
import { ToastrModule } from 'ngx-toastr';

import { APP_CONFIG, DplLiveConfiguration } from '../../config';
import { environment } from '../../environments/environment';
import { AuthGuard } from './auth.guard';
import {
  LoggingService,
  NotificationService,
  TimezoneOffsetHeaderInterceptor,
} from './services';
import { AppErrorHandlerService } from './services/app-error-handler.service';
import { ApplicationInsightsErrorHandlerService } from './services/application-insights-error-handler.service';
import { AuthenticationService } from './services/authentication.service';
import { OrderManagementApiService } from './services/backend-api.service';
import { DevExtremeLocalizationService } from './services/dev-extreme-localization.service';
import { LocalizationService } from './services/localization.service';
import { SessionService } from './services/session.service';
import { SessionQuery } from './session/state/session.query';
import { SessionStore } from './session/state/session.store';
import { getLanguageFromLocale } from './utils';

import * as deMessages from 'devextreme/localization/messages/de.json';
import * as enMessages from 'devextreme/localization/messages/en.json';
import * as frMessages from 'devextreme/localization/messages/fr.json';
import * as plMessages from 'devextreme/localization/messages/en.json';

declare const require; // this is neccessary to dynamically load translations

const DEV_EXTREME_LOCALIZATION_MESSAGES = new InjectionToken<{
  [key: string]: string;
}>('DEV_EXTREME_LOCALIZATION_MESSAGES');

@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    DplLibModule,
    HttpClientModule,
    MsalModule,
    ToastrModule.forRoot({
      timeOut: 0,
      extendedTimeOut: 0,
      positionClass: 'toast-bottom-right',
      preventDuplicates: true,
    }),
    // only load when in production
    environment.production ? ApplicationInsightsModule : [],
  ],
  exports: [ReactiveFormsModule, FormsModule, DplLibModule],
  providers: [
    {
      provide: AppInsightsConfig,
      useFactory: (config: DplLiveConfiguration) => config.applicationInsights,
      deps: [APP_CONFIG],
    },
    // this provider injects auth information into any request made via HttpClient
    {
      provide: HTTP_INTERCEPTORS,
      useClass: MsalInterceptor,
      multi: true,
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: TimezoneOffsetHeaderInterceptor,
      multi: true,
    },
    MsalService,
    BroadcastService,
    AuthenticationService,
    SessionService,
    OrderManagementApiService,
    SessionStore,
    SessionQuery,
    AuthGuard,

    // only load when in production
    environment.production
      ? [
          AppInsightsService,
          // send all errors to application insights when in production
          {
            provide: ErrorHandler,
            useClass: environment.production
              ? ApplicationInsightsErrorHandlerService
              : ErrorHandler,
          },
        ]
      : [],

    // Global App Error Handler -> for details/api/other notifications an logging
    { provide: ErrorHandler, useClass: AppErrorHandlerService },
    // start - localization
    { provide: TRANSLATIONS_FORMAT, useValue: 'xlf' },
    environment.production ? [] : { provide: LOCALE_ID, useValue: 'de' },
    {
      provide: TRANSLATIONS,
      useFactory: (locale: string) => {
        let language = getLanguageFromLocale(locale || 'de'); // default to english if no locale provided

        // always use german language in development
        if (!environment.production) {
          language = 'de';
          registerLocaleData(localeDe, language);
        }

        return require('raw-loader!../../locale/messages.' + language + '.xlf')
          .default;
      },
      deps: [LOCALE_ID],
    },
    {
      provide: DEV_EXTREME_LOCALIZATION_MESSAGES,
      useFactory: (locale: string) => {
        let language = getLanguageFromLocale(locale || 'de'); // default to english if no locale provided

        // always use german language in development
        if (!environment.production) {
          language = 'de';
          registerLocaleData(localeDe, language);
        }

        switch (language) {
          case 'de':
            return (deMessages as any).default;
          case 'en':
            return (enMessages as any).default;
          case 'fr':
            return (frMessages as any).default;
          case 'pl':
            return (plMessages as any).default;
          default:
            throw new Error(`Argument out of range: ${language}`);
        }
      },
      deps: [LOCALE_ID],
    },
    I18n,
    LocalizationService,
    DevExtremeLocalizationService,
    LoggingService,
    NotificationService,
    // end- localization
  ],
})
export class CoreModule {
  constructor(
    @Inject(DEV_EXTREME_LOCALIZATION_MESSAGES) messages: {},
    @Inject(LOCALE_ID) localeString: string
  ) {
    loadMessages(messages);
    locale(getLanguageFromLocale(localeString || 'de'));
  }
}
