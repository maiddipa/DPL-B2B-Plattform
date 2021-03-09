import { Pipe, PipeTransform } from '@angular/core';
import * as _ from 'lodash';
import { tap } from 'rxjs/operators';
import { LocalizationService } from 'apps/dpl-live/src/app/core/services/localization.service';
import { LanguagesService } from 'apps/dpl-live/src/app/master-data/languages/services/languages.service';
import { ILanguage } from 'apps/dpl-live/src/app/master-data/languages/state/language.model';

export type LanguagePipeFormat = 'long';

@Pipe({
  name: 'language',
})
export class LanguagePipe implements PipeTransform {
  languagesDict: {
    [id: number]: ILanguage;
  };

  constructor(
    private localizationService: LocalizationService,
    private languageService: LanguagesService
  ) {
    // cache countries
    this.languageService
      .getLanguages()
      .pipe(
        tap((languages) => {
          this.languagesDict = _(languages)
            .keyBy((i) => i.id)
            .value();
        })
      )
      .subscribe();
  }

  transform(id: number, format: LanguagePipeFormat = 'long'): any {
    if (!id) {
      throw 'id cannot be null';
    }

    switch (format) {
      case 'long':
      default:
        return this.getTranslation(id);
    }
  }

  private getTranslation(id: number) {
    return this.localizationService.getTranslation(
      'LocalizationLanguages',
      id.toString()
    );
  }
}
