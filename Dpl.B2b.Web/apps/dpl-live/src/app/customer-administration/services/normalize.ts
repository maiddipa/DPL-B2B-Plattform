import { HashMap } from '@datorama/akita';
import { denormalize, normalize, schema } from 'normalizr';
import { CustomerAdminOrganizationUnitHierarchy } from './customer-administration.service.types';

const units = new schema.Entity('units');
units.define({
  children: [units],
});

export function deNormalizeUnits<TRoot extends CustomerAdminOrganizationUnitHierarchy>(
  root: TRoot[],
  data: { units: HashMap<TRoot> }
) {
  const deNormalized = denormalize(
    root.map((i) => i.id),
    [units],
    data
  ) as TRoot[];
  return deNormalized;
}
