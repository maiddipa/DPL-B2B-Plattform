import {
  Component,
  OnInit,
  EventEmitter,
  Input,
  Output,
  Inject,
  ChangeDetectorRef,
} from '@angular/core';
import { NgxRootFormComponent, Controls, FormGroupOptions } from 'ngx-sub-form';
import {
  FormControl,
  FormGroup,
  ValidationErrors,
  ValidatorFn,
  Validators,
} from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ConfirmationDialogComponent } from '@dpl/dpl-lib';

export interface ConfirmActionDialogData {
  title: string;
  context: 'cancel' | 'other' | 'cancelVoucher';
}

export interface ConfirmActionDialogResult extends ConfirmActionDialogForm {
  confirmed: boolean;
  note: string;
}

interface ConfirmActionDialogForm {
  note: string;
  cancelVoucherReason: string;
  cancelVoucherNote: string;
}

@Component({
  template: `
    <div fxLayout="column" fxLayoutGap="10px">
      <mat-toolbar color="primary">
        <span>{{ dialogData.title }}</span>
      </mat-toolbar>
      <div fxLayout="column" [formGroup]="formGroup">
        <mat-form-field *ngIf="dialogData.context === 'other'">
          <input
            matInput
            [formControl]="formGroup.controls.note"
            placeholder="Notiz"
            i18n-placeholder="Notiz Label für Bestätigung von Aktionen@@Note"
          />

          <!-- ToDo insert select for Stornogrund hier -> new context cancelVoucher -->
          <!-- select -->

          <!-- voucherCancel  -->
        </mat-form-field>
        <mat-form-field *ngIf="dialogData.context === 'cancel'">
          <input
            matInput
            [formControl]="formGroup.controls.note"
            placeholder="Stornogrund"
            i18n-placeholder="Notiz Label für Storno@@Stornogrund"
          />
        </mat-form-field>
        <mat-form-field
          appearance="fill"
          *ngIf="dialogData.context === 'cancelVoucher'"
        >
          <mat-select
            [formControl]="formGroup.controls.cancelVoucherReason"
            placeholder="Grund auswählen"
          >
            <mat-option
              *ngFor="let reason of voucherCancelReasons"
              [value]="reason"
            >
              {{ reason }}
            </mat-option>
          </mat-select>
        </mat-form-field>
        <mat-form-field
          appearance="fill"
          *ngIf="
            dialogData.context === 'cancelVoucher' &&
            formGroup.value.cancelVoucherReason === 'Sonstiges'
          "
        >
          <input
            matInput
            [formControl]="formGroup.controls.cancelVoucherNote"
            placeholder="Stornogrund"
            i18n-placeholder="Notiz Label für Storno@@Stornogrund"
          />
        </mat-form-field>
      </div>
      <div fxLayout="row" fxLayoutAlign="end" fxLayoutGap="10px">
        <button
          mat-raised-button
          (click)="onResponse(false)"
          i18n="Button label für Abbrechen@@ConfirmationDialogCancelButtonLabel"
        >
          Abbrechen
        </button>
        <button
          mat-raised-button
          color="primary"
          [disabled]="formGroup.invalid"
          (click)="onResponse(true)"
          i18n="
            Button label für Bestätigung@@ConfirmationDialogConfirmButtonLabel"
        >
          Bestätigen
        </button>
      </div>
    </div>
  `,
  styles: [``],
})
export class ConfirmActionDialogComponent
  extends NgxRootFormComponent<ConfirmActionDialogForm>
  implements OnInit {
  @Input()
  dataInput: Required<ConfirmActionDialogForm>;

  @Output()
  dataOutput = new EventEmitter<ConfirmActionDialogForm>();

  voucherCancelReasons = [
    'falsche Menge eingetragen',
    'falsche Firma eingetragen',
    'Paletten nachträglich getauscht',
    'nachträgliche Reklamation',
    'Sonstiges',
  ];

  constructor(
    private dialogRef: MatDialogRef<
      ConfirmationDialogComponent,
      ConfirmActionDialogResult
    >,
    @Inject(MAT_DIALOG_DATA) public dialogData: ConfirmActionDialogData,
    cd: ChangeDetectorRef
  ) {
    super(cd);
  }

  protected getFormControls(): Controls<ConfirmActionDialogForm> {
    return {
      note: new FormControl(null),
      cancelVoucherReason: new FormControl(null),
      cancelVoucherNote: new FormControl(null),
    };
  }

  onResponse(confirmed: boolean) {
    const result: ConfirmActionDialogResult = {
      confirmed,
      ...this.formGroup.value,
    };

    this.dialogRef.close(result);
  }

  public getFormGroupControlOptions(): FormGroupOptions<
    ConfirmActionDialogForm
  > {
    return {
      validators: [this.formValidator()],
    };
  }

  formValidator(): ValidatorFn {
    return (group: FormGroup): ValidationErrors => {
      const controlNote = group.value.note as string;
      const controlVoucherReason = group.value.cancelVoucherReason as string;
      const controlVoucherNote = group.value.cancelVoucherNote as string;

      if (
        (this.dialogData.context === 'cancel' ||
          this.dialogData.context === 'other') &&
        !controlNote
      ) {
        return { required: true };
      } else if (this.dialogData.context === 'cancelVoucher') {
        if (!controlVoucherReason) {
          return { required: true };
        } else if (
          controlVoucherReason === 'Sonstiges' &&
          !controlVoucherNote
        ) {
          return { required: true };
        }
      }
      return;
    };
  }
}
