import { ID } from '@datorama/akita';
import { ICustomerDivision } from './customer-division.model';
import {CustomDocumentLabel, Customer, CustomerDocumentSettings} from 'apps/dpl-live/src/app/core/services/dpl-api-services';

export interface ICustomer<
  TDivision extends number | ICustomerDivision = ICustomerDivision
  > extends Omit<Customer, 'divisions'> {
  divisions: TDivision[];
}

/**
 * A factory function that creates Customers
 */
export function createCustomer(params: Partial<ICustomer>) {
  return {} as ICustomer;
}
