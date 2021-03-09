import {
  Component,
  OnInit,
  Input,
  Output,
  EventEmitter,
  OnDestroy,
  OnChanges,
  SimpleChanges,
  ChangeDetectorRef,
} from '@angular/core';
import {
  FilterType,
  FilterPosition,
  Filter,
  FilterContext,
} from '../../services/filter.service.types';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { FilterService } from '../../services/filter.service';
import * as moment from 'moment';
import { combineLatest, Observable } from 'rxjs';
import { takeUntilDestroyed } from 'ngx-sub-form';

@Component({
  selector: 'voucher-filter-input',
  templateUrl: './voucher-filter-input.component.html',
  styleUrls: ['./voucher-filter-input.component.scss'],
})
export class VoucherFilterInputComponent implements OnInit, OnDestroy {
  @Input() filterItem: Filter;
  @Input() filterPosition: FilterPosition;
  @Input() context: FilterContext;
  form: FormGroup;
  // workaround to use enum in angular expression
  checkType = FilterType;
  position = FilterPosition;

  value: string;
  valueFrom: string;
  valueTo: string;

  filterIsChanged$: Observable<boolean>;

  constructor(
    private filterService: FilterService,
    private formBuilder: FormBuilder,
    private cdref: ChangeDetectorRef
  ) {}

  ngOnInit() {
    this.form = this.formBuilder.group({});

    this.filterIsChanged$ = this.filterService
      .isFilterChanged(this.filterItem.propertyName)
      .pipe
      // tap((value) => {
      //   console.log('Change', value);
      //   this.cdref.detectChanges();
      // })
      ();

    if (this.filterItem.type === FilterType.Date) {
      const valueFrom =
        this.filterItem.value && this.filterItem.value.length > 0
          ? moment(this.filterItem.value[0])
          : null;
      const valueTo =
        this.filterItem.value && this.filterItem.value.length > 1
          ? moment(this.filterItem.value[1])
          : this.filterItem.value && this.filterItem.value.length > 0
          ? valueFrom
          : null;
      console.log('DEFAULT VALUE', valueFrom);
      this.form.addControl(
        'filterValueFrom',
        this.formBuilder.control(valueFrom)
      );
      this.form.addControl('filterValueTo', this.formBuilder.control(valueTo));
    } else if (
      !this.filterItem.isChoice &&
      this.filterItem.type === FilterType.Number
    ) {
      const valueFrom =
        this.filterItem.value && this.filterItem.value.length > 0
          ? this.filterItem.value[0]
          : null;
      const valueTo =
        this.filterItem.value && this.filterItem.value.length > 1
          ? this.filterItem.value[1]
          : this.filterItem.value && this.filterItem.value.length > 0
          ? valueFrom
          : null;
      this.form.addControl(
        'filterValueFrom',
        this.formBuilder.control(valueFrom)
      );
      this.form.addControl('filterValueTo', this.formBuilder.control(valueTo));
    } else if (this.filterItem.isChoice && this.filterItem.isMulti) {
      this.form.addControl(
        'filterValue',
        this.formBuilder.control(this.filterItem.value)
      );
    } else {
      // else "text" and (single choice = isChoice + !isMulti) handling
      this.form.addControl(
        'filterValue',
        this.formBuilder.control(
          this.filterItem.value && this.filterItem.value.length > 0
            ? this.filterItem.value[0]
            : null
        )
      );
    }

    this.form.markAsUntouched();
    combineLatest([
      this.form.valueChanges.pipe(takeUntilDestroyed(this)),
      this.filterService.getAppliedFilters(this.context),
    ]).subscribe(([value, appliedFilters]) => {
      if (this.filterItem.isChoice && this.filterItem.isMulti) {
        this.filterService.voucherFilterChange(
          this.filterItem.propertyName,
          value.filterValue
        );
      } else if (this.filterItem.type === FilterType.Date) {
        // check against applied filter values
        // mat-range triggers value changes twice
        const appliedFilterItem = appliedFilters.find(
          (x) => x.propertyName === this.filterItem.propertyName
        );
        if (value.filterValueFrom || value.filterValueTo) {
          if (
            appliedFilterItem &&
            appliedFilterItem.value &&
            appliedFilterItem.value.length > 1 &&
            value.filterValueFrom &&
            moment(appliedFilterItem.value[0]).isSame(value.filterValueFrom) &&
            value.filterValueTo &&
            moment(appliedFilterItem.value[1]).isSame(value.filterValueTo)
          ) {
            // set filter unchanged - needed?
          } else {
            if (value.filterValueFrom && value.filterValueTo) {
              this.filterService.voucherFilterChange(
                this.filterItem.propertyName,
                [value.filterValueFrom, value.filterValueTo]
              );
            }
          }
        } else {
          //remove validators
          if (
            appliedFilterItem &&
            ((appliedFilterItem.value &&
              appliedFilterItem.value.length === 0) ||
              !appliedFilterItem.value)
          ) {
            // set filter unchanged - needed?
          } else {
            this.filterService.voucherFilterChange(
              this.filterItem.propertyName,
              []
            );
          }
        }
      } else if (this.filterItem.type === FilterType.Number) {
        if (value.filterValueFrom || value.filterValueTo) {
          this.filterService.voucherFilterChange(this.filterItem.propertyName, [
            value.filterValueFrom,
            value.filterValueTo,
          ]);
        } else {
          this.filterService.voucherFilterChange(
            this.filterItem.propertyName,
            []
          );
        }
      } else {
        if (value.filterValue) {
          this.filterService.voucherFilterChange(this.filterItem.propertyName, [
            value.filterValue,
          ]);
        } else {
          this.filterService.voucherFilterChange(
            this.filterItem.propertyName,
            []
          );
        }
      }
    });
  }

  ngOnDestroy(): void {}

  clearValue() {
    Object.keys(this.form.controls).forEach((key) =>
      this.form.controls[key].patchValue(null)
    );
  }
}
