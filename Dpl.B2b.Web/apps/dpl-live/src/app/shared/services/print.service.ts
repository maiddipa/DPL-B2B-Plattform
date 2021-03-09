import { Injectable } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import {
  LoadCarrierReceipt,
  LoadCarrierReceiptType,
  PermissionResourceType,
  ResourceAction,
  Voucher,
  VoucherType,
} from '@app/api/dpl';
import * as printJS from 'print-js';
import { EMPTY, Observable, of } from 'rxjs';
import { map, pluck, switchMap } from 'rxjs/operators';
import { PermissionsService } from '../../core/services/permissions.service';
import { CustomerDivisionsService } from '../../customers/services/customer-divisions.service';

import {
  DynamicConfirmationDialogComponent,
  DynamicConfirmationDialogData,
  DynamicConfirmationDialogResult,
} from '../components/dynamic-confirmation-dialog/dynamic-confirmation-dialog.component';

export enum PrintContext {
  VoucherOriginal = 'VoucherOriginal',
  VoucherDigital = 'VoucherDigital',
  VoucherDirect = 'VoucherDirect',
  LoadCarrierReceiptPickup = 'LoadCarrierReceiptPickup',
  LoadCarrierReceiptDelivery = 'LoadCarrierReceiptDelivery',
}

@Injectable({
  providedIn: 'root',
})
export class PrintService {
  constructor(
    private dialog: MatDialog,
    private permissionsService: PermissionsService,
    private divisionsService: CustomerDivisionsService
  ) {}

  printUrl(
    url: string,
    showConfirmation: boolean,
    context?: PrintContext,
    data?: Voucher | LoadCarrierReceipt
  ): Observable<DynamicConfirmationDialogResult> {
    const printPromise = new Promise<void>((resolve, reject) => {
      printJS({
        printable: url,
        type: 'pdf',
        onPrintDialogClose: () => {
          resolve();
        },
      });
    });

    // return from(printPromise).pipe(
    //   switchMap(() => {

    // ignore print promise result (not working in firefox https://github.com/crabbly/Print.js/blob/master/src/js/functions.js#L80)
    const confirmation$ = showConfirmation
      ? this.getConfirmation(context, data).pipe(
          switchMap((result) => {
            if (!result) {
              return EMPTY;
            }
            return of(result);
          })
        )
      : of(null as DynamicConfirmationDialogResult);

    return confirmation$.pipe(
      switchMap((result) => {
        return result && result.confirmed === false
          ? this.printUrl(url, showConfirmation, context, data)
          : of(result);
      })
    );
  }

