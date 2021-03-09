import {
  Component,
  OnInit,
  ChangeDetectionStrategy,
  Inject,
  Optional,
} from '@angular/core';
import * as AspNetData from 'devextreme-aspnet-data-nojquery';
import CustomStore from 'devextreme/data/custom_store';
import * as moment from 'moment';
import { Observable, combineLatest, of } from 'rxjs';
import { filter, switchMap, map, tap } from 'rxjs/operators';
import {
  AuthenticationService,
  DplApiService,
  LocalizationService,
} from '../../../core';
import {
  LoadingLocation,
  API_BASE_URL,
  BusinessHourExceptionType,
  BusinessHourException,
} from '../../../core/services/dpl-api-services';
import {
  CustomerAdminOrganizationUnit,
  CustomerAdminScope,
} from '../../services/customer-administration.service.types';
import { UnitsQuery } from '../../state/units.query';

type ViewData = {
  unit: CustomerAdminOrganizationUnit;
  dataSource: CustomStore;
  dropdownExecptionType: any;
};

@Component({
  selector:
    'dpl-customer-administration-loading-location-business-hours-exceptions',
  templateUrl:
    './customer-administration-loading-location-business-hours-exceptions.component.html',
  styleUrls: [
    './customer-administration-loading-location-business-hours-exceptions.component.scss',
  ],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class CustomerAdministrationLoadingLocationBusinessHoursExceptionsComponent
  implements OnInit {
  baseUrl: string;
  viewData$: Observable<ViewData>;
  gridDataSource: LoadingLocation;

  constructor(
    private authenticationService: AuthenticationService,
    private localizationService: LocalizationService,
    private unitsQuery: UnitsQuery,
    private dpl: DplApiService,
    @Optional() @Inject(API_BASE_URL) baseUrl?: string
  ) {
    this.baseUrl = baseUrl;
  }

  ngOnInit(): void {
    const unit$ = this.unitsQuery.selectActive().pipe(filter((x) => !!x));
    this.viewData$ = combineLatest([unit$, this.getTypeData()]).pipe(
      map(([unit, dropdownExecptionType]) => {
        const dataSource = new CustomStore({
          key: 'id',
          loadMode: 'raw',
          load: async () => {
            return this.dpl.loadingLocationsAdministrationService
              .getById(unit.parent)
              .pipe(
                map((value) => {
                  this.gridDataSource = value;
                  return value.businessHourExceptions.map(
                    (businessHourExceptions) => {
                      return {
                        ...businessHourExceptions,
                        fromDateTime: moment(
                          businessHourExceptions.fromDateTime
                            .toString()
                            .substring(0, 19)
                        ).toDate(),
                        toDateTime: moment(
                          businessHourExceptions.toDateTime
                            .toString()
                            .substring(0, 19)
                        ).toDate(),
                      };
                    }
                  );
                }),
                tap((x) => {
                  this.gridDataSource.businessHourExceptions = x;
                })
              )
              .toPromise();
          },
          insert: (values: BusinessHourException) => {
            this.gridDataSource.businessHourExceptions.push(values);
            this.gridDataSource.businessHourExceptions = this.getRequestTimezoneManipulation(
              this.gridDataSource.businessHourExceptions
            );
            return this.dpl.loadingLocationsAdministrationService
              .patch(this.gridDataSource)
              .pipe(
                switchMap(() => {
                  return this.dpl.loadingLocationsAdministrationService
                    .getById(unit.parent)
                    .pipe(
                      tap((value) => {
                        this.gridDataSource = value;
                        return value.businessHourExceptions;
                      })
                    );
                })
              )
              .toPromise();
          },
          update: (key, values: BusinessHourException) => {
            const indexBusinessHourExceptions = this.gridDataSource.businessHourExceptions.findIndex(
              (x) => x.id === key
            );
            if (values.hasOwnProperty('id')) {
              this.gridDataSource.businessHourExceptions[
                indexBusinessHourExceptions
              ].id = values.id;
            }
            if (values.hasOwnProperty('type')) {
              this.gridDataSource.businessHourExceptions[
                indexBusinessHourExceptions
              ].type = values.type;
            }
            if (values.hasOwnProperty('fromDateTime')) {
              this.gridDataSource.businessHourExceptions[
                indexBusinessHourExceptions
              ].fromDateTime = values.fromDateTime;
            }
            if (values.hasOwnProperty('toDateTime')) {
              this.gridDataSource.businessHourExceptions[
                indexBusinessHourExceptions
              ].toDateTime = values.toDateTime;
            }
            this.gridDataSource.businessHourExceptions = this.getRequestTimezoneManipulation(
              this.gridDataSource.businessHourExceptions
            );

            return this.dpl.loadingLocationsAdministrationService
              .patch(this.gridDataSource)
              .pipe(
                switchMap(() => {
                  return this.dpl.loadingLocationsAdministrationService
                    .getById(unit.parent)
                    .pipe(
                      tap((value) => {
                        this.gridDataSource = value;
                        return value.businessHourExceptions;
                      })
                    );
                })
              )
              .toPromise();
          },
          remove: (key) => {
            const deleteIndex = this.gridDataSource.businessHourExceptions.findIndex(
              (x) => x.id === key
            );
            const deleteItem = this.gridDataSource.businessHourExceptions.splice(
              deleteIndex,
              1
            );
            console.log(deleteItem);
            console.log(this.gridDataSource);

            this.gridDataSource.businessHourExceptions = this.getRequestTimezoneManipulation(
              this.gridDataSource.businessHourExceptions
            );
            return (this.dpl.loadingLocationsAdministrationService
              .patch(this.gridDataSource)
              .pipe(
                switchMap(() => {
                  return this.dpl.loadingLocationsAdministrationService
                    .getById(unit.parent)
                    .pipe(
                      tap((value) => {
                        this.gridDataSource = value;
                        return value.businessHourExceptions;
                      })
                    );
                })
              )
              .toPromise() as unknown) as Promise<void>;
          },
        });
        const viewData: ViewData = {
          dataSource,
          unit,
          dropdownExecptionType,
        };
        return viewData;
      })
    );
  }

  getRequestTimezoneManipulation(
    businessHourExceptions: BusinessHourException[]
  ) {
    return businessHourExceptions.map((businessHourExceptions) => {
      return {
        ...businessHourExceptions,
        fromDateTime: moment(businessHourExceptions.fromDateTime)
          .add(
            'minutes',
            moment(businessHourExceptions.fromDateTime).utcOffset()
          )
          .toDate(),
        toDateTime: moment(businessHourExceptions.toDateTime)
          .add('minutes', moment(businessHourExceptions.toDateTime).utcOffset())
          .toDate(),
      };
    });
  }

  // get UserRole LookupData
  getTypeData() {
    const r = Object.keys(BusinessHourExceptionType).map((value, id) => {
      value = BusinessHourExceptionType[value as any];
      const display = this.localizationService.getTranslation(
        'BusinessHourExceptionType',
        value
      );
      return { id, value, display };
    });

    return of(r);
  }
  getTypeName = (data: any) => {
    const value = BusinessHourExceptionType[data.type as any];
    const display = this.localizationService.getTranslation(
      'BusinessHourExceptionType',
      value
    );
    return display;
  };

  customValidationCallbackBusinessHourExceptions = (params) => {
    if (params.data.fromDateTime && params.data.toDateTime) {
      if (params.data.fromDateTime < params.data.toDateTime) {
        return true;
      } else {
        return false;
      }
    } else {
      return false;
    }
  };
}
