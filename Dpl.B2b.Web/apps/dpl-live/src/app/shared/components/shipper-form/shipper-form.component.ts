import { Component, OnInit, Input } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';
import { Address } from '@app/api/dpl';
import { AddressCity } from '../../components';
import { ValidationDataService } from '../../services';
import {
  Controls,
  NgxSubFormRemapComponent,
  subformComponentProviders,
  SubFormGroup,
} from 'ngx-sub-form';

export interface Shipper {
  guid: string;
  name: string;
  address: Address;
}

export interface ShipperForm {
  guid: string;
  name: string;
  street1: string;
  cityInfo: AddressCity;
}

// TODO i18n
@Component({
  selector: 'app-shipper-form',
  template: `
    <mat-card [formGroup]="formGroup">
      <mat-card-header>
        <mat-card-title>
          <ng-container *ngIf="title; else fallbackTitle">
            {{ title }}
          </ng-container>
          <ng-template #fallbackTitle>
            <ng-container i18n="Spedition|Label Spedition@@Spedition">
              Spedition
            </ng-container>
          </ng-template></mat-card-title
        >
      </mat-card-header>
      <mat-card-content fxLayout="column">
        <div fxLayout="row" fxLayoutGap="10px">
          <mat-form-field fxFlex="50%">
            <input
              matInput
              [formControl]="formGroup.controls.name"
              placeholder="Name der Firma"
              i18n-placeholder="CompanyName|Label Name@@CompanyName"
            />
            <mat-error
              *ngIf="
                formGroup.controls.name.touched &&
                formGroup.controls.name.errors?.maxlength
              "
            >
              <span i18n="MaxLength|Label Pflichtfeld@@MaxLength"
                >Die Eingabe ist zu lang</span
              >
            </mat-error>
          </mat-form-field>
          <mat-form-field fxFlex="50%">
            <input
              matInput
              [formControl]="formGroup.controls.street1"
              placeholder="Straße"
              i18n-placeholder="Street|Label Straße@@Street"
            />
            <mat-error
              *ngIf="
                formGroup.controls.street1.touched &&
                formGroup.controls.street1.errors?.maxlength
              "
            >
              <span i18n="MaxLength|Label Pflichtfeld@@MaxLength"
                >Die Eingabe ist zu lang</span
              >
            </mat-error>
          </mat-form-field>
        </div>
        <app-address-city-form
          [subForm]="formGroup.controls.cityInfo"
        ></app-address-city-form>
      </mat-card-content>
    </mat-card>
  `,
  styles: [],
  providers: subformComponentProviders(ShipperFormComponent),
})
export class ShipperFormComponent extends NgxSubFormRemapComponent<
  Shipper,
  ShipperForm
> {
  constructor() {
    super();
  }

  @Input() title: string;

  protected getFormControls(): Controls<ShipperForm> {
    return {
      guid: new FormControl(null),
      name: new FormControl(null, [
        Validators.maxLength(ValidationDataService.maxLength.companyName),
      ]),
      street1: new FormControl(null, [
        Validators.maxLength(ValidationDataService.maxLength.street),
      ]),
      cityInfo: new SubFormGroup<AddressCity>(null),
    };
  }

  getDefaultValues() {
    const defaultValue: ShipperForm = {
      guid: null,
      name: null,
      street1: null,
      cityInfo: null,
    };
    return defaultValue;
  }

  protected transformToFormGroup(shipper: Shipper): ShipperForm {
    if (!shipper) {
      return null;
    }

    const { guid, name, address } = shipper || {};
    const { street1, postalCode, city, state, country } = address || {};

    return {
      guid,
      name,
      street1,
      cityInfo: {
        postalCode,
        city,
        state,
        country,
      },
    };
  }

  protected transformFromFormGroup({
    guid,
    name,
    street1,
    cityInfo,
  }: ShipperForm): Shipper {
    const { postalCode, city, state, country } = cityInfo || {};
    return {
      guid,
      name,
      address: {
        street1,
        postalCode,
        city,
        state,
        country,
      },
    };
  }
}
