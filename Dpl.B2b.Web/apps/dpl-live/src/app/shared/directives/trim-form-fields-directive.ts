import { FormControlDirective, FormControlName } from "@angular/forms";
import { Directive, HostListener, Input, Optional } from "@angular/core";

@Directive({
  selector: '[formControl],[formControlName],[trim],input.[formControl],input.[formControlName]',
   //selector: '[formControl][trim], [formControlName][trim]',  // benötigt weitere attribut trim
})
export class TrimFormFieldsDirective {
  @Input() type: string;

  constructor(
    @Optional() private formControlDir: FormControlDirective,
    @Optional() private formControlName: FormControlName) {}

  @HostListener('blur')
  @HostListener('keydown.enter')
  trimValue() {
    const control = this.formControlDir?.control || this.formControlName?.control;
    if (typeof control?.value === 'string' && this.type !== 'password') {
      control.setValue(control.value.trim());
    }
  }
}
