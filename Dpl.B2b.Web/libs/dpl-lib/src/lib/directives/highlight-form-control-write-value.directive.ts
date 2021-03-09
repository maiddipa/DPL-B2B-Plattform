import {
  Directive,
  ElementRef,
  Input,
  OnChanges,
  Renderer2,
  SimpleChanges,
} from '@angular/core';
import { NgControl, ControlValueAccessor } from '@angular/forms';

const highlightClass = 'form-control-highlight-change';

@Directive({
  selector:
    'mat-radio-group[dplHighlight], input[dplHighlight],mat-select[dplHighlight]',
})
export class HighlightFormControlWriteValueDirective implements OnChanges {
  @Input('dplHighlight') control: NgControl;

  constructor(private renderer: Renderer2, private hostElement: ElementRef) {}

  ngOnChanges(changes: SimpleChanges): void {
    if (changes.control && this.control) {
      const writeValue = this.control.valueAccessor.writeValue.bind(
        this.control.valueAccessor
      ) as ControlValueAccessor['writeValue'];
      this.control.valueAccessor.writeValue = (obj: any) => {
        writeValue(obj);

        if (!obj) {
          return;
        }

        // trigger animation by adding class
        this.renderer.addClass(this.hostElement.nativeElement, highlightClass);

        // make sure to remove the class so animation can be triggered again
        setTimeout(() => {
          this.renderer.removeClass(
            this.hostElement.nativeElement,
            highlightClass
          );
        }, 5000);
      };
    }
  }
}
