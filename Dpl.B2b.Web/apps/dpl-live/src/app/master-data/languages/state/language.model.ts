import { Language } from 'apps/dpl-live/src/app/core/services/dpl-api-services';

export interface ILanguage extends Language {}

export function createLanguage(params: Partial<ILanguage>) {
  return {} as ILanguage;
}
