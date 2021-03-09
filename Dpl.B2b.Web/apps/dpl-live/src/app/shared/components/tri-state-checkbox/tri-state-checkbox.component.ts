import {
  Component,
  forwardRef,
  OnInit,
  ElementRef,
  OnDestroy,
  HostBinding,
} from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';
import { MAT_CHECKBOX_CLICK_ACTION } from '@angular/material/checkbox';
import { FocusMonitor } from '@angular/cdk/a11y';

/** Coerces a data-bound value (typically a string) to a boolean. */
function coerceBooleanProperty(value: any): boolean {
  return value != null && `${value}` !== 'false';
}

@Component({
  selector: 'palletmanagement-tri-state-checkbox',
  templateUrl: './tri-state-checkbox.component.html',
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => TriStateCheckboxComponent),
      multi: true,
    },
    { provide: MAT_CHECKBOX_CLICK_ACTION, useValue: 'noop' },
  ],
})
export class TriStateCheckboxComponent
  implements ControlValueAccessor, OnDestroy {
  static nextId = 0;

  constructor(
    private fm: FocusMonitor,
    private elRef: ElementRef<HTMLElement>
  ) {
    fm.monitor(elRef.nativeElement, true).subscribe((origin) => {
      this.focused = !!origin;
      // this.stateChanges.next();
    });
  }

  tape = [null, true, false];

  value: boolean | null;

  disabled: boolean;

  focused = false;

  @HostBinding()
  id = `tri-state-checkbox-input-${TriStateCheckboxComponent.nextId++}`;

  private onChange: (val: boolean) => void;
  private onTouched: () => void;

  writeValue(value: boolean) {
    console.log('writeValue', { value });
    this.value = value;
  }

  setDisabledState(disabled: boolean) {
    this.disabled = coerceBooleanProperty(disabled);
  }

  toggle(e: any) {
    if (this.disabled) {
      if (e instanceof MouseEvent) {
        e.preventDefault();
        e.stopPropagation();
      }
      if (e instanceof KeyboardEvent) {
        e.preventDefault();
      }
    } else {
      this.onChange(
        (this.value = this.tape[
          (this.tape.indexOf(this.value) + 1) % this.tape.length
        ])
      );
      this.onTouched();
    }
  }

  registerOnChange(fn: any) {
    this.onChange = fn;
  }

  registerOnTouched(fn: any) {
    this.onTouched = fn;
  }

  onContainerClick(event: MouseEvent) {
    if ((event.target as Element).tagName.toLowerCase() != 'input') {
      this.elRef.nativeElement.querySelector('input').focus();
    }
  }

  ngOnDestroy() {
    this.fm.stopMonitoring(this.elRef.nativeElement);
  }
}
