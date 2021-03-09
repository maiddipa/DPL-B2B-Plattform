import { Component, OnInit } from '@angular/core';
import {
  NgxSubFormComponent,
  subformComponentProviders,
  Controls,
} from 'ngx-sub-form';
import { FormControl, FormGroup } from '@angular/forms';
import { CustomerDivisionsService } from 'apps/dpl-live/src/app/customers/services/customer-divisions.service';
import { UserService } from 'apps/dpl-live/src/app/user/services/user.service';
import { tap, filter, map } from 'rxjs/operators';
import { combineLatest, Observable } from 'rxjs';

export interface IssuerHeader {
  division: {
    id: number;
    name: string;
  };
  user: {
    id: number;
    name: string;
  };
}

@Component({
  selector: 'app-issuer-header-form',
  template: `
    <div
      *ngIf="viewData$ | async as data"
      fxFlex
      fxLayout="row"
      fxLayoutAlign=" center"
      fxLayoutGap="10px"
      [formGroup]="formGroup"
    >
      <mat-form-field fxFlex [formGroupName]="formGroup.controls.division">
        <input
          matInput
          formControlName="name"
          placeholder="Ausstelle Abteilung"
          i18n-placeholder="
            IssuingCompany|Label Ausstellende Abteilung@@IssuingDivision"
        />
      </mat-form-field>
      <mat-form-field fxFlex [formGroupName]="formGroup.controls.user">
        <input
          matInput
          formControlName="name"
          placeholder="Ausstellende Person"
          i18n-placeholder="
            IssuingPerson|Label Ausstellende Person@@IssuingPerson"
        />
      </mat-form-field>
    </div>
  `,
  styles: [],
  providers: subformComponentProviders(IssuerHeaderFormComponent),
})
export class IssuerHeaderFormComponent extends NgxSubFormComponent<IssuerHeader>
  implements OnInit {
  constructor(
    private user: UserService,
    private divisionService: CustomerDivisionsService
  ) {
    super();
  }

  viewData$: Observable<{}>;
  ngOnInit() {
    const currentDivision$ = this.divisionService.getActiveDivision().pipe(
      filter((division) => !!division),
      tap((division) => {
        this.formGroup.controls.division.patchValue(division);
      })
    );

    const currentUser$ = this.user.getCurrentUser().pipe(
      tap((user) => {
        // HACK hardcoded user ids for issues user.id as we dont currently retrieve the useridd from the api
        const fakeUser = {
          ...user,
          ...{
            id: 1,
          },
        };
        this.formGroup.controls.user.patchValue(fakeUser);
      })
    );

    this.viewData$ = combineLatest([currentDivision$, currentUser$]).pipe(
      map(([division, user]) => {
        return {};
      })
    );
  }

  protected getFormControls(): Controls<IssuerHeader> {
    return {
      division: new FormGroup({
        id: new FormControl(null),
        name: new FormControl({
          value: null,
          disabled: true,
        }),
      }),
      user: new FormGroup({
        id: new FormControl(null),
        name: new FormControl({
          value: null,
          disabled: true,
        }),
      }),
    };
  }
}
