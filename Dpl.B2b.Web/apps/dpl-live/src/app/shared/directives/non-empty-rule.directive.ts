import { Directive } from '@angular/core';
import { Validator, AbstractControl, NG_VALIDATORS } from '@angular/forms';

export function ValidateNonEmpty(control: AbstractControl): {[key: string]: any} | null  {
  if (control.value && control.value?.trim()?.length == 0) {
    return { 'required': true };
  }
  return null;
}

@Directive({
  selector: '[nonEmptyRule],[non-empty-rule]',   // OR
  //selector: '[required][nonEmptyRule],[required][non-empty-rule],[nonEmptyRule],[non-empty-rule]',   // AND+ OR
  //selector: '[required][nonEmptyRule]',   // AND
  providers: [{
    provide: NG_VALIDATORS,
    useExisting: NonEmptyRuleDirective,
    multi: true
  }]
})
export class NonEmptyRuleDirective implements Validator {
  validate(control: AbstractControl) : {[key: string]: any} | null {
    return ValidateNonEmpty(control);
  }
}
