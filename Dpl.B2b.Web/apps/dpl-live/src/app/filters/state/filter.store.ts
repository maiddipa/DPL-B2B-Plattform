import {
  EntityState,
  EntityStore,
  StoreConfig,
  EntityUIStore,
  ActiveState,
  MultiActiveState,
} from '@datorama/akita';
import { Injectable } from '@angular/core';
import { Filter, FilterContext } from '../services/filter.service.types';
import { FilterUI } from './filter.model';

export interface FilterState extends EntityState<Filter>, MultiActiveState {
  context: FilterContext | undefined;
  ui: {
    templateHasChanges: boolean;
  };
}

export interface FilterUIState extends EntityState<FilterUI> {}

const initialState: Partial<FilterState> = {
  active: [],
  ui: {
    templateHasChanges: false,
  },
};

@Injectable({ providedIn: 'root' })
@StoreConfig({
  name: 'voucher-filter',
  idKey: 'propertyName',
  resettable: true,
})
export class FilterStore extends EntityStore<FilterState> {
  ui: EntityUIStore<FilterUIState>;

  constructor() {
    super(initialState);
    this.createUIStore().setInitialEntityState((entity) => ({
      id: entity.propertyName,
      changed: false,
      pendingValue: null,
    }));
  }

  updateTemplateChanged(templateHasChanges: boolean, context: FilterContext) {
    this.update({ context, ui: { templateHasChanges } });
  }
}
