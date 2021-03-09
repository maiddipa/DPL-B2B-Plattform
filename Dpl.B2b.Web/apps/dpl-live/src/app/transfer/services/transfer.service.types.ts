import { LoadCarrierQuantity } from '@app/shared';

export interface TransferForm {
  code: string;
  sourceAccountId: number;
  targetAccountId: number;
  loadCarrier: LoadCarrierQuantity;
  message: string;
}
