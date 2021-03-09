import { Injectable } from '@angular/core';
import { EntityState, EntityStore, StoreConfig, ActiveState } from '@datorama/akita';


export interface MembershipState extends EntityState<any>, ActiveState {}

@Injectable({ providedIn: 'root' })
@StoreConfig({ name: 'customer-administration-membership' })
export class MembershipStore extends EntityStore<MembershipState> {

  constructor() {
    super();
  }

}
