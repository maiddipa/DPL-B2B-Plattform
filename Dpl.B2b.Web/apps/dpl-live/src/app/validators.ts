import { AbstractControl, ValidationErrors } from '@angular/forms';

export class CustomValidators {
  static isObject(c: AbstractControl): ValidationErrors {
    if (!c.value || typeof c.value === 'object') {
      return null;
    }

    return {
      isObject: true,
    };
  }
}
