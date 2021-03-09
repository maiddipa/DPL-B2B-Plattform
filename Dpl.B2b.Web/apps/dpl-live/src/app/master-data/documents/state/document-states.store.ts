import { Injectable } from '@angular/core';
import { IDocumentState } from './document-state.model';
import { EntityState, EntityStore, StoreConfig } from '@datorama/akita';

export interface DocumentStatesState
  extends EntityState<IDocumentState, number> {}

@Injectable({ providedIn: 'root' })
@StoreConfig({ name: 'document-states' })
export class DocumentStatesStore extends EntityStore<DocumentStatesState> {
  constructor() {
    super();
  }
}
