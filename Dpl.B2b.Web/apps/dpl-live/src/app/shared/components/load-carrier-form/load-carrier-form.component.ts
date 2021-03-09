import {
  Component,
  OnInit,
  Input,
  ChangeDetectorRef,
  OnChanges,
  SimpleChanges,
} from '@angular/core';
import { ILoadCarrier } from 'apps/dpl-live/src/app/master-data/load-carriers/state/load-carrier.model';
import { DocumentTypesService } from '@app/shared/services/document-types.service';
import {
  Controls,
  NgxSubFormComponent,
  subformComponentProviders,
  SubFormGroup,
} from 'ngx-sub-form';
import { FormControl, ValidatorFn, Validators } from '@angular/forms';
import {
  distinctUntilChanged,
  filter,
  map,
  publishReplay,
  refCount,
  startWith,
  switchMap,
  tap,
} from 'rxjs/operators';
import {
  combineLatest,
  EMPTY,
  Observable,
  of,
  Subject,
  merge,
  ReplaySubject,
} from 'rxjs';
import { AccountsService } from '../../../accounts/services/accounts.service';
import {
  Balance,
  CustomerDocumentSettings,
} from '../../../core/services/dpl-api-services';
import { CustomersService } from '../../../customers/services/customers.service';
import { LoadCarriersService } from '../../../master-data/load-carriers/services/load-carriers.service';
import { LoadCarrierPickerContext } from '../load-carrier-picker/load-carrier-picker.component';

export interface LoadCarrierQuantity {
  id: number;
  quantity: number;
}

type ViewData = {};

