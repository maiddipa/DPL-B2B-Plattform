import {
  Directive,
  Renderer2,
  ElementRef,
  forwardRef,
  HostListener,
} from '@angular/core';
import { NG_VALUE_ACCESSOR, DefaultValueAccessor } from '@angular/forms';

const LOWERCASE_INPUT_CONTROL_VALUE_ACCESSOR = {
  provide: NG_VALUE_ACCESSOR,
  useExisting: forwardRef(() => LowerCaseInputDirective),
  multi: true,
};
/**
 * Converts the input to lowercase letters
 *
 * @export
 * @class LowerCaseInputDirective
 * @extends {DefaultValueAccessor}
 */
@Directive({
  selector: 'input[lowercase]',
  host: {
    // When the user updates the input
    '(blur)': 'onTouched()',
  },
  providers: [LOWERCASE_INPUT_CONTROL_VALUE_ACCESSOR],
})
export class LowerCaseInputDirective extends DefaultValueAccessor {
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
      value && typeof value === 'string'
        ? value
          ? value.toLowerCase()
          : value
        : value;

    return result;
  }
}
