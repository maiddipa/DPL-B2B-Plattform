import { LoadCarrier } from 'apps/dpl-live/src/app/core/services/dpl-api-services';
import { ILoadCarrierType } from './load-carrier-type.model';
import { ILoadCarrierQuality } from './load-carrier-quality.model';

export interface ILoadCarrier<
  TType extends number | ILoadCarrierType = ILoadCarrierType,
  TQuality extends number | ILoadCarrierQuality = ILoadCarrierQuality
> {
  id: number;
  type: TType;
  quality: TQuality;
}

/**
 * A factory function that creates LoadCarriers
 */
export function createLoadCarrier(params: Partial<ILoadCarrier>) {
  return {} as ILoadCarrier;
}