//TODO i18n
@Component({
  selector: 'app-load-carrier-form',
  template: `
    <ng-container *ngIf="viewData$ | async as data" [formGroup]="formGroup">
      <load-carrier-picker
        [subForm]="formGroup.controls.id"
        [allowLoadCarrierSelection]="allowLoadCarrierSelection"
        [context]="context"
        fxFlex
      ></load-carrier-picker>
      <mat-form-field fxFlex>
        <input
          matInput
          [formControl]="formGroup.controls.quantity"
          type="number"
          digit-only
          placeholder="Menge"
          i18n-placeholder="Amount|Label Menge@@Amount"
          #loadCarrierQuantity="ngForm"
          [dplHighlight]="loadCarrierQuantity"
        />
        <!-- TODO i18n -->
        <mat-error
          *ngIf="
            formGroup.controls.quantity.touched &&
            formGroup.controls.quantity.errors?.required
          "
          >Pflichtfeld</mat-error
        >
        <mat-error
          i18n="
            ErrorMustbeGreaterZero|Fehler Wert muss größer 0
            sein@@ErrorMustBeGreaterZero"
          *ngIf="
            formGroup.controls.quantity.touched &&
            formGroup.controls.quantity.errors?.min
          "
          >Der Wert muss größer 0 sein.</mat-error
        >
        <mat-error
          i18n="
            LoadCarrierErrorMaxBalance|Saldo nicht
            ausreichend@@LoadCarrierErrorMaxBalance"
          *ngIf="
            formGroup.controls.quantity.touched &&
            formGroup.controls.quantity.errors?.maxBalance
          "
          >Saldo nicht ausreichend</mat-error
        >
        <mat-error
          i18n="
            LoadCarrierErrorMaxQuantity|Konfigurierte Maximale Anzahl
            überschritten@@LoadCarrierErrorMaxQuantity"
          *ngIf="
            formGroup.controls.quantity.touched &&
            formGroup.controls.quantity.errors?.max
          "
          >Maximale Anzahl überschritten</mat-error
        >
        <mat-hint
          class="app-mat-hint-warning"
          i18n="
            LoadCarrierErrorThresholdForWarningQuantity|Der Wert scheint sehr
            Hoch@@LoadCarrierErrorThresholdForWarningQuantity"
          *ngIf="
            !formGroup.controls.quantity.errors &&
            formGroup.controls.quantity.touched &&
            (showQuantityWarning$ | async)
          "
          >Der Wert scheint sehr hoch!</mat-hint
        >
      </mat-form-field>
    </ng-container>
  `,
  styles: [],
  providers: subformComponentProviders(LoadCarrierFormComponent),
})
export class LoadCarrierFormComponent
  extends NgxSubFormComponent<LoadCarrierQuantity>
  implements OnInit, OnChanges {
  @Input() allowLoadCarrierSelection = true;
  @Input() context: LoadCarrierPickerContext;

  private allowLoadCarrierSelection$ = new ReplaySubject<boolean>();
  protected emitInitialValueOnInit = false;
  viewData$: Observable<ViewData>;
  showQuantityWarning$: Observable<boolean>;

  constructor(
    private accountsService: AccountsService,
    private customerService: CustomersService,
    private loadCarriers: LoadCarriersService,
    private doucmentTypes: DocumentTypesService
  ) {
    super();
  }

  ngOnChanges(changes: SimpleChanges) {
    super.ngOnChanges(changes);

    this.allowLoadCarrierSelection$.next(this.allowLoadCarrierSelection);
  }

  ngOnInit() {
    const loadCarrierId$ = this.formGroup.controls.id.valueChanges.pipe(
      startWith(this.formGroup.controls.id.value),
      distinctUntilChanged(),
      tap((id) => {
        if (id === null || id === undefined) {
          return this.formGroup.controls.quantity.disable();
        }

        this.formGroup.controls.quantity.enable();
      })
    );

    const loadCarrierType$ = loadCarrierId$.pipe(
      filter(i => !!i),
      switchMap((loadCarrierId) =>
        this.loadCarriers.getLoadCarrierById(loadCarrierId)
      ),
      publishReplay(1),
      refCount()
    );

    const documentSettings$ = combineLatest([
      this.customerService.getActiveCustomer(),
      loadCarrierType$,
    ]).pipe(
      map(([customer, loadCarrier]) => {
        const documentTypeId = this.doucmentTypes.getDocumentTypeIdForLoadCarrierPickerContext(
          this.context
        );
        return customer.documentSettings?.find(
          (i) =>
            i.documentTypeId === documentTypeId &&
            i.loadCarrierTypeId == loadCarrier.type
        );
      }),
      publishReplay(1),
      refCount()
    );

    this.showQuantityWarning$ = combineLatest([
      documentSettings$,
      this.formGroup.controls.quantity.valueChanges,
    ]).pipe(
      map(([documentSettings, quantity]) => {
        return (
          documentSettings?.thresholdForWarningQuantity &&
          quantity > documentSettings.thresholdForWarningQuantity
        );
      }),
      distinctUntilChanged(),
      publishReplay(1),
      refCount()
    );

    const quantityValidator$ = documentSettings$.pipe(
      switchMap((documentSettings) =>
        this.getQuantityValidator(documentSettings)
      ),
      tap((validators) => {
        if (!validators) {
          return;
        }

        this.formGroup.controls.quantity.setValidators(validators);
        if (this.context === 'transfer') {
          this.formGroup.controls.quantity.markAsTouched();
        }
        this.formGroup.controls.quantity.updateValueAndValidity();
      })
    );

    const formSync$ = combineLatest([
      loadCarrierId$,
      quantityValidator$,
      this.showQuantityWarning$,
    ]).pipe(
      switchMap(() => EMPTY),
      startWith([null])
    );

    this.viewData$ = combineLatest([formSync$]).pipe(
      map(() => {
        const viewData: ViewData = {};
        return viewData;
      })
    );

    this.formGroup.markAsUntouched();
  }

  private getQuantityValidator(
    documentSettings: CustomerDocumentSettings
  ): Observable<ValidatorFn[]> {
    switch (this.context) {
      case 'transfer':
        return this.formGroup.controls.id.valueChanges.pipe(
          switchMap((loadCarrierId) => {
            return loadCarrierId
              ? this.accountsService.getBalanceForLoadCarrier(loadCarrierId)
              : of(null as Balance);
          }),
          map((balance) => {
            const balanceMax = balance ? balance.availableBalance : 0;
            return [
              Validators.required,
              Validators.min(1),
              maxCustom(balanceMax, 'Balance'),
            ];
          })
        );

      case 'voucher': {
        return of(documentSettings).pipe(
          map((settings) => {
            const validators = [Validators.min(1)];

            if (documentSettings?.maxQuantity) {
              validators.push(Validators.max(settings.maxQuantity));
            }

            return validators;
          })
        );
      }
      case 'delivery':
      case 'pickup': {
        return this.allowLoadCarrierSelection$.pipe(
          map((allowLoadCarrierSelection) => {
            // if this is true this is not a sorting case
            // HACK add additional contexts to be able to distiguish between sorting / non sorting
            if (allowLoadCarrierSelection) {
              return [Validators.required, Validators.min(1)];
            }

            return [Validators.min(0)];
          })
        );
      }

      default:
        return of([Validators.required, Validators.min(1)]);
    }
  }

  protected getFormControls(): Controls<LoadCarrierQuantity> {
    return {
      id: new SubFormGroup(null, Validators.required),
      quantity: new FormControl(null),
    };
  }

  getDefaultValue() {
    const defaultValue: LoadCarrierQuantity = {
      id: null,
      quantity: null,
    };

    return defaultValue;
  }
}

function maxCustom(max: number, postfix: string) {
  const maxValidator = Validators.max(max);

  return (control) => {
    const result = maxValidator(control);
    if (result) {
      const error = {} as any;
      error[`max${postfix}`] = true;
      return error;
    }

    return null;
  };
}
