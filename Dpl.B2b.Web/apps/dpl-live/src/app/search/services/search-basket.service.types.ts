import { OrderLoad } from '@app/api/dpl';
import { IBasket } from '@app/shared';

import { IDestination, ISearchResponse } from './search.service.types';

export type SearchBasket = IBasket<ISearchBasketData, OrderLoad[]>;
export type SearchBasketItem = SearchBasket['items'][0];

export interface ISearchBasketData {
  dayOfWeek: number;
  orderGroupGuid: string;
  baseLoadCarrierId?: number;
  destination: IDestination;
  response: ISearchResponse;
}
