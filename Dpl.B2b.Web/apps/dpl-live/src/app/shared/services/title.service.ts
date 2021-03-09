import { Injectable } from '@angular/core';

import { map } from 'rxjs/operators';

import { UserService } from '../../user/services/user.service';
import { Observable } from 'rxjs';
import { LocalizationService } from '@app/core';

const APP_TITLE = 'DPL: live';
const SEPARATOR = ' - ';

@Injectable({
  providedIn: 'root',
})
export class TitleService {
  constructor(
    private user: UserService,
    private localization: LocalizationService
  ) {}

  buildTitle(): Observable<string> {
    const title$ = this.user.getCurrentUser().pipe(
      map((user) => {
        if (user && user.role) {
          const userRoleI18n = this.localization.getTranslation(
            'UserRole',
            user.role.toString()
          );

          return `${APP_TITLE}${SEPARATOR}${userRoleI18n}`;
        } else {
          return `${APP_TITLE}`;
        }
      })
    );

    return title$;
  }
}
