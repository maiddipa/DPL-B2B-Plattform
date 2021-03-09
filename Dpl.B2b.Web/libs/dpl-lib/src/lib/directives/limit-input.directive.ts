import {
  Directive,
  ElementRef,
  Input,
  OnInit,
  Renderer2,
  HostListener,
} from '@angular/core';
import { DefaultValueAccessor } from '@angular/forms';
import { subformComponentProviders } from 'ngx-sub-form';

export type TransformValueFn = (value: string) => string;

@Directive({
  selector: 'input[limitInput] , generic-lookup[limitInput]',
  host: {
    // When the user updates the input
    '(blur)': 'onTouched()',
  },
  providers: subformComponentProviders(LimitInputDirective),
})
export class LimitInputDirective extends DefaultValueAccessor
  implements OnInit {
  @Input() limitInput: RegExp | TransformValueFn = /[^a-zA-Z0-9]+/g; // default to alphanumeric

  constructor(renderer: Renderer2, elementRef: ElementRef) {
    super(renderer, elementRef, false);
  }

  transformValue: TransformValueFn;

  ngOnInit(): void {
    if (typeof this.limitInput === 'function') {
      this.transformValue = this.limitInput;
    } else {
      this.transformValue = this.transformValueRegex(this.limitInput);
    }
  }

  @HostListener('input', ['$event']) onInput(e: {
    target: { value: string };
  }): void {
    const transformed = this.transformValue(e.target.value);
    super.writeValue(transformed);
    this.onChange(transformed);
  }

  // @HostListener('change', ['$event']) onChangeEvent(e: {
  //   target: { value: string };
  // }): void {
  // }

  // @HostListener('click', ['$event']) onClick(e: {
  //   target: { value: string };
  // }): void {
  // }

  writeValue(value: any): void {
    const transformed = this.transformValue(value as string);
    super.writeValue(transformed);
  }

  private transformValueRegex(regex: RegExp) {
    return (value: string): string => {
      if (value) {
        const upperCaseValue = value.toUpperCase();

        // Filter unerlaubte Zeichen
        const converted = upperCaseValue.replace(regex, '');
        return converted;
      } else {
        return null;
      }
    };
  }
}
