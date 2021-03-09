import { InjectionToken } from '@angular/core';
import { MsalAngularConfiguration } from '@azure/msal-angular';
import { AppInsightsConfig } from '@markpieszak/ng-application-insights';
import { Configuration as MsalConfiguration } from 'msal';
import { LazyMapsAPILoaderConfigLiteral } from '@agm/core';

export const APP_CONFIG = new InjectionToken<DplLiveConfiguration>(
  'APP_CONFIG'
);

export interface DplLiveConfiguration {
  app: {
    apiBaseUrl: string;
    info: {
      environment: string;
      build: string;
      buildTime: string;
      release: string;
      releaseTime: string;
    };
    ui: {
      autoComplete: {
        minLength: number,
        debounceTime: number,
      },
      loading: {
        durationUntilUserCanHide: number,
        durationMax: number,
      },
    },
  };
  msal: {
    config: MsalConfiguration;
    ngConfig: MsalAngularConfiguration;
  };
  applicationInsights: AppInsightsConfig;
  googleMaps: LazyMapsAPILoaderConfigLiteral;
  stream: {
    key: string;
  };
  chat: {
    functions: {
      createChannelOrAddMeAsMember: string;
      getChannelsWithMessages: string;
    };
  };

}
