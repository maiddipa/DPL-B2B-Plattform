import { Injectable } from '@angular/core';
import { QueryConfig, Order, QueryEntity } from '@datorama/akita';
import {
  VoucherFilterTemplateState,
  FilterTemplateStore,
} from './filter-template.store';

@Injectable({ providedIn: 'root' })
@QueryConfig({
  sortBy: 'title',
  sortByOrder: Order.ASC,
})
export class FilterTemplateQuery extends QueryEntity<
  VoucherFilterTemplateState
> {
  allTemplates$ = this.selectAll();
  activeTemplate$ = this.selectActive();
  constructor(protected store: FilterTemplateStore) {
    super(store);
  }
}
