import { Component, OnInit } from '@angular/core';

import { LocalizationService } from '@app/core';
import { UserService } from '../../../user/services/user.service';
import { Observable, combineLatest } from 'rxjs';
import { IUser } from '../../../user/state/user.model';
import { tap, map } from 'rxjs/operators';

interface IViewData {
  user: IUser<number, number>;
}

@Component({
  selector: 'app-user-role',
  templateUrl: './user-role.component.html',
  styleUrls: ['./user-role.component.scss'],
})
export class UserRole implements OnInit {
  viewData$: Observable<IViewData>;
  userRoleI18n: string;

  constructor(
    private user: UserService,
    private localization: LocalizationService
  ) {}

  ngOnInit() {
    const user$ = this.user.getCurrentUser().pipe(
      tap((user) => {
        if (user && user.role) {
          this.userRoleI18n = this.localization.getTranslation(
            'UserRole',
            user.role.toString()
          );
        }
      })
    );

    this.viewData$ = combineLatest(user$).pipe(
      map(([user]) => {
        return <IViewData>{
          user,
        };
      })
    );
  }
}
