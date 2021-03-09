import {
  Component,
  Input,
  OnChanges,
  OnInit,
  SimpleChanges,
} from '@angular/core';
import { AbstractControl, ValidationErrors } from '@angular/forms';
import {
  GenericArrayFormConfig,
  NgxSingleFieldSubFormComponent,
  WrappedControlForm,
} from '@dpl/dpl-lib';
import { subformComponentProviders, SubFormGroup } from 'ngx-sub-form';
import { combineLatest, EMPTY, Observable, of, ReplaySubject } from 'rxjs';
import { map, startWith, switchMap, tap } from 'rxjs/operators';

import { LoadCarrierQuantity } from '../load-carrier-form/load-carrier-form.component';
import { LoadCarrierPickerContext } from '../load-carrier-picker/load-carrier-picker.component';

export type LoadCarriersForm = {
  loadCarriers: LoadCarrierQuantity;
};

function getValue(value: LoadCarrierQuantity[] | undefined) {
  if (!value || !Array.isArray(value)) {
    return [];
  }

  return value;
}

function oneHasQuantity(control: AbstractControl): ValidationErrors | null {
  const filtered = getValue(control.value).filter((i) => i.quantity);

  if (filtered.length !== 0) {
    return;
  }

  return {
    oneHasQuantity: true,
  };
}

function onlyOnce(control: AbstractControl): ValidationErrors | null {
  const counter = getValue(control.value).reduce((prev, current) => {
    prev[current.id] = (prev[current.id] || 0) + 1;
    return prev;
  }, {} as { [key: number]: number });

  if (Object.keys(counter).some((key) => counter[key] > 1)) {
    return {
      onlyOnce: true,
    };
  }

  return null;
}

interface ViewData {}

@Component({
  selector: 'app-load-carriers-form',
  template: `
    <mat-card
      fxFlex
      *ngIf="(viewData$ | async) && formGroup"
      [formGroup]="formGroup"
    >
      <mat-card-header>
        <mat-card-title i18n="LoadCarrier|Label Ladungsträger@@LoadCarrier"
          >Ladungsträger<strong> *</strong></mat-card-title
        >
      </mat-card-header>
      <mat-card-content fxLayout="column">
        <div *ngIf="context === 'exchange'" fxLayout="row" fxLayoutAlign="end">
          <div fxFlex="1 0 75px"></div>
          <label fxFlex="1 0 75px" i18n="Incoming|Label Eingang@@Incoming"
            >Eingang</label
          >
          <label fxFlex="1 0 75px" i18n="Outgoing|Label Ausgang@@Outgoing"
            >Ausgang</label
          >
          <label fxFlex="1 0 75px" i18n="Difference|Label Differenz@@Difference"
            >Differenz</label
          >
          <div fxFlex="0 0 40px"></div>
        </div>
        <ng-container [ngSwitch]="context">
          <generic-array-form
            *ngSwitchCase="'exchange'"
            [config]="loadCarriersConfig"
            [subForm]="formGroup.controls.innerControl"
          >
            <ng-template let-control="control">
              <app-load-carrier-in-out-form
                [subForm]="control"
                [context]="context"
                fxLayoutGap="10px"
              ></app-load-carrier-in-out-form>
            </ng-template>
          </generic-array-form>
          <generic-array-form
            *ngSwitchDefault
            [config]="loadCarriersConfig"
            [subForm]="formGroup.controls.innerControl"
          >
            <ng-template
              let-control="control"
              let-context="context"
              let-allowLoadCarrierSelection="allowLoadCarrierSelection"
            >
              <app-load-carrier-form
                [subForm]="control"
                [context]="context"
                [allowLoadCarrierSelection]="allowLoadCarrierSelection"
                fxLayoutGap="10px"
              ></app-load-carrier-form>
            </ng-template>
          </generic-array-form>
        </ng-container>
        <mat-error
          *ngIf="
            formGroup.controls.innerControl.touched &&
            formGroup.controls.innerControl.errors?.oneHasQuantity
          "
          >Mindestens für einen Ladungsträger muss eine Menge angegeben
          werden.</mat-error
        >
        <mat-error
          *ngIf="
            formGroup.controls.innerControl.touched &&
            formGroup.controls.innerControl.errors?.onlyOnce
          "
          >Jeder Ladungsträger kann nur einmal ausgewählt werden.</mat-error
        >
      </mat-card-content>
    </mat-card>
  `,
  styles: [],
  providers: subformComponentProviders(LoadCarriersFormComponent),
})
export class LoadCarriersFormComponent
  extends NgxSingleFieldSubFormComponent<LoadCarrierQuantity[]>
  implements OnChanges, OnInit {
  protected emitInitialValueOnInit = false;
  @Input() showAddAndRemove: boolean;
  @Input() allowLoadCarrierSelection = true;
  @Input() context: LoadCarrierPickerContext;

  changes$ = new ReplaySubject<void>();
  viewData$: Observable<ViewData>;

  loadCarriersConfig: GenericArrayFormConfig;

  constructor() {
    super();
  }

  ngOnInit(): void {
    const setValidators$ = this.changes$.pipe(
      map(() => {
        const isDepoDelivery =
          this.context === 'delivery' && !this.allowLoadCarrierSelection;

        const validators =
          this.context === 'voucher' || isDepoDelivery
            ? [oneHasQuantity, onlyOnce]
            : [onlyOnce];

        return validators;
      }),
      tap((validators) => {
        this.formGroup.controls.innerControl.setValidators(validators);
        this.formGroup.controls.innerControl.updateValueAndValidity();
      })
    );

    const formSync$ = combineLatest([setValidators$]).pipe(
      switchMap(() => EMPTY),
      startWith(null)
    );

    this.viewData$ = combineLatest([formSync$]).pipe(
      map(() => {
        const viewData: ViewData = {};
        return viewData;
      })
    );
  }

  ngOnChanges(changes: SimpleChanges) {
    super.ngOnChanges(changes);

    this.loadCarriersConfig = {
      defaultValue: {
        id: null,
        quantity: null,
      },
      options: {
        showAddAndRemove:
          this.showAddAndRemove !== undefined
            ? this.showAddAndRemove
            : this.context !== 'voucher',
      },
      requiredError: $localize`:Validierungsfehler der angezeigt wird wenn weniger nicht keine Palette ausgewählt ist.@@LoadCarrierFormRequiredError:Mindestens eine Palette`,
      context: {
        context: this.context,
        allowLoadCarrierSelection: this.allowLoadCarrierSelection,
      },
    };

    this.changes$.next();
  }

  protected transformFromFormGroup(
    formValue: WrappedControlForm<LoadCarrierQuantity[]>
  ) {
    const controlValue = super.transformFromFormGroup(formValue);
    if (
      controlValue &&
      (this.context === 'voucher' ||
        // depo delivery
        (this.context === 'delivery' && !this.allowLoadCarrierSelection))
    ) {
      return controlValue.filter((i) => i.quantity);
    }

    return controlValue;
  }

  getFormControl() {
    return (new SubFormGroup<LoadCarrierQuantity[]>(
      null,
      undefined
    ) as unknown) as AbstractControl;
  }
}
