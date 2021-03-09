import { Injectable } from '@angular/core';
import { CustomersQuery } from '../state/customers.query';
import { map, switchMap } from 'rxjs/operators';

@Injectable({
  providedIn: 'root',
})
export class CustomersService {
  constructor(private customersQuery: CustomersQuery) {}

  getCustomers() {
    return this.customersQuery.customers$;
  }

  getActiveCustomer() {
    return this.customersQuery.activeCustomers$;
  }

  getCustomDocumentLabel(labelKey: string) {
    return this.getActiveCustomer().pipe(
      map((customer) => {
        if (customer.customDocumentLabels?.length > 0) {
          for (const cdl of customer.customDocumentLabels) {
            if (cdl.uiLabel === labelKey) {
              return cdl.text;
            }
          }
        }
        switch (labelKey) {
          case 'CustomerReference':
            return 'Referenznummer (Aussteller)';
          case 'VoProLog':
            return 'Beschaffungslogistik';
          default:
            return labelKey;
        }
      })
    );
  }
}
