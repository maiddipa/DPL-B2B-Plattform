import {
  Order,
  OrderLoad,
  TransportOffering,
  Voucher,
  AccountingRecord,
} from '@app/api/dpl';

export interface ChannelTypeOption {
  label: string;
  labelShort: string;
  value: ChannelType | '';
}

export interface LanguageTypeOption {
  label: string;
  labelShort: string;
  value: string;
}

export type ChannelType =
  | 'accounting-record'
  | 'voucher'
  | 'demand'
  | 'supply'
  | 'transport'
  | 'general'
  | 'order-load'
  | 'load-carrier-receipt';

export type ChannelInfoData =
  | AccountingRecord
  | Order
  | OrderLoad
  | TransportOffering
  | Voucher
  | {}
  | undefined;

export type ChannelInfoDataType =
  | 'accounting-record'
  | 'order'
  | 'orderLoad'
  | 'loadCarrierReceipt'
  | 'transport'
  | 'voucher'
  | 'general';

export interface ChatConversationInfo {
  channelId: string;
  referenceId: number | string;
  name: string;
  type: ChannelType;
  data?: ChannelInfoData;
}

export interface ChatConversationCreateInfo extends ChatConversationInfo {
  userIds: string[];
}

export interface ChatCache {
  [channelId: string]: boolean;
}
