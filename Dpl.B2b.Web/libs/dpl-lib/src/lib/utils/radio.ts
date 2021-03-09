import { MatRadioButton } from '@angular/material/radio';
import { AbstractControl } from '@angular/forms';

export function checkResetRadio(
  radioButton: MatRadioButton,
  control: AbstractControl
) {
  if (control.value === radioButton.value) {
    setTimeout(() => {
      control.reset();
    });
  }
}
