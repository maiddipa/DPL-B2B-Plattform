import { Injectable } from '@angular/core';
import {
  Address,
  CustomerPartner,
  CustomerPartnersCreateRequest,
  PartnerType
} from '@app/api/dpl';
import { DplApiService } from '@app/core';
import { applyTransaction } from '@datorama/akita';
import { combineLatest, EMPTY, Observable } from 'rxjs';
import { map, switchMap, tap } from 'rxjs/operators';

import { PartnerDirectoriesQuery } from '../state/partner-directories.query';
import { PartnerDirectoriesStore } from '../state/partner-directories.store';
import { IPartner } from '../state/partner.model';
import { PartnersQuery } from '../state/partners.query';
import { PartnersStore } from '../state/partners.store';
import {
  PartnerPickerContext,
  PartnersSearchRequest,
} from './partner.service.types';

@Injectable({
  providedIn: 'root',
})
export class PartnerService {
  constructor(
    private dpl: DplApiService,
    private partnerDiretoriesStore: PartnerDirectoriesStore,
    private partnerDiretoriesQuery: PartnerDirectoriesQuery,
    private partnersStore: PartnersStore,
    private partnersQuery: PartnersQuery
  ) {}

  private fixAddress(partner: CustomerPartner) {
    // assign empty address if there is no address
    if (partner.address === undefined) {
      Object.assign(partner, { address: {} });
    }

    // assigne missing address fields
    if (partner.address && typeof partner.address === 'object') {
      const akkumulator = {
        id: null,
        city: null,
        country: null,
        postalCode: null,
        state: null,
        street1: null,
        street2: null,
      } as Address;

      Object.assign(akkumulator, partner.address);
      Object.assign(partner.address, akkumulator);
    }

    return partner;
  }

  getPartnerDirectories() {
    // HACK for demo that gets all partners at once and compiles partner directory
    const request = this.dpl.partnerDirectories.get().pipe(
      tap((data) => {
        // Store data
        this.partnerDiretoriesStore.set(data);
      }),
      switchMap(() => this.partnerDiretoriesQuery.partnerDirectories$)
    );

    return this.partnerDiretoriesQuery.getHasCache()
      ? this.partnerDiretoriesQuery.partnerDirectories$
      : request;
  }

  searchPartners(search: Partial<PartnersSearchRequest>) {
    const partnerDirectories$ = this.getPartnerDirectories().pipe(
      switchMap(() => this.partnerDiretoriesQuery.selectAll({ asObject: true }))
    );
    const partnerResponse$ = this.dpl.partners.get(search);
    return combineLatest([partnerResponse$, partnerDirectories$]).pipe(
      map(([response, directories]) => {
        return {
          ...response,
          data: response.data.map((partner) => {
            const partnerWithDirectoryNames: IPartner = {
              ...this.fixAddress(partner),
              directoryNames: partner.directoryIds.map(
                (id) => directories[id].name
              ),
            };

            return partnerWithDirectoryNames;
          }),
        };
      }),
      tap(partners => {
        this.partnersStore.upsertMany(partners.data || []);
      })
    );
  }

  setActivePartnerDirectory(
    context: PartnerPickerContext,
    directoryId?: number
  ) {
    this.partnerDiretoriesStore.setActiveByContext(context, directoryId);
    this.partnersStore.setActiveByContext(context, null);
  }

  getActivePartnerDirectory(context: PartnerPickerContext) {
    return this.partnerDiretoriesQuery
      .selectHasCache()
      .pipe(
        switchMap((hasCache) =>
          hasCache
            ? this.partnerDiretoriesQuery.selectActiveByContext(context)
            : EMPTY
        )
      );
  }

  setActivePartner(context: PartnerPickerContext, partnerId?: number) {
    this.partnersStore.setActiveByContext(context, partnerId);
  }

  getActivePartner(context: PartnerPickerContext) {
    return this.partnersQuery.selectActiveByContext(context);
  }

  createPartner(data: {
    directoryId: number;
    type: PartnerType;
    partner: CustomerPartner;
  }): Observable<CustomerPartner> {
    const partnerCreateRequest: CustomerPartnersCreateRequest = {
      companyName: data.partner.companyName,
      address: data.partner.address,
      type: data.type,
      directoryId: data.directoryId,
    };

    return this.dpl.partners.post(partnerCreateRequest);
  }
}
