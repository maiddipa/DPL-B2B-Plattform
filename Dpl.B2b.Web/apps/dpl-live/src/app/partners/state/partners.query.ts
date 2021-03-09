import { Injectable } from '@angular/core';
import { QueryEntity } from '@datorama/akita';
import { PartnersStore, PartnersState } from './partners.store';
import { Observable, of } from 'rxjs';
import { IPartner } from './partner.model';
import { switchMap } from 'rxjs/operators';

@Injectable({ providedIn: 'root' })
export class PartnersQuery extends QueryEntity<PartnersState> {
  constructor(protected store: PartnersStore) {
    super(store);
  }

  public selectActiveByContext(context: string): Observable<IPartner> {
    return this.select((state) =>
      state.active ? state.active[context] : null
    ).pipe(
      switchMap((id?: number) =>
        id !== null ? this.selectEntity(id) : of(null)
      )
    );
  }
}
