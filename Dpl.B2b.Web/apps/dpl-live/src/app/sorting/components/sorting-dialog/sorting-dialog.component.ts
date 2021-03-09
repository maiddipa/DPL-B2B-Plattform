import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { BehaviorSubject, combineLatest, Observable, Subject } from 'rxjs';
import { map, tap } from 'rxjs/operators';
import { LoadingService } from '../../../../../../../libs/dpl-lib/src';
import { DplApiService } from '../../../core';
import { LoadCarrierReceiptSortingOption } from '../../../core/services/dpl-api-services';
import { SortingService } from '../../services/sorting.service';

type ViewData = {
  sortInput: LoadCarrierReceiptSortingOption;
};

export type SortDialogData = {
  receiptId: number;
};

export type SortDialogResult = {};

@Component({
  templateUrl: './sorting-dialog.component.html',
  styleUrls: ['./sorting-dialog.component.scss'],
})
export class SortingDialogComponent implements OnInit {
  data: SortDialogData;
  viewData$: Observable<ViewData>;
  formValid = false;
  submitSub = new Subject<boolean>();
  submit$ = this.submitSub.asObservable();
  resetSub = new Subject<boolean>();
  reset$ = this.resetSub.asObservable();

  constructor(
    private dpl: DplApiService,
    private dialogRef: MatDialogRef<SortingDialogComponent>,
    private loadingService: LoadingService,
    private sortingService: SortingService,
    @Inject(MAT_DIALOG_DATA) data: SortDialogData
  ) {
    this.data = data;
  }

  ngOnInit(): void {
    const receipt$ = this.dpl.loadCarrierReceipts
      .getSortingOptionsByReceiptId(this.data.receiptId)
      .pipe(this.loadingService.showLoadingWhile());

    this.viewData$ = combineLatest([receipt$]).pipe(
      map(([receipt]) => {
        const viewData: ViewData = {
          sortInput: receipt,
        };
        return viewData;
      })
    );
  }

  close() {
    this.sortingService
      .sortCancelDialog()
      .pipe(
        tap((result) => {
          if (result?.confirmed) {
            this.dialogRef.close();
          }
        })
      )
      .subscribe();
  }

  formStatusChange(valid: boolean) {
    this.formValid = valid;
  }

  formSubmitted(result: boolean) {
    this.dialogRef.close();
  }

  submit() {
    this.sortingService
      .sortCompleteDialog()
      .pipe(
        tap((result) => {
          if (result?.confirmed) {
            this.submitSub.next(true);
          }
        })
      )
      .subscribe();
  }
  reset() {
    this.resetSub.next(true);
  }
}
