import { Component, Input, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms';
import { TypedFormControl } from '@dpl/dpl-lib';
import { NgxSubFormRemapComponent, Controls } from 'ngx-sub-form';

import { DplApiService } from '../../../core/services/dpl-api.service';
import { filter, switchMap, tap, map, startWith } from 'rxjs/operators';
import { Observable, combineLatest, EMPTY } from 'rxjs';
import { Order, PostingAccount, ExpressCode } from '@app/api/dpl';

export type DigitalCodeLookupType = 'order' | 'expressCode';

export interface DigitalCodeLookup {
  digitalCode: string;
  type: DigitalCodeLookupType;
  order?: Order;
  expressCode?: ExpressCode;
}

interface DigitalCodeLookupForm {
  digitalCode: string;
  data: Order | ExpressCode;
}

const digitalCodeLength = 6;

type ViewData = {};

// TODO i18n
@Component({
  selector: 'app-digital-code-lookup',
  template: `
    <ng-container *ngIf="viewData$ | async as data">
      <mat-form-field fxFlex>
        <mat-label i18n="DplDigitalCode|Label DPL-Digital-Code@@DplDigitalCode"
          >DPL-Digital-Code</mat-label
        >
        <input
          matInput
          placeholder="DPL-Digital-Code"
          i18n-placeholder="
            DplDigitalCode|Label DPL-Digital-Code@@DplDigitalCode"
          [formControl]="formGroup.controls.digitalCode"
        />
        <mat-icon matSuffix>flash_auto</mat-icon>
        <!-- <mat-error>
          
        </mat-error> -->
      </mat-form-field>
    </ng-container>
  `,
  styles: [],
})
export class DigitalCodeLookupComponent
  extends NgxSubFormRemapComponent<DigitalCodeLookup, DigitalCodeLookupForm>
  implements OnInit {
  @Input() type: DigitalCodeLookupType;

  viewData$: Observable<ViewData>;
  constructor(private dpl: DplApiService) {
    super();
  }

  ngOnInit() {
    const digitalCode$ = this.formGroup.controls.digitalCode.valueChanges.pipe(
      filter((value: DigitalCodeLookupForm['digitalCode']) => {
        switch (this.type) {
          case 'order':
            return !!value && value.length > 0;
          case 'expressCode':
          default:
            return !!value && value.length === digitalCodeLength;
        }
      })
    );

    const data$ = digitalCode$.pipe(
      switchMap((code) => {
        switch (this.type) {
          case 'order':
            return this.dpl.orders.search({});
          case 'expressCode':
          default:
            return this.dpl.expressCodes.get({
              expressCode: code,
            });
        }
      }),
      tap((data) => {
        // HACK Migration
        this.formGroup.controls.data.setValue(data as any);
      })
    );

    const formSync$ = combineLatest(data$).pipe(
      switchMap(() => EMPTY), // we do not wanne trigger new view data
      startWith(null) // needed so viewdata outputs values
    );

    this.viewData$ = combineLatest(formSync$).pipe(
      map(() => {
        return {};
      })
    );
  }

  protected transformToFormGroup(
    obj: DigitalCodeLookup
  ): DigitalCodeLookupForm {
    const getData = (obj: DigitalCodeLookup) => {
      switch (obj.type) {
        case 'order':
          return obj.order;
        case 'expressCode':
        default:
          return obj.expressCode;
      }
    };
    return {
      digitalCode: obj.digitalCode,
      data: getData(obj),
    };
  }

  protected transformFromFormGroup(
    formValue: DigitalCodeLookupForm
  ): DigitalCodeLookup {
    return {
      digitalCode: formValue.digitalCode,
      type: this.type,
      order: this.type === 'order' ? formValue.data : null,
      expressCode: this.type === 'expressCode' ? formValue.data : null,
    };
  }

  protected getFormControls(): Controls<DigitalCodeLookupForm> {
    return {
      digitalCode: new FormControl(null),
      data: new FormControl(null),
    };
  }

  getDefaultValue() {
    const defaultValue: DigitalCodeLookup = null;
    return defaultValue;
  }
}
