import { Injectable } from '@angular/core';
import { Store, StoreConfig } from '@datorama/akita';

import { IUser } from './user.model';

@Injectable({ providedIn: 'root' })
@StoreConfig({ name: 'user' })
export class UserStore extends Store<IUser<number, number>> {
  constructor() {
    super({});
  }
}
