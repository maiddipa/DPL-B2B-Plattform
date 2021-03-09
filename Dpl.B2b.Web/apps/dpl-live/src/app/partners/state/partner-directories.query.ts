import { Injectable } from '@angular/core';
import { QueryEntity, combineQueries } from '@datorama/akita';
import {
  PartnerDirectoriesStore,
  PartnerDirectoriesState,
} from './partner-directories.store';
import { PartnersQuery } from './partners.query';
import { map, publishReplay, refCount, switchMap } from 'rxjs/operators';
import { IPartnerDirectory } from './partner-directory.model';
import { of, Observable } from 'rxjs';
import { PartnerPickerContext } from '../services/partner.service.types';

@Injectable({ providedIn: 'root' })
export class PartnerDirectoriesQuery extends QueryEntity<
  PartnerDirectoriesState
> {
  partnerDirectories$ = this.selectPartnerDirectories();
  activePartnerDirectory$ = this.selectActivePartnerDirectory();

  constructor(
    protected store: PartnerDirectoriesStore,
    private partnersQuery: PartnersQuery
  ) {
    super(store);
  }

  public selectActiveByContext(
    context: string
  ): Observable<IPartnerDirectory> {
    return this.select((state) =>
      state.active ? state.active[context] : null
    ).pipe(
      switchMap((id?: number) =>
        id !== null ? this.selectEntity(id) : of(null)
      )
    );
  }

  private selectPartnerDirectories() {
    return this.selectAll().pipe(
      publishReplay(1),
      refCount()
    );
  }

  private selectActivePartnerDirectory() {
    return this.selectActive().pipe(
      publishReplay(1),
      refCount()
    );
  }
}
