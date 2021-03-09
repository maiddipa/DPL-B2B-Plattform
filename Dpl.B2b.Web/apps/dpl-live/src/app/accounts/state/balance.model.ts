import {
  LoadCarrierQualityType,
  Balance,
} from 'apps/dpl-live/src/app/core/services/dpl-api-services';

export interface IBalance extends Omit<Balance, 'loadCarrierId'> {
  generatedId: string;
  loadCarrierTypeId?: number;
  qualityType: LoadCarrierQualityType;
}
