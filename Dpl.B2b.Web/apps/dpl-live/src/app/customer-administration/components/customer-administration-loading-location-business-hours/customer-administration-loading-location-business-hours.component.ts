import {
  Component,
  OnInit,
  ChangeDetectionStrategy,
  Inject,
  Optional,
  ViewContainerRef,
  TemplateRef,
  ViewChild,
} from '@angular/core';
import CustomStore from 'devextreme/data/custom_store';
import { BehaviorSubject, combineLatest, of } from 'rxjs';
import { Observable } from 'rxjs';
import { filter, map, pluck, switchMap, tap } from 'rxjs/operators';
import {
  AuthenticationService,
  DplApiService,
  LocalizationService,
} from '../../../core';
import {
  API_BASE_URL,
  BusinessHours,
  DayOfWeek,
  LoadingLocation,
  LoadingLocationAdministration,
} from '../../../core/services/dpl-api-services';
import {
  CustomerAdminOrganizationUnit,
  CustomerAdminScope,
} from '../../services/customer-administration.service.types';
import { UnitsQuery } from '../../state/units.query';
import * as AspNetData from 'devextreme-aspnet-data-nojquery';
import * as moment from 'moment';
import { DxValidationGroupComponent } from 'devextreme-angular';

type ViewData = {
  unit: CustomerAdminOrganizationUnit;
  dataSource: CustomStore;
  dayOfWeeksTypeData: any;
};

