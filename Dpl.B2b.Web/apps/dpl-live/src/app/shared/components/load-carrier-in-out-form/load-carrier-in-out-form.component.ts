import { Component, OnInit, Input } from '@angular/core';
import { ILoadCarrier } from 'apps/dpl-live/src/app/master-data/load-carriers/state/load-carrier.model';
import {
  NgxSubFormComponent,
  Controls,
  subformComponentProviders,
  NgxSubFormRemapComponent,
  FormGroupOptions,
  TypedValidatorFn,
  SubFormGroup,
} from 'ngx-sub-form';
import { FormControl, Validators } from '@angular/forms';

import { atLeastOneValidator, WrappedControlForm } from '@dpl/dpl-lib';
import { merge, Observable, EMPTY, combineLatest } from 'rxjs';
import { tap, switchMap, startWith, map } from 'rxjs/operators';
import { LoadCarrierQuantity } from '../load-carrier-form/load-carrier-form.component';
import { LoadCarrierPickerContext } from '../load-carrier-picker/load-carrier-picker.component';

export interface LoadCarrierInOutForm {
  id: number;
  quantityIn: number;
  quantityOut: number;
  quantity: number;
}

@Component({
  selector: 'app-load-carrier-in-out-form',
  template: `
    <div *ngIf="viewData$ | async" fxFlex fxLayout="row" fxLayoutGap="10px">
      <ng-container *ngIf="formGroup" [formGroup]="formGroup">
        <load-carrier-picker
          [subForm]="formGroup.controls.id"
          [context]="context"
          filter="all"
          fxFlex="1 0 75px"
        ></load-carrier-picker>
        <mat-form-field fxFlex="1 0 75px" class="app-form-field-auto">
          <input
            matInput
            [formControl]="formGroup.controls.quantityIn"
            type="number"
            digit-only
            placeholder="Menge"
          />
          <mat-error
            i18n="
              ErrorMustbeGreaterZero|Fehler Wert muss größer 0
              sein@@ErrorMustBeGreaterZero"
          >
            Der Wert muss größer 0 sein
          </mat-error>
        </mat-form-field>
        <mat-form-field fxFlex="1 0 75px" class="app-form-field-auto">
          <input
            matInput
            [formControl]="formGroup.controls.quantityOut"
            type="number"
            digit-only
            placeholder="Menge"
          />
          <mat-error
            i18n="
              ErrorMustbeGreaterZero|Fehler Wert muss größer 0
              sein@@ErrorMustBeGreaterZero"
          >
            Der Wert muss größer 0 sein
          </mat-error>
        </mat-form-field>
        <mat-form-field fxFlex="1 0 75px" class="app-form-field-auto">
          <input
            matInput
            [formControl]="formGroup.controls.quantity"
            type="number"
            digit-only
            placeholder="Menge"
          />
          <mat-error
            i18n="
              ErrorMustbeGreaterZero|Fehler Wert muss größer 0
              sein@@ErrorMustBeGreaterZero"
          >
            Der Wert muss größer 0 sein
          </mat-error>
        </mat-form-field>
      </ng-container>
    </div>
  `,
  styles: [],
  providers: subformComponentProviders(LoadCarrierInOutFormComponent),
})
export class LoadCarrierInOutFormComponent
  extends NgxSubFormRemapComponent<LoadCarrierQuantity, LoadCarrierInOutForm>
  implements OnInit {
  constructor() {
    super();
  }

  @Input() context: LoadCarrierPickerContext;

  viewData$: Observable<{}>;

  ngOnInit() {
    // TODO attach to value changes of in / out and calculate
    const quantityChanges$ = merge(
      this.formGroup.controls.quantityIn.valueChanges,
      this.formGroup.controls.quantityOut.valueChanges
    ).pipe(
      tap(() => {
        const difference =
          this.formGroup.controls.quantityIn.value -
          this.formGroup.controls.quantityOut.value;
        this.formGroup.controls.quantity.patchValue(difference);
      })
    );

    const formSync$ = combineLatest(quantityChanges$).pipe(
      switchMap(() => EMPTY), // we do not wanne trigger new view data
      startWith(null) // needed so viewdata outputs values
    );

    this.viewData$ = formSync$.pipe(map(() => ({})));
  }

  protected getFormControls(): Controls<LoadCarrierInOutForm> {
    return {
      id: new SubFormGroup<number, WrappedControlForm<number>>(
        null,
        Validators.required
      ),
      quantityIn: new FormControl(null),
      quantityOut: new FormControl(null),
      quantity: new FormControl(
        { value: null, disabled: true },
        Validators.required
      ),
    };
  }

  getFormGroupControlOptions(): FormGroupOptions<LoadCarrierInOutForm> {
    return {
      // validators: atLeastOneValidator(
      //   [this.formGroup.controls.quantityIn, this.formGroup.controls.quantityOut],
      //   [Validators.min(1), Validators.required]
      // ) as TypedValidatorFn<any>
    };
  }

  getDefaultValue() {
    const defaultValue: LoadCarrierInOutForm = {
      id: null,
      quantityIn: null,
      quantityOut: null,
      quantity: null,
    };

    return defaultValue;
  }

  protected transformToFormGroup(
    obj: LoadCarrierQuantity
  ): LoadCarrierInOutForm {
    return {
      ...obj,
      ...{
        quantityIn: obj.quantity > 0 ? obj.quantity : 0,
        quantityOut: obj.quantity < 0 ? obj.quantity * -1 : 0,
      },
    };
  }
  protected transformFromFormGroup(
    formValue: LoadCarrierInOutForm
  ): LoadCarrierQuantity {
    const {
      id,
      quantity,
    } = this.formGroup.getRawValue() as LoadCarrierInOutForm;
    return {
      id,
      quantity,
    };
  }
}
