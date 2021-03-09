import { ID } from '@datorama/akita';
import { ICustomer } from 'apps/dpl-live/src/app/customers/state/customer.model';
import {
  User,
  UserRole,
} from 'apps/dpl-live/src/app/core/services/dpl-api-services';
import { IAccount } from 'apps/dpl-live/src/app/accounts/state/account.model';
import { FilterTemplate } from 'apps/dpl-live/src/app/filters/services/filter.service.types';

export type IUserSettings = {
  filterTemplates: {
    vouchers?: SettingsFilterTemplate;
    supplyOrders?: SettingsFilterTemplate;
    demandOrders?: SettingsFilterTemplate;
    orders?: SettingsFilterTemplate;
    accountingRecords?: SettingsFilterTemplate;
    transports?: SettingsFilterTemplate;
    journal?: SettingsFilterTemplate;
    livePoolingOrders?: SettingsFilterTemplate;
    customerAdminUsers?: SettingsFilterTemplate;
    customerAdminGroups?: SettingsFilterTemplate;
    customerAdminGroupMembership?:SettingsFilterTemplate;
  };
  viewSettings?: {
    ordersCompletedToggle?: boolean;
  };
};

export type SettingsFilterTemplate = {
  templates?: FilterTemplate[];
  activeTemplateId?: string;
};

export interface IUser<
  TCustomer extends number | ICustomer = ICustomer,
  TPostingAccount extends number | IAccount = IAccount
> {
  id: number;
  email: string;
  name: string;
  role: UserRole;
  customers: TCustomer[];
  postingAccounts: TPostingAccount[];
  permissions: User['permissions'];
  settings: IUserSettings;
}

/**
 * A factory function that creates User
 */
export function createUser(params: Partial<IUser>) {
  return {} as IUser;
}
