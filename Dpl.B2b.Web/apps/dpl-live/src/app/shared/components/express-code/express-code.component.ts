import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import {
  DplProblemDetails,
  ExpressCode,
  ExpressCodesApiService,
  CustomerPartner,
  PartnerType, StateItem,
  VoucherType, PrintType,
} from '@app/api/dpl';
import { LoadCarrierQuantity } from '@app/shared';
import { ErrorService, ValidationDataService } from '@app/shared/services';
import { LoadingService, NgxSingleFieldSubFormComponent } from '@dpl/dpl-lib';
import { BehaviorSubject, combineLatest, EMPTY, Observable } from 'rxjs';
import { catchError, map, startWith, switchMap, tap } from 'rxjs/operators';
import {CustomerDivisionsService} from "../../../customers/services/customer-divisions.service";
import {AccountsService} from "../../../accounts/services/accounts.service";

export interface VoucherExpressCode extends ExpressCode {
  demoProperties?: {
    recipient?: CustomerPartner | null;
    shipper?: CustomerPartner | null;
    subShipper?: CustomerPartner | null;
    supplier?: CustomerPartner | null;
    ntg?: number;
    voucherType?: VoucherType;
    loadCarrier?: LoadCarrierQuantity[];
  };
}

export type ExpressCodePreset = keyof Pick<
  ExpressCode,
  'voucherPresets' | 'destinationAccountPreset' | 'loadCarrierReceiptPreset'
>;

type ViewData = {
  icon?: 'check_circle' | 'remove_circle';
  color?: 'primary' | 'accent' | 'warn';
};

type ExpressCodeStatus = 'valid' | 'invalid' | 'resetted';

