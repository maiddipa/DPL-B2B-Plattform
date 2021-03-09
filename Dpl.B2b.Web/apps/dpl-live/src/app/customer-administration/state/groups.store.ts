import { Injectable } from '@angular/core';
import {
  EntityState,
  EntityStore,
  StoreConfig,
  ActiveState,
} from '@datorama/akita';


export interface GroupsState
  extends EntityState<any>,
    ActiveState {}

@Injectable({ providedIn: 'root' })
@StoreConfig({ name: 'customer-administration-groups' })
export class GroupsStore extends EntityStore<GroupsState> {
  constructor() {
    super();
  }
}
