import { AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';

export function atLeastOneValidator(
  controls: AbstractControl[],
  validators: ValidatorFn[]
) {
  // we dont care about the control passed in from the validator directly
  return (c: AbstractControl) => {
    if (controls) {
      const validControl = controls.find(
        (control) =>
          validators
            .map((validator) => validator(control))
            .filter((result) => !result).length > 0
      );

      return validControl
        ? null
        : ({
            atLeastOne: true,
          } as ValidationErrors);
    }
  };
}
