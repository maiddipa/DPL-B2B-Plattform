import { Injectable } from '@angular/core';
import { applyTransaction, filterNil } from '@datorama/akita';
import { AuthenticationService } from 'apps/dpl-live/src/app/core/services/authentication.service';
import { DplApiService } from 'apps/dpl-live/src/app/core/services/dpl-api.service';
import { EMPTY, Observable, of } from 'rxjs';
import { catchError, map, switchMap, tap } from 'rxjs/operators';

import { CountriesStore } from '../countries/state/countries.store';
import { CountryStatesStore } from '../countries/state/country-states.store';
import { DocumentStatesStore } from '../documents/state/document-states.store';
import { LanguagesStore } from '../languages/state/languages.store';
import { LoadCarrierQualitiesStore } from '../load-carriers/state/load-carrier-qualities.store';
import { LoadCarrierTypesStore } from '../load-carriers/state/load-carrier-types.store';
import { LoadCarriersStore } from '../load-carriers/state/load-carriers.store';
import { VoucherReasonTypesStore } from '../voucher-reason-types/state/voucher-reason-types.store';
import { normalizeMasterData } from './master-data.normalize';

@Injectable({
  providedIn: 'root',
})
export class MasterDataService {
  constructor(
    private auth: AuthenticationService,
    private dpl: DplApiService,
    private languagesStore: LanguagesStore,
    private countriesStore: CountriesStore,
    private countryStatesStore: CountryStatesStore,
    private documentStatesStore: DocumentStatesStore,
    private loadCarriersStore: LoadCarriersStore,
    private loadCarrierTypesStore: LoadCarrierTypesStore,
    private loadCarrierQualitiesStore: LoadCarrierQualitiesStore,
    private voucherReasonTypesStore: VoucherReasonTypesStore
  ) {}

  refreshMasterData(): Observable<void> {
    const request = this.auth.isLoggedIn().pipe(
      switchMap((isLoggedIn) => {
        if (!isLoggedIn) {
          return EMPTY;
        }
        return this.dpl.masterData.get().pipe(
          catchError((error) => {
            return of(null);
            //return throwError(error);
          })
        );
      }),
      filterNil,
      map((masterData) => normalizeMasterData(masterData)),
      tap((entities) => {
        applyTransaction(() => {
          this.countriesStore.set(entities.countries);
          this.countryStatesStore.set(entities.states);
          this.documentStatesStore.set(entities.documentStates);
          this.loadCarriersStore.set(entities.loadCarriers);
          this.loadCarrierTypesStore.set(entities.loadCarrierTypes);
          this.loadCarrierQualitiesStore.set(entities.loadCarrierQualities);
          this.voucherReasonTypesStore.set(entities.voucherReasonTypes);

          // this needs to remain the last store we update as we rely on theis stores loading flag
          this.languagesStore.set(entities.languages);
          this.languagesStore.setActive(this.languagesStore.getValue().ids[0]);
        });
      }),
      map((i) => null)
    );

    return request;
  }
}
