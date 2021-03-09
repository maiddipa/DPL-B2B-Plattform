import { Injectable } from '@angular/core';
import { QueryEntity } from '@datorama/akita';

import {
  DocumentStatesState,
  DocumentStatesStore,
} from './document-states.store';

@Injectable({ providedIn: 'root' })
export class DocumentStatesQuery extends QueryEntity<DocumentStatesState> {
  documentStates$ = this.selectAll();

  constructor(protected store: DocumentStatesStore) {
    super(store);
  }
}
