import {
  Component,
  OnInit,
  ChangeDetectionStrategy,
  Optional,
  Inject,
} from '@angular/core';
import CustomStore from 'devextreme/data/custom_store';
import { combineLatest, Observable, of } from 'rxjs';
import { filter, first, map, switchMap, tap } from 'rxjs/operators';
import { AuthenticationService } from '../../../core/services/authentication.service';
import {
  Address,
  API_BASE_URL,
  LoadingLocation,
  Partner,
} from '../../../core/services/dpl-api-services';
import { DplApiService } from '../../../core/services/dpl-api.service';
import { CustomerAdministrationService } from '../../services/customer-administration.service';
import { CustomerAdminOrganizationUnit } from '../../services/customer-administration.service.types';
import { UnitsQuery } from '../../state/units.query';
import { UnitsStore } from '../../state/units.store';
import * as AspNetData from 'devextreme-aspnet-data-nojquery';

type ViewData = {
  unit: CustomerAdminOrganizationUnit;
  loadingLocation: LoadingLocation;
};

@Component({
  selector: 'dpl-customer-administration-loading-location-general',
  templateUrl:
    './customer-administration-loading-location-general.component.html',
  styleUrls: [
    './customer-administration-loading-location-general.component.scss',
  ],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class CustomerAdministrationLoadingLocationGeneralComponent
  implements OnInit {
  viewData$: Observable<ViewData>;
  formData: LoadingLocation;
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
    const loadingLocation$ = unit$.pipe(
      switchMap((unit) => {
        return this.dpl.loadingLocationsAdministrationService.getById(
          unit.parent
        );
      })
    );

    this.viewData$ = combineLatest([unit$, loadingLocation$]).pipe(
      map(([unit, loadingLocation]) => {
        const viewData: ViewData = {
          unit,
          loadingLocation,
        };
        return viewData;
      }),
      tap((viewData) => {
        console.log(viewData);
        this.formData = viewData.loadingLocation;
      })
    );
  }

  onFormSubmit(e, viewData: ViewData) {    console.log('Form submit', this.formData);
    this.unitsQuery.selectAll({
      filterBy: (x) => x.idString === viewData.unit.parentIdString,
    });
    this.unitsQuery
      .selectEntity((x) => x.idString === viewData.unit.parentIdString)
      .pipe(first())
      .pipe(
        switchMap((unit) => {
          return this.dpl.loadingLocationsAdministrationService
            .patch({
              ...this.formData,
            })
            .pipe(
              switchMap((updateLoadingLocation) => {
                return this.dpl.addressesAdministrationService
                  .getById(updateLoadingLocation.addressId)
                  .pipe(
                    tap((address) => {
                      this.unitsStore.updateUnitName(
                        unit.idString,
                        `${address.postalCode}-${address.city}`
                      );
                    })
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
