import { Injectable } from '@angular/core';
import { EntityState, EntityStore, StoreConfig } from '@datorama/akita';

import { IPartner } from './partner.model';

export interface PartnersState extends EntityState<IPartner, number> {
  active: {
    [context: string]: number | null;
  };
}

const initialState: PartnersState = {
  active: {
    prefill: null,
    supplier: null,
    recipient: null,
    shipper: null,
    subShipper: null,
  },
};

@Injectable({ providedIn: 'root' })
@StoreConfig({ name: 'partners' })
export class PartnersStore extends EntityStore<PartnersState> {
  constructor() {
    super(initialState);
  }

  setActiveByContext(context: string, id?: number) {
    const activeState = this.getValue().active;
    const updated = {};
    updated[context] = id;
    this.update({
      active: {
        ...activeState,
        ...updated,
      },
    });
  }
}
