import { Component, OnInit } from '@angular/core';
import { MatIconRegistry } from '@angular/material/icon';
import { DomSanitizer, Title } from '@angular/platform-browser';
import { UserRole } from '@app/api/dpl';
import { LoadingService } from '@dpl/dpl-lib';
import { combineLatest, NEVER, Observable, timer } from 'rxjs';
import { map, startWith, switchMap, tap } from 'rxjs/operators';

import { isMsalIframe } from '../utils';
import { ChatService } from './chat/services/chat.service';
import { AuthenticationService } from './core/services/authentication.service';
import { MasterDataService } from './master-data/services/master-data.service';
import { TitleService } from './shared/services/title.service';
import { UserService } from './user/services/user.service';

interface IViewData {
  isLoggedIn: boolean;
  isInitialized: boolean;
}

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
})
export class AppComponent implements OnInit {
  viewData$: Observable<IViewData>;
  userRole = UserRole;
  loading$: Observable<boolean>;
  constructor(
    private auth: AuthenticationService,
    private masterData: MasterDataService,
    private userData: UserService,
    private chat: ChatService,
    private title: Title,
    private titleService: TitleService,
    private matIconRegistry: MatIconRegistry,
    private domSanitizer: DomSanitizer,
    private loadingService: LoadingService
  ) {
    this.matIconRegistry.addSvgIcon(
      'dpl_incoming',
      this.domSanitizer.bypassSecurityTrustResourceUrl(
        'assets/icons/incoming.svg'
      )
    );
    this.matIconRegistry.addSvgIcon(
      'dpl_outgoing',
      this.domSanitizer.bypassSecurityTrustResourceUrl(
        'assets/icons/outgoing.svg'
      )
    );
  }

  // collect that title data properties from all child routes
  // there might be a better way but this worked for me

  ngOnInit(): void {
    if (isMsalIframe()) {
      return;
    }

    this.loading$ = this.loadingService.getLoading();

    const isLoggedIn$ = this.auth.isLoggedIn();
    const setCredentials$ = this.auth.setCrediantials();

    const appData$ = isLoggedIn$.pipe(
      switchMap((isLoggedIn) => {
        if (!isLoggedIn) {
          return NEVER;
        }

        return combineLatest([
          this.userData.refreshUserData(),
          this.masterData.refreshMasterData(),
        ]);
      })
    );

    const loadChat$ = this.chat.load();
    const minLoadingTime$ = timer(750); // to display loading bars for a certain time

    const title$ = this.titleService.buildTitle().pipe(
      tap((title) => {
        this.title.setTitle(title);
      })
    );

    const intialized$ = combineLatest([
      appData$,
      loadChat$,
      minLoadingTime$,
      title$,
    ]).pipe(
      map(() => true),
      startWith(false)
    );

    this.viewData$ = combineLatest([
      setCredentials$,
      isLoggedIn$,
      intialized$,
    ]).pipe(
      map(([setCredentials, isLoggedIn, isInitialized]) => {
        return <IViewData>{
          isLoggedIn,
          isInitialized,
        };
      })
    );
  }
}
