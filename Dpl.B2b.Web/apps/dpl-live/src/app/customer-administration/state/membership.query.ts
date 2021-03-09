import { Injectable } from '@angular/core';
import { QueryEntity } from '@datorama/akita';
import { MembershipStore, MembershipState } from './membership.store';

@Injectable({ providedIn: 'root' })
export class MembershipQuery extends QueryEntity<MembershipState> {

  constructor(protected store: MembershipStore) {
    super(store);
  }

}
