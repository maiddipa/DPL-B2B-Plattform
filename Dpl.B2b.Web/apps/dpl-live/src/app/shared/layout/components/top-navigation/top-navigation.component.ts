import { Component, OnInit } from '@angular/core';
import {
  UserRole,
  User,
  PermissionResourceType,
  ResourceAction,
  LoadCarrierReceiptType,
} from '@app/api/dpl';
import { combineLatest, Observable } from 'rxjs';
import { map } from 'rxjs/operators';

import { AuthenticationService } from '../../../../core/services/authentication.service';
import { UserService } from '../../../../user/services/user.service';
import { IUser } from '../../../../user/state/user.model';
import { OrdersViewType, OrderLoadsViewType } from '../../../types';

interface IViewData {
  user: IUser<number, number>;
  userRole: UserRole;
  showAll: boolean;
}

@Component({
  selector: 'app-top-navigation',
  templateUrl: './top-navigation.component.html',
  styleUrls: ['./top-navigation.component.scss'],
})
export class TopNavigationComponent implements OnInit {
  viewData$: Observable<IViewData>;
  ordersViewType = OrdersViewType;
  orderLoadsViewType = OrderLoadsViewType;
  userRole = UserRole;
  permissionResourceType = PermissionResourceType;
  resourceAction = ResourceAction;
  loadCarrierReceiptType = LoadCarrierReceiptType;

  // debugUpns = ['kt.it@dpl-ltms.com'];
  constructor(
    private auth: AuthenticationService,
    private userService: UserService
  ) {}

  ngOnInit() {
    this.viewData$ = combineLatest([
      this.auth.isLoggedIn(),
      this.userService.getCurrentUser(),
    ]).pipe(
      map(([isLoggedIn, user]) => {
        const showAll =
          // user && this.debugUpns.find(x => x === (user as User).upn)
          user && user.role === UserRole.DplEmployee ? true : false;
        console.log(user);
        const viewData: IViewData = {
          user,
          userRole: user ? user.role : undefined,
          showAll,
        };
        return viewData;
      })
    );
  }
}
