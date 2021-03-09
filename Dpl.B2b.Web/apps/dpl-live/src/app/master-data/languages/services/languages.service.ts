import { Injectable } from '@angular/core';
import { map, switchMap } from 'rxjs/operators';

import { LanguagesQuery } from '../state/languages.query';

@Injectable({ providedIn: 'root' })
export class LanguagesService {
  constructor(private languagesQuery: LanguagesQuery) {}

  getLanguages() {
    return this.languagesQuery.languages$.pipe(
      map((languages) => {
        // HACK for NETTO go live - only german
        return languages.filter((x) => x.locale === 'de');
      })
    );
  }

  getActiveLanguage() {
    return this.languagesQuery.activeLanguage$;
  }
}
