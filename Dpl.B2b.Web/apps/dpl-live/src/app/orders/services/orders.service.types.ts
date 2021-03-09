import {
  IPaginationResultOfOrder,
  Order,
  OrderLoadStatus,
  OrderStatus,
} from '@app/api/dpl';
import { DplApiService } from '@app/core';

export type OrderSearchRequest = Parameters<
  DplApiService['orders']['search']
>[0];

export interface OrderData extends Omit<Order, 'status'> {
  baseLoadCarrierQuantity: number;

  //NOTE: Falls singuläre Spalten Darstellung verwendet werden soll. Kann eventuell wieder entfernt werden
  fulfillmentDateTime: string;
  matchType: OrderMatchType;
  status: OrderLoadStatus | OrderStatus;
  isLoad: boolean;
}

export type OrderMatchType = 'none' | 'partial' | 'full';

export type PaginationResultOfOrderData = Omit<
  IPaginationResultOfOrder,
  'data'
> & { data: OrderData[] };
