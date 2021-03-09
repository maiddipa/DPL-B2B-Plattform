import {
  ChangeDetectionStrategy,
  ChangeDetectorRef,
  Component,
  EventEmitter,
  Input,
  OnInit,
  Output,
} from '@angular/core';
import { AbstractControl, FormControl, Validators } from '@angular/forms';
import { TransportOfferingBidCreateRequest } from '@app/api/dpl';
import { LoadingService } from '@dpl/dpl-lib';
import * as moment from 'moment';
import { Controls, NgxAutomaticRootFormComponent } from 'ngx-sub-form';
import { combineLatest, Observable } from 'rxjs';
import { map, startWith, tap } from 'rxjs/operators';

import { TransportsService } from '../services/transports.service';
import { ITransport } from '../state/transport.model';

interface TransportBid {
  pickupDate: {
    start: moment.Moment;
  };
  deliveryDate: {
    start: moment.Moment;
  };
  price: number;
  note: string;
}

type ViewData = {
  supply: {
    earliestDate: moment.Moment;
    latestDate: moment.Moment;
  };
  demand: {
    earliestDate: moment.Moment;
    latestDate: moment.Moment;
  };
};

type TransportOfferingBidCreateRequestBase = Omit<
  TransportOfferingBidCreateRequest,
  'divisionId'
>;

