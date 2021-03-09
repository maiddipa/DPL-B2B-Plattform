import { Component, Input, OnChanges, OnInit, SimpleChanges } from '@angular/core';
import {
  NgxSubFormComponent,
  subformComponentProviders,
  Controls,
  SubFormGroup,
} from 'ngx-sub-form';
import { FormControl, Validators } from '@angular/forms';
import { CountryPickerComponentMode } from '../country-picker/country-picker.component';
import { ValidationDataService } from '@app/shared/services';

export interface LicensePlate {
  countryId: number;
  number: string;
}

// TODO i18n
@Component({
  selector: 'app-license-plate-form',
  template: `
    <ng-container *ngIf="formGroup" [formGroup]="formGroup">
      <app-country-picker
        [subForm]="formGroup.controls.countryId"
        [mode]="countryPickerMode"
        [prefill]="false"
        [required]="required"
      ></app-country-picker>
      <mat-form-field fxFlex>
        <input
          matInput
          placeholder="Kennzeichen"
          i18n-placeholder="LicensePlate|Label Kennzeichen@@LicensePlate"
          [formControl]="formGroup.controls.number"
        />
        <mat-error
          *ngIf="
            formGroup.controls.number?.touched &&
            formGroup.controls.number.errors?.required
          "
          >Pflichtfeld</mat-error
        >
        <mat-error *ngIf="formGroup.controls.number?.errors?.maxlength">
          <span i18n="MaxLength|Label Pflichtfeld@@MaxLength"
            >Die Eingabe ist zu lang</span
          >
        </mat-error>
      </mat-form-field>
    </ng-container>
  `,
  styles: [],
  providers: subformComponentProviders(LicensePlateFormComponent),
})
export class LicensePlateFormComponent extends NgxSubFormComponent<LicensePlate>
  implements OnChanges {
    @Input() required: boolean;
  constructor() {
    super();
  }

  countryPickerMode: CountryPickerComponentMode = 'licensePlate';

  protected emitInitialValueOnInit = false;

  protected getFormControls(): Controls<LicensePlate> {
    return {
      countryId: new SubFormGroup(null),
      number: new FormControl(null, [
        Validators.required,
        Validators.maxLength(ValidationDataService.maxLength.licensePlate),
      ]),
    };
  }

  public getDefaultValues() {
    return {
      countryId: null,
      number: null,
    } as LicensePlate;
  }

  ngOnChanges(changes: SimpleChanges) {
    super.ngOnChanges(changes);
    this.setRequiredState(this.required);
  }

  public setRequiredState(isRequired: boolean) {    
    const number = this.formGroup.controls.number;
    number.setValidators(
      isRequired
        ? [Validators.required]
        : []
    );
    number.updateValueAndValidity({ onlySelf: true, emitEvent: false });
  }
}
