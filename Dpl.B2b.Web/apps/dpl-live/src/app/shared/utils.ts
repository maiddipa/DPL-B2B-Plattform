import { DplApiSort } from '../core/utils';
import { Sort } from '@angular/material/sort';
import { ListSortDirection } from '@app/api/dpl';
import { NormalizedSchema } from 'normalizr';
import * as _ from 'lodash';
import { Moment } from 'moment';

export const EMPTY_PAGINATION_RESULT = {
  perPage: 0,
  lastPage: 0,
  currentPage: 0,
  total: 0,
  data: [],
};

export function getMatSort<TSortOptions>(apiSort: DplApiSort<TSortOptions>) {
  const sort: Sort = {
    active: (apiSort.sortBy as unknown) as string,
    direction:
      apiSort.sortDirection === ListSortDirection.Descending ? 'desc' : 'asc',
  };
  return sort;
}

export function getEntities<TEntities extends { [key: string]: any[] }>(
  normalizedSchema: NormalizedSchema<{}, any>
) {
  const entities = _(normalizedSchema.entities)
    .mapValues((values) => _(values).values().value())
    .value();
  return entities as TEntities;
}

export function parseMomentToUtcDate(inDate: Moment) {
  const tzOffset = inDate.utcOffset();
  return inDate.clone().utc().add(tzOffset, 'm').toDate();
}
