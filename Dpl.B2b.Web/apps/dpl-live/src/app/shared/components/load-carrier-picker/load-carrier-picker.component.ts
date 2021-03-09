import { Component, OnInit, Input, OnChanges, SimpleChange, SimpleChanges } from '@angular/core';
import { Observable, of } from 'rxjs';
import { ILoadCarrier } from '../../../master-data/load-carriers/state/load-carrier.model';
import { LoadCarriersService } from '../../../master-data/load-carriers/services/load-carriers.service';
import { FormControl, AbstractControl, Validators } from '@angular/forms';
import { map, switchMap, filter } from 'rxjs/operators';
import { AccountsService } from '../../../accounts/services/accounts.service';
import { PostingAccountCondition } from '@app/api/dpl';
import { IAccount } from '../../../accounts/state/account.model';
import { NgxSingleFieldSubFormComponent } from '@dpl/dpl-lib';
import { subformComponentProviders } from 'ngx-sub-form';

export type LoadCarrierPickerContext =
  | 'demand'
  | 'supply'
  | 'voucher'
  | 'pickup'
  | 'delivery'
  | 'exchange'
  | 'transfer'
  | 'demandSelfTransport'
  | 'supplySelfTransport';

export type LoadCarrierPickerMode = 'id' | 'loadCarrier';

type Conditions = keyof Pick<
  IAccount,
  | 'dropoffConditions'
  | 'pickupConditions'
  | 'transferConditions'
  | 'supplyConditions'
  | 'demandConditions'
  | 'voucherConditions'
>;

@Component({
  selector: 'load-carrier-picker',
  template: `
    <mat-form-field fxFill [formGroup]="formGroup">
      <mat-select
        placeholder="Ladungsträger"
        [formControl]="formGroup.controls.innerControl"
        i18n-placeholder="LoadCarrier|Label Ladungsträger@@LoadCarrier"
        #loadCarrier="ngForm"
        [dplHighlight]="loadCarrier"
      >
        <mat-option
          *ngFor="let loadCarrier of loadCarriers$ | async"
          [value]="mode === 'id' ? loadCarrier.id : loadCarrier"
        >
          <dpl-load-carrier-display-title
            [context]="context"
            [loadCarrier]="loadCarrier"
          ></dpl-load-carrier-display-title>
          <!-- {{ loadCarrier.id | loadCarrier }} ({{
            loadCarrier.type.id | loadCarrier: 'type':'LongName'
          }}) -->
        </mat-option>
      </mat-select>
      <!-- TODO i18n -->
      <mat-error *ngIf="formGroup.controls.innerControl.errors?.required"
        >Pflichtfeld</mat-error
      >
      <!-- <mat-error *ngIf="form.hasError('required')"></mat-error> -->
    </mat-form-field>
  `,
  styles: [],
  providers: subformComponentProviders(LoadCarrierPickerComponent),
})
export class LoadCarrierPickerComponent
  extends NgxSingleFieldSubFormComponent<number>
  implements OnInit, OnChanges {

  @Input() allowLoadCarrierSelection = true;
  @Input() context: LoadCarrierPickerContext;
  @Input() mode: LoadCarrierPickerMode = 'id';

  protected emitInitialValueOnInit = false;
  loadCarriers$: Observable<ILoadCarrier[]>;

  constructor(
    private loadCarrierService: LoadCarriersService,
    private accounts: AccountsService
  ) {
    super();
  }

  ngOnChanges(changes: SimpleChanges){
    super.ngOnChanges(changes);

    if(this.allowLoadCarrierSelection){
      this.formGroup.controls.innerControl.enable();
    } else {
      this.formGroup.controls.innerControl.disable();
    }
  }

  ngOnInit() {


    this.loadCarriers$ = this.loadCarrierService.getLoadCarriers().pipe(
      switchMap((loadCarriers) => {

        // HACK once we have additional oreder conditions specified we canuse them here and activate the filters
        switch (this.context) {
          case 'pickup': {
            return this.filterByCondition(loadCarriers, 'supplyConditions');
          }
          case 'delivery': {
            return this.filterByCondition(loadCarriers, 'demandConditions');
          }
          case 'exchange': {
            return this.filterByCondition(loadCarriers, 'demandConditions');
          }
          case 'demand': {
            return this.filterByCondition(loadCarriers, 'demandConditions');
          }
          case 'supply': {
            return this.filterByCondition(loadCarriers, 'supplyConditions');
          }
          case 'transfer': {
            return this.filterByCondition(loadCarriers, 'transferConditions');
          }
          case 'demandSelfTransport': {
            return this.filterByCondition(loadCarriers, 'pickupConditions');
          }
          case 'supplySelfTransport': {
            return this.filterByCondition(loadCarriers, 'dropoffConditions');
          }
          case 'voucher': {
            return this.filterByCondition(loadCarriers, 'voucherConditions');
          }
          default:
            return of(loadCarriers);
        }
      })
    );
  }

  filterByCondition(loadCarriers: ILoadCarrier[], conditionsType: Conditions) {
    return this.accounts.getActiveAccount().pipe(
      filter((account) => !!account),
      map((account) => {
        const conditions = account[conditionsType];
        return loadCarriers.filter((i) =>
          conditions.some((c) => c.loadCarrierId === i.id)
        );
      })
    );
  }

  protected getFormControl() {
    return new FormControl(null, Validators.required);
  }
}
