import { AbstractControl, FormControl } from '@angular/forms';
import { Controls, NgxSubFormRemapComponent } from 'ngx-sub-form';
import { Directive } from '@angular/core';

export interface WrappedControlForm<TControl> {
  innerControl: TControl;
}

@Directive()
export abstract class NgxSingleFieldSubFormComponent<
  FormInterface
> extends NgxSubFormRemapComponent<
  FormInterface,
  WrappedControlForm<FormInterface>
> {
  constructor() {
    super();
  }

  protected getFormControl(): AbstractControl {
    return new FormControl(null);
  }

  protected getFormControls(): Controls<WrappedControlForm<FormInterface>> {
    return {
      innerControl: this.getFormControl(),
    };
  }
  protected transformToFormGroup(
    innerControl: FormInterface
  ): WrappedControlForm<FormInterface> {
    return innerControl
      ? { innerControl }
      : ((this.getDefaultValues() || {}) as WrappedControlForm<FormInterface>);
    //return innerControl ? { innerControl } : null;
  }
  protected transformFromFormGroup(
    formValue: WrappedControlForm<FormInterface>
  ): FormInterface {
    return formValue ? formValue.innerControl : null;
  }
}
