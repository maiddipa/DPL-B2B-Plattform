import { PostingAccount } from '@app/api/dpl';

import { IBalance } from './balance.model';

export interface IAccount<TBalance extends string | IBalance = IBalance>
  extends PostingAccount {
  loadCarrierTypeBalances: TBalance[];
  loadCarrierTypeIds: number[];
}
