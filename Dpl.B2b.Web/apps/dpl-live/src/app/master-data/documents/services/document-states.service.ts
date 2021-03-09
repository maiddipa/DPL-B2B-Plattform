import { Injectable } from '@angular/core';
import { switchMap } from 'rxjs/operators';

import { DocumentStatesQuery } from '../state/document-states.query';

@Injectable({
  providedIn: 'root',
})
export class DocumentStatesService {
  constructor(private documentStatesQuery: DocumentStatesQuery) {}

  getDocumentStates() {
    return this.documentStatesQuery.documentStates$;
  }
}
