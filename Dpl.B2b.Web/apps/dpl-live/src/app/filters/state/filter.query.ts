import { FilterState, FilterStore, FilterUIState } from './filter.store';
import {
  QueryEntity,
  QueryConfig,
  Order,
  EntityUIQuery,
} from '@datorama/akita';
import { Injectable } from '@angular/core';
import { FilterPosition } from '../services/filter.service.types';
import { map } from 'rxjs/operators';
import { FilterUI } from './filter.model';
import { Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
@QueryConfig({
  sortBy: 'order',
  sortByOrder: Order.ASC,
})
export class FilterQuery extends QueryEntity<FilterState> {
  ui: EntityUIQuery<FilterUIState>;
  avaibleVoucherFilters$ = this.selectAll();
  appliedVoucherFilters$ = this.selectActive();
  extendedVoucherFilters$ = this.extendedVoucherFilters();
  changedVoucherFilters$: Observable<FilterUI[]>;
  shouldActiveVoucherFilterIds$: Observable<string[]>;
  hasChangedFilters$: Observable<boolean>;
  hasAvailableExtendedFilters$: Observable<boolean>;
  hasTemplateChanges$ = this.select((state) => state.ui.templateHasChanges);

  constructor(protected store: FilterStore) {
    super(store);
    this.createUIQuery();
    this.changedVoucherFilters$ = this.getChangedFiltersWithValue();
    this.shouldActiveVoucherFilterIds$ = this.getShouldActiveVoucherFilterIds();
    this.hasChangedFilters$ = this.getHasChangedFilters();
    this.hasAvailableExtendedFilters$ = this.getHasAvailableExtendedFilters();
  }

  private extendedVoucherFilters() {
    return this.selectAll({
      filterBy: (entity) => !this.hasActive(entity.propertyName),
    });
  }

  private getChangedFiltersWithValue() {
    return this.ui.selectAll({
      filterBy: (entity) => entity.changed === true,
    });
  }

  private getShouldActiveVoucherFilterIds() {
    return this.selectAll({
      filterBy: (entity) =>
        entity.positions === FilterPosition.primary ||
        (entity.value && entity.value.length > 0),
    }).pipe(map((filters) => filters.map((f) => f.propertyName)));
  }

  public getIsFilterChanged(id: string) {
    return this.ui.selectEntity(id, (e) => e.changed);
  }

  private getHasChangedFilters() {
    return this.ui
      .selectCount((x) => x.changed)
      .pipe(map((count) => count > 0));
  }

  private getHasAvailableExtendedFilters() {
    return this.extendedVoucherFilters$.pipe(
      map((filters) => filters.length > 0)
    );
  }
}
