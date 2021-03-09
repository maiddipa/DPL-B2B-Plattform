import { OnInit, OnDestroy } from '@angular/core';
import {
  NgxSubFormComponent,
  NgxSubFormRemapComponent,
  takeUntilDestroyed,
} from 'ngx-sub-form';
import { merge, Subject } from 'rxjs';
import { tap } from 'rxjs/operators';

import {
  NgxSingleFieldSubFormComponent,
  WrappedControlForm,
} from '../components';
import { FormControlName, AbstractControl } from '@angular/forms';

export function updateSingleFieldDefaultValues<
  TComponent extends NgxSingleFieldSubFormComponent<FormInterface> &
    OnInit &
    OnDestroy,
  FormInterface
>(component: TComponent) {
  const { ngOnInit } = component as any;
  let defaultValue: WrappedControlForm<FormInterface>;
  component.ngOnInit = () => {
    const controlValue$ = component.formGroup.controls.innerControl.valueChanges
      .pipe(
        takeUntilDestroyed(component),
        tap((value: FormInterface) => {
          if (!value) {
            return;
          }
          defaultValue = {
            innerControl: value,
          };
        })
      )
      .subscribe();

    ngOnInit.bind(component)();
  };

  (component as any).getDefaultValues = () => {
    return defaultValue;
  };
}

export function updateDefaultValues<
  TComponent extends NgxSubFormComponent<FormInterface> & OnInit & OnDestroy,
  FormInterface
>(component: TComponent) {
  const { ngOnInit } = component as any;
  let defaultValue: FormInterface;
  component.ngOnInit = () => {
    const controlValue$ = component.formGroup.valueChanges
      .pipe(
        takeUntilDestroyed(component),
        tap((value: FormInterface) => {
          if (!value) {
            return;
          }
          defaultValue = value;
        })
      )
      .subscribe();

    ngOnInit.bind(component)();
  };

  (component as any).getDefaultValues = () => {
    return defaultValue;
  };
}

export function updateRemapDefaultValues<
  TComponent extends NgxSubFormRemapComponent<ControlInterface, FormInterface> &
    OnInit &
    OnDestroy,
  ControlInterface,
  FormInterface
>(component: TComponent) {
  const { ngOnInit, writeValue, transformToFormGroup } = component as any;
  const formValueSubject$ = new Subject<FormInterface>();
  let defaultValue: FormInterface;
  component.ngOnInit = () => {
    const controlValue$ = component.formGroup.valueChanges
      .pipe(
        takeUntilDestroyed(component),
        tap((value: ControlInterface) => {
          if (!value) {
            return;
          }
          defaultValue = transformToFormGroup(value) as FormInterface;
        })
      )
      .subscribe();

    ngOnInit.bind(component)();
  };

  (component as any).getDefaultValues = () => {
    return defaultValue;
  };
}

// this is a mixin that is supposed to be used to add fucntionaility to the formCOntrolname directive
export type Constructor<T> = new (...args: any[]) => T;
export function ngxSubFormMixIn<T extends Constructor<{}>>(base: T) {
  return class extends base {
    constructor(...args: any[]) {
      super(args);

      const self = (this as unknown) as FormControlName;
      if (self.valueAccessor instanceof NgxSubFormComponent) {
        const subForm = self.valueAccessor;

        const markAsTouched = self.control.markAsTouched.bind(
          self.control
        ) as AbstractControl['markAsTouched'];
        self.control.markAsTouched = (
          opts: Parameters<AbstractControl['markAsTouched']>[0]
        ) => {
          markAsTouched(opts);
          subForm.formGroup.markAllAsTouched();
        };

        const markAllAsTouched = self.control.markAllAsTouched.bind(
          self.control
        ) as AbstractControl['markAllAsTouched'];
        self.control.markAllAsTouched = () => {
          markAllAsTouched();
          subForm.formGroup.markAllAsTouched();
        };

        const markAsUntouched = self.control.markAsUntouched.bind(
          self.control
        ) as AbstractControl['markAsUntouched'];
        self.control.markAsUntouched = (
          opts: Parameters<AbstractControl['markAsUntouched']>[0]
        ) => {
          markAsUntouched(opts);
          subForm.formGroup.markAsUntouched();
        };
      }
    }
  };
}

// export function ngxSubFormMixIn2<
//   T extends Constructor<{}> & NgxSubFormComponent<any, any> & OnInit
// >(base: T) {
//   return class extends base implements OnInit {
//     constructor(...args: any[]) {
//       super(args);
//     }

//     ngOnInit(): void {
//       throw new Error("Method not implemented.");
//     }
//   };
// }
