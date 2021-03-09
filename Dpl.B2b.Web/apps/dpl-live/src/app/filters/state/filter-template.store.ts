import { FilterTemplate } from '../services/filter.service.types';
import {
  EntityState,
  ActiveState,
  StoreConfig,
  EntityStore,
} from '@datorama/akita';
import { Injectable } from '@angular/core';

export interface VoucherFilterTemplateState
  extends EntityState<FilterTemplate>,
    ActiveState {}

const initialState = {
  active: null,
};

@Injectable({ providedIn: 'root' })
@StoreConfig({ name: 'voucher-filter-template', idKey: 'id', resettable: true })
export class FilterTemplateStore extends EntityStore<
  VoucherFilterTemplateState
> {
  constructor() {
    super(initialState);
  }
}
