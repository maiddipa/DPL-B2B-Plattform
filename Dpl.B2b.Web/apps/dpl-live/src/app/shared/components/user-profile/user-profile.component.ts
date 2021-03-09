import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { AuthenticationService } from 'apps/dpl-live/src/app/core/services/authentication.service';
import { UserService } from 'apps/dpl-live/src/app/user/services/user.service';
import { IUser } from 'apps/dpl-live/src/app/user/state/user.model';

@Component({
  selector: 'app-user-profile',
  templateUrl: './user-profile.component.html',
  styleUrls: ['./user-profile.component.scss'],
})
export class UserProfileComponent implements OnInit {
  currentUser$: Observable<IUser<number, number>>;
  constructor(
    private router: Router,
    private user: UserService,
    private auth: AuthenticationService
  ) {}

  ngOnInit() {
    this.currentUser$ = this.user.getCurrentUser();
  }

  login() {
    this.auth.login().subscribe();
  }

  logout() {
    this.auth.logout();
    this.router.navigate(['/']);
  }
}
