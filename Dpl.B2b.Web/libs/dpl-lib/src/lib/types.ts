import {
  FormGroup,
  FormControl,
  FormArray,
  AbstractControl,
} from '@angular/forms';
import { Observable } from 'rxjs';

// Can be removed after typescript upgrade as this is now part of the latest typescript version
export type Omit<T, K> = Pick<T, Exclude<keyof T, K>>;

export interface ILabelValue<T> {
  label: string;
  value: T;
  disabled?: boolean;
  icon?: string;
}

export interface ILabelValueIcon<T> extends ILabelValue<T> {
  icon: string;
}

type UntypedControlBase<TControl extends AbstractControl | FormArray> = Omit<
  TControl,
  'value' | 'valueChanges' | 'setValue' | 'patchValue'
>;

export type TypedAbstractControl<TValue extends any> = {
  value: TValue;
  valueChanges: Observable<TValue>;
  setValue(
    value: TValue,
    options?: {
      onlySelf?: boolean;
      emitEvent?: boolean;
      emitModelToViewChange?: boolean;
      emitViewToModelChange?: boolean;
    }
  ): void;
  patchValue(
    value: Partial<TValue>,
    options?: {
      onlySelf?: boolean;
      emitEvent?: boolean;
      emitModelToViewChange?: boolean;
      emitViewToModelChange?: boolean;
    }
  ): void;
} & UntypedControlBase<AbstractControl>;

export type TypedFormControl<TValue extends any> = TypedAbstractControl<
  TValue
> &
  UntypedControlBase<FormControl>;

export type TypedFormGroupControl<TValue extends any> = TypedAbstractControl<
  TValue
> &
  UntypedControlBase<FormGroup>;

export type TypedFormArray<
  TValue extends any,
  TControlType = TValue extends {}
    ? TypedFormGroupControl<TValue>
    : TypedFormControl<TValue>
> = TypedAbstractControl<TValue[]> &
  UntypedControlBase<FormArray> & {
    controls: TControlType[];
  };
