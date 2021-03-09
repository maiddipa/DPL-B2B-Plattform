import { Inject, Injectable } from '@angular/core';
import {
  ActivatedRouteSnapshot,
  CanActivate,
  Router,
  RouterStateSnapshot,
  CanActivateChild,
} from '@angular/router';
import { of, Observable } from 'rxjs';
import { map, switchMap, tap } from 'rxjs/operators';

import { AuthenticationService } from './services/authentication.service';
import { API_BASE_URL } from './services/dpl-api-services';

// import { AuthenticationService } from './services/authentication.service';
@Injectable({ providedIn: 'root' })
export class AuthGuard implements CanActivate, CanActivateChild {
  constructor(
    @Inject(API_BASE_URL) private apiBaseUrl: string,
    private router: Router,
    private auth: AuthenticationService
  ) {}

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
    return this.auth
      .isLoggedIn()
      .pipe(
        switchMap((isLoggedIn) =>
          isLoggedIn
            ? of(isLoggedIn)
            : this.auth.login().pipe(switchMap(() => this.auth.isLoggedIn()))
        )
      );
  }

  public canActivateChild(
    childRoute: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ) {
    return this.canActivate(childRoute, state);
  }
}
