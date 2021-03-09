import {
  OrderGroup,
  Order,
  OrderLoad,
  LoadCarrierReceipt,
  AccountingRecord,
} from '@app/api/dpl';

export interface TreeData<Type, TData, TChildren> {
  rootId: string;
  id: string;
  type: Type;
  data: TData;
  children: TChildren[];
  level: number;
}

export interface OrderProcess
  extends TreeData<
    | 'orderGroup'
    | 'order'
    | 'orderLoad'
    | 'loadCarrierReceipt'
    | 'accountingRecord',
    OrderGroup | Order | OrderLoad | LoadCarrierReceipt | AccountingRecord,
    OrderProcess
  > {
  date: Date | undefined;
  referenceNumber: string;
  description: string;
  showExtendedDetails: boolean;
}
