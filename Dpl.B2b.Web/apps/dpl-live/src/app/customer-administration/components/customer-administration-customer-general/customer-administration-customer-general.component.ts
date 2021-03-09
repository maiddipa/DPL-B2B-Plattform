import { Inject } from '@angular/core';
import {
  Component,
  OnInit,
  ChangeDetectionStrategy,
  Optional,
} from '@angular/core';
import { combineLatest, Observable, of } from 'rxjs';
import { filter, first, map, switchMap, tap } from 'rxjs/operators';
import { AuthenticationService, DplApiService } from '../../../core';

import {
  Address,
  API_BASE_URL,
  Customer,
} from '../../../core/services/dpl-api-services';
import { CustomerAdministrationService } from '../../services/customer-administration.service';
import { CustomerAdminOrganizationUnit } from '../../services/customer-administration.service.types';
import { UnitsQuery } from '../../state/units.query';
import { UnitsStore } from '../../state/units.store';
import * as AspNetData from 'devextreme-aspnet-data-nojquery';
import CustomStore from 'devextreme/data/custom_store';

type ViewData = {
  unit: CustomerAdminOrganizationUnit;
  customer: Customer;
};

@Component({
  selector: 'dpl-customer-administration-customer-general',
  templateUrl: './customer-administration-customer-general.component.html',
  styleUrls: ['./customer-administration-customer-general.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class CustomerAdministrationCustomerGeneralComponent implements OnInit {
  viewData$: Observable<ViewData>;
  baseUrl: string;

  constructor(
    private dpl: DplApiService,
    private unitsQuery: UnitsQuery,
    private unitsStore: UnitsStore,
    private customerAdministrationService: CustomerAdministrationService,
    private authenticationService: AuthenticationService,
    @Optional() @Inject(API_BASE_URL) baseUrl?: string
  ) {
    this.baseUrl = baseUrl;
  }

  ngOnInit(): void {
    const unit$ = this.unitsQuery.selectActive().pipe(filter((x) => !!x));
    const customer$ = unit$.pipe(
      switchMap((unit) => {
        return this.dpl.customersAdministrationService.getById(unit.parent);
      })
    );
    this.viewData$ = combineLatest([unit$, customer$]).pipe(
      map(([unit, customer]) => {
        const viewData: ViewData = {
          unit,
          customer,
        };
        return viewData;
      })
    );
  }

  onFormSubmit(formData: Customer, viewData: ViewData) {
    this.unitsQuery.selectAll({
      filterBy: (x) => x.idString === viewData.unit.parentIdString,
    });
    this.unitsQuery
      .selectEntity((x) => x.idString === viewData.unit.parentIdString)
      .pipe(first())
      .pipe(
        switchMap((unit) => {
          return this.dpl.customersAdministrationService
            .patch({
              ...formData,
            })
            .pipe(
              tap((updateCustomer) => {
                this.unitsStore.updateUnitName(
                  unit.idString,
                  updateCustomer.name
                );
              })
            );
        })
      )
      .subscribe();
  }

  delete() {
    this.customerAdministrationService
      .getDeleteConfirmationDialog()
      .pipe(
        switchMap((dialogResult) => {
          if (dialogResult?.confirmed) {
            return this.unitsQuery
              .selectActive()
              .pipe(first())
              .pipe(
                switchMap((unit) => {
                  return this.dpl.customersAdministrationService.delete(
                    unit.parent
                  );
                })
              );
          }
          return of(null);
        })
      )
      .subscribe();
  }
}
