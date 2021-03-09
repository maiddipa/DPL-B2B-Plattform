import {
  ChangeDetectionStrategy,
  ChangeDetectorRef,
  Component,
  EventEmitter,
  Input,
  OnDestroy,
  OnInit,
  Output,
} from '@angular/core';
import {
  AbstractControl,
  FormArray,
  FormBuilder,
  FormControl,
  FormGroup,
  Validators,
} from '@angular/forms';
import {
  BaseLoadCarrierInfo,
  OrderQuantityType,
  OrderType,
} from '@app/api/dpl';
import {
  GoogleMapsPlacesLookupComponentForm,
  LoadCarrierInOutFormComponent,
  LoadCarrierPipe,
} from '@app/shared';
import { filterNil } from '@datorama/akita';
import { ILabelValue, ILabelValueIcon } from '@dpl/dpl-lib';
import { AccountsService } from 'apps/dpl-live/src/app/accounts/services/accounts.service';
import {
  ILoadCarrierAccountOrderOption,
  ILoadCarrierOrderOption,
} from 'apps/dpl-live/src/app/accounts/services/accounts.service.types';
import * as moment from 'moment';
import { SubFormGroup } from 'ngx-sub-form';
import {
  BehaviorSubject,
  combineLatest,
  EMPTY,
  Observable,
  of,
  timer,
} from 'rxjs';
import {
  debounceTime,
  delayWhen,
  distinctUntilChanged,
  filter,
  first,
  map,
  publishReplay,
  refCount,
  startWith,
  switchMap,
  tap,
} from 'rxjs/operators';

import {
  ISearchInput,
  ISearchResponse,
  SearchInputOrderType,
} from '../../services/search.service.types';

type SearchMode = 'geoPoint' | 'zip';

interface IViewData {
  searchMode: SearchMode;
  loadCarrierOptions: ILoadCarrierOrderOption[];
  accountOptions: ILoadCarrierAccountOrderOption[];
  quantityTypeOptions: ILabelValue<OrderQuantityType>[];
  orderTypeOptions: any[];
  baseLoadCarriersOptions: ILabelValue<number>[];
  stackHeightOptions: number[];
}

const selectnullValue = -99999999;

