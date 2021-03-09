import { Inject, Injectable } from '@angular/core';
import {
  ActivatedRouteSnapshot,
  CanActivate,
  CanActivateChild,
  RouterStateSnapshot,
} from '@angular/router';
import { ajaxSetup } from '@devexpress/analytics-core/analytics-utils';
import { map, tap } from 'rxjs/operators';

import { AuthenticationService } from '../core/services/authentication.service';
import { API_BASE_URL } from '../core/services/dpl-api-services';

// import { AuthenticationService } from './services/authentication.service';
@Injectable({ providedIn: 'root' })
export class ReportingAuthGuard implements CanActivate, CanActivateChild {
  constructor(
    @Inject(API_BASE_URL) private apiBaseUrl: string,
    private authenticationService: AuthenticationService
  ) {}

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
    return this.authenticationService
      .getTokenForRequestUrl(this.apiBaseUrl)
      .pipe(
        tap((token) => {
          //DevExpress.Reporting.Viewer.Settings.AsyncExportApproach = true;
          ajaxSetup.ajaxSettings = {
            headers: {
              Authorization: `Bearer ${token}`,
            },
          };
        }),
        map((token) => token !== null),
        tap((hasToken) => {
          if (!hasToken) {
            return console.error(
              'User is logged in but token for Api could not be obtained.'
            );
          }
        })
      );
  }

  public canActivateChild(
    childRoute: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ) {
    return this.canActivate(childRoute, state);
  }
}
