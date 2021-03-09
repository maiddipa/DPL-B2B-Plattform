import { normalize, schema } from 'normalizr';
import { MasterData } from 'apps/dpl-live/src/app/core/services/dpl-api-services';

import { ICountryState } from '../countries/state/country-state.model';
import { ICountry } from '../countries/state/country.model';
import { IDocumentState } from '../documents/state/document-state.model';
import { ILoadCarrierQuality } from '../load-carriers/state/load-carrier-quality.model';
import { ILoadCarrierType } from '../load-carriers/state/load-carrier-type.model';
import { ILoadCarrier } from '../load-carriers/state/load-carrier.model';
import { IVoucherReasonType } from '../voucher-reason-types/state/voucher-reason-type.model';
import { ILanguage } from '../languages/state/language.model';

const countryState = new schema.Entity('states');
const country = new schema.Entity('countries', {
  states: [countryState],
});

const documentState = new schema.Entity('documentStates');
const language = new schema.Entity('languages');

const loadCarrierType = new schema.Entity('loadCarrierTypes');
const loadCarrierQuality = new schema.Entity('loadCarrierQualities');
const loadCarrier = new schema.Entity('loadCarriers', {
  type: loadCarrierType,
  quality: loadCarrierQuality,
});

const voucherReasonType = new schema.Entity('voucherReasonTypes');

const masterData = new schema.Entity('masterData', {
  countries: [country],
  languages: [language],
  loadCarriers: [loadCarrier],
  documentStates: [documentState],
  voucherReasonTypes: [voucherReasonType],
});

export function normalizeMasterData(serverResponse: MasterData) {
  const normalized = normalize(serverResponse, masterData) as {
    entities: {
      languages: ILanguage[];
      countries: ICountry<number>[];
      states: ICountryState[];
      documentStates: IDocumentState[];
      loadCarriers: ILoadCarrier<number, number>[];
      loadCarrierTypes: ILoadCarrierType[];
      loadCarrierQualities: ILoadCarrierQuality[];
      voucherReasonTypes: IVoucherReasonType[];
    };
  };
  return normalized.entities;
}
