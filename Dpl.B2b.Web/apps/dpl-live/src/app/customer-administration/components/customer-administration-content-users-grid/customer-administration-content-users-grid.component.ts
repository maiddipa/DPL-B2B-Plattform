import { ChangeDetectorRef, Inject, ViewChild } from '@angular/core';
import {
  Component,
  OnInit,
  ChangeDetectionStrategy,
  Optional,
} from '@angular/core';
import CustomStore from 'devextreme/data/custom_store';
import {
  API_BASE_URL,
  PersonGender,
  UserRole,
  UserAdministrationApiService,
  UserListItem,
} from '../../../core/services/dpl-api-services';
import {
  CustomerAdminOrganizationUnit,
  CustomerAdminScope,
} from '../../services/customer-administration.service.types';
import * as AspNetData from 'devextreme-aspnet-data-nojquery';
import { AuthenticationService } from '../../../core/services/authentication.service';
import { filter, map, pluck, switchMap, tap } from 'rxjs/operators';
import { BehaviorSubject, combineLatest, Observable, of } from 'rxjs';
import { UnitsQuery } from '../../state/units.query';
import { DplApiService, LocalizationService } from '../../../core';
import {
  DxDataGridComponent,
  DxPopupComponent,
  DxSelectBoxComponent,
} from 'devextreme-angular';

type ViewData = {
  unit: CustomerAdminOrganizationUnit;
  dataSource: CustomStore;
  dropdownRole: any;
  dropdownGender: any;
  allUsersExceptActiveCustomer: UserListItem[];
};

