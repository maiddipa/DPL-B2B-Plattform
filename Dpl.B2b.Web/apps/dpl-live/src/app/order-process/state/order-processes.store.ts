import { Injectable } from '@angular/core';
import {
  EntityState,
  EntityStore,
  MultiActiveState,
  StoreConfig,
} from '@datorama/akita';

import { OrderProcess } from './order-process.model';

export interface OrderProcessDetailsState
  extends EntityState<OrderProcess, string>,
    MultiActiveState<string> {}

@Injectable({ providedIn: 'root' })
@StoreConfig({ name: 'order-processes' })
export class OrderProcessesStore extends EntityStore<OrderProcessDetailsState> {
  constructor() {
    super();
  }

  public generateId(type: OrderProcess['type'], id: number) {
    return `${type}|${id}`;
  }

  public updateOrderProcess(
    type: OrderProcess['type'],
    data: OrderProcess['data']
  ) {
    const id = this.generateId(type, data.id);
    this.update(id, {
      data,
    });
  }
}
