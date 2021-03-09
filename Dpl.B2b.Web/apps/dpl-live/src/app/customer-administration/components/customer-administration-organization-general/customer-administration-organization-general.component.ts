import { Component, OnInit, ChangeDetectionStrategy } from '@angular/core';
import { combineLatest, Observable, of } from 'rxjs';
import { filter, first, map, switchMap, tap } from 'rxjs/operators';
import { Organization } from '../../../core/services/dpl-api-services';
import { DplApiService } from '../../../core/services/dpl-api.service';
import { CustomerAdministrationService } from '../../services/customer-administration.service';
import { CustomerAdminOrganizationUnit } from '../../services/customer-administration.service.types';
import { UnitsQuery } from '../../state/units.query';
import { UnitsStore } from '../../state/units.store';

type ViewData = {
  unit: CustomerAdminOrganizationUnit;
  organization: Organization;
};

@Component({
  selector: 'dpl-customer-administration-organization-general',
  templateUrl: './customer-administration-organization-general.component.html',
  styleUrls: ['./customer-administration-organization-general.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class CustomerAdministrationOrganizationGeneralComponent
  implements OnInit {
  viewData$: Observable<ViewData>;

  constructor(
    private dpl: DplApiService,
    private unitsQuery: UnitsQuery,
    private unitsStore: UnitsStore,
    private customerAdministrationService: CustomerAdministrationService
  ) {}

  ngOnInit(): void {
    const unit$ = this.unitsQuery.selectActive().pipe(filter((x) => !!x));
    const organization$ = unit$.pipe(
      filter((unit) => unit?.parent! != undefined),
      switchMap((unit) => {
        return this.dpl.organizationsAdministrationService.getOrganizationById(
          unit.parent
        );
      })
    );

    this.viewData$ = combineLatest([unit$, organization$]).pipe(
      map(([unit, organization]) => {
        const viewData: ViewData = {
          unit,
          organization,
        };
        return viewData;
      })
    );
  }

  onFormSubmit(formData: Organization, viewData: ViewData) {
    console.log('Form submit', formData);

    this.unitsQuery
      .selectEntity((x) => x.idString === viewData.unit.parentIdString)
      .pipe(first())
      .pipe(
        switchMap((unit) => {
          return this.dpl.organizationsAdministrationService
            .patch({
              ...formData,
            })
            .pipe(
              tap((updateOrganization) => {
                this.unitsStore.updateUnitName(
                  unit.idString,
                  updateOrganization.name
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
                  return this.dpl.organizationsAdministrationService.delete(
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