  private getConfirmation(
    context?: PrintContext,
    data?: Voucher | LoadCarrierReceipt
  ): Observable<DynamicConfirmationDialogResult> {
    //Todo dialog storno
    //todo context for printDialog

    let extendedCancelDialogEnabled = false;
    let dialogCancelPermission: ResourceAction;
    switch (context) {
      case PrintContext.VoucherOriginal:
      case PrintContext.VoucherDigital:
        extendedCancelDialogEnabled = true;
        dialogCancelPermission = ResourceAction.CancelVoucher;
        break;
      case PrintContext.LoadCarrierReceiptPickup:
        extendedCancelDialogEnabled = true;
        dialogCancelPermission = ResourceAction.CancelLoadCarrierReceipt;
        break;

      default:
        break;
    }

    return this.permissionsService
      .hasPermissionOnDivision(dialogCancelPermission)
      .pipe(
        switchMap((hasPermission) => {
          console.log('hasPermission', hasPermission);
          return this.dialog
            .open<
              DynamicConfirmationDialogComponent,
              DynamicConfirmationDialogData,
              DynamicConfirmationDialogResult
            >(DynamicConfirmationDialogComponent, {
              data: {
                labels: {
                  title: $localize`:Titel für Dialog zur Bestätigung ob Durck geklappt hat@@PrintConfirmationDialogTitle:War der Druck erfolgreich?`,
                  description: $localize`:Beschreibung für Dialog zur Bestätigung ob Durck geklappt hat@@PrintConfirmationDialogDescription:ACHTUNG!!! Der Beleg wurde wie erfasst bereits gespeichert. Auch wenn Sie diesen jetzt nicht gedruckt haben bzw. drucken konnten, müssen Sie ihn gegebenenfalls stornieren. Dies gilt auch, wenn der Druck erfolgreich war, Sie den Beleg dennoch nicht herausgegeben haben.`,
                  confirm: $localize`:Label für ja Button@@PrintConfirmationYesButtonLabel:Ja`,
                  reject: $localize`:Label für Nein Button@@PrintConfirmationNoButtonLabel:Nein`,

                  cancel: extendedCancelDialogEnabled
                    ? $localize`:Label für Stornieren Button@@PrintConfirmationStornoButtonLabel:Stornieren`
                    : $localize`:Label für Abbrechen Button@@PrintConfirmationCancelButtonLabel:Vorgang abbrechen`,
                  hideCancel: hasPermission === true ? false : true, // show cancel if user has "cancel" permission
                },
              },
              disableClose: true,
              autoFocus: false,
            })
            .afterClosed();
        })
      )
      .pipe(
        switchMap((result) => {
          if (result) {
            return of(result);
          }
          //extendedCancelDialog?
          if (extendedCancelDialogEnabled) {
            let dialogQuestion = '';
            switch (context) {
              case PrintContext.VoucherOriginal:
              case PrintContext.VoucherDigital:
                dialogQuestion = $localize`:Frage für Dialog Druck Stornieren Voucher@@PrintCancelDialogQuestionVoucher:Wurde der Beleg ausgegeben? Mit einem Klick auf "Nein" bestätigen Sie den Beleg zu stornieren nicht ausgegeben und das Original vernichtet zu haben. Mit einem Klick auf "Ja" bestätigen Sie den Beleg stornieren zu möchten, obwohl dieser von Ihnen ausgegeben wurde. Nur sofern der Beleg bei DPL nicht eingegangen ist, kann eine Stornierung vollzogen werden.`;

                break;
              case PrintContext.LoadCarrierReceiptPickup:
                dialogQuestion = $localize`:Frage für Dialog Druck Stornieren LoadCarrierReceipt@@PrintCancelDialogQuestionLoadCarrierReceipt:Wurde die Ware ausgegeben?`;

                break;
            }

            // Extended Cancel Dialog
            return this.dialog
              .open<
                DynamicConfirmationDialogComponent,
                DynamicConfirmationDialogData,
                DynamicConfirmationDialogResult
              >(DynamicConfirmationDialogComponent, {
                data: {
                  labels: {
                    title: $localize`:Titel für Dialog Druck Stornieren@@PrintCancelDialogCancelTitle:Stornieren des Belegs`,
                    description: dialogQuestion,
                    confirm: $localize`:@@Yes:Ja`,
                    reject: $localize`:@@No:Nein`,
                    cancel: $localize`:@@Cancel:Abbrechen`,
                  },
                },
                disableClose: true,
                autoFocus: false,
              })
              .afterClosed()
              .pipe(
                switchMap((extendedResult) => {
                  // check Storno abgebrochen
                  if (extendedResult?.confirmed === false) {
                    console.log('Storno abgebrochen');
                  }

                  console.log('extendedResult', extendedResult);
                  const cancelResult: DynamicConfirmationDialogResult = {
                    cancel: true,
                  };

                  if (context === PrintContext.LoadCarrierReceiptPickup) {
                    // Ware ausgegeben--> Storno nicht möglich!!!
                    if (extendedResult?.confirmed) {
                      // Melduung Storno nur über DPL
                      return this.dialog
                        .open<
                          DynamicConfirmationDialogComponent,
                          DynamicConfirmationDialogData,
                          DynamicConfirmationDialogResult
                        >(DynamicConfirmationDialogComponent, {
                          data: {
                            labels: {
                              title: $localize`:Titel für Dialog Druck Stornieren @@PrintCancelDialogCancelTitle:Stornieren des Belegs`,
                              description: $localize`:Description für Dialog Druck Stornieren nicht moeglich @@PrintCancelDialogNoCancelPossible:Stornieren ist in diesem Fall nicht möglich, wenden Sie sich an DPL`,
                              confirm: $localize`:@@Yes:Ok`,
                              hideCancel: true,
                            },
                          },
                          disableClose: true,
                          autoFocus: false,
                        })
                        .afterClosed()
                        .pipe(
                          map((extendedConfirmationResult) => {
                            cancelResult.cancel = false;
                            cancelResult.cancelReason =
                              'Druck abgebrochen. Ware ausgegeben'; //Storno Text Voucher/LoadCarrierReceipt
                            return cancelResult;
                          })
                        );
                    } else if (extendedResult?.confirmed === false) {
                      // Sind Sie wirklich sicher Frage
                      return this.dialog
                        .open<
                          DynamicConfirmationDialogComponent,
                          DynamicConfirmationDialogData,
                          DynamicConfirmationDialogResult
                        >(DynamicConfirmationDialogComponent, {
                          data: {
                            labels: {
                              title: $localize`:Titel für Dialog Druck Stornieren@@PrintCancelDialogCancelTitle:Stornieren des Belegs`,
                              description: $localize`:Description für Dialog Druck Stornieren Bestaetigung @@PrintCancelDialogCancelConfirmation:Wurde die Ware wirklich nicht ausgegeben? Sind Sie da ganz sicher?`,
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
                          map((extendedConfirmationResult) => {
                            if (extendedConfirmationResult?.confirmed) {
                              cancelResult.cancel = true;
                              cancelResult.cancelReason =
                                'Druck abgebrochen. Ware nicht ausgegeben. Bestätigung erteilt';
                            } else if (
                              extendedConfirmationResult?.confirmed === false
                            ) {
                              cancelResult.cancel = false;
                              cancelResult.cancelReason =
                                'Druck abgebrochen. Ware nicht ausgegeben. Bestätigung nicht erteilt';
                            }
                            return cancelResult;
                          })
                        );
                    } else {
                      //Storno abgebrochen, Druck erfolgreich
                      const abortResult: DynamicConfirmationDialogResult = {
                        confirmed: true,
                      };
                      return of(abortResult);
                    }
                  } else {
                    // voucher case
                    if (extendedResult?.confirmed) {
                      cancelResult.cancelReason =
                        'Druck abgebrochen. Beleg ausgegeben';
                    } else if (extendedResult?.confirmed === false) {
                      cancelResult.cancelReason =
                        'Druck abgebrochen. Beleg nicht ausgegeben';
                    } else {
                      //Storno abgebrochen, Druck erfolgreich
                      const abortResult: DynamicConfirmationDialogResult = {
                        confirmed: true,
                      };
                      return of(abortResult);
                    }
                    return of(cancelResult);
                  }
                })
              );

            //confimrbtn
            //cancelbtn
          } else {
            // Cancel Info Dialog
            return this.dialog
              .open<
                DynamicConfirmationDialogComponent,
                DynamicConfirmationDialogData,
                DynamicConfirmationDialogResult
              >(DynamicConfirmationDialogComponent, {
                data: {
                  labels: {
                    title: $localize`:Titel für Dialog Druck abgebrochen@@PrintCancelDialogTitle:Druck abgebrochen`,
                    description: $localize`:Text für Dialog Druck abgebrochen@@PrintCancelDialogDescription:Hiermit wurde der Druckvorgang abgebrochen. Der Beleg wurde bereits erstellt. Falls Sie eine Stornierung des Belegs wünschen, nehmen Sie bitte über den Chat Kontakt mit unserem Service auf.`,
                    confirm: 'Ok',
                    hideCancel: true,
                    // TODO reenable cancel button
                    // cancel: $localize`:Label für Abbrechen Button@@PrintConfirmationCancelButtonLabel:Vorgang abbrechen`
                  },
                },
                disableClose: true,
                autoFocus: false,
              })
              .afterClosed()
              .pipe(map(() => result));
          }
        })
      );
  }

  getPrintContextVoucher(voucher: Voucher) {
    switch (voucher.type) {
      case VoucherType.Original:
        return PrintContext.VoucherOriginal;
      case VoucherType.Digital:
        return PrintContext.VoucherDigital;
      case VoucherType.Direct:
        return PrintContext.VoucherDirect;
    }
  }

  getPrintContextLoadCarrierReceipt(loadCarrierReceipt: LoadCarrierReceipt) {
    switch (loadCarrierReceipt.type) {
      case LoadCarrierReceiptType.Delivery:
        return PrintContext.LoadCarrierReceiptDelivery;
      case LoadCarrierReceiptType.Pickup:
        return PrintContext.LoadCarrierReceiptPickup;
    }
  }
}