@Component({
  selector: 'dpl-customer-administration-content-users-grid',
  templateUrl: './customer-administration-content-users-grid.component.html',
  styleUrls: ['./customer-administration-content-users-grid.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class CustomerAdministrationContentUsersGridComponent implements OnInit {
  baseUrl: string;
  viewData$: Observable<ViewData>;
  allUsersExceptActiveCustomer$: Observable<any>;
  popupSelectUser: number = undefined;
  filterValue: any;
  // example pattern for telefon numbers
  // pattern: any = /^\+?(?:[0-9] ⋅?)?{6,14}[0-9]$/;
  reloadSub = new BehaviorSubject<boolean>(false);
  popupVisible = false;
  popupInitialPasswordVisible = false;
  popupInitialPassword = '';
  resetItem: any = {};

  constructor(
    private dpl: DplApiService,
    private unitsQuery: UnitsQuery,
    private authenticationService: AuthenticationService,
    private localizationService: LocalizationService,
    private cd: ChangeDetectorRef,
    // private chat: ChatService,
    @Optional() @Inject(API_BASE_URL) baseUrl?: string
  ) {
    this.baseUrl = baseUrl;
  }

  ngOnInit(): void {
    const unit$ = this.unitsQuery.selectActive().pipe(filter((x) => !!x));
    const reload$ = this.reloadSub.asObservable();
    const allUsersExceptActiveCustomer$ = combineLatest([unit$, reload$]).pipe(
      switchMap(([unit]) => {
        if (unit.scope === CustomerAdminScope.CustomerUsers) {
          const activeCustomerId = unit.parent;
          return this.dpl.customersAdministrationService
            .getById(activeCustomerId)
            .pipe(
              pluck('organizationId'),
              switchMap((activeOrganizationId) => {
                return this.dpl.userAdmistrationService.allByOrganization({
                  organizationId: activeOrganizationId,
                  exceptCustomerId: activeCustomerId,
                });
              })
            );
        }
        return of([] as UserListItem[]);
      })
    );

    this.viewData$ = combineLatest([
      unit$,
      this.getUserRoleTypeData(),
      this.getPersonGenderTypeData(),
      allUsersExceptActiveCustomer$,
      reload$,
    ]).pipe(
      map(
        ([
          unit,
          dropdownRole,
          dropdownGender,
          allUsersExceptActiveCustomer,
        ]) => {
          console.log('Active Customer');
          const dataSource = AspNetData.createStore({
            key: 'id',
            loadUrl: this.baseUrl + '/useradministration',
            loadParams: {
              customerId:
                unit.scope === CustomerAdminScope.CustomerUsers
                  ? unit.parent
                  : undefined,
              organizationId:
                unit.scope === CustomerAdminScope.OrganizationUsers
                  ? unit.parent
                  : undefined,
            },
            insertUrl: this.baseUrl + '/useradministration',
            updateUrl: this.baseUrl + '/useradministration',
            // deleteUrl: this.baseUrl + '/DeleteCustomer',
            onBeforeSend: async (method, ajaxOptions) => {
              return await this.authenticationService
                .getTokenForRequestUrl(this.baseUrl)
                .pipe(
                  map((token) => {
                    ajaxOptions.xhrFields = { withCredentials: false };
                    ajaxOptions.headers = { Authorization: 'Bearer ' + token };
                    if (ajaxOptions.method === 'POST') {
                      // add custom request data to insert request
                      const payload = JSON.parse(ajaxOptions.data.values);
                      if (unit.scope === CustomerAdminScope.CustomerUsers) {
                        payload.CustomerId = unit.parent;
                      }
                      ajaxOptions.data.values = JSON.stringify(payload);
                    }
                  })
                )
                .toPromise();
            },
            onInserted: (value: UserListItem, key: number) => {
              if (value?.firstLoginPassword) {
                this.popupInitialPasswordVisible = true;
                this.popupInitialPassword = value.firstLoginPassword;
                this.cd.detectChanges();
              }
            },
          });
          const viewData: ViewData = {
            dataSource,
            unit,
            dropdownGender,
            dropdownRole,
            allUsersExceptActiveCustomer,
          };
          return viewData;
        }
      )
    );
  }
  // get UserRole LookupData
  getUserRoleTypeData() {
    const r = Object.keys(UserRole).map((value, id) => {
      value = UserRole[value as any];
      const display = this.localizationService.getTranslation(
        'UserRole',
        value
      );
      return { id, value, display };
    });

    return of(r);
  }
  // get Gender LookupData
  getPersonGenderTypeData() {
    const r = Object.keys(PersonGender).map((value, id) => {
      value = PersonGender[value as any];
      const display = this.localizationService.getTranslation('Gender', value);
      return { id, value, display };
    });

    return of(r);
  }

  // add User to Aktiv Customer
  addToCustomer(unit) {
    if (this.popupSelectUser !== undefined) {
      // Implement request
      this.dpl.userAdmistrationService
        .addToCustomer(
          // userId
          this.popupSelectUser,
          // CustomerID
          unit.parent
        )
        .pipe(
          tap((result) => {
            this.reloadSub.next(true);
            this.popupVisible = false;
            this.popupSelectUser = undefined;
          })
        )
        .subscribe();
    }
  }

  // removeFromCustomer
  removeFromCustomer(removeRowData, unit) {
    const removeFromCustomer$ = this.dpl.userAdmistrationService
      .removeFromCustomer(
        // userId
        removeRowData.row.data.id, //user id out of the row
        // CustomerID
        unit.parent
      )
      .pipe(
        tap((result) => {
          this.reloadSub.next(true);
        })
      )
      .subscribe();
  }

  // get chat data for later use
  getChatData(item) {
    // implement chat call
    // add a new Chat Type, in the chat client and function
    // Parse UPN to chat Ids
    // open a chat with both ChatIDs as channelIds, maybe with customerId
    return {};
  }


  // Get Sort Value for User Role
  // Todo: change & get it to work
  getSortValueRole(value) {
    console.log(value);
    let sortValue = 0;
    switch (value.userRole) {
      case UserRole.Retailer:
        sortValue = 1;
        break;
      case UserRole.Warehouse:
        sortValue = 2;
        break;
      case UserRole.Shipper:
        sortValue = 3;
        break;
      case UserRole.DplEmployee:
        sortValue = 4;
        break;
    }
    return sortValue;
  }
  // set a filter Expression
  getFilterExpressionRole(value) {
    let column = this as any;
    var filterExpression = [[column.dataField, 'contains', value]];
    return filterExpression;
  }

  // DisplayExpression for Lookups
  lookUpDisplayExpression(item) {
    // DisplayExpr for UserGridLookUps
    // used for Gender & Role
    return item ? `${item.display}` : ``;
  }

  // ValueExpression for Lookups
  lookUpValueExpression(item) {
    // ValueExpr for UserGridLookUps
    // used for Gender & Role
    return item ? `${item.value}` : ``;
  }

  // get unlocked value for the button/Icon
  getUnlockedValue(item) {
    // show Icon
    // unlock
    return !item.row.data.locked ? true : false;
  }

  // Button click to Lock/Unlock a user
  onClickLock(item) {
    // click to lock/Unlock
    // call the userAdminApiService patchLocked
    const lock$ = this.dpl.userAdmistrationService
      .patchLocked(item.row.data.id, !item.row.data.locked)
      .pipe(
        tap((objtData) => {
          this.reloadSub.next(true);
        })
      )
      .subscribe();
  }

  // show or hide PopUp
  showPopup() {
    // toggle popupVisble
    this.popupVisible = !this.popupVisible;
    this.popupSelectUser = undefined;
    // add a detectChange vor changedetection, needed to react on this change
    this.cd.detectChanges();
  }

  // Editor Preparing
  onEditorPreparing(e) {
    if (
      e.dataField === 'upn' &&
      e.parentType === 'dataRow' &&
      !e.row.isNewRow
    ) {
      //disable upn in edit
      e.editorOptions.disabled = true;
    }
  }

  // Click for Reset-Password-Icon in a row
  clickResetPassword(item) {
    const result$ = this.dpl.userAdmistrationService
      .postResetPassword(item.row.data.id)
      .pipe(
        tap((value) => {
          if (value?.newPassword) {
            this.popupInitialPasswordVisible = true;
            this.popupInitialPassword = value.newPassword;
            this.cd.detectChanges();
          }
        })
      )
      .subscribe();
  }

  // Toolbar Prepairing
  // defines how the toolbar will look
  onToolbarPreparing(e, data) {
    if (data.unit.scope === 'CustomerUsers') {
      e.toolbarOptions.items.unshift({
        location: 'after',
        widget: 'dxButton',
        options: {
          icon: 'pin',
          hint: 'Hinzufügen zum Kunden',
          onClick: () => {
            this.showPopup();
          },
        },
      });
    }
  }

  popUserDisplayExpr(user: UserListItem) {
    return user ? `${user.lastName}, ${user.firstName} (${user.id})` : '';
  }

  // // validation call for e-mail - validate that the email is only one time in use
  //   asyncValidation(params) {
  //     return this.httpClient.post("https://js.devexpress.com/Demos/Mvc/RemoteValidation/CheckUniqueEmailAddress", {
  //             id: params.data.ID,
  //             email: params.value
  //         }, {
  //             responseType: "json"
  //         }).toPromise();
  // }
  // }
}
