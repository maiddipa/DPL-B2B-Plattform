import { AbstractControl, ValidatorFn, FormArray } from '@angular/forms';

export class PalletValidators {
  static required(): ValidatorFn {
    return (c: AbstractControl): { [key: string]: boolean } | null => {
      if (c instanceof FormArray) {
        const fa = <FormArray>c;
        const r = fa.controls.filter((c) => c.value > 0).map((c) => c);
        if (r.length === 0) {
          return { required: true };
        }
      }
      return null;
    };
  }
}
