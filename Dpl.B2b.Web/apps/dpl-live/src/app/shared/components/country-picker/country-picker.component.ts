import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';
import { Observable, combineLatest } from 'rxjs';
import { tap, map } from 'rxjs/operators';
import { CountriesService } from 'apps/dpl-live/src/app/master-data/countries/services/countries.service';
import { ICountry } from 'apps/dpl-live/src/app/master-data/countries/state/country.model';
import { CountryPipeFormat } from '@app/shared/pipes/country.pipe';
import * as _ from 'lodash';
import { NgxSingleFieldSubFormComponent } from '@dpl/dpl-lib';
import { subformComponentProviders } from 'ngx-sub-form';

export type CountryPickerComponentMode = 'licensePlate' | 'long';

type ViewData = {
  countries: ICountry[];
  format: CountryPipeFormat;
};

@Component({
  selector: 'app-country-picker',
  template: `
    <mat-form-field
      *ngIf="formGroup"
      [formGroup]="formGroup"
      [fxFlex]="fxFlexValue"
    >
      <mat-select
        placeholder="Land"
        i18n-placeholder="Country|Label Land@@Country"
        [formControl]="formGroup.controls.innerControl"
        (valueChange)="selectionChanged.emit($event)"
      >
        <ng-container *ngIf="viewData$ | async as data">
          <mat-option
            *ngFor="let country of data.countries"
            [value]="country.id"
          >
            {{ country.id | country: data.format }}
          </mat-option>
        </ng-container>
      </mat-select>
      <mat-error
        *ngIf="
          formGroup.controls.innerControl.touched &&
          formGroup.controls.innerControl.errors?.required
        "
        >Pflichtfeld</mat-error
      >
    </mat-form-field>
  `,
  styles: [],
  providers: subformComponentProviders(CountryPickerComponent),
})
export class CountryPickerComponent
  extends NgxSingleFieldSubFormComponent<number>
  implements OnInit {
  @Input() mode: CountryPickerComponentMode = 'long';
  @Input() prefill = true;

  @Input() required = false;

  @Input() width = '200px';

  @Output() selectionChanged = new EventEmitter<number>();

  protected emitInitialValueOnInit = false;

  viewData$: Observable<ViewData>;
  constructor(private countryService: CountriesService) {
    super();
  }

  ngOnInit() {
    if (this.required) {
      this.formGroup.controls.innerControl.setValidators(Validators.required);
      this.formGroup.controls.innerControl.updateValueAndValidity();
    }

    const countries$ = this.countryService.getCountries().pipe(
      map((countries) => {
        const countriesByMode = _(countries)
          .filter((country) => {
            switch (this.mode) {
              case 'licensePlate':
                // TODO implement further filters when in license plate mode
                return !!country.licensePlateCode;
              default:
                return true;
            }
          })
          .sortBy((i) => {
            switch (this.mode) {
              case 'licensePlate':
                return i.licensePlateCode;
              default:
                return i.name;
            }
          })
          .value();

        return countriesByMode;
      }),
      tap((countries) => {
        if (!this.prefill || !countries || countries.length === 0) {
          return;
        }

        // do not pre select a country as this should be a concious choice
        // this.formGroup.controls.innerControl.setValue(countries[0].id);
      })
    );

    this.viewData$ = combineLatest(countries$).pipe(
      map(([countries]) => {
        return {
          countries,
          // TODO expand use cases beyond license plate
          format: this.mode === 'licensePlate' ? 'licensePlate' : null,
        };
      })
    );
  }

  get fxFlexValue(): string {
    switch (this.mode) {
      case 'licensePlate':
        return '50px';
      default:
        return this.width;
    }
  }
}
