import {
  Component,
  ContentChild,
  Input,
  OnChanges,
  SimpleChanges,
  TemplateRef,
} from '@angular/core';
import { subformComponentProviders } from 'ngx-sub-form';

import { hostFlexColumn } from '../utils';
import {
  ArrayOptions,
  NgxArraySubFormComponent,
} from './ngx-array-sub-form.component';
import { AbstractControl } from '@angular/forms';

export interface GenericArrayFormConfig<
  TValue = any,
  TContext extends {} = {}
> {
  defaultValue: TValue;
  options?: Partial<ArrayOptions>;
  requiredError?: string;
  fxLayoutGap?: string;
  context?: TContext;
}

const defaultConfig: Partial<GenericArrayFormConfig> = {
  options: {
    initial: 1,
    min: 1,
  },
  fxLayoutGap: '10px',
  context: {},
  defaultValue: {},
};

/**
 * Use ng-template to define array element as shown below:
 *
 * <ng-template let-control="control">
 *   <app-load-carrier-form [subForm]="control"></app-load-carrier-form>
 * </ng-template>
 *
 */
@Component({
  selector: 'generic-array-form',
  template: `
    <ng-container *ngIf="options && formGroup" [formGroup]="formGroup">
      <ng-container [formArrayName]="formArrayName">
        <div
          fxLayout="row"
          fxLayoutAlign=" center"
          [fxLayoutGap]="mergedConfig.fxLayoutGap"
          *ngFor="let control of formArray.controls; let i = index"
        >
          <div
            fxFlex
            fxLayout="column"
            [fxLayoutGap]="mergedConfig.fxLayoutGap"
          >
            <ng-template
              [ngTemplateOutlet]="template"
              [ngTemplateOutletContext]="getContext(control)"
            ></ng-template>
          </div>

          <button
            *ngIf="showAddAndRemove"
            mat-icon-button
            [disabled]="!canRemoveItem"
            (click)="removeItem(i)"
          >
            <mat-icon>remove_circle_outline</mat-icon>
          </button>
        </div>
        <mat-error *ngIf="formArray.touched && formArrayErrors.required">
          {{ config.requiredError }}
        </mat-error>
        <button
          *ngIf="showAddAndRemove"
          mat-raised-button
          [disabled]="canAddItem && (formArray.invalid || formArray.disabled)"
          (click)="addItem()"
        >
          <mat-icon>add</mat-icon>
        </button>
      </ng-container>
    </ng-container>
  `,
  styles: [hostFlexColumn],
  providers: subformComponentProviders(GenericArrayFormComponent),
})
export class GenericArrayFormComponent extends NgxArraySubFormComponent<any>
  implements OnChanges {
  @Input() config: GenericArrayFormConfig;
  @ContentChild(TemplateRef, { static: true }) template: TemplateRef<any>;

  showAddAndRemove: boolean;
  protected emitInitialValueOnInit = false;
  mergedConfig: GenericArrayFormConfig;

  ngOnChanges(changes: SimpleChanges) {
    this.mergedConfig = { ...defaultConfig, ...this.config };

    super.ngOnChanges(changes);

    this.showAddAndRemove = this.getShowAddAndRemove();
  }

  getDefaultValue() {
    this.mergedConfig.defaultValue;
  }

  protected getOptions(): Partial<ArrayOptions> {
    return this.mergedConfig.options;
  }

  private getShowAddAndRemove(): boolean {
    return (
      this.options.showAddAndRemove && this.options.min !== this.options.max
    );
  }

  public getContext(control: AbstractControl, context: {}) {
    return {
      ...this.mergedConfig.context,
      control,
    };
  }
}