@Component({
  selector: 'app-search-input',
  templateUrl: './search-input.component.html',
  styleUrls: ['./search-input.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class SearchInputComponent implements OnDestroy, OnInit {
  @Input() responses: ISearchResponse[];
  @Output() search = new EventEmitter<ISearchInput>();
  @Output() changed = new EventEmitter<ISearchInput>();

  initialSearchText: string;
  radiusOptions = [10, 20, 30, 40, 50, 60, 70, 80, 100];

  searchAction = SearchInputOrderType;
  orderTypeOptions: Array<ILabelValueIcon<SearchInputOrderType>>;
  quantityTpe = OrderQuantityType;
  baseLoadCarrierInfo = BaseLoadCarrierInfo;

  formGroup: FormGroup;
  searchMode$ = new BehaviorSubject<SearchMode>('geoPoint');
  viewData$: Observable<IViewData>;

  constructor(
    private fb: FormBuilder,
    private cd: ChangeDetectorRef,
    private accounts: AccountsService,
    private loadCarrierPipe: LoadCarrierPipe
  ) {}

  ngOnInit() {
    const anyDayOfWeekSelected = (control: FormArray) => {
      return control.controls.map((c) => c.value).filter((i) => i === true)
        .length > 0
        ? null
        : { noDayOfWeekSelected: true };
    };

    const minOne = (control: AbstractControl) => {
      const value = control.value;
      if (Array.isArray(value) && value.length === 0) {
        return {
          minOne: true,
        };
      }
      return;
    };

    this.formGroup = this.fb.group({
      type: [null, []],
      location: new SubFormGroup(null),
      company: [null, []],
      street: [null, []],
      city: [null, []],
      zip: [null, []],
      country: [null, []],

      loadCarrierOption: [null, [Validators.required]],
      accountOption: [null, [Validators.required]],
      quantityType: [OrderQuantityType.Load, Validators.required],
      // quantity validator (max) is attached dynamically based on ordertype selection
      quantity: [null, []],
      radius: [null, [Validators.required]],
      calendarWeek: [null, [Validators.required]],
      daysOfWeek: this.fb.array(this.getDaysOfWeek(), [anyDayOfWeekSelected]),
      orderType: [null, []],
      routing: [null, []],
      baseLoadCarrierIds: [null, [minOne]],
      stackHeight: [null, []],
    });

    this.formGroup.patchValue({
      type: 'form',
      radius: 60 * 1000,
      routing: false,
    });

    const searchMode$ = this.searchMode$.asObservable().pipe(
      tap((mode) => {
        const locationControl = this.formGroup.get('location');
        const zipControl = this.formGroup.get('zip');
        switch (mode) {
          case 'geoPoint': {
            zipControl.reset(null);
            zipControl.clearValidators();
            break;
          }
          case 'zip': {
            locationControl.reset(null);
            zipControl.setValidators(Validators.required);
            break;
          }
          default:
            break;
        }

        locationControl.updateValueAndValidity();
        zipControl.updateValueAndValidity();
      })
    );

    const orderOptions$ = this.accounts
      .getLoadCarrierOrderOptions()
      .pipe(publishReplay(1), refCount());

    const orderTypeOptions$ = orderOptions$.pipe(
      map((loadCarrierOption) => {
        return [
          {
            label: $localize`:Fetch|Label Abholen@@Fetch:Abholen`,
            value: OrderType.Demand,
            disabled: loadCarrierOption.demand.length === 0,
            icon: 'arrow_upward',
          },
          {
            label: $localize`:Give|Label Abgeben@@Give:Abgeben`,
            value: OrderType.Supply,
            disabled: loadCarrierOption.supply.length === 0,
            icon: 'arrow_downward',
          },
        ];
      })
    );

    const orderTypeControl = this.formGroup.get('orderType');
    const orderType$ = orderTypeOptions$.pipe(
      tap((orderTypeOptions) => {
        const firstEnabledOption = orderTypeOptions.find((i) => !i.disabled);
        if (!firstEnabledOption) {
          return;
        }

        orderTypeControl.patchValue(firstEnabledOption.value);
      }),
      switchMap(() => {
        return orderTypeControl.valueChanges.pipe(
          startWith(orderTypeControl.value)
        );
      }),
      publishReplay(1),
      refCount()
    ) as Observable<OrderType>;

    const loadCarrierOptions$ = combineLatest([orderOptions$, orderType$]).pipe(
      map(([orderOptions, orderType]) => {
        return orderType === OrderType.Supply
          ? orderOptions.supply
          : orderOptions.demand;
      })
    );

    const loadCarrierOptionControl = this.formGroup.get('loadCarrierOption');
    const selectedLoadCarrierOption$ = loadCarrierOptions$.pipe(
      tap((loadCarrierOptions) => {
        if (!loadCarrierOptions || loadCarrierOptions.length === 0) {
          return;
        }

        loadCarrierOptionControl.patchValue(loadCarrierOptions[0]);
      }),
      switchMap(() => {
        return loadCarrierOptionControl.valueChanges.pipe(
          startWith(loadCarrierOptionControl.value)
        ) as Observable<ILoadCarrierOrderOption>;
      }),
      publishReplay(1),
      refCount()
    );

    const accountOptions$ = selectedLoadCarrierOption$.pipe(
      map((loadCarrierOption) => {
        if (!loadCarrierOption) {
          return [];
        }

        return loadCarrierOption.accountInfos;
      }),
      startWith([] as ILoadCarrierAccountOrderOption[]),
      publishReplay(1),
      refCount()
    );

    const accountOptionControl = this.formGroup.get('accountOption');
    const selectedAccountOption$ = accountOptions$.pipe(
      tap((accountOptions) => {
        if (!accountOptions || accountOptions.length === 0) {
          return;
        }

        accountOptionControl.patchValue(accountOptions[0]);
      }),
      switchMap((options) => {
        return accountOptionControl.valueChanges.pipe(
          startWith(accountOptionControl.value)
        ) as Observable<ILoadCarrierAccountOrderOption>;
      }),
      tap((accountOption) => {
        if (!accountOption) {
          return;
        }

        quantityControl.setValidators([
          Validators.min(accountOption.min),
          Validators.max(accountOption.max),
        ]);
        quantityControl.updateValueAndValidity();
      }),
      publishReplay(1),
      refCount()
    );

    const quantityTypeOptions$ = selectedAccountOption$.pipe(
      filterNil,
      map((selectedAccountOption) => {
        const options: ILabelValue<OrderQuantityType>[] = [
          {
            label: 'Ladung',
            value: OrderQuantityType.Load,
            disabled: selectedAccountOption.max < 495,
            icon: 'local_shipping',
          },
          {
            label: 'Stück',
            value: OrderQuantityType.LoadCarrierQuantity,
            icon: 'reorder',
          },
        ];
        return options;
      })
    );

    const quantityTypeControl = this.formGroup.get('quantityType');
    const quantityType$ = quantityTypeControl.valueChanges.pipe(
      startWith(quantityTypeControl.value),
      distinctUntilChanged(),
      tap((quantityType: OrderQuantityType) => {
        const quantity = this.formGroup.get('quantity');
        if (quantityType === OrderQuantityType.Load) {
          quantity.patchValue(1);
          quantity.disable();
          return;
        }

        quantity.enable();
      })
    );

    const baseLoadCarrierIdsControl = this.formGroup.get('baseLoadCarrierIds');
    const baseLoadCarrierOptions$ = selectedLoadCarrierOption$.pipe(
      map((option) => {
        const baseLoadCarrierOptions: ILabelValue<number>[] = [];

        if (
          option.loadCarrier.type.baseLoadCarrier !== BaseLoadCarrierInfo.None
        ) {
          option.loadCarrier.type.baseLoadCarriers.forEach((i) => {
            baseLoadCarrierOptions.push({
              label: this.loadCarrierPipe.transform(i.id, 'both'),
              value: i.id,
            });
          });
        }

        if (
          option.loadCarrier.type.baseLoadCarrier !==
          BaseLoadCarrierInfo.Required
        ) {
          baseLoadCarrierOptions.push({
            label: 'Keine',
            value: selectnullValue,
          });
        }

        return baseLoadCarrierOptions;
      }),
      distinctUntilChanged(),
      tap((baseLoadCarriers) => {
        baseLoadCarrierIdsControl.patchValue(
          baseLoadCarriers.map((i) => i.value)
        );
      })
    );

    const stackHeightControl = this.formGroup.get('stackHeight');
    const stackHeightOptions$ = selectedLoadCarrierOption$.pipe(
      switchMap((option) => of([15])),
      tap((options) => {
        if (options.length === 0) {
          throw new Error(
            'For every load carrier there should be defined stack heights'
          );
        }
        const stackHeight = stackHeightControl.value as number;
        if (!options.includes(stackHeight)) {
          stackHeightControl.patchValue(options[0]);
        }
      })
    );

    const quantityControl = this.formGroup.get('quantity');
    const quantitySync$ = combineLatest([
      selectedAccountOption$,
      quantityType$,
    ]).pipe(
      tap(([selectedAccountOption, quantityType]) => {
        switch (quantityType) {
          case OrderQuantityType.Load: {
            quantityControl.patchValue(1);
            break;
          }
          case OrderQuantityType.LoadCarrierQuantity: {
            const quantity = Math.min(100, selectedAccountOption.max);
            quantityControl.patchValue(quantity);
            break;
          }

          default:
            break;
        }
      })
    );

    const calendarWeekControl = this.formGroup.get('calendarWeek');
    const daysOfWeekControl = this.formGroup.get('daysOfWeek') as FormArray;

    const calendarWeek$ = calendarWeekControl.valueChanges.pipe(
      tap((calendarWeekDate: Date) => {
        // disabled all day options tha tare in the past
        const today = moment(new Date());
        const calendarWeek = moment(calendarWeekDate);
        const difference = calendarWeek.diff(today, 'days');
        const disabledCount = difference < 0 ? difference * -1 : 0;

        [...new Array(6)].forEach((v, i) => {
          if (i < disabledCount) {
            daysOfWeekControl.controls[i].setValue(false);
            daysOfWeekControl.controls[i].disable();
          } else {
            // by default set all days to checked, but saturday
            if (!daysOfWeekControl.touched && i < 5) {
              daysOfWeekControl.controls[i].setValue(true);
            }

            daysOfWeekControl.controls[i].enable();
          }
        });
      })
    );

    // this is neccessary so the most current search data is available on parent component for map click based searches
    const searchInput$ = combineLatest(selectedAccountOption$).pipe(
      first(),
      switchMap(() => this.formGroup.valueChanges),
      debounceTime(200),
      filter((value) => !!value.accountOption),
      tap((input) => {
        this.changed.emit(this.parseFormValue(this.formGroup.getRawValue()));
      })
    );

    const lastResponse$ = of(
      this.responses?.length > 0
        ? this.responses[this.responses.length - 1]
        : null
    ).pipe(
      delayWhen((data) => (data ? timer(200) : of(0))),
      tap((lastResponse) => {
        if (!lastResponse) {
          return;
        }

        console.log(lastResponse.input.formValue);
        // setTimeout(() => {
        this.formGroup.patchValue(lastResponse.input.formValue, {
          emitEvent: false,
        });

        this.formGroup.updateValueAndValidity();
        // });
      })
    );

    const formSync$ = combineLatest([
      selectedAccountOption$,
      calendarWeek$,
      searchInput$,
      quantitySync$,
      lastResponse$,
    ]).pipe(
      switchMap(() => EMPTY),
      startWith(null)
    );

    const options$ = combineLatest([
      orderTypeOptions$,
      loadCarrierOptions$,
      accountOptions$,
      quantityTypeOptions$,
      stackHeightOptions$,
      baseLoadCarrierOptions$,
    ]).pipe(
      map(
        ([
          orderTypeOptions,
          loadCarrierOptions,
          accountOptions,
          quantityTypeOptions,
          stackHeightOptions,
          baseLoadCarriersOptions,
        ]) => {
          return {
            orderTypeOptions,
            loadCarrierOptions,
            accountOptions,
            quantityTypeOptions,
            stackHeightOptions,
            baseLoadCarriersOptions,
          };
        }
      )
    );

    this.viewData$ = combineLatest([searchMode$, options$, formSync$]).pipe(
      map(([searchMode, options]) => {
        return {
          searchMode,
          ...options,
        };
      }),
      tap(() => this.cd.markForCheck())
    );
  }

  ngOnDestroy() {}

  private getDaysOfWeek() {
    return [...new Array(6)].map((day) => this.fb.control(null, []));
  }

  onSearchTypeChanged(index: number) {
    this.searchMode$.next(index === 0 ? 'geoPoint' : 'zip');
  }

  onSubmit() {
    this.search.emit(this.parseFormValue(this.formGroup.getRawValue()));
  }

  parseFormValue(value: any) {
    const getDaysOfWeek = (daysOfWeek: boolean[]) => {
      return daysOfWeek
        .map((value, index) => ({ value, index }))
        .filter((i) => i.value)
        .map((i) => i.index);
    };

    //HACK adjust to new input form format
    const searchInput: ISearchInput = {
      postingAccount: value.accountOption.account,
      type: value.type,
      isZipOnlySearch: this.searchMode$.value === 'zip',
      geoPoint: value?.location?.geoPoint,
      searchText: value?.location?.searchText,
      zip: value.zip,
      routing: value.routing,
      palletId: value.loadCarrierOption.loadCarrier.id,
      quantityType: value.quantityType,
      quantity: value.quantity,
      // HACK we need to centralize order quantity calculation
      calculatedQuantity:
        value.quantityType === OrderQuantityType.LoadCarrierQuantity
          ? value.quantity
          : 1 *
            value.loadCarrierOption.loadCarrier.type.quantityPerEur *
            33 *
            value.stackHeight,
      stackHeight: value.stackHeight,
      baseLoadCarrierIds: value.baseLoadCarrierIds, //.map(i => i === selectnullValue ? null : i),
      radius: value.radius,
      calendarWeek: value.calendarWeek,
      daysOfWeek: getDaysOfWeek(value.daysOfWeek),
      orderType: value.orderType,
      formValue: value,
    };

    return searchInput;
  }

  get calendarWeek() {
    return this.formGroup.get('calendarWeek') as FormControl;
  }

  compareLoadCarriers(
    option1: ILoadCarrierOrderOption,
    option2: ILoadCarrierOrderOption
  ) {
    return option1.loadCarrier.id === option2.loadCarrier.id;
  }

  compareAccounts(
    option1: ILoadCarrierAccountOrderOption,
    option2: ILoadCarrierAccountOrderOption
  ) {
    return option1.account.id === option2.account.id;
  }
}
