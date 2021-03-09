// tslint:disable: no-console

import {
  Directive,
  Renderer2,
  ElementRef,
  forwardRef,
  HostListener,
  HostBinding,
  Input,
  OnInit,
} from '@angular/core';
import { NG_VALUE_ACCESSOR, DefaultValueAccessor } from '@angular/forms';
import * as _ from 'lodash';

const EXPRESS_CODE_INPUT_CONTROL_VALUE_ACCESSOR = {
  provide: NG_VALUE_ACCESSOR,
  useExisting: forwardRef(() => ExpressCodeDirective),
  multi: true,
};

function specialKeys(e: KeyboardEvent) {
  switch (e.keyCode) {
    case 8: // Backspace
    case 9: // Tab
    case 13: // Enter
    case 32: // Space
    case 116: // F5
      return true;
    default:
      return false;
  }
}

/**
 * Converts the input into uppercase letters
 *
 * @export
 * @class UpperCaseInputDirective
 * @extends {DefaultValueAccessor}
 */
@Directive({
  selector: 'input[express-code]',
  host: {
    // When the user updates the input
    '(blur)': 'onTouched()',
  },
  providers: [EXPRESS_CODE_INPUT_CONTROL_VALUE_ACCESSOR],
})
export class ExpressCodeDirective extends DefaultValueAccessor
  implements OnInit {
  @Input() numericEnabled = false;

  ngOnInit(): void {
    // Der Input Value kann ein String sein dann wird 'false'-> true und String('false')=='true' -> false
    this.numericEnabled = String(this.numericEnabled) === 'true';
  }

  constructor(renderer: Renderer2, elementRef: ElementRef) {
    super(renderer, elementRef, false);
  }

  writeValue(value: any): void {
    // console.log('writeValue', value);
    const transformed = this.transformValue(value);
    // console.log('transformed', transformed);
    super.writeValue(transformed);
  }

  private isAlphaKey(e: KeyboardEvent) {
    return (
      (e.keyCode >= 65 && e.keyCode <= 90) ||
      (e.keyCode >= 97 && e.keyCode <= 122)
    );
  }

  private isNumericKey(e: KeyboardEvent) {
    return e.keyCode >= 48 && e.keyCode <= 57;
  }

  private transformValue(value: string): string {
    if (value) {
      const upperCaseValue = value.toUpperCase();

      // Filter unerlaubte Zeichen
      let converted = upperCaseValue.replace(/[^a-zA-Z0-9 ]+/g, '');

      // Falls Zahlen nicht erlaubt sind diese filtern
      if (!this.numericEnabled) {
        converted = converted.replace(/[0-9]+/g, '');
      }

      return converted;
    } else {
      return null;
    }
  }

  @HostListener('input', ['$event'])
  onInput(e: any): void {
    // console.log('input', { e: e, value: e.target.value });

    const transformed = this.transformValue(e.target.value);

    super.writeValue(transformed);
    this.onChange(transformed);
  }

  @HostListener('keypress', ['$event'])
  onKeypress(e: KeyboardEvent): void {
    if (this.isAlphaKey(e)) {
      return;
    }

    if (this.numericEnabled && this.isNumericKey(e)) {
      return;
    }

    if (specialKeys(e)) {
      return;
    }

    e.preventDefault();
  }
}
