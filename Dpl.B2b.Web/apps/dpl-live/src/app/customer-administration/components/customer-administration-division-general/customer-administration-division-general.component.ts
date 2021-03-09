import { Component, OnInit, ChangeDetectionStrategy } from '@angular/core';
import { combineLatest, forkJoin, Observable, of } from 'rxjs';
import { filter, first, map, switchMap, tap } from 'rxjs/operators';
import { DplApiService } from '../../../core';
import {
  CustomerDivision,
  LoadingLocationAdministration,
  PostingAccount,
} from '../../../core/services/dpl-api-services';
import { CustomerAdministrationService } from '../../services/customer-administration.service';
import { CustomerAdminOrganizationUnit } from '../../services/customer-administration.service.types';
import { UnitsQuery } from '../../state/units.query';
import { UnitsStore } from '../../state/units.store';

type ViewData = {
  unit: CustomerAdminOrganizationUnit;
  division: CustomerDivision;
  customerId: number;
};

@Component({
  selector: 'dpl-customer-administration-division-general',
  templateUrl: './customer-administration-division-general.component.html',
  styleUrls: ['./customer-administration-division-general.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class CustomerAdministrationDivisionGeneralComponent implements OnInit {
  viewData$: Observable<ViewData>;

  constructor(
    private dpl: DplApiService,
    private unitsQuery: UnitsQuery,
    private unitsStore: UnitsStore,
    private customerAdministrationService: CustomerAdministrationService
  ) {}

  ngOnInit(): void {
    const unit$ = this.unitsQuery.selectActive().pipe(filter((x) => !!x),first());
    const division$ = unit$.pipe(
      switchMap((unit) => {
        return this.dpl.divisionsAdministrationService.getById(unit.parent);
      })
    );

    const customerId$ = unit$.pipe(
      switchMap((unit) => {
        // get customer id for active division
        return this.unitsQuery.selectAll().pipe(
          map((units) => {
            return units.find((x) => x.idString === unit.parentIdString).parent;
          })          
        );
      })
    );

    this.viewData$ = combineLatest([unit$, division$, customerId$]).pipe(
      map(([unit, division, customerId]) => {
        const viewData: ViewData = {
          unit,
          division,
          customerId,
        };
        return viewData;
      })
    );
  }

  onFormSubmit(formData: CustomerDivision, viewData: ViewData) {
    return this.dpl.divisionsAdministrationService
      .patch({
        ...formData,
        customerId: viewData.customerId,
      })
      .pipe(
        tap((updatedDivision) => {
          this.unitsStore.updateUnitName(
            viewData.unit.parentIdString,
            updatedDivision.name
          );
        })
      ).subscribe();
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
                  return this.dpl.divisionsAdministrationService.delete(
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
