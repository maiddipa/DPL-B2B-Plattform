import { Component, OnInit } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';
import { Address } from '@app/api/dpl';
import {
  Controls,
  NgxSubFormComponent,
  subformComponentProviders,
  NgxSubFormRemapComponent,
  SubFormGroup,
} from 'ngx-sub-form';
import { Omit } from '@dpl/dpl-lib';
import { AddressCity } from '../address-city-form/address-city-form.component';
import { ValidationDataService } from '@app/shared/services';

interface AddressForm {
  street1: string;
  cityInfo: AddressCity;
}

@Component({
  selector: 'app-address-form',
  template: `
    <div *ngIf="formGroup" fxFlex fxLayout="column" [formGroup]="formGroup">
      <mat-form-field>
        <input
          matInput
          [formControl]="formGroup.controls.street1"
          placeholder="Straße"
          i18n-placeholder="Street|Label Straße@@Street"
        />
      </mat-form-field>
      <app-address-city-form
        [subForm]="formGroup.controls.cityInfo"
      ></app-address-city-form>
    </div>
  `,
  styles: [],
  providers: subformComponentProviders(AddressFormComponent),
})
export class AddressFormComponent
  extends NgxSubFormRemapComponent<Address, AddressForm>
  implements OnInit {
  constructor() {
    super();
  }

  ngOnInit() {}

  protected getFormControls(): Controls<AddressForm> {
    return {
      street1: new FormControl(
        null,
        Validators.maxLength(ValidationDataService.maxLength.street)
      ),
      cityInfo: new SubFormGroup<AddressCity>(null),
    };
  }

  getDefaultValues() {
    const defaultValue: AddressForm = {
      street1: null,
      cityInfo: null,
    };
    return defaultValue;
  }

  protected transformToFormGroup(address: Address): AddressForm {
    if (!address) {
      return null;
    }

    const { street1, postalCode, city, state, country } = address;

    return {
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
    street1,
    cityInfo,
  }: AddressForm): Address {
    return {
      street1,
      ...cityInfo,
    };
  }
}
