import {Injectable} from '@angular/core';
import {MatSnackBar} from '@angular/material/snack-bar';
import {
  EmployeeNoteCreateRequest,
  EmployeeNoteType,
  ExpressCode,
  PrintType,
  UserRole,
  Voucher,
  VouchersCreateRequest,
} from '@app/api/dpl';
import {DplApiService} from '@app/core';
import {PrintService} from '@app/shared/services/print.service';
import {ErrorService} from '@app/shared/services/error.service';
import {Observable, of} from 'rxjs';
import {catchError, first, map, switchMap, tap} from 'rxjs/operators';

import {CustomerDivisionsService} from '../../customers/services/customer-divisions.service';
import {delayCreation, LoadingService} from '@dpl/dpl-lib';
import {MatDialog} from '@angular/material/dialog';
import {
  ConfirmActionDialogComponent,
  ConfirmActionDialogData,
  ConfirmActionDialogResult,
  DynamicConfirmationDialogComponent,
  DynamicConfirmationDialogData,
  DynamicConfirmationDialogResult,
} from '@app/shared/components';
import {OnBehalfOfService} from '@app/shared/services';

import {UserService} from '../../user/services/user.service';
import {VouchersStore} from '../../voucher-register/state/vouchers.store';
import {VoucherRegisterService} from '../../voucher-register/services/voucher-register.service';

@Injectable({ providedIn: 'root' })
export class VoucherService {
  constructor(
    private dpl: DplApiService,
    private print: PrintService,
    private division: CustomerDivisionsService,
    private snackBar: MatSnackBar,
    private error: ErrorService,
    private loadingService: LoadingService,
    private dialog: MatDialog,
    private onBehalfOfService: OnBehalfOfService,
    private userService: UserService,
    private store: VouchersStore,
    private voucherRegisterService: VoucherRegisterService
  ) {}

  public getExpressCode(expressCode: string): Observable<ExpressCode> {
    const code = expressCode.toUpperCase();
    return this.dpl.expressCodes.getByCode({expressCode: code, printType: PrintType.VoucherCommon}).pipe(
      catchError((error) =>
        this.error.handleApiError(error, {
          404: {
            snackBar: {
              text: $localize`:Fehlermeldung wenn eingegebener ExpressCode nicht existiert oder nicht mehr gültig ist.@@PalletAcceptanceServiceExpressCodeInvalidMessage:DPL-Digial-Code existiert nicht oder kann nicht mehr verwendet werden.`,
              duration: 3000,
            },
          },
          default: {
            snackBar: {
              text: this.error.getUnknownApiError(),
            },
          },
        })
      ),
      tap((expressCode) => {
        if (!expressCode) {
          return;
        }

        const message = $localize`:Meldung wenn eingegebener ExpressCode erfolgreich angewandt wurde.@@PalletAcceptanceServiceExpressCodeAppliedMessage:DPL-Digial-Code wurde erfogreich angewendet.`;

        this.snackBar.open(message, null, {
          duration: 2000,
        });
      })
    );
  }

  public createVoucher(
    request: Omit<
      VouchersCreateRequest,
      'customerDivisionId' | 'printDateTimeOffset'
    >,
    print: boolean,
    dplNote: EmployeeNoteCreateRequest
  ) {
    return this.division.getActiveDivision().pipe(
      first(),
      switchMap((division) => {
        if (!division) {
          throw 'Active division needs to be set when creating voucher';
        }

        return this.dpl.vouchers
          .post({
            ...request,
            ...{
              customerDivisionId: division.id,
              printDateTimeOffset: new Date().getTimezoneOffset(),
              dplNote,
            },
          })
          .pipe(
            this.loadingService.showLoadingWhile(),
            switchMap((voucher) => {
              return print
                ? this.print
                    .printUrl(
                      voucher.downloadLink,
                      true,
                      this.print.getPrintContextVoucher(voucher),
                      voucher
                    )
                    .pipe(
                      switchMap((result) => {
                        if (result && result.cancelReason) {
                          console.log(
                            'CANCEL',
                            result.cancel,
                            result.cancelReason
                          );
                          return this.dpl.vouchers.patchVoucherCancel(
                            voucher.id,
                            { reason: result.cancelReason }
                          );
                        }
                        return of(voucher);
                      })
                    )
                : of(voucher);
            })
          );
      })
    );
  }

  cancel(voucher: Voucher, source:'grid'|'print') {
    const executeCancel = (
      dplNote: EmployeeNoteCreateRequest = undefined,
      userNote: string = undefined
    ) => {
      return this.dpl.vouchers
        .patchVoucherCancel(voucher.id, {
          dplNote,
          reason: userNote,
        })
        .pipe(
          this.loadingService.showLoadingWhile(),
          tap((data) => {
            this.store.update((entity) => entity.id === data.id, data);
            this.voucherRegisterService.forceRefreshSubject.next(true);
          })
        );
    };
    return this.userService.getCurrentUser().pipe(
      map((user) => {
        return user.role === UserRole.DplEmployee;
      }),
      first(),
      switchMap((dplEmployee) => {
        if (dplEmployee) {
          return this.onBehalfOfService
            .openOnBehalfofDialog(EmployeeNoteType.Cancellation)
            .pipe(
              switchMap((executionResult) => {
                if (!executionResult) {
                  return of(null as Voucher);
                }
                return executeCancel(executionResult);
              })
            );
        }
        return delayCreation(() =>
          this.dialog
            .open<
              ConfirmActionDialogComponent,
              ConfirmActionDialogData,
              ConfirmActionDialogResult
            >(ConfirmActionDialogComponent, {
              data: {
                title: 'Stornieren',
                context: 'cancelVoucher'
              },
              disableClose: true,
              autoFocus: false,
            })
            .afterClosed()
            .pipe(
              switchMap((result) => {
                if (!result || !result.confirmed) {
                  return of(null as Voucher);
                }

                // beleg ausgegeben dialog
                return this.dialog
                  .open<
                    DynamicConfirmationDialogComponent,
                    DynamicConfirmationDialogData,
                    DynamicConfirmationDialogResult
                  >(DynamicConfirmationDialogComponent, {
                    data: {
                      labels: {
                        title: $localize`:Titel für Dialog Druck Stornieren@@PrintCancelDialogCancelTitle:Stornieren`,
                        description: $localize`:Frage für Dialog Druck Stornieren Voucher@@PrintCancelDialogQuestionVoucher:Wurde der Beleg ausgegeben?`,
                        confirm: $localize`:@@Yes:Ja`,
                        reject: $localize`:@@No:Nein`,
                        hideCancel: true,
                      },
                    },
                    disableClose: true,
                    autoFocus: false,
                  })
                  .afterClosed()
                  .pipe(
                    switchMap((extendedResult) => {
                      let extendedCancelReason;

                      if (extendedResult?.confirmed) {
                        extendedCancelReason =
                          'Beleg ausgegeben';
                      } else if (extendedResult?.confirmed === false) {
                        extendedCancelReason =
                          'Beleg nicht ausgegeben';
                      }
                      if(result.cancelVoucherReason==="Sonstiges"){
                        return executeCancel(
                          undefined,
                          result.cancelVoucherReason + ' '+ result.cancelVoucherNote+' | ' + extendedCancelReason
                        );
                      } else{
                        return executeCancel(
                          undefined,
                          result.cancelVoucherReason + ' | ' + extendedCancelReason
                        );
                      }
                      
                    })
                  );
              })
            )
        );
      })
    );
  }
}
