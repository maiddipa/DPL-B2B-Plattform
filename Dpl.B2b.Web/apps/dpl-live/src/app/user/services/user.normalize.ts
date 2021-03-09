import { normalize, schema } from 'normalizr';
import { IAccount } from 'apps/dpl-live/src/app/accounts/state/account.model';
import { IBalance } from 'apps/dpl-live/src/app/accounts/state/balance.model';
import {
  Address,
  User,
} from 'apps/dpl-live/src/app/core/services/dpl-api-services';
import { ICustomerDivision } from 'apps/dpl-live/src/app/customers/state/customer-division.model';
import { ICustomer } from 'apps/dpl-live/src/app/customers/state/customer.model';
import { ILoadingLocation } from 'apps/dpl-live/src/app/loading-locations/state/loading-location.model';
import { IUser } from 'apps/dpl-live/src/app/user/state/user.model';

const address = new schema.Entity('addresses');
const loadingLocation = new schema.Entity('loadingLocations', {
  address: address,
});

const division = new schema.Entity('divisions', {
  loadingLocations: [loadingLocation],
});

const customer = new schema.Entity('customers', {
  divisions: [division],
  loadingLocations: [loadingLocation],
});

// const balance = new schema.Entity("balances");
// const postingAccount = new schema.Entity("postingAccounts", {
//   balances: [balance],
//   address: address,
// });

const user = new schema.Entity('user', {
  customers: [customer],
  //postingAccounts: [postingAccount],
});

export function normalizeUserData(serverResponse: User) {
  const normalized = normalize(serverResponse, user) as {
    entities: {
      user: IUser<number, number>[];
      customers: ICustomer<number>[];
      divisions: ICustomerDivision<number>[];
      // tehse two need to be processed specially because of weird balance handling by load carrier type + intakt/defekt
      // postingAccounts: IAccount<string>[];
      // balances: IBalance[];
      loadingLocations: ILoadingLocation<number>[];
      addresses: Address[];
    };
  };
  return normalized.entities;
}
