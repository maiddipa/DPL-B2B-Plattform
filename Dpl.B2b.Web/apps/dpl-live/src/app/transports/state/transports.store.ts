import { Injectable } from '@angular/core';
import { ITransport } from './transport.model';
import {
  EntityState,
  ActiveState,
  EntityStore,
  StoreConfig,
} from '@datorama/akita';

export interface TransportsState
  extends EntityState<ITransport, number>,
    ActiveState {}

@Injectable({ providedIn: 'root' })
@StoreConfig({ name: 'transports' })
export class TransportsStore extends EntityStore<TransportsState> {
  constructor() {
    super();
  }
}
