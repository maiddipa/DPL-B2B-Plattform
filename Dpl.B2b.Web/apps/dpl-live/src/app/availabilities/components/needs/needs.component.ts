import {
  Component,
  OnInit,
  ChangeDetectorRef,
  EventEmitter,
  Input,
  Output,
} from '@angular/core';
import {
  FormArray,
  FormBuilder,
  FormControl,
  FormGroup,
  ValidationErrors,
  ValidatorFn,
  Validators,
} from '@angular/forms';
import { Router } from '@angular/router';
import {
  LoadCarrierPickerContext,
  OnBehalfOfService,
  OrdersViewType,
  parseMomentToUtcDate,
} from '@app/shared';
import {
  LoadingService,
  TypedFormArray,
  WrappedControlForm,
} from '@dpl/dpl-lib';
import * as _ from 'lodash';
import * as moment from 'moment';
import { combineLatest, forkJoin, Observable } from 'rxjs';
import {
  first,
  map,
  publishReplay,
  refCount,
  startWith,
  switchMap,
  tap,
} from 'rxjs/operators';

import { AccountsService } from '../../../accounts/services/accounts.service';
import {
  Address,
  EmployeeNoteCreateRequest,
  EmployeeNoteType,
  LoadCarrier,
  OrderGroup,
  OrderGroupsCreateRequest,
  OrderTransportType,
  OrderType,
} from '../../../core/services/dpl-api-services';
import { LocalizationService } from '../../../core/services/localization.service';
import { CustomerDivisionsService } from '../../../customers/services/customer-divisions.service';
import { ICustomerDivision } from '../../../customers/state/customer-division.model';
import { ILoadingLocation } from '../../../loading-locations/state/loading-location.model';
import { UserService } from '../../../user/services/user.service';
import { IUser } from '../../../user/state/user.model';
import { ILoadsPerDay } from '../../services/availabilies.service.types';
import { AvailabilitiesService } from '../../services/availabilities.service';
import {
  Controls,
  NgxAutomaticRootFormComponent,
  SubFormArray,
  SubFormGroup,
} from 'ngx-sub-form';

interface IViewData {
  currentUser: IUser<number, number>;
  locationOptions: ILoadingLocation[];
  selectedLocation: ILoadingLocation;
  currentDivision: ICustomerDivision<ILoadingLocation<Address>>;
}

interface OrderGroupCreateRequest {
  postingAccountId: number;
  id: number;
  salutation: string;
  name1: string;
  name2: string;
  name3: string;
  zip: string;
  city: string;
  country: string;
  locationControl: ILoadingLocation;
  availabilities: [];
  loadingOptions: {};
}

interface OrderGroupForm {
  postingAccountId: number;
  id: number;
  salutation: string;
  name1: string;
  name2: string;
  name3: string;
  zip: string;
  city: string;
  country: string;
  locationControl: ILoadingLocation;
  availabilities: [];
  loadingOptions: {};
}

function validateLoadingOptions(group: FormGroup): ValidationErrors {
  if (group) {
    const {
      supportsSideLoading,
      supportsRearLoading,
      supportsJumboVehicles,
    } = group.getRawValue();

    if (!supportsSideLoading && !supportsRearLoading && !supportsJumboVehicles)
      return { noLoadingOptionSelected: true };

    if (supportsJumboVehicles == true && !supportsRearLoading)
      return { noJumboWithoutRearLoading: true };
  }
  return null;
}