@Component({
  selector: 'dpl-customer-administration-loading-location-business-hours',
  templateUrl:
    './customer-administration-loading-location-business-hours.component.html',
  styleUrls: [
    './customer-administration-loading-location-business-hours.component.scss',
  ],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class CustomerAdministrationLoadingLocationBusinessHoursComponent
  implements OnInit {
  baseUrl: string;
  viewData$: Observable<ViewData>;
  popupDuplicateVisible: boolean = false;
  selectDefaultValuePopUpDayOfWeek: DayOfWeek;
  selectDefaultValuePopUpId: any;
  selectDefaultValuePopUpFromTime: any;
  selectDefaultValuePopUpToTime: any;
  gridDataSource: LoadingLocation;
  businessHourId: number;
  dataSource: CustomStore;
  reloadSub = new BehaviorSubject<boolean>(false);
  @ViewChild('popUpValidationGroup', { static: false })
  validationGroup: DxValidationGroupComponent;
  saveButtonDisable: boolean = true;

  constructor(
    private authenticationService: AuthenticationService,
    private unitsQuery: UnitsQuery,
    private dpl: DplApiService,
    private localizationService: LocalizationService,

    @Optional() @Inject(API_BASE_URL) baseUrl?: string
  ) {
    this.baseUrl = baseUrl;
  }

  ngOnInit(): void {
    const unit$ = this.unitsQuery.selectActive().pipe(filter((x) => !!x));
    const reload$ = this.reloadSub.asObservable();
    this.viewData$ = combineLatest([
      unit$,
      this.getDayOfWeeksTypeData(),
      reload$,
    ]).pipe(
      map(([unit, dayOfWeeksTypeData]) => {
        this.dataSource = new CustomStore({
          key: 'id',
          loadMode: 'raw',
          load: async () => {
            return this.dpl.loadingLocationsAdministrationService
              .getById(unit.parent)
              .pipe(
                map((value) => {
                  this.gridDataSource = value;
                  return value.businessHours.map((businessHour) => {
                    return {
                      ...businessHour,
                      fromTime: moment(
                        '1970-01-01:' +
                          businessHour.fromTime.toString().substring(11, 19)
                      ).toDate(),
                      toTime: moment(
                        '1970-01-01:' +
                          businessHour.toTime.toString().substring(11, 19)
                      ).toDate(),
                    };
                  });
                }),
                tap((x) => {
                  this.gridDataSource.businessHours = x;
                })
              )
              .toPromise();
          },
          insert: (values: BusinessHours) => {
            this.gridDataSource.businessHours.push(values);
            this.gridDataSource.businessHours = this.getRequestTimezoneManipulation(
              this.gridDataSource.businessHours
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
                        this.reloadSub.next(true);
                        return value.businessHours;
                      })
                    );
                })
              )
              .toPromise();
          },
          update: (key, values: BusinessHours) => {
            const indexBusinessHours = this.gridDataSource.businessHours.findIndex(
              (x) => x.id === key
            );
            if (values.hasOwnProperty('id')) {
              this.gridDataSource.businessHours[indexBusinessHours].id =
                values.id;
            }
            if (values.hasOwnProperty('toTime')) {
              this.gridDataSource.businessHours[indexBusinessHours].toTime =
                values.toTime;
            }
            if (values.hasOwnProperty('fromTime')) {
              this.gridDataSource.businessHours[indexBusinessHours].fromTime =
                values.fromTime;
            }
            if (values.hasOwnProperty('dayOfWeek')) {
              this.gridDataSource.businessHours[indexBusinessHours].dayOfWeek =
                values.dayOfWeek;
            }

            this.gridDataSource.businessHours = this.getRequestTimezoneManipulation(
              this.gridDataSource.businessHours
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
                        return value.businessHours;
                      })
                    );
                })
              )
              .toPromise();
          },
          remove: (key) => {
            // find index to remove
            const deleteIndex = this.gridDataSource.businessHours.findIndex(
              (x) => x.id === key
            );
            // remove Item
            const deleteItem = this.gridDataSource.businessHours.splice(
              deleteIndex,
              1
            );
            console.log(deleteItem);
            console.log(this.gridDataSource);

            this.gridDataSource.businessHours = this.getRequestTimezoneManipulation(
              this.gridDataSource.businessHours
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
                        // this.reloadSub.next(true);
                        return value.businessHours;
                      })
                    );
                })
              )
              .toPromise() as unknown) as Promise<void>;
          },
        });
        const dataSource = this.dataSource;
        const viewData: ViewData = {
          dataSource,
          unit,
          dayOfWeeksTypeData,
        };
        return viewData;
      })
    );
  }

  getRequestTimezoneManipulation(businessHours: BusinessHours[]) {
    return businessHours.map((businessHour) => {
      return {
        ...businessHour,
        fromTime: moment(businessHour.fromTime)
          .add('minutes', moment(businessHour.fromTime).utcOffset())
          .toDate(),
        toTime: moment(businessHour.toTime)
          .add('minutes', moment(businessHour.toTime).utcOffset())
          .toDate(),
      };
    });
  }

  getDayOfWeekSortValues(value) {
    let sortValue = 0;
    switch (value.dayOfWeek) {
      case DayOfWeek.Monday:
        sortValue = 1;
        break;
      case DayOfWeek.Tuesday:
        sortValue = 2;
        break;
      case DayOfWeek.Wednesday:
        sortValue = 3;
        break;
      case DayOfWeek.Thursday:
        sortValue = 4;
        break;
      case DayOfWeek.Friday:
        sortValue = 5;
        break;
      case DayOfWeek.Saturday:
        sortValue = 6;
        break;
      case DayOfWeek.Sunday:
        sortValue = 7;
        break;
    }
    return sortValue;
  }

  clickDuplicateRow(data) {
    // disable save button
    this.saveButtonDisable = true;
    // set data for PopUp
    this.selectDefaultValuePopUpId = data.data.id;
    this.selectDefaultValuePopUpDayOfWeek = data.data.dayOfWeek;
    this.selectDefaultValuePopUpFromTime = data.data.fromTime;
    this.selectDefaultValuePopUpToTime = data.data.toTime;
    // open PopUp
    this.popupDuplicateVisible = true;
    // validate PopUp validation group
    let result = this.validationGroup.instance.validate();
  }
  clickCancelDuplicatePopUp() {
    this.popupDuplicateVisible = false;
    this.saveButtonDisable = true;
  }
  clickSaveDuplicatePopUp() {
    const result = this.validationGroup.instance.validate();
    if (result.isValid) {
      const values: BusinessHours = {
        dayOfWeek: this.selectDefaultValuePopUpDayOfWeek,
        fromTime: this.selectDefaultValuePopUpFromTime,
        toTime: this.selectDefaultValuePopUpToTime,
      };
      this.dataSource.insert(values);
      this.popupDuplicateVisible = false;
    } else {
      this.popupDuplicateVisible = true;
    }
  }

  getDayOfWeeksTypeData() {
    const r = Object.keys(DayOfWeek).map((value, id) => {
      value = DayOfWeek[value as any];
      const display = this.localizationService.getTranslation(
        'DayOfWeek',
        value
      );
      return { id, value, display };
    });
    return of(r);
  }

  customValidationCallbackBusinessHours = (params) => {
    if (params.data.fromTime && params.data.toTime) {
      const fromTime = moment(params.data.fromTime, 'YYYY-MM-DD HH:mm:ss');
      const toTime = moment(params.data.toTime, 'YYYY-MM-DD HH:mm:ss');
      if (fromTime.format('HH:mm:ss') < toTime.format('HH:mm:ss')) {
        return true;
      } else {
        return false;
      }
    } else {
      return false;
    }
  };

  customValidationCallbackBusinessHourPopUp = (params) => {
    const fromTime = moment(
      this.selectDefaultValuePopUpFromTime,
      'YYYY-MM-DD HH:mm:ss'
    );
    const toTime = moment(
      this.selectDefaultValuePopUpToTime,
      'YYYY-MM-DD HH:mm:ss'
    );
    if (fromTime.format('HH:mm:ss') < toTime.format('HH:mm:ss')) {
      this.saveButtonDisable = false;
      return true;
    } else {
      this.saveButtonDisable = true;
      return false;
    }
  };

  onHidePopUp(e: any) {
    this.popupDuplicateVisible = false;
    this.saveButtonDisable = true;
  }
}
