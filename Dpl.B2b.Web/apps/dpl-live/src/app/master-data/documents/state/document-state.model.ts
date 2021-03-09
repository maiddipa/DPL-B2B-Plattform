import { DocumentState } from 'apps/dpl-live/src/app/core/services/dpl-api-services';

export interface IDocumentState extends DocumentState {}

/**
 * A factory function that creates DocumentStates
 */
export function createDocumentState(params: Partial<IDocumentState>) {
  return {} as IDocumentState;
}
