import {
  Component,
  EventEmitter,
  OnInit,
  Output,
  OnDestroy,
} from '@angular/core';
import {
  hostFlexColumn,
  NgxSingleFieldSubFormComponent,
  updateSingleFieldDefaultValues,
} from '@dpl/dpl-lib';
import { subformComponentProviders } from 'ngx-sub-form';
import { combineLatest, Observable } from 'rxjs';
import { first, map, tap } from 'rxjs/operators';

import { LanguagesService } from '../../../master-data/languages/services/languages.service';
import { ILanguage } from '../../../master-data/languages/state/language.model';

type ViewData = {
  languages: ILanguage[];
};

@Component({
  selector: 'app-print-language-picker',
  template: `
    <mat-form-field *ngIf="formGroup" [formGroup]="formGroup">
      <mat-select
        placeholder="Belegsprache"
        i18n-placeholder="PrintLanguage|Label Drucksprache@@PrintLanguage"
        [formControl]="formGroup.controls.innerControl"
        (valueChange)="selectionChanged.emit($event)"
      >
        <ng-container *ngIf="viewData$ | async as data">
          <mat-option
            *ngFor="let language of data.languages"
            [value]="language.id"
          >
            {{ language.id | language }}
          </mat-option>
        </ng-container>
      </mat-select>
    </mat-form-field>
  `,
  styles: [hostFlexColumn],
  providers: subformComponentProviders(PrintLanguagePickerComponent),
})
export class PrintLanguagePickerComponent
  extends NgxSingleFieldSubFormComponent<number>
  implements OnInit, OnDestroy {
  @Output() selectionChanged = new EventEmitter<number>();

  viewData$: Observable<ViewData>;
  constructor(private languageService: LanguagesService) {
    super();
    updateSingleFieldDefaultValues(this);
  }

  ngOnInit() {
    const currentLanguage$ = this.languageService
      .getActiveLanguage()
      .pipe(first());

    const languages$ = combineLatest(
      this.languageService.getLanguages(),
      currentLanguage$
    ).pipe(
      tap(([languages, current]) => {
        if (!languages || languages.length === 0) {
          return;
        }

        const language = current || languages[0];
        this.formGroup.controls.innerControl.patchValue(language.id);
      }),
      map(([languages, current]) => {
        return languages;
      })
    );

    this.viewData$ = combineLatest(languages$).pipe(
      map(([languages]) => {
        return {
          languages,
        };
      })
    );
  }

  ngOnDestroy(): void {}
}
