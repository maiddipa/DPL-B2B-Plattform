import { Sort } from '@angular/material/sort';
import {
  ListSortDirection,
  DayOfWeek,
  VoucherType,
} from './services/dpl-api-services';
export function chunkArray<T>(array: Array<T>, batchSize: number) {
  const input = [...array];
  const results: Array<Array<T>> = [];

  while (input.length) {
    results.push(input.splice(0, batchSize));
  }

  return results;
}

const supportedLanguages = ['de', 'en', 'fr', 'pl'];
export function getLanguageFromLocale(locale: string) {
  if (!locale) {
    throw new Error('locale cannot be null when getting langauge');
  }

  const language = locale.split('-')[0];
  if (!supportedLanguages.find((i) => i == language)) {
    return 'de';
  }

  return language;
}

export function compareById<T extends { id: number }>(x: T, y: T) {
  if (x == y || (x != null && y != null && x.id === y.id)) {
    return true;
  }
  return false;
}

export interface DplApiSort<TSortOption> {
  sortBy: TSortOption;
  sortDirection: ListSortDirection;
}

export function convertToDplApiSort<TSortOption>(
  sort: Sort
): DplApiSort<TSortOption> {
  return {
    sortBy: (sort.active as unknown) as TSortOption,
    sortDirection:
      sort.direction === 'desc'
        ? ListSortDirection.Descending
        : ListSortDirection.Ascending,
  };
}

const dayOfWeekOrderDict: {
  [dayOfWeek: string]: number;
} = [
  DayOfWeek.Monday,
  DayOfWeek.Tuesday,
  DayOfWeek.Wednesday,
  DayOfWeek.Thursday,
  DayOfWeek.Friday,
  DayOfWeek.Saturday,
  DayOfWeek.Sunday,
].reduce((prev, dayOfWeek, index) => {
  prev[dayOfWeek] = index;
  return prev;
}, {});

export function getOffsetSinceStartOfWeek(dayOfWeek: DayOfWeek) {
  return dayOfWeekOrderDict[dayOfWeek];
}

export function voucherTypeToDocumentTypeId(voucherType: VoucherType): number {
  switch (voucherType) {
    case VoucherType.Direct:
      return 1;
    case VoucherType.Digital:
      return 2;
    case VoucherType.Original:
      return 3;
    default:
      throw new Error(`Undefined VoucherType:${voucherType}`);
  }
}

export function documentTypeIdToVoucherType(
  documentTypeId: number
): VoucherType {
  switch (documentTypeId) {
    case 1:
      return VoucherType.Direct;
    case 2:
      return VoucherType.Digital;
    case 3:
      return VoucherType.Original;
    default:
      throw new Error(`Undefined documentTypeId:${documentTypeId}`);
  }
}
