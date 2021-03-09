import { Component, OnInit, Input, OnDestroy } from '@angular/core';
import {
  subformComponentProviders,
  NgxSubFormComponent,
  Controls,
  SubFormGroup,
  takeUntilDestroyed,
} from 'ngx-sub-form';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { CustomerDivisionsService } from 'apps/dpl-live/src/app/customers/services/customer-divisions.service';
import { filter, tap, map } from 'rxjs/operators';
import { Observable } from 'rxjs';
import { updateDefaultValues, WrappedControlForm } from '@dpl/dpl-lib';

export interface PrintSettings {
  languageId: number;
  count: number;
}

@Component({
  selector: 'app-print-settings-form',
  template: `
    <ng-container *ngIf="viewData$ | async as data">
      <div
        *ngIf="formGroup"
        fxLayout="row"
        fxLayoutAlign=" center"
        fxLayoutGap="10px"
        [formGroup]="formGroup"
      >
        <app-print-language-picker
          fxFlex="0 0 150px"
          [subForm]="formGroup.controls.languageId"
        ></app-print-language-picker>
        <mat-form-field fxFlex="0 0 75px">
          <input
            type="number"
            digit-only
            matInput
            [formControl]="formGroup.controls.count"
            placeholder="Belege"
            i18n-placeholder="Printouts|Label Ausdrucke@@Printouts"
          />
          <mat-error
            *ngIf="
              formGroup.controls.count.touched &&
              formGroup.controls.count.errors?.required
            "
            >Pflichtfeld</mat-error
          >
        </mat-form-field>
      </div>
    </ng-container>
  `,
  styles: [],
})
export class PrintSettingsFormComponent
  extends NgxSubFormComponent<PrintSettings>
  implements OnInit, OnDestroy {
  @Input() documentTypeId: number;

  defaultValue: PrintSettings;
  viewData$: Observable<{}>;
  constructor(private divisionService: CustomerDivisionsService) {
    super();
    //updateDefaultValues(this);
  }

  ngOnInit() {
    this.formGroup.valueChanges
      .pipe(
        takeUntilDestroyed(this),
        tap((value: PrintSettings) => {
          if (!value || !value.languageId || !value.count) {
            return;
          }

          this.defaultValue = value;
        })
      )
      .subscribe();

    const documentSettings$ = this.divisionService.getActiveDivision().pipe(
      filter((division) => !!division),
      map((division) =>
        division.documentSettings.find(
          (i) => i.documentTypeId === this.documentTypeId
        )
      ),
      tap((settings) => {
        if (!settings) {
          return;
        }
        const validators = [Validators.required];
        if (settings.printCountMin !== undefined) {
          validators.push(Validators.min(settings.printCountMin));
        }
        if (settings.printCountMax !== undefined) {
          validators.push(Validators.max(settings.printCountMax));
        }

        this.formGroup.controls.count.setValidators(validators);
        this.formGroup.controls.count.patchValue(settings.defaultPrintCount);

        // TODO check if we need to trigger updating validity (we shouldn't have to as this only happens once)
        //printCount.updateValueAndValidity();
      })
    );

    this.viewData$ = documentSettings$;
  }

  ngOnDestroy(): void {}

  protected getFormControls(): Controls<PrintSettings> {
    return {
      languageId: new SubFormGroup<number, WrappedControlForm<number>>(null),
      count: new FormControl(null, Validators.required),
    };
  }

  protected getDefaultValues() {
    return this.defaultValue;
  }
}
