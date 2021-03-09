import { Component, OnInit, ChangeDetectionStrategy } from '@angular/core';
import { combineLatest, Observable } from 'rxjs';
import { filter, first, map, pluck, switchMap, tap } from 'rxjs/operators';
import { CustomerDivision } from '../../../core/services/dpl-api-services';
import { DplApiService } from '../../../core/services/dpl-api.service';
import { CustomerAdministrationService } from '../../services/customer-administration.service';
import {
  CustomerAdminOrganizationUnit,
  CustomerAdminScope,
} from '../../services/customer-administration.service.types';
import { UnitsQuery } from '../../state/units.query';

type ViewData = {
  unit: CustomerAdminOrganizationUnit;
  customerId: number;
};
@Component({
  selector: 'dpl-customer-administration-division-add',
  templateUrl: './customer-administration-division-add.component.html',
  styleUrls: ['./customer-administration-division-add.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class CustomerAdministrationDivisionAddComponent implements OnInit {
  viewData$: Observable<ViewData>;

  constructor(
    private dpl: DplApiService,
    private unitsQuery: UnitsQuery,
    private customerAdminService: CustomerAdministrationService
  ) {}

  ngOnInit(): void {
    const unit$ = this.unitsQuery.selectActive().pipe(
      filter((x) => !!x),
      first()
    );

    this.viewData$ = combineLatest([unit$]).pipe(
      map(([unit]) => {
        const viewData: ViewData = {
          unit,
          customerId: unit.parent,
        };
        return viewData;
      })
    );
  }

  onFormSubmit(formData: CustomerDivision, viewData: ViewData) {
    return this.dpl.divisionsAdministrationService
      .post({
        ...formData,
        customerId: viewData.customerId,
      })
      .pipe(
        switchMap((createdDivision) => {
          return this.customerAdminService.refreshUnits().pipe(
            tap(() => {
              this.customerAdminService.setActiveUnit(
                CustomerAdminScope.Division + createdDivision.id
              );
            })
          );
        })
      )
      .subscribe();
  }
}