@Component({
  selector: 'app-needs',
  templateUrl: './needs.component.html',
  styleUrls: ['./needs.component.scss'],
})
export class NeedsComponent
  extends NgxAutomaticRootFormComponent<OrderGroupCreateRequest, OrderGroupForm>
  implements OnInit {
  @Input() dataInput: Required<OrderGroupCreateRequest>;
  @Output() dataOutput = new EventEmitter<OrderGroupCreateRequest>();

  locationOptions$: Observable<ILoadingLocation[]>;
  selectedLocation$: Observable<ILoadingLocation<Address>>;
  loadsPerDay: [ILoadsPerDay[]];
  totalLoads: number;

  savedFilters: any;
  loadFilterChanged: any;

  loadCarrierContext: LoadCarrierPickerContext = 'demand';

  viewData$: Observable<IViewData>;

  // used to enable submit based on loads
  allAvailabilitiesLoadExists = false;

  postingAccountIdControl: SubFormGroup<any>;

  constructor(
    private formBuilder: FormBuilder,
    private availabilityService: AvailabilitiesService,
    private user: UserService,
    private customerDivisionService: CustomerDivisionsService,
    private router: Router,
    private localizationService: LocalizationService,
    private accounts: AccountsService,
    private onBehalfOfService: OnBehalfOfService,
    private loadingService: LoadingService,
    private cdRef: ChangeDetectorRef
  ) {
    super(cdRef);
    this.loadsPerDay = [[]];
    this.totalLoads = 0;
  }

  protected getFormControls(): Controls<OrderGroupForm> {
    return {
      postingAccountId: new SubFormGroup<Number, WrappedControlForm<number>>(
        null
      ),
      id: new FormControl(null),
      salutation: new FormControl({ value: '', disabled: true }),
      name1: new FormControl({ value: '', disabled: true }),
      name2: new FormControl(null),
      name3: new FormControl(null),
      zip: new FormControl({ value: '', disabled: true }),
      city: new FormControl({ value: '', disabled: true }),
      country: new FormControl({ value: '', disabled: true }),
      locationControl: new FormControl(null, Validators.required),
      availabilities: new SubFormArray(this, [
        this.createAvailabilitiesGroup(),
      ]),
      loadingOptions: this.formBuilder.group(
        {
          supportsSideLoading: false,
          supportsRearLoading: false,
          supportsJumboVehicles: false,
        },
        { validators: validateLoadingOptions }
      ),
    };
  }

  public getDefaultValues(): Partial<OrderGroupCreateRequest> {
    return {
      availabilities: [{}] as any,
    };
  }

  ngOnInit() {
    super.ngOnInit();
    // create form

    // this.formGroup.controls.availabilities.push(
    //   this.createAvailabilitiesGroup()
    // );

    const currentUser$ = this.user.getCurrentUser().pipe(
      tap((currentUser) => {
        this.formGroup.patchValue({
          name1: currentUser ? currentUser.name : null,
        });
      })
    );

    const currentDivision$ = this.customerDivisionService
      .getActiveDivision()
      .pipe(
        tap(() => {
          const name1 = this.formGroup.controls.name1.value;
          this.formGroup.reset({ name1 });
          this.availabilities.clear();
          this.availabilities.push(this.createAvailabilitiesGroup());
        })
      );

    const locationOptions$ = currentDivision$.pipe(
      map((division) =>
        division ? division.loadingLocations : ([] as ILoadingLocation[])
      ),
      tap((loadingLocations) => {
        const loadingLocation =
          loadingLocations && loadingLocations.length > 0
            ? loadingLocations[0]
            : null;

        if (!loadingLocation) {
          return;
        }

        this.formGroup.controls.locationControl.patchValue(loadingLocation);
      })
    );

    const selectedLocation$ = this.formGroup.controls.locationControl.valueChanges.pipe(
      map((value) => value as ILoadingLocation),
      tap((location) => {
        if (!location) {
          return;
        }

        this.availabilityService.setActiveLoadingLocation(location);

        this.formGroup.patchValue({
          city: location.address.city,
          zip: location.address.postalCode,
          country: this.localizationService.getTranslation(
            'Countries',
            location.address.country
          ),
          loadingOptions: {
            supportsSideLoading: location.detail.supportsSideLoading,
            supportsRearLoading: location.detail.supportsRearLoading,
            supportsJumboVehicles: location.detail.supportsJumboVehicles,
          },
        });
      }),
      startWith(null)
    );

    this.viewData$ = combineLatest([
      currentDivision$,
      selectedLocation$,
      locationOptions$,
      currentUser$,
    ]).pipe(
      map((result) => {
        const [
          currentDivision,
          selectedLocation,
          locationOptions,
          currentUser,
        ] = result;

        const viewData: IViewData = {
          currentDivision,
          currentUser,
          locationOptions,
          selectedLocation,
        };

        return viewData;
      }),
      publishReplay(1),
      refCount()
    );
  }

  get availabilities() {
    return (this.formGroup.get(
      'availabilities'
    ) as unknown) as TypedFormArray<{}>;
  }

  createAvailabilitiesGroup() {
    return new SubFormGroup<any>(null);
    // const group = this.formBuilder.group({
    //   loadCarrier: new SubFormGroup<number, WrappedControlForm<number>>(
    //     null,
    //     Validators.required
    //   ),
    //   loadCarrierQuantity: [{ value: null, disabled: true }],
    //   loadCarrierQuantityRange: [{ value: null, disabled: true }],
    //   loadConfiguration: this.formBuilder.group({
    //     quantityPerEur: [{ value: '', disabled: true }, Validators.required],
    //     requiresSpecialAmount: [false],
    //     numberOfStacks: [
    //       { value: 33, disabled: true },
    //       [Validators.required, Validators.max(37), Validators.min(15)],
    //     ],
    //     // TODO fill stackheight min/max based on loading location + potentially load carrier
    //     stackHeightMin: [
    //       15,
    //       [Validators.required, Validators.max(30), Validators.min(1)],
    //     ],
    //     stackHeightMax: [
    //       15,
    //       [Validators.required, Validators.max(30), Validators.min(1)],
    //     ],
    //   }),
    //   baseLoadCarrier: [null],
    //   baseLoadCarrierQuantity: [
    //     { value: null, disabled: true },
    //     Validators.required,
    //   ],
    //   dateRange: [
    //     {
    //       startDate: null,
    //       endDate: null,
    //       numberOfLoadsRange: null,
    //     },
    //   ],
    // });

    // return group;
  }

  selectedLoadsPerDayChanged(event) {
    this.loadsPerDay[event.index] = event.loads;
    this.totalLoads = _.sumBy(_.flatten(this.loadsPerDay), (x) =>
      x && x.loads ? x.loads : 0
    );
    // validate load per day <-> availabilities
    this.allAvailabilitiesLoadExists =
      this.loadsPerDay.filter((x) => x.filter((y) => y.loads > 0).length > 0)
        .length >= this.availabilities.length;
    this.cdRef.detectChanges();
  }

  reset() {
    this.loadsPerDay = [[]];
    this.totalLoads = 0;
    const { name1, locationControl } = this.formGroup.getRawValue();
    const location = this.formGroup.get('locationControl') as FormControl;
    this.formGroup.reset({ name1 });
    location.patchValue(locationControl);
    this.availabilities.clear();
    this.availabilities.push(this.createAvailabilitiesGroup());
  }

  removeRow(i: number) {
    this.availabilities.removeAt(i);
    this.loadsPerDay.splice(this.availabilities.controls.length - i, 1);
    this.totalLoads = _.sumBy(_.flatten(this.loadsPerDay), (x) =>
      x && x.loads ? x.loads : 0
    );
  }
  submit() {
    if (this.formGroup.valid) {
      combineLatest([this.accounts.getActiveAccount()])
        .pipe(
          first(),
          switchMap(([account]) => {
            // onBehalfDialog if user is dpl employee
            return this.onBehalfOfService
              .openOnBehalfofDialog(EmployeeNoteType.Create)
              .pipe(
                switchMap((executionResult) => {
                  const result: Observable<OrderGroup>[] = [];

                  const array = this.formGroup.controls
                    .availabilities as FormArray;
                  for (let i = 0; i < this.loadsPerDay.length; i++) {
                    const flatLoadsPerDay = _.flatten(this.loadsPerDay[i]);
                    const loadCarrierFormGroup = array.controls[
                      array.controls.length - 1 - i
                    ] as FormGroup;
                    flatLoadsPerDay.forEach((loadPerDay) => {
                      if (loadPerDay.loads > 0) {
                        result.push(
                          this.availabilityService.createAvailability(
                            this.mapToCreateRequest(
                              account.id,
                              loadPerDay.date,
                              loadPerDay.loads,
                              loadCarrierFormGroup,
                              loadPerDay.dateTo,
                              executionResult
                            )
                          )
                        );
                      }
                    });
                  }
                  return forkJoin(result).pipe(
                    this.loadingService.showLoadingWhile()
                  );
                }),
                switchMap((result) => {
                  const highlightIds = _.flatten(
                    result.map((orderGroup) => {
                      return orderGroup.orderIds;
                    })
                  );
                  return this.router.navigate(['orders'], {
                    queryParams: {
                      type: OrdersViewType.Demand,
                      highlightIds,
                    },
                  });
                })
              );
          })
        )
        .subscribe();
    }

    // this.availabilities.createAvailability(this.mapToCreateRequest()).subscribe(result => {
    //   console.log(result);
    // });
  }

  private mapToCreateRequest(
    postingAccountId: number,
    date: moment.Moment,
    loads: number,
    loadCarrierFormGroup: FormGroup,
    dateTo?: moment.Moment,
    behalfOfResult?: EmployeeNoteCreateRequest
  ) {
    const locationValue = this.formGroup.getRawValue() as {
      id: number;
      salutation: string;
      name1: string;
      name2: string;
      name3: string;
      zip: string;
      city: string;
      country: string;
      locationControl: ILoadingLocation;
    };
    const loadCarrierValue = loadCarrierFormGroup.getRawValue() as {
      loadCarrier: LoadCarrier;
      loadCarrierQuantity: number;
      loadCarrierQuantityRange: string;
      loadConfiguration: {
        quantityPerEur: string;
        numberOfStacks: number;
        requiresSpecialAmount: boolean;
        stackHeightMin: number;
        stackHeightMax: number;
      };
      baseLoadCarrier: LoadCarrier;
      baseLoadCarrierQuantity: number;
      dateRange: [
        {
          startDate: Date;
          endDate: Date;
          numberOfLoadsRange: number;
        }
      ];
    };

    const earliestFulfillmentDateTime = parseMomentToUtcDate(
      date.startOf('day')
    );
    const latestFulfillmentDateTime = parseMomentToUtcDate(
      (dateTo ? dateTo : date).startOf('day')
    );

    const request: OrderGroupsCreateRequest = {
      loadingLocationId: locationValue.locationControl.id,
      type: OrderType.Demand,
      loadCarrierId: loadCarrierValue.loadCarrier.id,
      loadCarrierQuantity: loadCarrierValue.loadCarrierQuantity,
      stackHeightMax: loadCarrierValue.loadConfiguration.stackHeightMax,
      stackHeightMin: loadCarrierValue.loadConfiguration.stackHeightMin,
      numberOfLoads: loads,
      numberOfStacks: loadCarrierValue.loadConfiguration.numberOfStacks,
      transportType: OrderTransportType.ProvidedByOthers,
      baseLoadCarrierId: loadCarrierValue.baseLoadCarrier
        ? loadCarrierValue.baseLoadCarrier.id
        : undefined,
      earliestFulfillmentDateTime,
      latestFulfillmentDateTime,
      supportsSideLoading: (this.formGroup.get(
        'loadingOptions'
      ) as FormGroup).get('supportsSideLoading').value,
      supportsRearLoading: (this.formGroup.get(
        'loadingOptions'
      ) as FormGroup).get('supportsRearLoading').value,
      supportsJumboVehicles: (this.formGroup.get(
        'loadingOptions'
      ) as FormGroup).get('supportsJumboVehicles').value,
      postingAccountId: postingAccountId,
      dplNote: behalfOfResult,
    };
    return request;
  }

  // maxHeightValidator(formControl: AbstractControl) {
  //   if (!formControl.parent) {
  //     return null;
  //   }
  //   if ( formControl.parent.get("stackHeightMin").value) {
  //     return  Validators.min(formControl.parent.get("stackHeightMin").value);
  //   }
  //   return null;
  // }
}
