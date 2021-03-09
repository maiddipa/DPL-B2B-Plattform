import { Injectable } from '@angular/core';
import { QueryEntity } from '@datorama/akita';
import { TransportsStore, TransportsState } from './transports.store';

@Injectable({ providedIn: 'root' })
export class TransportsQuery extends QueryEntity<TransportsState> {
  constructor(protected store: TransportsStore) {
    super(store);
  }
}
