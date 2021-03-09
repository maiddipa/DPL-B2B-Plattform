import { Component, OnInit, ChangeDetectionStrategy } from '@angular/core';
import CustomStore from 'devextreme/data/custom_store';
import { Customer } from '../../../core/services/dpl-api-services';
import { CustomerAdministrationService } from '../../services/customer-administration.service';
import {
  CustomerAdminOrganizationUnit,
  CustomerAdminScope,
} from '../../services/customer-administration.service.types';
import { UnitsQuery } from '../../state/units.query';
import { UnitsStore } from '../../state/units.store';
import { DplApiService } from '../../../core';
import { filter, first, map, pluck, switchMap, tap } from 'rxjs/operators';
import { combineLatest, Observable } from 'rxjs';

type ViewData = {
  unit: CustomerAdminOrganizationUnit;
};

@Component({
  selector: 'dpl-customer-administration-customer-add',
  templateUrl: './customer-administration-customer-add.component.html',
  styleUrls: ['./customer-administration-customer-add.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class CustomerAdministrationCustomerAddComponent implements OnInit {
  viewData$: Observable<ViewData>;
  constructor(
    private unitsQuery: UnitsQuery,
    private unitsStore: UnitsStore,
    private customerAdminService: CustomerAdministrationService,
    private dpl: DplApiService
  ) {}

  ngOnInit(): void {
    const unit$ = this.unitsQuery.selectActive().pipe(filter((x) => !!x));
    this.viewData$ = combineLatest([unit$]).pipe(
      map(([unit]) => {
        const viewData: ViewData = {
          unit,
        };
        return viewData;
      })
    );
  }

  onFormSubmit(formData: Customer, viewData: ViewData) {
    // organisationid !!!

    this.unitsQuery
      .selectEntity((x) => x.idString === viewData.unit.parentIdString)
      .pipe(
        first(),
        pluck('id'),
        switchMap((organizationId) => {
          return (
            this.dpl.customersAdministrationService
              .post({
                ...formData,
                organizationId: organizationId,
              })
              .pipe(
                switchMap((updateCustomer) => {
                  // reload tree                  
                  return this.customerAdminService.refreshUnits().pipe(
                    tap(() => {
                      this.customerAdminService.setActiveUnit(
                        CustomerAdminScope.Customer + updateCustomer.id
                      );
                    })
                  );
                })
              )
          );
        })
      )
      .subscribe();
  }
}
