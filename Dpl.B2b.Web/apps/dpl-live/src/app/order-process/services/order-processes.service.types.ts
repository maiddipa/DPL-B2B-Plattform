import { DplApiService } from '@app/core';

export type OrderGroupsSearchRequest = Parameters<
  DplApiService['orderGroups']['search']
>[0];
