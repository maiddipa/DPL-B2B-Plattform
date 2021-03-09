import { NgModule } from '@angular/core';

import { AngularMaterialModule } from './angular-material.module';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { FlexLayoutModule } from '@angular/flex-layout';
import { CommonModule } from '@angular/common';
import { AngularCdkModule } from './angular-cdk.module';
import {
  ConfirmationDialogComponent,
  GenericArrayFormComponent,
  GenericLookupComponent,
  RadioGroupFormField,
} from './components';
import {
  HighlightFormControlWriteValueDirective,
  LimitInputDirective,
  PageDirective,
  LoadingDirective,
} from './directives';
import {
  MAT_MOMENT_DATE_ADAPTER_OPTIONS,
  MomentDateAdapter,
} from '@angular/material-moment-adapter';
import {
  DateAdapter,
  MAT_DATE_FORMATS,
  MAT_DATE_LOCALE,
} from '@angular/material/core';
import { HttpClientModule } from '@angular/common/http';
import { HighlightPipe } from './pipes/highlight.pipe';
import { LoadingService } from './services';
import { MAT_AUTOCOMPLETE_DEFAULT_OPTIONS } from '@angular/material/autocomplete';
import { DevExtremeModule } from './dev-extreme.module';

const directives = [
  HighlightFormControlWriteValueDirective,
  LimitInputDirective,
  LoadingDirective,
  PageDirective,
];

export const DPL_DATE_FORMATS = {
  parse: {
    dateInput: [
      'DD.MM.YY',
      'D.MM.YY',
      'DD.M.YY',
      'D.M.YY',
      'DD.MM.YYYY',
      'D.MM.YYYY',
      'DD.M.YYYY',
      'D.M.YYYY',
    ],
  },
  display: {
    dateInput: 'DD.MM.YY',
    monthYearLabel: 'YYYY',
    dateA11yLabel: 'LL',
    monthYearA11yLabel: 'YYYY',
  },
};

@NgModule({
  declarations: [
    GenericLookupComponent,
    GenericArrayFormComponent,
    ConfirmationDialogComponent,
    RadioGroupFormField,
    ...directives,
    HighlightPipe,
  ],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    FlexLayoutModule,
    AngularMaterialModule,
    AngularCdkModule,
    HttpClientModule,
    DevExtremeModule,
  ],
  exports: [
    AngularMaterialModule,
    AngularCdkModule,
    FlexLayoutModule,

    HttpClientModule,

    // components
    GenericLookupComponent,
    GenericArrayFormComponent,
    RadioGroupFormField,
    DevExtremeModule,

    ...directives,

    HighlightPipe,
  ],
  entryComponents: [ConfirmationDialogComponent],
  providers: [
    { provide: MAT_MOMENT_DATE_ADAPTER_OPTIONS, useValue: { strict: true } },
    { provide: MAT_DATE_LOCALE, useValue: 'de-DE' },
    {
      provide: MAT_AUTOCOMPLETE_DEFAULT_OPTIONS,
      useValue: { autoActiveFirstOption: true },
    },
  ],
})
export class DplLibModule {}
