import { Injectable } from '@angular/core';
import {
  ActiveState,
  EntityState,
  EntityStore,
  StoreConfig,
} from '@datorama/akita';

export interface UsersState
  extends EntityState<any>,
    ActiveState {}

@Injectable({ providedIn: 'root' })
@StoreConfig({ name: 'customer-administration-users' })
export class UsersStore extends EntityStore<UsersState> {
  constructor() {
    super();
    
  }
}
