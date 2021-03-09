import { getEntities } from '@app/shared';
import { HashMap } from '@datorama/akita';
import { denormalize, normalize, schema } from 'normalizr';

import { OrderProcess } from './order-process.model';

const orderProcess = new schema.Entity('orderProcesses');
orderProcess.define({
  children: [orderProcess],
});

type OrderProcessEntities = {
  orderProcesses: OrderProcess[];
};

export function normalizeOrderProcesses(data: OrderProcess[]) {
  const normalized = normalize(data, [orderProcess]);
  return getEntities<OrderProcessEntities>(normalized);
}

export function deNormalizeOrderProcess<TRoot extends OrderProcess>(
  root: TRoot[],
  data: { orderProcesses: HashMap<TRoot> }
) {
  const deNormalized = denormalize(
    root.map((i) => i.id),
    [orderProcess],
    data
  ) as TRoot[];
  return deNormalized;
}
