import {
  AfterContentChecked,
  AfterContentInit,
  AfterViewInit,
  ChangeDetectionStrategy,
  ChangeDetectorRef,
  Component,
  ContentChild,
  ElementRef,
  Inject,
  NgZone,
  Optional,
  ViewEncapsulation,
} from '@angular/core';
import {
  CanColor,
  LabelOptions,
  MAT_LABEL_GLOBAL_OPTIONS,
} from '@angular/material/core';
import {
  MAT_FORM_FIELD_DEFAULT_OPTIONS,
  MatFormField,
  matFormFieldAnimations,
  MatFormFieldDefaultOptions,
} from '@angular/material/form-field';
import { MatRadioGroup } from '@angular/material/radio';
import { ANIMATION_MODULE_TYPE } from '@angular/platform-browser/animations';
import { startWith } from 'rxjs/operators';
import { Directionality } from '@angular/cdk/bidi';
import { Platform } from '@angular/cdk/platform';

export function getMatRadioGroupMissingControlError(): Error {
  return Error('radio-group-form-field must contain a MatRadioGroup.');
}

@Component({
  selector: 'radio-group-form-field',
  template: `
    <div class="mat-form-field-wrapper">
      <div class="mat-form-field-flex">
        <div class="mat-form-field-infix">
          <ng-content></ng-content>
          <span class="mat-form-field-label-wrapper">
            <label
              class="mat-form-field-label"
              [class.mat-accent]="color == 'accent'"
              [class.mat-warn]="color == 'warn'"
              #label
              *ngIf="_hasLabel"
            >
              <ng-content select="mat-label"></ng-content>
              <span
                class="mat-placeholder-required mat-form-field-required-marker"
                aria-hidden="true"
                *ngIf="
                  !hideRequiredMarker &&
                  radioGroup.required &&
                  !radioGroup.disabled
                "
                >&nbsp;*</span
              >
              <!-- @deletion-target 8.0.0 remove \`mat-placeholder-required\` class -->
            </label>
          </span>
        </div>
      </div>

      <div class="mat-form-field-subscript-wrapper">
        <div *ngIf="hasError" [@transitionMessages]="_subscriptAnimationState">
          <ng-content select="mat-error"></ng-content>
        </div>

        <div
          class="mat-form-field-hint-wrapper"
          *ngIf="!hasError"
          [@transitionMessages]="_subscriptAnimationState"
        >
          <ng-content select="mat-hint:not([align='end'])"></ng-content>
          <div class="mat-form-field-hint-spacer"></div>
          <ng-content select="mat-hint[align='end']"></ng-content>
        </div>
      </div>
    </div>
  `,
  styles: [
    `
      .mat-form-field.radio-group-form-field.mat-form-field-invalid
        .mat-radio-outer-circle {
        border-color: #f44336;
      }

      .mat-form-field.radio-group-form-field.mat-form-field-invalid
        .mat-radio-label-content {
        color: #f44336;
      }

      .mat-form-field.radio-group-form-field .mat-radio-group {
        position: relative;
        top: 4px;
      }
    `,
  ],
  animations: [matFormFieldAnimations.transitionMessages],
  host: {
    class: 'mat-form-field radio-group-form-field',
    '[class.mat-form-field-appearance-standard]': 'appearance == "standard"',
    '[class.mat-form-field-appearance-fill]': 'appearance == "fill"',
    '[class.mat-form-field-appearance-outline]': 'appearance == "outline"',
    '[class.mat-form-field-appearance-legacy]': 'appearance == "legacy"',
    '[class.mat-form-field-invalid]': 'hasError',
    '[class.mat-form-field-can-float]': '_canLabelFloat',
    '[class.mat-form-field-should-float]': 'shouldLabelFloat',
    '[class.mat-form-field-disabled]': 'radioGroup.disabled',
    '[class.mat-accent]': 'color == "accent"',
    '[class.mat-warn]': 'color == "warn"',
    '[class._mat-animation-noopable]': '!_animationsEnabled',
  },
  inputs: ['color'],
  encapsulation: ViewEncapsulation.None,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class RadioGroupFormField extends MatFormField
  implements AfterContentInit, AfterContentChecked, AfterViewInit, CanColor {
  /**
   * The radio group associated with the radio group form field.
   */
  @ContentChild(MatRadioGroup) radioGroup: MatRadioGroup;

  constructor(
    public _elementRef: ElementRef,
    private _newChangeDetectorRef: ChangeDetectorRef,
    @Optional()
    @Inject(MAT_LABEL_GLOBAL_OPTIONS)
    private labelOptions: LabelOptions,
    @Optional() private _newDir: Directionality,
    @Optional()
    @Inject(MAT_FORM_FIELD_DEFAULT_OPTIONS)
    private formFieldDefaultOptions: MatFormFieldDefaultOptions,
    // @deletion-target 7.0.0 _platform, _ngZone and _animationMode to be made required.
    private _newPlatform?: Platform,
    private _newNgZone?: NgZone,
    @Optional() @Inject(ANIMATION_MODULE_TYPE) _animationMode?: string
  ) {
    super(
      _elementRef,
      _newChangeDetectorRef,
      labelOptions,
      _newDir,
      formFieldDefaultOptions,
      _newPlatform,
      _newNgZone,
      _animationMode
    );

    if (labelOptions) {
      this.floatLabel = labelOptions.float;
    } else {
      this.floatLabel = 'always';
    }
  }

  /** Whether there are one or more errors associated with the radio group form field. */
  get hasError(): boolean {
    if (this._errorChildren) {
      if (this._errorChildren.length > 0) {
        return true;
      }
    }

    return false;
  }

  /** Returns whether the label should float or not. */
  get shouldLabelFloat(): boolean {
    return this._canLabelFloat && this._shouldAlwaysFloat;
  }

  ngAfterContentChecked() {
    this._validateRadioGroupChild();
  }

  ngAfterContentInit() {
    this._validateRadioGroupChild();

    // Re-validate when the number of hints changes.
    this._hintChildren.changes.pipe(startWith(null)).subscribe(() => {
      this._newChangeDetectorRef.markForCheck();
    });

    // Re-validate when the number of errors changes.
    this._errorChildren.changes.pipe(startWith(null)).subscribe(() => {
      this._newChangeDetectorRef.markForCheck();
    });
  }

  ngAfterViewInit() {
    // Avoid animations on load.
    this._subscriptAnimationState = 'enter';
    this._newChangeDetectorRef.detectChanges();
  }

  protected _validateRadioGroupChild() {
    if (!this.radioGroup) {
      throw getMatRadioGroupMissingControlError();
    }
  }
}
