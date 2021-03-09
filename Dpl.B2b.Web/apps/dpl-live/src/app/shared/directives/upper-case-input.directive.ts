import {
  Directive,
  Renderer2,
  ElementRef,
  forwardRef,
  HostListener,
  HostBinding,
} from '@angular/core';
import { NG_VALUE_ACCESSOR, DefaultValueAccessor } from '@angular/forms';

const UPPERCASE_INPUT_CONTROL_VALUE_ACCESSOR = {
  provide: NG_VALUE_ACCESSOR,
  useExisting: forwardRef(() => UpperCaseInputDirective),
  multi: true,
};
/**
 * Converts the input into uppercase letters
 *
 * @export
 * @class UpperCaseInputDirective
 * @extends {DefaultValueAccessor}
 */
@Directive({
  selector: 'input[uppercase]',
  host: {
    // When the user updates the input
    '(blur)': 'onTouched()',
  },
  providers: [UPPERCASE_INPUT_CONTROL_VALUE_ACCESSOR],
})
export class UpperCaseInputDirective extends DefaultValueAccessor {
  constructor(renderer: Renderer2, elementRef: ElementRef) {
    super(renderer, elementRef, false);
  }

  writeValue(value: any): void {
    const transformed = this.transformValue(value);

    super.writeValue(transformed);
  }

  @HostListener('input', ['$event.target.value'])
  onInput(value: any): void {
    const transformed = this.transformValue(value);

    super.writeValue(transformed);
    this.onChange(transformed);
  }

  private transformValue(value: any): any {
    const result =
      value && typeof value === 'string' ? value.toUpperCase() : value;

    return result;
  }
}
