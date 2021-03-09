import {
  Component,
  EventEmitter,
  InjectionToken,
  Input,
  OnInit,
  Output,
} from '@angular/core';
import { FormControl } from '@angular/forms';
import { subformComponentProviders } from 'ngx-sub-form';
import { EMPTY, Observable, of, Subject } from 'rxjs';
import {
  debounceTime,
  distinctUntilChanged,
  filter,
  startWith,
  switchMap,
  tap,
} from 'rxjs/operators';

import { TypedFormControl } from '../types';
import { NgxSingleFieldSubFormComponent } from './ngx-single-field-sub-form.component';
import { MatFormFieldAppearance } from '@angular/material/form-field';
import { hostFlexColumn } from '../utils';

export interface GenericLookupGlobalConfig {
  debounceTime: number;
  minLength: number;
}

export const GENERIC_LOOKUP_GLOBAL_CONFIG = new InjectionToken<
  GenericLookupGlobalConfig
>('GENERIC_LOOKUP_CONFIG');

export interface GenericLookupConfig<TOption> {
  getOptions: (filter: string) => Observable<TOption[]>;
  placeholder?: string;
  icon?: string;
  appearance?: MatFormFieldAppearance;
  /**
   * Specify the width of the autocomplete panel.  Can be any CSS sizing value, otherwise it will
   * match the width of its host.
   */
  panelWidth?: string | number;
  optionKey?: string;
  getLabel?: (item?: TOption) => string;
  getOptionLabel?: (item?: TOption) => string;
  debounceTime?: number;
  minLength?: number;
}

@Component({
  selector: 'generic-lookup',
  template: `
    <mat-form-field
      *ngIf="initialized && formGroup"
      [formGroup]="formGroup"
      [appearance]="mergedConfig.appearance"
    >
      <input
        type="text"
        [placeholder]="mergedConfig.placeholder"
        matInput
        [formControl]="this.formGroup.controls.innerControl"
        [matAutocomplete]="auto"
      />
      <mat-icon *ngIf="mergedConfig.icon" matSuffix>{{
        mergedConfig.icon
      }}</mat-icon>
      <mat-autocomplete
        #auto="matAutocomplete"
        [displayWith]="getLabel.bind(this)"
        [panelWidth]="mergedConfig.panelWidth"
        (optionSelected)="onOptionSelected($event.option.value)"
      >
        <mat-option *ngFor="let option of options$ | async" [value]="option">
          {{ getOptionLabel(option) }}
        </mat-option>
      </mat-autocomplete>
    </mat-form-field>
  `,
  styles: [hostFlexColumn],
  providers: subformComponentProviders(GenericLookupComponent),
})
export class GenericLookupComponent<TOption = any, TValue = any>
  extends NgxSingleFieldSubFormComponent<TValue>
  implements OnInit {
  @Input() config: GenericLookupConfig<TOption>;
  @Output() optionSelected: EventEmitter<TValue> = new EventEmitter();
  @Output() selectionChanged: EventEmitter<TValue> = new EventEmitter();

  selection$: Subject<TValue> = new Subject();
  options$: Observable<TOption[]>;

  initialized: boolean;
  constructor() {
    super();
  }

  mergedConfig: GenericLookupConfig<TOption>;

  private defaultConfig: Partial<GenericLookupConfig<TOption>> = {
    placeholder: '',
    icon: null,
    appearance: 'legacy',
    optionKey: 'id',
    panelWidth: 'auto',
    getLabel: (item) => (item as unknown) as string,
    getOptionLabel: (item?: TOption) => this.mergedConfig.getLabel(item),
  };

  ngOnInit() {
    if (!this.config) {
      throw new Error('No config was provided for generic lookup component.');
    }

    this.mergedConfig = {
      ...this.defaultConfig,
      ...this.config,
    };

    const lookupControl = (this.formGroup.controls
      .innerControl as unknown) as TypedFormControl<string | TOption>;

    const selectionSync$ = this.selection$.pipe(
      distinctUntilChanged(
        (prev, current) =>
          // compare by unique key (default = id)
          (prev || ({} as any))[this.mergedConfig.optionKey] !==
          (current || ({} as any))[this.mergedConfig.optionKey]
      ),
      tap((value) => this.selectionChanged.emit(value)),
      switchMap(() => EMPTY),
      startWith(null)
    );

    this.options$ = selectionSync$.pipe(
      switchMap(() => lookupControl.valueChanges),
      startWith(''),
      switchMap((value) => {
        if (typeof value !== 'string') {
          return EMPTY;
        }

        return of(value as string).pipe(
          tap(() => this.selection$.next(null)),
          filter(
            (searchTerm) =>
              searchTerm && searchTerm.length >= this.mergedConfig.minLength
          ),
          distinctUntilChanged(),
          debounceTime(this.mergedConfig.debounceTime),
          switchMap((searchTerm) => this.getOptions(searchTerm))
        );
      })
    );

    this.initialized = true;
  }

  onOptionSelected(value: TValue) {
    this.optionSelected.emit(value);
    this.selection$.next(value);
  }

  getOptions(searchTerm: string) {
    return this.mergedConfig.getOptions(searchTerm);
  }

  getLabel(item?: TOption) {
    return this.mergedConfig.getLabel(item);
  }

  getOptionLabel(item?: TOption) {
    return this.mergedConfig.getOptionLabel(item);
  }

  protected getFormControl() {
    return new FormControl(null);
  }
}
