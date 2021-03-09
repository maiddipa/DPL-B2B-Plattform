import {
  Component,
  OnInit,
  ChangeDetectionStrategy,
  Input,
  Output,
  EventEmitter,
  OnDestroy,
  ChangeDetectorRef,
} from '@angular/core';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import * as _ from 'lodash';
import { Observable, Subject } from 'rxjs';
import { first, startWith, takeUntil, tap } from 'rxjs/operators';
import { LoadingService } from '../../../../../../../libs/dpl-lib/src';
import { DplApiService } from '../../../core';
import {
  LoadCarrierReceiptSortingOption,
  LoadCarrierSortingCreateRequest,
} from '../../../core/services/dpl-api-services';

type PositionsQuantity = {
  [key: number]: {
    sourceQuantity: number;
    sortQuantity: number;
  };
};

@Component({
  selector: 'dpl-sorting-form',
  templateUrl: './sorting-form.component.html',
  styleUrls: ['./sorting-form.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class SortingFormComponent implements OnInit, OnDestroy {
  @Input() sortInput: LoadCarrierReceiptSortingOption;
  @Input() openedInDialog: boolean;
  @Input() submitFromDialog: Observable<boolean>;
  @Input() resetFromDialog: Observable<boolean>;
  @Output() formSubmitted = new EventEmitter<boolean>();
  @Output() formValid = new EventEmitter<boolean>();

  form: FormGroup;
  positions: FormArray;

  positionsQuantity: PositionsQuantity = {};

  notifier = new Subject();

  constructor(
    private formBuilder: FormBuilder,
    private dpl: DplApiService,
    private router: Router,
    private loadingService: LoadingService,
    private changeDetectorRef: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.buildForm();
    if (this.openedInDialog) {
      this.form.statusChanges
        .pipe(
          startWith(this.form.status),
          takeUntil(this.notifier),
          tap((status) => {
            console.log('form status');
            this.formValid.next(status === 'VALID');
          })
        )
        .subscribe();

      this.resetFromDialog
        .pipe(
          takeUntil(this.notifier),
          tap((submit) => this.resetSort())
        )
        .subscribe();

      this.submitFromDialog
        .pipe(
          takeUntil(this.notifier),
          tap((submit) => this.submitSort())
        )
        .subscribe();
    }
  }

  ngOnDestroy(): void {
    this.notifier.next();
    this.notifier.complete();
  }

  buildForm() {
    this.positions = this.formBuilder.array([]);

    this.form = this.formBuilder.group({
      loadCarrierReceiptId: [this.sortInput.id],
      positions: this.positions,
    });

    for (const position of this.sortInput.sortingPositions) {
      const outputs = this.formBuilder.array([], validatePositionOutput);
      this.positionsQuantity[position.loadCarrierId] = {
        sourceQuantity: position.quantity,
        sortQuantity: 0,
      };
      outputs.valueChanges
        .pipe(
          takeUntil(this.notifier),
          tap((value) => {
            this.positionsQuantity[
              position.loadCarrierId
            ].sortQuantity = value
              .map((v) => v.quantity)
              .reduce((sum, current) => sum + current, 0);
          })
        )
        .subscribe();

      this.positions.push(
        this.formBuilder.group({
          loadCarrierId: [position.loadCarrierId, []],
          quantity: [{ value: position.quantity, disabled: true }],
          outputs: outputs,
        })
      );

      for (const sortingQuality of position.possibleSortingQualities) {
        outputs.push(
          this.formBuilder.group({
            loadCarrierQualityId: [sortingQuality.id],
            quantity: [null, Validators.min(0)],
          })
        );
      }
    }
    if (this.openedInDialog) {
      this.form.statusChanges
        .pipe(
          startWith(this.form.status),
          takeUntil(this.notifier),
          tap((status) => {
            console.log('form status');
            this.formValid.next(status === 'VALID');
          })
        )
        .subscribe();
    }
  }

  getOutput(i: number) {
    return this.positions.controls[i].get('outputs') as FormArray;
  }

  submitSort() {
    if (this.form.valid) {
      const sortingRequest: LoadCarrierSortingCreateRequest = this.form.value;
      console.log(sortingRequest);

      this.dpl.loadCarrierSortingService
        .post({
          ...sortingRequest,
          positions: sortingRequest.positions.map((position) => {
            return {
              loadCarrierId: position.loadCarrierId,
              outputs: position.outputs.filter((x) => x.quantity),
            };
          }),
        })
        .pipe(
          this.loadingService.showLoadingWhile(),
          tap((result) => {
            if (this.openedInDialog) {
              // opened in dialog
              this.formSubmitted.next(true);
            } else {
              // opened by route
              this.router.navigate(['/load-carrier-receipts']);
            }
          })
        )
        .subscribe();
    }
  }
  resetSort() {
    this.buildForm();
    this.changeDetectorRef.markForCheck();
    this.changeDetectorRef.detectChanges();
    this.formValid.next(false);
  }

  getPositionQuantity(loadCarrierId: number) {
    return this.positionsQuantity[loadCarrierId];
    // return this.positionsQuantity.find(
    //   (x) => x.loadCarrierId === loadCarrierId
    // );
  }

  takeRemaining(
    loadCarrierId: number,
    positionIndex: number,
    outputIndex: number
  ) {
    const quantityInput = ((this.positions.controls[positionIndex] as FormGroup)
      .controls['outputs'] as FormArray).controls[outputIndex].get('quantity');

    quantityInput.patchValue(
      quantityInput.value +
        (this.getPositionQuantity(loadCarrierId).sourceQuantity -
          this.getPositionQuantity(loadCarrierId).sortQuantity)
    );
  }

  takeAll(loadCarrierId: number, positionIndex: number, outputIndex: number) {
    const quantityInput = ((this.positions.controls[positionIndex] as FormGroup)
      .controls['outputs'] as FormArray).controls[outputIndex].get('quantity');

    quantityInput.patchValue(
      this.getPositionQuantity(loadCarrierId).sourceQuantity
    );
  }
}

function validatePositionOutput(arr: FormArray) {
  if (arr?.parent) {
    const positionQuantity = parseInt(arr.parent.get('quantity').value);

    const outputQuantity = arr.controls
      .map((outputFormGroup: FormGroup) => {
        return parseInt(
          outputFormGroup.get('quantity').value
            ? outputFormGroup.get('quantity').value
            : 0
        );
      })
      .reduce((sum, current) => sum + current, 0);

    if (positionQuantity === outputQuantity) {
      return null;
    } else if (positionQuantity < outputQuantity) {
      return { quantityHigh: outputQuantity - positionQuantity };
    } else {
      return {
        quantityLow: outputQuantity
          ? positionQuantity - outputQuantity
          : positionQuantity,
      };
    }
  }
  return null;
}
