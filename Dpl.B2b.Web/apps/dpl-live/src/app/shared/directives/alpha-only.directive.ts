import { Directive, ElementRef, HostListener } from '@angular/core';

function isNavigationKeyCode(e: KeyboardEvent) {
  switch (e.keyCode) {
    case 8: //backspace
    case 35: //end
    case 36: //home
    case 37: //left
    case 38: //up
    case 39: //right
    case 40: //down
    case 45: //ins
    case 46: //del
      return true;
    default:
      return false;
  }
}

function isDotKeyCode(e: KeyboardEvent) {
  return e.keyCode === 190;
}

function isForwardSlashKeyCode(e: KeyboardEvent) {
  return e.keyCode === 191;
}

function isNumericKeyCode(e: KeyboardEvent) {
  return (
    (e.keyCode >= 48 && e.keyCode <= 57) ||
    (e.keyCode >= 96 && e.keyCode <= 105)
  );
}

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

function isAlphaKey(e: KeyboardEvent) {
  return e.keyCode >= 65 && e.keyCode <= 90;
}

function convert(input: string): string {
  return input.replace(/[A-Z]/gi, '');
}

@Directive({
  selector: 'input[alpha-only]',
})
export class AlphaOnlyDirective {
  private navigationKeys = [
    'Backspace',
    'Delete',
    'Tab',
    'Escape',
    'Enter',
    'Home',
    'End',
    'ArrowLeft',
    'ArrowRight',
    'Clear',
    'Copy',
    'Paste',
  ];
  inputElement: HTMLElement;
  constructor(public el: ElementRef) {
    this.inputElement = el.nativeElement;
  }

  @HostListener('input', ['$event'])
  input(e: KeyboardEvent) {
    console.log('input', { e: e });

    if (e.keyCode == 220) {
      console.log(220);
      e.preventDefault();
      return;
    }
  }

  @HostListener('keydown', ['$event'])
  onKeyDown(e: KeyboardEvent) {
    console.log('keydown', { e: e });
    if (
      this.navigationKeys.indexOf(e.key) > -1 || // Allow: navigation keys: backspace, delete, arrows etc.
      (e.key === 'a' && e.ctrlKey === true) || // Allow: Ctrl+A
      (e.key === 'c' && e.ctrlKey === true) || // Allow: Ctrl+C
      (e.key === 'v' && e.ctrlKey === true) || // Allow: Ctrl+V
      (e.key === 'x' && e.ctrlKey === true) || // Allow: Ctrl+X
      (e.key === 'a' && e.metaKey === true) || // Allow: Cmd+A (Mac)
      (e.key === 'c' && e.metaKey === true) || // Allow: Cmd+C (Mac)
      (e.key === 'v' && e.metaKey === true) || // Allow: Cmd+V (Mac)
      (e.key === 'x' && e.metaKey === true) // Allow: Cmd+X (Mac)
    ) {
      // let it happen, don't do anything
      console.log('navigationKeys1');
      return;
    }

    if (isNavigationKeyCode(e)) {
      console.log('navigationKeys2');
      return;
    }

    if (!isAlphaKey(e)) {
      console.log({ isAlphaKey: false });
      e.preventDefault();
      return;
    }
  }

  @HostListener('paste', ['$event'])
  onPaste(event: any) {
    if (event.clipboardData) {
      event.preventDefault();

      const clipboardData = event.clipboardData || (<any>window).clipboardData; // typecasting to any
      if (!clipboardData) {
        return;
      }

      const pastedInput: string = event.clipboardData.getData('text/plain');
      const convertedInput = convert(pastedInput);
      console.log({ pastedInput, convertedInput });

      document.execCommand('insertText', false, convertedInput);
    }
  }

  @HostListener('drop', ['$event'])
  onDrop(event: DragEvent) {
    event.preventDefault();
    const textData = event.dataTransfer.getData('text');
    this.inputElement.focus();
    const convertedInput = convert(textData);
    console.log({ textData, convertedInput });

    document.execCommand('insertText', false, convertedInput);
  }
}
