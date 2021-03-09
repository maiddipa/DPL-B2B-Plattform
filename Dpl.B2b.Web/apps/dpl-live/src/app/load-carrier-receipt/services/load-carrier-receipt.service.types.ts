import {
  DriverInfo,
  LoadCarrierQuantity,
  Shipper,
  PrintSettings,
} from '@app/shared';
import {LoadCarrierReceiptDepotPreset, LoadCarrierReceiptType} from "@app/api/dpl";

export interface LoadCarrierReceiptFormState {
  loadCarrierReceipt: LoadCarrierReceipt;
}

export interface LoadCarrierReceipt {
  type: LoadCarrierReceiptType;
  generated: boolean;
  depoPresetId?: number;
  sourcePostingAccountId: number;
  targetPostingAccountId: number;
  digitalCode: string;
  date: Date;
  deliveryNoteNumber: string;
  deliveryNoteDocument: boolean;
  pickupNoteNumber: string;
  pickupNoteDocument: boolean;
  referenceNumber: string;
  instructions: string;
  note: string;
  loadCarriers: LoadCarrierQuantity[];
  driver: DriverInfo;
  shipper: Shipper;
  print: PrintSettings;
  isSortingRequired: boolean
}

export type ReceiptPositionPositionType = 'pickup' | 'delivery';
