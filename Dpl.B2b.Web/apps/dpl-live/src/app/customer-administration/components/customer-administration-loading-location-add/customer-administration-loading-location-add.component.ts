import { Component, OnInit, ChangeDetectionStrategy } from '@angular/core';
import { combineLatest, Observable } from 'rxjs';
import { filter, map, switchMap, tap } from 'rxjs/operators';
import { LoadingLocation } from '../../../core/services/dpl-api-services';
import { DplApiService } from '../../../core/services/dpl-api.service';
import { CustomerAdministrationService } from '../../services/customer-administration.service';
import { CustomerAdminScope } from '../../services/customer-administration.service.types';
import { UnitsQuery } from '../../state/units.query';

type ViewData = {
  divisionId: number;
};

@Component({
  selector: 'dpl-customer-administration-loading-location-add',
  templateUrl: './customer-administration-loading-location-add.component.html',
  styleUrls: ['./customer-administration-loading-location-add.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class CustomerAdministrationLoadingLocationAddComponent
  implements OnInit {
  viewData$: Observable<ViewData>;

  constructor(
    private dpl: DplApiService,
    private unitsQuery: UnitsQuery,
    private customerAdminService: CustomerAdministrationService
  ) {}

  ngOnInit(): void {
    const unit$ = this.unitsQuery.selectActive().pipe(filter((x) => !!x));

    this.viewData$ = combineLatest([unit$]).pipe(
      map(([unit]) => {
        const viewData: ViewData = {
          divisionId: unit.parent,
        };
        return viewData;
      })
    );
  }

  onFormSubmit(formData:LoadingLocation, viewData:ViewData){
    return this.dpl.loadingLocationsAdministrationService
      .post({
        ...formData,
        customerDivisionId: viewData.divisionId,
      })
      .pipe(
        switchMap((createdLoadingLocation) => {
          return this.customerAdminService.refreshUnits().pipe(
            tap(() => {
              this.customerAdminService.setActiveUnit(
                CustomerAdminScope.LoadingLocation + createdLoadingLocation.id
              );
            })
          );
        })
      )
      .subscribe();
  }
}
