import { DplApiService } from '@app/core';
import { PostingAccountCondition } from 'apps/dpl-live/src/app/core/services/dpl-api-services';
import { ILoadCarrier } from 'apps/dpl-live/src/app/master-data/load-carriers/state/load-carrier.model';

import { IAccount } from '../state/account.model';
import { IBalance } from '../state/balance.model';

export interface ISelfServiceData {
  account: IAccount;
  balance: IBalance;
  loadCarriers: {
    all: ILoadCarrier[];
    pickup: PostingAccountCondition[];
    dropoff: PostingAccountCondition[];
  };
}

export interface IOrderOptions {
  supply: ILoadCarrierOrderOption[];
  demand: ILoadCarrierOrderOption[];
}

export interface ILoadCarrierOrderOption {
  loadCarrier: ILoadCarrier;
  accountInfos: ILoadCarrierAccountOrderOption[];
}

export interface ILoadCarrierAccountOrderOption {
  account: IAccount;
  min: number;
  max: number;
}

export type AccountingRecordsSearchRequest = Parameters<
  DplApiService['accountingRecords']['get']
>[0];

export interface ActiveAccountLoadCarrierType {
  accountId: number;
  loadCarrierTypeId: number;
}

export enum AccountOverviewType {
  Record = 'Record',
  Request = 'Request',
}
