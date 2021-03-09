import { Injectable } from '@angular/core';
import { QueryEntity } from '@datorama/akita';
import {
  filter,
  map,
  publishReplay,
  refCount,
  switchMap,
} from 'rxjs/operators';

import { deNormalizeOrderProcess } from './order-process.normalize';
import {
  OrderProcessDetailsState,
  OrderProcessesStore,
} from './order-processes.store';

@Injectable({ providedIn: 'root' })
export class OrderProcessesQuery extends QueryEntity<OrderProcessDetailsState> {
  activeOrderProcesses$ = this.selectActiveOrderProcesses();

  constructor(protected store: OrderProcessesStore) {
    super(store);
  }

  private selectActiveOrderProcesses() {
    return this.selectLoading().pipe(
      filter((loading) => !loading),
      switchMap(() => {
        return this.selectActive();
      }),
      switchMap((rootProcesses) => {
        const ids = new Set(rootProcesses.map((i) => i.id));
        return this.selectAll({
          asObject: true,
          filterBy: (i) => ids.has(i.rootId),
        }).pipe(
          map((orderProcesses) =>
            deNormalizeOrderProcess(rootProcesses, {
              orderProcesses,
            })
          )
        );
      }),
      publishReplay(1),
      refCount()
    );
  }
}
