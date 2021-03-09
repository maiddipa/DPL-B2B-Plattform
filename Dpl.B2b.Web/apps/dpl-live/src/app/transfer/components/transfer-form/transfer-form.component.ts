import {
  ChangeDetectorRef,
  Component,
  ElementRef,
  EventEmitter,
  Input,
  OnDestroy,
  OnInit,
  Output,
  Renderer2,
  ViewChild,
} from '@angular/core';
import { FormControl, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import {
  BalanceTransferCreateRequest,
  EmployeeNoteType,
  ExpressCode,
} from '@app/api/dpl';
import {
  DynamicConfirmationDialogComponent,
  DynamicConfirmationDialogData,
  DynamicConfirmationDialogResult,
  ExpressCodePreset,
  LoadCarrierPickerContext,
  OnBehalfOfService,
} from '@app/shared';
import { LoadingService, WrappedControlForm } from '@dpl/dpl-lib';
import {
  Controls,
  NgxAutomaticRootFormComponent,
  SubFormGroup,
} from 'ngx-sub-form';
import { combineLatest, EMPTY, of } from 'rxjs';
import { first, map, switchMap } from 'rxjs/operators';

import { LoadCarriersService } from '../../../master-data/load-carriers/services/load-carriers.service';
import { UserService } from '../../../user/services/user.service';
import { TransferService } from '../../services/transfer.service';
import { TransferForm } from '../../services/transfer.service.types';

export interface ITransferFormState {
  transferForm: TransferForm;
}

@Component({
  selector: 'dpl-transfer-form',
  templateUrl: './transfer-form.component.html',
  styleUrls: ['./transfer-form.component.scss'],
})
export class TransferFormComponent
  extends NgxAutomaticRootFormComponent<
    BalanceTransferCreateRequest,
    TransferForm
  >
  implements OnInit, OnDestroy {
  @Input('transfer') dataInput: Required<BalanceTransferCreateRequest>;
  @Output('valueChange') dataOutput = new EventEmitter<TransferForm>();
  @ViewChild('destinationAccountPicker', { static: true })
  destinationAccountPicker: ElementRef;

  @ViewChild('targetAccountPicker')
  targetAccountPicker: ElementRef;
  expressCodePreset: ExpressCodePreset = 'destinationAccountPreset';
  loadCarrierContext: LoadCarrierPickerContext = 'transfer';

  constructor(
    private transferService: TransferService,
    private loadCarriersService: LoadCarriersService,
    private user: UserService,
    private router: Router,
    private dialog: MatDialog,
    private renderer: Renderer2,
    private onBehalfOfService: OnBehalfOfService,
    private loadingService: LoadingService,
    private cd: ChangeDetectorRef
  ) {
    super(cd);
  }

  ngOnInit() {
    super.ngOnInit();

    // this.formsManager.upsert(
    //   'transferForm',
    //   (this.formGroup as unknown) as FormGroup,
    //   {
    //     persistForm: true,
    //   }
    // );
  }

  ngOnDestroy() {
    // this.formsManager.unsubscribe();
    super.ngOnDestroy();
  }

  protected getFormControls(): Controls<TransferForm> {
    return {
      code: new SubFormGroup<string, WrappedControlForm<number>>(null),
      sourceAccountId: new SubFormGroup<number, WrappedControlForm<number>>(
        null,
        Validators.required
      ),
      targetAccountId: new SubFormGroup<number, WrappedControlForm<number>>(
        null,
        Validators.required
      ),
      loadCarrier: new SubFormGroup<number, WrappedControlForm<number>>(
        null,
        Validators.required
      ),
      message: new FormControl(''),
    };
  }

  protected getDefaultValues(): Partial<TransferForm> {
    return {
      code: null,
      loadCarrier: null,
      message: null,
      sourceAccountId: null,
      targetAccountId: null,
    };
  }

  onSubmit() {
    // check loadcarrier balance
    if (this.formGroup.valid) {
      console.log('valid');

      const value = this.dataValue;

      // TODO add on behalf of dialog here
      this.onBehalfOfService
        .openOnBehalfofDialog(EmployeeNoteType.Create)
        .pipe(
          switchMap((dplNote) => {
            return this.getConfirmation().pipe(
              switchMap((result) => {
                if (!result || !result.confirmed) {
                  return EMPTY;
                }

                return combineLatest([
                  this.transferService.transfer({ ...value, dplNote }),
                  this.loadCarriersService.getLoadCarrierById(
                    value.loadCarrierId
                  ),
                  of(result),
                ]).pipe(first(), this.loadingService.showLoadingWhile());
              }),
              switchMap(([transferResult, loadCarrier, result]) => {
                console.log(transferResult);
                if (!result.redirect) {
                  this.reset();
                  return EMPTY;
                }

                // hack? use accounts overview with route params?
                this.loadCarriersService.setActiveLoadCarrierType(
                  loadCarrier.type
                );
                return this.user
                  .updateAccountBalance(transferResult.postingAccountId)
                  .pipe(map((balances) => ({ balances, result })));
              }),
              switchMap(() => {
                return this.router.navigate(['accounts']);
              })
            );
          })
        )
        .subscribe();
    }
    // check valid
    // map form to model, emit valueChange
    // transform FormGroup
  }

  private getConfirmation() {
    return this.dialog
      .open<
        DynamicConfirmationDialogComponent,
        DynamicConfirmationDialogData,
        DynamicConfirmationDialogResult
      >(DynamicConfirmationDialogComponent, {
        data: {
          labels: {
            title: $localize`:Titel für Dialog zur Bestätigung von Umbuchungen@@TransferConfirmationDialogTitle:Umbuchung bestätigen`,
            description: $localize`:Beschreibung für Dialog zur Bestätigung von Umbuchungen@@TransferConfirmationDialogDescription:Die Erstellung einer Umbuchung löst eine Buchung aus`,
          },
          redirect: true,
          defaultValues: {
            redirect: false,
          },
        },
      })
      .afterClosed();
  }

  reset() {
    this.formGroup.reset();
  }

  expressCodeChange(expressCode: ExpressCode) {
    if (expressCode && expressCode.destinationAccountPreset) {
      this.formGroup.controls.loadCarrier.setValue({
        id: expressCode.destinationAccountPreset.loadCarrierId,
        quantity: 497,
        // quantity: expressCode.destinationAccountPreset.loadCarrierQuantity
      });

      if (expressCode.destinationAccountPreset.postingAccountId) {
        this.formGroup.controls.targetAccountId.setValue(
          expressCode.destinationAccountPreset.postingAccountId
        );
        if (this.destinationAccountPicker.nativeElement) {
          // ToDo nativeElement undefined?
          this.highlightFormControl(
            this.destinationAccountPicker.nativeElement
          );
        }
      }
    } else {
      console.log('express code null');
    }
  }

  private highlightFormControl(nativeElement) {
    // trigger animation by adding class
    this.renderer.addClass(nativeElement, 'form-control-highlight-change');

    // make sure to remove the class so animation can be triggered again
    setTimeout(() => {
      this.renderer.removeClass(nativeElement, 'form-control-highlight-change');
    }, 5000);
  }

  protected transformToFormGroup(
    obj: BalanceTransferCreateRequest | null
  ): TransferForm | null {
    // hack
    return {
      code: null,
      loadCarrier: null,
      message: null,
      sourceAccountId: null,
      targetAccountId: null,
    };
  }

  protected transformFromFormGroup(
    formValue: TransferForm
  ): BalanceTransferCreateRequest | null {
    return {
      digitalCode: formValue.code,
      sourceAccountId: formValue.sourceAccountId,
      loadCarrierId: formValue.loadCarrier?.id,
      quantity: formValue.loadCarrier?.quantity,
      destinationAccountId: formValue.targetAccountId,
      note: formValue.message,
    };
  }
}