@Component({
  selector: 'dpl-express-code',
  template: `
    <ng-container *ngIf="viewData$ | async as data">
      <div fxLayout="row" *ngIf="formGroup" [formGroup]="formGroup">
        <mat-form-field fxFlex="1 0 auto" appearance="outline">
          <mat-label i18n="DigitalCode|Label Digital-Code@@DigitalCode"
            >DPL-Digital-Code</mat-label
          >

          <input
            matInput
            placeholder="DPL-Digital-Code"
            [formControl]="formGroup.controls.innerControl"
            (keydown.enter)="onValidateExpressCode()"
            i18n-placeholder="DigitalCode|Label Digital-Code@@DigitalCode"
          />

          <mat-icon *ngIf="data.icon" matSuffix [color]="data.color">{{
            data.icon
          }}</mat-icon>
          <mat-error *ngIf="formGroup.controls.innerControl.errors?.minlength">
            <span i18n="MinLength|Die Eingabe ist zu kurz@@MinLength"
              >Die Eingabe ist zu kurz</span
            >
          </mat-error>
          <mat-error *ngIf="formGroup.controls.innerControl.errors?.maxlength">
            <span i18n="MaxLength|Die Eingabe ist zu lang@@MaxLength"
              >Die Eingabe ist zu lang</span
            >
          </mat-error>
        </mat-form-field>
      </div>
    </ng-container>
  `,
  styles: [],
})
export class ExpressCodeComponent extends NgxSingleFieldSubFormComponent<string>
  implements OnInit {
  @Input() preset: ExpressCodePreset;
  @Input() printType: PrintType;
  @Output() validatedExpressCode = new EventEmitter<ExpressCode>();

  loading$ = new BehaviorSubject<boolean>(false);
  expressCodeStatus$ = new BehaviorSubject<ExpressCodeStatus>('resetted');

  viewData$: Observable<ViewData>;

  constructor(
    private expressCodeService: ExpressCodesApiService,
    private customerDivisionsService: CustomerDivisionsService,
    private accountsService: AccountsService,
    private errorService: ErrorService,
    private snackBar: MatSnackBar,
    private loadingService: LoadingService,
  ) {
    super();
  }

  ngOnInit() {
    const expressCode$ = this.formGroup.controls.innerControl.valueChanges.pipe(
      tap(() => this.expressCodeStatus$.next('resetted')),
      tap(() => this.formGroup.controls.innerControl.markAsTouched()),
      switchMap(() => EMPTY),
      startWith(null)
    );

    const expressCodeStatus$ = this.expressCodeStatus$.asObservable();

    this.viewData$ = combineLatest([expressCodeStatus$, expressCode$]).pipe(
      map(([status]) => {
        const viewData: ViewData = this.getViewData(status);
        return viewData;
      })
    );
  }

  getViewData(status: ExpressCodeStatus): ViewData {
    switch (status) {
      case 'valid':
        return {
          icon: 'check_circle',
          color: 'primary',
        };
      case 'invalid':
        return {
          icon: 'remove_circle',
          color: 'warn',
        };

      default:
        return {};
    }
  }

  findStateItem(detail: DplProblemDetails, code: string): StateItem | null {
    let retValue: StateItem = null;
    detail.ruleStates.forEach((items) => {
      items.forEach((item) => {
        if ((item) => item.messageId === code) {
          retValue = item;
        }
      });
    });
    return retValue;
  }

  mapErrorCodeToText(err: any) {
    //TODO: i18n
    if (err.response) {
      let detailObj = JSON.parse(err.response);
      let detail = detailObj as DplProblemDetails;
      if (detail) {
        let resourceNotFound = this.findStateItem(
          detail,
          'Error|Common|ResourceNotFound'
        );
        if (resourceNotFound) {
          return 'Digital-Code existiert nicht.';
        }

        let digitalCodeCanceled = this.findStateItem(
          detail,
          'Error|ExpressCode|DigitalCodeCanceled'
        );
        if (digitalCodeCanceled) {
          return 'Digital-Code ist storniert.';
        }

        let digitalCodeExpired = this.findStateItem(
          detail,
          'Error|ExpressCode|DigitalCodeExpired'
        );
        if (digitalCodeExpired) {
          return 'Digital-Code ist abgelaufen.';
        }

        let digitalCodeInvalid = this.findStateItem(
          detail,
          'Error|ExpressCode|DigitalCodeInvalid'
        );
        if (digitalCodeInvalid) {
          return 'Digital-Code ist ungültig.';
        }
      }
    }

    return 'Digital-Code existiert nicht oder kann nicht mehr verwendet werden.';
  }

  onValidateExpressCode() {
    console.log(this.formGroup);
    if (this.formGroup.valid && this.formGroup.value) {
      let issuingPostingAccountId = null;
      this.accountsService.getActiveAccount().subscribe(account => {
        issuingPostingAccountId = account.id});

      let issuingCustomerDivisionId = null;
      this.customerDivisionsService.getActiveDivision().subscribe(division => {
        issuingCustomerDivisionId = division.id
      });
      const code = this.formGroup.value.toUpperCase();

      this.expressCodeService
          .getByCode({expressCode: code, printType: this.printType,
            issuingCustomerDivisionId: issuingCustomerDivisionId,issuingPostingAccountId: issuingPostingAccountId  })
          .pipe(
            catchError((err) =>
              this.errorService.handleApiError<ExpressCode>(err, {
                400: {
                  snackBar: {
                    text: this.mapErrorCodeToText(err),
                    duration: 5000,
                  },
                },
                404: {
                  snackBar: {
                    text:
                      'Digital-Code existiert nicht oder kann nicht mehr verwendet werden.',
                    duration: 5000,
                  },
                },
                default: {
                  snackBar: {
                    text: 'Unbekannter Fehler, bitte versuchen Sie es erneut',
                    duration: 3000,
                  },
                },
              })
            ),
            tap((data) => {
              if (data) {
                if (!data[this.preset]) {
                  this.expressCodeStatus$.next('invalid');

                  this.snackBar.open(
                    'Der Digital-Code kann hier nicht verwendet werden.',
                    null,
                    {
                      duration: 5000,
                    }
                  );
                  return;
                }

                this.validatedExpressCode.next(data);
                this.expressCodeStatus$.next('valid');
              }
            }),
            this.loadingService.showLoadingWhile()
          )
          .subscribe();

    }
  }

  protected getFormControl() {
    return new FormControl(null, [
      Validators.minLength(ValidationDataService.maxLength.expressCode),
      Validators.maxLength(ValidationDataService.maxLength.expressCode),
    ]);
  }
}