@Component({
  selector: 'dpl-transport-bid-form',
  template: `
    <ng-container *ngIf="viewData$ | async as data">
      <mat-card *ngIf="formGroup" [formGroup]="formGroup">
        <mat-card-header>
          <mat-card-title
            i18n="Label für Gebot abgeben form@@TransportBidFormTitle"
            >Angebot abgeben</mat-card-title
          >
        </mat-card-header>
        <div fxLayout="row" fxLayoutGap="10px" fxLayoutAlign="end center">
          <mat-form-field>
            <input
              matInput
              [formControl]="formGroup.controls.price"
              type="number"
              digit-only
              placeholder="Angebot (€)"
              i18n-placeholder="Label Preis für Gebot@@TransportBidPrice"
            />
            <mat-error
              i18n="
                Fehelr wenn Mindestgebot für Transport
                unterschritten@@TransportBidMinimumPriceError"
              >Der Wert muss größer 1 sein.</mat-error
            >
          </mat-form-field>
          <mat-form-field>
            <input
              matInput
              ngxDaterangepickerMd
              [minDate]="data.supply.earliestDate"
              [maxDate]="data.supply.latestDate"
              [autoApply]="true"
              startKey="start"
              singleDatePicker="true"
              [formControl]="formGroup.controls.pickupDate"
              placeholder="Abholdatum"
              i18n-placeholder="
                Label für Transport Abholdatum@@TransportBidPickupDate"
            />
          </mat-form-field>
          <mat-form-field>
            <input
              matInput
              ngxDaterangepickerMd
              [minDate]="data.demand.earliestDate"
              [maxDate]="data.demand.latestDate"
              [autoApply]="true"
              startKey="start"
              singleDatePicker="true"
              [formControl]="formGroup.controls.deliveryDate"
              placeholder="Lieferdatum"
              i18n-placeholder="
                Label für Transport Lieferdatum@@TransportBidDeliveryDate"
            />
            <mat-error
              *ngIf="formGroup.controls.deliveryDate.errors?.minDate"
              i18n="
                Form Validierungsfehler der angezeigt wird wenn Lieferdatum vor
                Abholdatum@@TransportBidDeliveryDatePriorToPickupDateError"
            >
              Lieferdatum kann nicht vor Abholdatum liegen
            </mat-error>
          </mat-form-field>
          <mat-form-field fxFlex>
            <input
              matInput
              [formControl]="formGroup.controls.note"
              placeholder="Notiz"
              i18n-placeholder="Notiz für Transport Gebot@@TransportBidNote"
            />
          </mat-form-field>
          <button
            mat-raised-button
            color="primary"
            (click)="onPlaceBid()"
            [disabled]="formGroup.invalid"
          >
            <mat-icon>add_box</mat-icon
            ><span
              i18n="
                Button Label um Gebot abzugeben@@TransportBidPlaceBidButtonLabel"
              >Gebot abgeben</span
            >
          </button>
        </div>
      </mat-card>
    </ng-container>
  `,
  styles: [],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class TransportBidFormComponent
  extends NgxAutomaticRootFormComponent<
    TransportOfferingBidCreateRequestBase,
    TransportBid
  >
  implements OnInit {
  @Input() transport: ITransport;

  @Input()
  dataInput: Required<TransportOfferingBidCreateRequestBase>;

  @Output()
  dataOutput = new EventEmitter<TransportOfferingBidCreateRequestBase>();

  viewData$: Observable<ViewData>;
  constructor(
    private transportService: TransportsService,
    private loadingService: LoadingService,
    cd: ChangeDetectorRef
  ) {
    super(cd);
  }

  ngOnInit() {
    super.ngOnInit();

    const minDate = (date: moment.Moment) => (control: AbstractControl) => {
      const value: moment.Moment = control.value ? control.value.start : null;

      if (!value || value.startOf('day') >= date.startOf('day')) {
        return null;
      }

      return {
        minDate: true,
      };
    };

    const earliestDeliveryDate$ = this.formGroup.controls.pickupDate.valueChanges.pipe(
      map((value: { start: moment.Moment }) =>
        value && value.start ? value.start.toDate() : null
      ),
      tap((value) => {
        if (value) {
          this.formGroup.controls.deliveryDate.enable();
          this.formGroup.controls.deliveryDate.setValidators(
            minDate(moment(value))
          );
        } else {
          this.formGroup.controls.deliveryDate.disable();
        }
      }),
      map((pickupDate) => {
        const earliestDeliveryDate = new Date(
          this.transport.demandInfo.earliestFulfillmentDateTime
        );
        return pickupDate > earliestDeliveryDate
          ? pickupDate
          : earliestDeliveryDate;
      }),
      startWith(this.transport.demandInfo.earliestFulfillmentDateTime)
    );

    this.viewData$ = combineLatest([earliestDeliveryDate$]).pipe(
      map(([earliestDeliveryDate]) => {
        const viewData: ViewData = {
          supply: {
            earliestDate: moment(
              this.transport.supplyInfo.earliestFulfillmentDateTime
            ),
            latestDate: moment(
              this.transport.supplyInfo.latestFulfillmentDateTime
            ),
          },
          demand: {
            earliestDate: moment(earliestDeliveryDate),
            latestDate: moment(
              this.transport.demandInfo.latestFulfillmentDateTime
            ),
          },
        };
        return viewData;
      })
    );
  }

  onPlaceBid() {
    if (this.formGroup.valid) {
      this.transportService
        .placeBid(this.transport.id, this.dataValue)
        .pipe(
          tap(() => this.formGroup.reset()),
          this.loadingService.showLoadingWhile()
        )
        .subscribe();
    }
  }

  protected getFormControls(): Controls<TransportBid> {
    return {
      pickupDate: new FormControl(null, Validators.required),
      deliveryDate: new FormControl(
        { value: null, disabled: true },
        Validators.required
      ),
      price: new FormControl(null, [Validators.required, Validators.min(1)]),
      note: new FormControl(null),
    };
  }

  getDefaultValue(): TransportBid {
    return {
      pickupDate: null,
      deliveryDate: null,
      price: null,
      note: null,
    };
  }

  transformFromFormGroup(
    value: TransportBid
  ): TransportOfferingBidCreateRequestBase {
    return {
      note: value.note,
      price: value.price,
      pickupDate:
        value.pickupDate && value.pickupDate.start
          ? value.pickupDate.start.startOf('day').toDate()
          : null,
      deliveryDate:
        value.deliveryDate && value.deliveryDate.start
          ? value.deliveryDate.start.startOf('day').toDate()
          : null,
      transportId: this.transport.id,
    };
  }
}
