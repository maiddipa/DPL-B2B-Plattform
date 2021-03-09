import { Inject, Injectable } from '@angular/core';
import { BroadcastService, MsalService } from '@azure/msal-angular';
import { MessageCallback } from '@azure/msal-angular/src/broadcast.service';
import { applyTransaction, PersistState } from '@datorama/akita';
import {
  bindCallback,
  EMPTY,
  from,
  NEVER,
  Observable,
  of,
  Subscription,
  throwError,
} from 'rxjs';
import { catchError, map, switchMap, tap } from 'rxjs/operators';

import { isMsalIframe } from '../../../utils';
import { UserStore } from '../../user/state/user.store';
import { SessionQuery } from '../session/state/session.query';
import { SessionStore } from '../session/state/session.store';
import { API_BASE_URL } from './dpl-api-services';

@Injectable({
  providedIn: 'root',
})
export class AuthenticationService {
  constructor(
    private authStore: SessionStore,
    private userStore: UserStore,
    private authQuery: SessionQuery,
    private msal: MsalService,
    private broadcast: BroadcastService,
    @Inject('persistStorage') private persistStorage: PersistState,
    @Inject(API_BASE_URL) private apiBaseUrl: string
  ) {
    if (isMsalIframe()) {
      return;
    }

    this.broadcast.subscribe('msal:acquireTokenSuccess', (payload) => {});

    this.broadcast.subscribe('msal:acquireTokenFailure', (payload) => {});

    this.broadcast.subscribe('msal:loginSuccess', (payload) => {});

    this.broadcast.subscribe('msal:loginFailure', (payload) => {});
  }

  public isLoggedIn() {
    return this.authQuery.isLoggedIn$;
  }

  public getCurrentSession() {
    return this.authQuery.currentSession$;
  }

  setCrediantials() {
    return of(this.msal.getAccount()).pipe(
      switchMap((account) => {
        if (!account) {
          this.authStore.logout();
          return of(false);
        }

        return this.getTokenForRequestUrl(this.apiBaseUrl).pipe(
          // if token cant be retrieved user session is expired
          map((token) => !!token),          
          catchError((error) => {
            return of(false);
          }),
          tap((isLoggedInAndNotExpired) => {
            if (!isLoggedInAndNotExpired) {
              return this.authStore.logout();
            }

            const session = {
              name: account.name,
              email: account.userName,
              token: account.idToken.toString(),
            };

            applyTransaction(() => {
              this.userStore.update({
                ...this.userStore.getValue(),
                ...{
                  email: session.email,
                  name: session.name,
                },
              });
              this.authStore.login(session);
            });
          })
        );
      })
    );
  }

  login() {
    this.persistStorage.clearStore();
    return from(this.msal.loginPopup()).pipe(
      switchMap((data) => {
        return this.setCrediantials();
      }),
      map(() => true),
      catchError((error, obs) => {
        // if the page was opened on a protected route use redirect login
        // modern browsers prevent pages to trigger a popup without user interaction
        if (
          typeof error === 'string' &&
          error.indexOf('popup_window_error') >= 0
        ) {
          this.msal.loginRedirect();
          return NEVER;
        }
        return of(false);
      })
    );
  }

  logout() {
    this.msal.logout();
    this.authStore.logout();
    this.persistStorage.clearStore();
  }

  private getObservable(
    eventType:
      | 'msal:loginSuccess'
      | 'msal:loginFailure'
      | 'msal:acquireTokenSuccess'
      | 'msal:acquireTokenFailure'
  ) {
    return new Observable((observer) => {
      let broadcastSubscription: Subscription;
      const obs = bindCallback(
        (callback: MessageCallback) =>
          (broadcastSubscription = this.broadcast.subscribe(
            eventType,
            callback
          ))
      )();

      const subscriptionInnerObs = obs.subscribe({
        next: observer.next,
        error: observer.error,
        complete: observer.complete,
      });

      return () => {
        broadcastSubscription.unsubscribe();
        subscriptionInnerObs.unsubscribe();
      };
    });
  }

  getTokenForRequestUrl(url: string) {
    const scopes = this.msal.getScopesForEndpoint(url);
    return from(this.msal.acquireTokenSilent({ scopes })).pipe(
      map((resp) => resp.accessToken)
    );
  }
}
