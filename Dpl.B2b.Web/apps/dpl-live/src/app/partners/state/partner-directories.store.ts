import { Injectable } from '@angular/core';
import {
  IPartnerDirectory,
  IPartnerDirectoryUI,
} from './partner-directory.model';
import {
  EntityState,
  EntityStore,
  StoreConfig,
  ActiveState,
  EntityUIStore,
} from '@datorama/akita';

export interface PartnerDirectoriesState
  extends EntityState<IPartnerDirectory, number> {
  active: {
    [context: string]: number | null;
  };
}
export interface PartnerDirectoriesUIState
  extends EntityState<IPartnerDirectoryUI> {}

const initialState: PartnerDirectoriesState = {
  active: {
    prefill: null,
    supplier: null,
    recipient: null,
    shipper: null,
    subShipper: null,
  },
};

@Injectable({ providedIn: 'root' })
@StoreConfig({ name: 'partner-directories' })
export class PartnerDirectoriesStore extends EntityStore<
  PartnerDirectoriesState
> {
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
