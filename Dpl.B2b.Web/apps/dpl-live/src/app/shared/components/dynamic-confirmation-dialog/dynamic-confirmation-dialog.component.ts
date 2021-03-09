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
import { Controls, NgxAutomaticRootFormComponent } from 'ngx-sub-form';
import { Observable } from 'rxjs';
import { filter, map } from 'rxjs/operators';
import { UserService } from '../../../user/services/user.service';

export interface DynamicConfirmationDialogData {
  labels: {
    title: string;
    description?: string;
    confirm?: string;
    reject?: string;
    cancel?: string;
    redirect?: string;
    hideCancel?: boolean;
  };
  print?: true | PrintConfig;
  redirect?: true | RedirectConfig;
  defaultValues?: Partial<DynamicConfirmationDialogResult>;
}

type PrintConfig = {
  show: boolean;
  label?: string;
  disabled?: boolean;
};

type RedirectConfig = {
  show: boolean;
  label?: string;
};

export interface DynamicConfirmationDialogResult {
  confirmed?: boolean;
  print?: boolean;
  redirect?: boolean;
  cancel?: boolean;
  cancelReason?: string;
}

interface ParsedDynamicConfirmationDialogData
  extends DynamicConfirmationDialogData {
  print: PrintConfig;
  redirect: RedirectConfig;
}

type UserTimestamp = {
  name: string;
  timestamp: Date;
};

@Component({
  selector: 'dpl-confirmation-dialog',
  template: `
    <div fxLayout="column" fxLayoutGap="10px">
      <mat-toolbar color="primary">
        <span>{{ dialogData.labels.title }}</span>
      </mat-toolbar>
      <div fxLayout="column" [formGroup]="formGroup">
        <p *ngIf="dialogData.labels?.description">
          {{ dialogData.labels.description }}
        </p>
        <mat-checkbox
          *ngIf="dialogData.print.show"
          [formControl]="formGroup.controls.print"
          i18n="
            Checkbox label für
            Belegdruck@@ConfirmationDialogPrintReceiptCheckkboxLabel"
          >Beleg drucken</mat-checkbox
        >
        <mat-checkbox
          *ngIf="dialogData.redirect.show"
          [formControl]="formGroup.controls.redirect"
        >
          <ng-container
            *ngIf="dialogData.redirect.label; else redirectFallback"
            >{{ dialogData.redirect.label }}</ng-container
          >
          <ng-template #redirectFallback>
            <ng-container
              i18n="
                Checkbox label für redirect zur
                Übersicht@@ConfirmationDialogRedirectToOverviewLabel"
              >Zur Übersicht wechseln</ng-container
            >
          </ng-template>
        </mat-checkbox>
      </div>
      <div fxLayout="column" fxLayoutGap="10px">
        <div fxLayout="row" fxLayoutAlign="start" [formGroup]>
          <div fxLayout="row" fxLayoutAlign="space-between" fxLayoutGap="10px">
            <div fxFlex="20">              
              <button
                mat-raised-button
                (click)="onAnswerTapped(null)"
                *ngIf="!dialogData.labels.hideCancel"
              >
                <ng-container
                  *ngIf="dialogData.labels?.cancel; else cancelFallback"
                  >{{ dialogData.labels.cancel }}</ng-container
                >
                <ng-template #cancelFallback>
                  <span
                    i18n="
                      Button label für
                      Abbrechen@@ConfirmationDialogCancelButtonLabel"
                    >Abbrechen</span
                  >
                </ng-template>
              </button>
            </div>
          </div>
          <div fxLayout="row" fxLayoutAlign="end" fxLayoutGap="10px" fxFlex>
            <button
              *ngIf="dialogData.labels?.reject"
              mat-raised-button
              (click)="onAnswerTapped(false)"
            >
              {{ dialogData.labels.reject }}
            </button>
            <button
              mat-raised-button
              color="primary"
              [disabled]="formGroup.invalid"
              (click)="onAnswerTapped(true)"
            >
              <ng-container
                *ngIf="dialogData.labels?.confirm; else confirmFallback"
                >{{ dialogData.labels.confirm }}</ng-container
              >
              <ng-template #confirmFallback>
                <span
                  i18n="
                    Button label für
                    Bestätigung@@ConfirmationDialogConfirmButtonLabel"
                  >Bestätigen</span
                >
              </ng-template>
            </button>
          </div>
        </div>
        <div fxLayout="row" fxLayoutAlign="end">
          <span *ngIf="userTimestamp$ | async as userTimespamp"
            >({{ userTimespamp.name }} |
            {{ userTimespamp.timestamp | dateEx }})</span
          >
        </div>
      </div>
    </div>
  `,
  styles: [],
})
export class DynamicConfirmationDialogComponent
  extends NgxAutomaticRootFormComponent<DynamicConfirmationDialogResult>
  implements OnInit {
  @Input()
  dataInput: Required<DynamicConfirmationDialogResult>;

  @Output()
  dataOutput = new EventEmitter<DynamicConfirmationDialogResult>();

  userTimestamp$: Observable<UserTimestamp>;

  constructor(
    private dialogRef: MatDialogRef<
      DynamicConfirmationDialogComponent,
      DynamicConfirmationDialogResult
    >,
    private userService: UserService,
    @Inject(MAT_DIALOG_DATA) public _dialogData: DynamicConfirmationDialogData,
    cd: ChangeDetectorRef
  ) {
    super(cd);
  }

  get dialogData(): ParsedDynamicConfirmationDialogData {
    return this._dialogData as ParsedDynamicConfirmationDialogData;
  }

  ngOnInit() {
    super.ngOnInit();
    this.userTimestamp$ = this.userService.getCurrentUser().pipe(
      filter((x) => !!x),
      map((user) => {
        const userTimestamp: UserTimestamp = {
          name: user.name,
          timestamp: new Date(),
        };
        return userTimestamp;
      })
    );

    const printConfig =
      !this._dialogData.print || typeof this._dialogData.print === 'boolean'
        ? ({ show: this._dialogData.print } as PrintConfig)
        : this._dialogData.print;

    if (printConfig.disabled) {
      this.formGroup.controls.print.disable();
    }

    // ensure print always is an object
    this._dialogData.print = printConfig;

    const redirectConfig =
      !this._dialogData.redirect ||
      typeof this._dialogData.redirect === 'boolean'
        ? ({ show: this._dialogData.redirect } as RedirectConfig)
        : this._dialogData.redirect;

    // ensure print always is an object
    this._dialogData.redirect = redirectConfig;

    if (this._dialogData.defaultValues) {
      this.formGroup.patchValue(this._dialogData.defaultValues);
    }

    this.formGroup.controls.print.updateValueAndValidity();
  }

  onAnswerTapped(confirmed: boolean | null) {
    let result: DynamicConfirmationDialogResult;
    if (confirmed === null) {
      result = null;
    } else {
      result = {
        ...this.formGroup.getRawValue(),
        confirmed,
      };
    }
    this.dialogRef.close(result);
  }

  protected getFormControls(): Controls<DynamicConfirmationDialogResult> {
    return {
      confirmed: new FormControl(null),
      print: new FormControl(null),
      redirect: new FormControl(null),
      cancel: new FormControl(null),
      cancelReason: new FormControl(null),
    };
  }
}
