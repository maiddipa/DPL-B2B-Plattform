import { ID } from '@datorama/akita';
import {
  LoadCarrier,
  LoadCarrierQuality,
} from 'apps/dpl-live/src/app/core/services/dpl-api-services';

export interface ILoadCarrierQuality extends LoadCarrierQuality {}

/**
 * A factory function that creates LoadCarrierQuality
 */
export function createLoadCarrierQuality(params: Partial<ILoadCarrierQuality>) {
  return {} as ILoadCarrierQuality;
}
