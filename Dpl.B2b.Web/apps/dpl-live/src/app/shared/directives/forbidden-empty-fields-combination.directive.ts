import { Directive } from '@angular/core';
import {
  FormGroup,
  NG_VALIDATORS,
  ValidationErrors,
  ValidatorFn,
} from '@angular/forms';

export const forbiddenEmptyFieldsCombinationValidator = (
  fields: string[]
): ValidatorFn => {
  return (control: FormGroup): ValidationErrors | null => {
    let fieldAreEmpty = true;
    fields.forEach((field) => {
      const fieldControl = control.get(field);
      if (fieldControl && fieldControl.value.length !== 0) {
        fieldAreEmpty = false;
      }
    });

    return fieldAreEmpty ? { forbiddenEmptyFieldsCombination: true } : null;
  };
};

@Directive({
  selector: '[appForbiddenEmptyFieldsCombination]',
  providers: [
    {
      provide: NG_VALIDATORS,
      useExisting: ForbiddenEmptyFieldsCombinationValidatorDirective,
      multi: true,
    },
  ],
})
export class ForbiddenEmptyFieldsCombinationValidatorDirective {
  validate(fields: string[]): ValidationErrors {
    return forbiddenEmptyFieldsCombinationValidator(fields);
  }
}
