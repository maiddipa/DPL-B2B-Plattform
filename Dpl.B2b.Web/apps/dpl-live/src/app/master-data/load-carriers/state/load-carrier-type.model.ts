import { ID } from '@datorama/akita';
import { LoadCarrierType } from 'apps/dpl-live/src/app/core/services/dpl-api-services';

export interface ILoadCarrierType extends LoadCarrierType {}

/**
 * A factory function that creates LoadCarrierType
 */
export function createLoadCarrierType(params: Partial<ILoadCarrierType>) {
  return {} as ILoadCarrierType;
}
