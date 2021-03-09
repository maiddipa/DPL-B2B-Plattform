import { enableProdMode } from '@angular/core';
import { platformBrowserDynamic } from '@angular/platform-browser-dynamic';
import { MSAL_CONFIG, MSAL_CONFIG_ANGULAR } from '@azure/msal-angular';

import { AppModule } from './app/app.module';
import { APP_CONFIG, DplLiveConfiguration } from './config';
import { environment } from './environments/environment';
import { isIE, localizeUrl } from './utils';
import { persistState } from '@datorama/akita';

const storage = persistState({
  include: ['customer-divisions.active'],
});

fetch(localizeUrl('/assets/config.json'))
  .then((response) => response.json())
  .then((config: DplLiveConfiguration) => {
    if (environment.production) {
      enableProdMode();
    }

    platformBrowserDynamic([
      { provide: APP_CONFIG, useValue: config },
      {
        provide: MSAL_CONFIG,
        useValue: {
          ...config.msal.config,
          auth: {
            ...config.msal.config.auth,
            redirectUri: localizeUrl(
              config.msal.config.auth.redirectUri as string
            ),
            postLogoutRedirectUri: localizeUrl(
              config.msal.config.auth.postLogoutRedirectUri as string
            ),
          },
          cache: { ...config.msal.config.cache, storeAuthStateInCookie: isIE },
        },
      },
      {
        provide: MSAL_CONFIG_ANGULAR,
        useValue: { ...config.msal.ngConfig, popUp: !isIE },
      },
      { provide: 'persistStorage', useValue: storage },
    ])
      .bootstrapModule(AppModule)
      .catch((err) => console.error(err));
  });
