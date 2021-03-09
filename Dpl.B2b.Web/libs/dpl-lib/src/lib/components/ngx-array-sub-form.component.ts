import {
  FormArray,
  FormControl,
  Validators,
  AbstractControl,
  FormGroup,
} from '@angular/forms';
import {
  ArrayPropertyKey,
  ArrayPropertyValue,
  NgxFormWithArrayControls,
  SubFormArray,
  SubFormGroup,
} from 'ngx-sub-form';

import {
  NgxSingleFieldSubFormComponent,
  WrappedControlForm,
} from './ngx-single-field-sub-form.component';
import { Directive, Input, OnChanges, SimpleChanges } from '@angular/core';

export interface ArrayOptions {
  initial: number;
  min: number;
  max?: number;
  showAddAndRemove?: boolean;
}

const defaultOptions: ArrayOptions = {
  initial: 1,
  min: 1,
};

@Directive()
export abstract class NgxArraySubFormComponent<FormInterface>
  extends NgxSingleFieldSubFormComponent<FormInterface[]>
  implements
    NgxFormWithArrayControls<WrappedControlForm<FormInterface[]>>,
    OnChanges {
  abstract getDefaultValue(): FormInterface;
  
  options: ArrayOptions;
  canAddItem: boolean;
  canRemoveItem: boolean;

  ngOnChanges(changes: SimpleChanges) {
    this.options = { ...defaultOptions, ...this.getOptions() };

    super.ngOnChanges(changes);
    
    this.canAddItem = this.getCanAddItem();
    this.canRemoveItem = this.getCanRemoveItem();
  }

  protected getOptions(): Partial<ArrayOptions> {
    return defaultOptions;
  }

  // protected get options(): ArrayOptions {
  //   if (!this._options) {
  //     this._options = { ...defaultOptions, ...this.getOptions() };
  //   }
  //   return this._options;
  // }

  public get formArray(): FormArray {
    return (this.formGroup.controls.innerControl as unknown) as FormArray;
  }

  public get formArrayName(): string {
    return 'innerControl';
  }

  public get formArrayErrors(): any {
    return (this.formGroup.errors || {}).innerControl || {};
  }

  private getCanAddItem(): boolean {
    return (
      !this.options.showAddAndRemove &&
      (!this.options.max ||
        this.formGroup.controls.innerControl.length < this.options.max)
    );
  }

  public getCanRemoveItem(): boolean {
    return (
      !this.options.showAddAndRemove &&
      this.formGroup.controls.innerControl.length > this.options.min
    );
  }

  public addItem(value: FormInterface = null) {
    this.formArray.push(
      (new SubFormGroup<FormInterface>(
        value || this.getDefaultValue()
      ) as unknown) as FormGroup
    );
  }

  public removeItem(index: number) {
    this.formArray.removeAt(index);
  }

  getDefaultValues() {
    const items: FormInterface[] = [];
    for (let i = 0; i < this.options.initial; i++) {
      items.push(this.getDefaultValue());
    }
    return {
      innerControl: items,
    };
  }

  getFormControl() {
    const controls: FormGroup[] = [];
    for (let i = 0; i < this.options.initial; i++) {
      controls.push(
        (new SubFormGroup<FormInterface>(
          this.getDefaultValue()
        ) as unknown) as FormGroup
      );
    }

    const validators = [Validators.minLength(this.options.min)];
    if (this.options.max) {
      validators.push(Validators.maxLength(this.options.min));
    }

    return (new SubFormArray<any, any>(
      this,
      controls,
      validators
    ) as unknown) as AbstractControl;
  }

  protected transformToFormGroup(
    innerControl: FormInterface[]
  ): WrappedControlForm<FormInterface[]> {
    return super.transformToFormGroup(innerControl || []);
  }

  protected transformFromFormGroup(
    formValue: WrappedControlForm<FormInterface[]>
  ) {
    return formValue.innerControl;
  }

  public createFormArrayControl(
    key: ArrayPropertyKey<WrappedControlForm<FormInterface[]>> | undefined,
    value: ArrayPropertyValue<WrappedControlForm<FormInterface[]>>
  ): FormControl {
    return (new SubFormGroup(value) as unknown) as FormControl;
  }
}
