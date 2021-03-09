import { Component, OnInit } from '@angular/core';
import {
  subformComponentProviders,
  NgxSubFormComponent,
  Controls,
  SubFormGroup,
} from 'ngx-sub-form';
import { FormControl, MinLengthValidator, Validators } from '@angular/forms';
import { ValidationDataService } from '@app/shared/services';
import { WrappedControlForm } from '@dpl/dpl-lib';

export interface AddressCity {
  postalCode?: string | null;
  city?: string | null;
  state?: number;
  country?: number;
}

@Component({
  selector: 'app-address-city-form',
  template: `
    <div
      *ngIf="formGroup"
      fxFlex
      fxLayout="row"
      fxLayoutGap="10px"
      [formGroup]="formGroup"
    >
      <app-country-picker
        [subForm]="formGroup.controls.country"
        mode="licensePlate"
        [prefill]="false"
      ></app-country-picker>
      <mat-form-field fxFlex="20%">
        <input
          matInput
          [formControl]="formGroup.controls.postalCode"
          placeholder="PLZ"
          i18n-placeholder="PostalCode|Label PLZ@@PostalCode"
        />
        <mat-error *ngIf="formGroup.controls.postalCode.errors?.maxlength">
          <span i18n="MaxLength|Label Pflichtfeld@@MaxLength"
            >Die Eingabe ist zu lang</span
          >
        </mat-error>
      </mat-form-field>
      <mat-form-field fxFlex="80%">
        <input
          matInput
          [formControl]="formGroup.controls.city"
          placeholder="Ort"
          i18n-placeholder="Location|Label Ort@@Location"
        />
        <mat-error *ngIf="formGroup.controls.city?.errors?.maxlength">
          <span i18n="MaxLength|Label Pflichtfeld@@MaxLength"
            >Die Eingabe ist zu lang</span
          >
        </mat-error>
      </mat-form-field>
    </div>
  `,
  styles: [],
  providers: subformComponentProviders(AddressCityFormComponent),
})
export class AddressCityFormComponent extends NgxSubFormComponent<AddressCity> {
  constructor() {
    super();
  }

  protected getFormControls(): Controls<AddressCity> {
    return {
      postalCode: new FormControl(
        null,
        Validators.maxLength(ValidationDataService.maxLength.postalCode)
      ),
      city: new FormControl(
        null,
        Validators.maxLength(ValidationDataService.maxLength.city)
      ),
      state: new FormControl(null),
      country: new SubFormGroup<number, WrappedControlForm<number>>(
        null,
        Validators.maxLength(ValidationDataService.maxLength.country)
      ),
    };
  }

  getDefaultValues() {
    const defaultValue: AddressCity = {
      postalCode: null,
      city: null,
      state: null,
      country: null,
    };
    return defaultValue;
  }
}
