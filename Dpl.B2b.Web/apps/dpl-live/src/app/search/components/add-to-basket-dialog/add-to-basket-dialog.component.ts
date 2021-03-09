import {
  ChangeDetectorRef,
  Component,
  EventEmitter,
  Inject,
  Input,
  OnInit,
  Output,
} from '@angular/core';
import { FormControl } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { LoadCarrierOfferingDetail } from '@app/api/dpl';
import { ILabelValue } from '@dpl/dpl-lib';
import { startsWith } from 'lodash';
import { Controls, NgxAutomaticRootFormComponent } from 'ngx-sub-form';
import { combineLatest, EMPTY, Observable, of } from 'rxjs';
import { map, publishReplay, refCount, startWith, tap } from 'rxjs/operators';

export interface AddToBasketDialogComponentData {
  options: {
    dayOfWeekOption: ILabelValue<number>;
    offeringDetails: LoadCarrierOfferingDetail[];
  }[];
}

export interface AddToBasketDialogComponentResponse {
  dayOfWeek: number;
  orderGroupGuid: string;
  baseLoadCarrierId: number;
}

type AddToBasketDialogComponentDataOption = AddToBasketDialogComponentData['options'][0];

type ViewData = {
  options: AddToBasketDialogComponentDataOption[];
  baseLoadCarrierOptions: LoadCarrierOfferingDetail[];
};

type AddToBasketDialogComponentForm = {
  day: AddToBasketDialogComponentDataOption;
  baseLoadCarrier: LoadCarrierOfferingDetail;
};

@Component({
  templateUrl: './add-to-basket-dialog.component.html',
  styleUrls: ['./add-to-basket-dialog.component.scss'],
})
export class AddToBasketDialogComponent
  extends NgxAutomaticRootFormComponent<AddToBasketDialogComponentForm>
  implements OnInit {
  @Input()
  dataInput: Required<AddToBasketDialogComponentForm>;

  @Output()
  dataOutput = new EventEmitter<AddToBasketDialogComponentForm>();

  viewData$: Observable<ViewData>;
  constructor(
    public dialogRef: MatDialogRef<
      AddToBasketDialogComponent,
      AddToBasketDialogComponentResponse
    >,
    @Inject(MAT_DIALOG_DATA) public data: AddToBasketDialogComponentData,
    cd: ChangeDetectorRef
  ) {
    super(cd);
  }

  ngOnInit() {
    super.ngOnInit();

    const options$ = of(this.data.options).pipe(
      tap((options) => {
        this.formGroup.controls.day.patchValue(options[0]);
        this.formGroup.controls.baseLoadCarrier.patchValue(
          options[0].offeringDetails[0]
        );
      }),
      publishReplay(1),
      refCount()
    );

    const baseLoadCarrierSync$ = this.formGroup.controls.day.valueChanges.pipe(
      tap((day) => {
        // ensure base load carrier stays the same after changing days
        // if not possible select first entry of base load carriers for new day
        const prevSelected = this.formGroup.controls.baseLoadCarrier.value;
        const newSelection =
          day.offeringDetails.find(
            (i) => i.baseLoadCarrierId === prevSelected.baseLoadCarrierId
          ) || day.offeringDetails[0];

        this.formGroup.controls.baseLoadCarrier.patchValue(newSelection);
      }),
      map(() => EMPTY),
      startWith(null)
    );

    const baseLoadCarrierOptions$ = this.formGroup.controls.day.valueChanges.pipe(
      map((option: AddToBasketDialogComponentDataOption) => {
        return option.offeringDetails;
      })
    );

    this.viewData$ = combineLatest([
      baseLoadCarrierOptions$,
      options$,
      baseLoadCarrierSync$,
    ]).pipe(
      map(([baseLoadCarrierOptions, options]) => {
        const viewData: ViewData = {
          options,
          baseLoadCarrierOptions,
        };

        return viewData;
      })
    );
  }

  onAddToBasket() {
    const formValue = this.dataValue;

    const response: AddToBasketDialogComponentResponse = {
      dayOfWeek: formValue.day.dayOfWeekOption.value,
      baseLoadCarrierId: formValue.baseLoadCarrier.baseLoadCarrierId,
      orderGroupGuid: formValue.baseLoadCarrier.guid,
    };

    this.dialogRef.close(response);
  }

  onCancel() {
    this.dialogRef.close();
  }

  protected getFormControls(): Controls<AddToBasketDialogComponentForm> {
    return {
      day: new FormControl(null),
      baseLoadCarrier: new FormControl(null),
    };
  }
}
