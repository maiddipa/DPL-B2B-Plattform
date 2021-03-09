import {
  Component,
  OnInit,
  Input,
  OnChanges,
  SimpleChanges,
} from '@angular/core';
import {
  NgxSubFormComponent,
  subformComponentProviders,
  Controls,
  SubFormGroup,
} from 'ngx-sub-form';
import { FormControl, Validators, MaxLengthValidator } from '@angular/forms';
import { hostFlexColumn } from '@dpl/dpl-lib';
import { LicensePlate } from '../license-plate-form/license-plate-form.component';
import { ValidationDataService } from '@app/shared/services';
import { IPartner } from '../../../partners/state/partner.model';

export interface DriverInfo {
  driverName: string;
  companyName: string;
  licensePlate: LicensePlate;
}

@Component({
  selector: 'app-driver-info-form',
  template: `
    <mat-card *ngIf="formGroup" fxFlex [formGroup]="formGroup">
      <mat-card-header>
        <mat-card-title
          >{{ title }}<strong *ngIf="required"> *</strong></mat-card-title
        >
        <div fxFlex></div>
        <button
          *ngIf="shipper"
          mat-stroked-button
          (click)="copyShipperCompanyName()"
          [disabled]="
            (formGroup.controls.companyName.value !== '' &&
              formGroup.controls.companyName.value !== null)
          "
        >
          Anl. Sped. übernehmen
        </button>
      </mat-card-header>
      <mat-card-content fxLayout="column">
        <mat-form-field fxFlex="50%">
          <input
            matInput
            trim
            [formControl]="formGroup.controls.companyName"
            placeholder="Firmenname (auf LKW)"
            i18n-placeholder="CompanyNameOnTruck|Label Name@@CompanyNameOnTruck"
          />
          <mat-error
            *ngIf="
              formGroup.controls.companyName?.touched &&
              formGroup.controls.companyName.errors?.required
            "
            >Pflichtfeld</mat-error
          >
          <mat-error *ngIf="formGroup.controls.companyName?.errors?.maxlength">
            <span i18n="MaxLength|Label Pflichtfeld@@MaxLength"
              >Die Eingabe ist zu lang</span
            >
          </mat-error>
          <button
            mat-button
            *ngIf="formGroup.controls.companyName.value"
            matSuffix
            mat-icon-button
            aria-label="Clear"
            (click)="formGroup.controls.companyName.setValue('')"
          >
            <mat-icon>close</mat-icon>
          </button>
        </mat-form-field>
        <div fxFlex fxLayout="row" fxLayoutGap="10px">
          <app-license-plate-form
            fxLayoutGap="10px"
            [subForm]="formGroup.controls.licensePlate"
            placeholder="Kennzeichen"
            i18n-placeholder="LicensePlate|Label Kennzeichen@@LicensePlate"
            [required]="required"
          ></app-license-plate-form>
          <mat-form-field fxFlex>
            <input
              matInput
              trim
              [formControl]="formGroup.controls.driverName"
              placeholder="Name des Fahrers"
              i18n-placeholder="
                NameOfTheDriver|Label Name des Fahrers@@NameOfTheDriver"
            />
            <mat-error
              *ngIf="
                formGroup.controls.driverName?.touched &&
                formGroup.controls.driverName.errors?.required
              "
              >Pflichtfeld</mat-error
            >
            <mat-error *ngIf="formGroup.controls.driverName?.errors?.maxlength">
              <span i18n="MaxLength|Label Pflichtfeld@@MaxLength"
                >Die Eingabe ist zu lang</span
              >
            </mat-error>
          </mat-form-field>
        </div>
      </mat-card-content>
    </mat-card>
  `,
  providers: subformComponentProviders(DriverInfoFormComponent),
})
export class DriverInfoFormComponent extends NgxSubFormComponent<DriverInfo>
  implements OnChanges {
  constructor() {
    super();
  }

  @Input() title: string;
  @Input() shipper: IPartner;
  @Input() required: boolean;

  protected emitInitialValueOnInit = false;

  protected getFormControls(): Controls<DriverInfo> {
    return {
      driverName: new FormControl(null, [
        Validators.required,
        // Validators.pattern(/^(\s+\S+\s*)*(?!\s).*$/),
        Validators.maxLength(ValidationDataService.maxLength.driverName),
      ]),
      companyName: new FormControl(null, [
        Validators.required,
        // Validators.pattern(/^(\s+\S+\s*)*(?!\s).*$/),
        Validators.maxLength(ValidationDataService.maxLength.companyName),
      ]),
      licensePlate: new SubFormGroup(null, [
        Validators.required,
        // Validators.pattern(/^(\s+\S+\s*)*(?!\s).*$/)
      ]),
    };
  }

  public getDefaultValues(): DriverInfo {
    return {
      driverName: null,
      companyName: null,
      licensePlate: null,
    };
  }

  ngOnChanges(changes: SimpleChanges) {
    super.ngOnChanges(changes);
    this.setRequiredState(this.required);
  }

  copyShipperCompanyName() {
    this.formGroup.controls.companyName.setValue(this.shipper.companyName);
  }

  public setRequiredState(isRequired: boolean) {
    const driverName = this.formGroup.controls.driverName;
    driverName.setValidators(isRequired ? [Validators.required] : []);
    driverName.updateValueAndValidity({ onlySelf: true, emitEvent: false });
    const companyName = this.formGroup.controls.companyName;
    companyName.setValidators(isRequired ? [Validators.required] : []);
    companyName.updateValueAndValidity({ onlySelf: true, emitEvent: false });
  }
}
