import {
  ChangeDetectorRef,
  Component,
  EventEmitter,
  Input,
  OnInit,
  Output,
} from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { LoadCarrierPickerContext, LoadCarrierPickerMode } from '@app/shared';
import { WrappedControlForm } from '@dpl/dpl-lib';
import {
  BaseLoadCarrierInfo,
  LoadCarrier,
} from 'apps/dpl-live/src/app/core/services/dpl-api-services';
import { LoadCarriersService } from 'apps/dpl-live/src/app/master-data/load-carriers/services/load-carriers.service';
import { ILoadCarrier } from 'apps/dpl-live/src/app/master-data/load-carriers/state/load-carrier.model';
import * as _ from 'lodash';
import { Controls, NgxSubFormComponent, SubFormGroup } from 'ngx-sub-form';
import { combineLatest, EMPTY, Observable } from 'rxjs';
import { debounceTime, map, startWith, switchMap, tap } from 'rxjs/operators';

interface IViewData {
  loadCarriers: ILoadCarrier[];
  baseLoadCarrierInfo: BaseLoadCarrierInfo;
  baseLoadCarrierDisabled: boolean;
  baseLoadCarriers: LoadCarrier[];
}

interface OrderPosition {
  loadCarrier: number;
  loadCarrierQuantity: number;
  loadCarrierQuantityRange: number;
  loadConfiguration: {
    quantityPerEur: number;
    requiresSpecialAmount: boolean;
    numberOfStacks: number;
    // TODO fill stackheight min/max based on loading location + potentially load carrier
    stackHeightMin: number;
    stackHeightMax: number;
  };
  baseLoadCarrier?: number;
  baseLoadCarrierQuantity: number;
  dateRange: {
    startDate: Date;
    endDate: Date;
    numberOfLoadsRange: number;
  };
}

@Component({
  selector: 'app-availability-row',
  templateUrl: './availability-row.component.html',
  styleUrls: ['./availability-row.component.scss'],
})
export class AvailabilityRowComponent extends NgxSubFormComponent<OrderPosition>
  implements OnInit {
  @Input() index: number;
  @Input() context: LoadCarrierPickerContext;
  @Output() selectedLoadsPerDayChanged: EventEmitter<any> = new EventEmitter();

  options: number[] = [15, 16, 17];
  baseLoadCarrierInfo = BaseLoadCarrierInfo;
  loadCarrierPickerMode: LoadCarrierPickerMode = 'loadCarrier';

  loadCarrierControl: SubFormGroup<
    ILoadCarrier,
    WrappedControlForm<ILoadCarrier>
  >;

  viewData$: Observable<IViewData>;

  nonBaseLoadCarrier = { id: null };

  constructor(private loadCarrierService: LoadCarriersService) {
    super();
  }

  ngOnInit() {
    // initial value for loads
    this.selectedLoadsPerDayChanged.emit({ loads: [], index: this.index });

    this.loadCarrierControl = this.formGroup.get('loadCarrier') as any;
    const loadCarrierQuantityControl = this.formGroup.get(
      'loadCarrierQuantity'
    );
    const loadCarrierQuantityRangeControl = this.formGroup.get(
      'loadCarrierQuantityRange'
    );

    const loadConfigurationControl = this.formGroup.get(
      'loadConfiguration'
    ) as FormGroup;
    const quantityPerEurControl = loadConfigurationControl.get(
      'quantityPerEur'
    );
    const stackHeightMinControl = loadConfigurationControl.get(
      'stackHeightMin'
    );
    const stackHeightMaxControl = loadConfigurationControl.get(
      'stackHeightMax'
    );
    const numberOfStacksControl = loadConfigurationControl.get(
      'numberOfStacks'
    );

    const requiresSpecialAmountControl = loadConfigurationControl.get(
      'requiresSpecialAmount'
    );

    const baseLoadCarrierControl = this.formGroup.get('baseLoadCarrier');
    const baseLoadCarrierQuantityControl = this.formGroup.get(
      'baseLoadCarrierQuantity'
    );

    const loadCarriers$ = this.loadCarrierService.getLoadCarriers().pipe(
      tap((loadCarriers) => {
        if (loadCarriers.length > 0) {
          this.loadCarrierControl.setValue(null);
        }
      }),
      // we need both the list and doct of load carriers
      map((loadCarriers) => ({
        list: loadCarriers,
        dict: _(loadCarriers)
          .keyBy((i) => i.id)
          .value(),
      }))
    );

    const selectedLoadCarrier$ = this.loadCarrierControl.valueChanges.pipe(
      startWith(null),
      tap((loadCarrier) => {
        if (!loadCarrier) {
          return;
        }
        quantityPerEurControl.patchValue(loadCarrier.type.quantityPerEur);

        // HACK find Fleischkiste
        if (
          loadCarrier.type.baseLoadCarriers &&
          loadCarrier.type.baseLoadCarriers.length === 2
        ) {
          this.options = [10];
          stackHeightMinControl.patchValue(10);
        } else {
          this.options = [14, 15, 16, 17, 21];
          stackHeightMinControl.patchValue(15);
        }
        // handle base load carrier
        const baseLoadCarrierInfo = loadCarrier
          ? loadCarrier.type.baseLoadCarrier
          : null;

        if (
          !baseLoadCarrierInfo ||
          baseLoadCarrierInfo === BaseLoadCarrierInfo.None
        ) {
          baseLoadCarrierControl.clearValidators();
          baseLoadCarrierControl.setValue(null);
          baseLoadCarrierControl.disable();
        } else {
          baseLoadCarrierControl.setValidators(
            baseLoadCarrierInfo === BaseLoadCarrierInfo.Required
              ? Validators.required
              : []
          );
          baseLoadCarrierControl.enable();
          if (loadCarrier.type.baseLoadCarriers.length > 0) {
            if (baseLoadCarrierInfo === BaseLoadCarrierInfo.Optional) {
              baseLoadCarrierControl.patchValue(this.nonBaseLoadCarrier);
            } else {
              baseLoadCarrierControl.patchValue(
                loadCarrier.type.baseLoadCarriers[0]
              );
            }
          }
        }
      })
    );

    const loadConfig$ = loadConfigurationControl.valueChanges.pipe(
      debounceTime(300),
      tap(() => {
        // need to grab the value via getRaw as number per eur is a disabled form field
        const config: {
          quantityPerEur: number;
          numberOfStacks: number;
          stackHeightMin: number;
          stackHeightMax: number;
        } = loadConfigurationControl.getRawValue();
        // only checking one of the values here as they should always be be set in tandem
        if (!config.quantityPerEur) {
          return;
        }
        const quantityMin =
          config.stackHeightMin * config.numberOfStacks * config.quantityPerEur;
        const quantityMax =
          config.stackHeightMax * config.numberOfStacks * config.quantityPerEur;
        const range =
          quantityMin !== quantityMax
            ? `${quantityMin} - ${quantityMax}`
            : `${quantityMin}`;

        loadCarrierQuantityControl.patchValue(quantityMax);

        loadCarrierQuantityRangeControl.patchValue(range);

        numberOfStacksControl.patchValue(config.numberOfStacks);
      })
    );

    const stackHeightMin$ = stackHeightMinControl.valueChanges.pipe(
      // debounceTime(200),
      tap((min: number) => {
        // if (stackHeightMaxControl.value < min) {
        //   stackHeightMaxControl.patchValue(min);
        // }
        stackHeightMaxControl.patchValue(min);
      })
    );

    const stackHeightMax$ = stackHeightMaxControl.valueChanges.pipe(
      debounceTime(200),
      tap((max: number) => {
        if (stackHeightMinControl.value > max) {
          stackHeightMinControl.patchValue(max);
        }
      })
    );

    const numberOfStacks$ = numberOfStacksControl.valueChanges.pipe(
      startWith(numberOfStacksControl.value),
      map((value) => value as number)
    );

    const requiresSpecialAmount$ = requiresSpecialAmountControl.valueChanges.pipe(
      startWith(false),
      debounceTime(100),
      tap((requires: boolean) => {
        if (requires) {
          numberOfStacksControl.enable();
        } else {
          numberOfStacksControl.patchValue(33);
          numberOfStacksControl.disable();
        }
      })
    );

    const baseLoadCarrierQuantity$ = combineLatest([
      loadCarriers$,
      numberOfStacks$,
      baseLoadCarrierControl.valueChanges as Observable<LoadCarrier>,
    ]).pipe(
      tap(([{ dict }, numberOfStacks, baseLoadCarrier]) => {
        let baseLoadCarrierQuantity: number;
        // check for baseload carrier id as we have a dummy entry with id null
        // for load carriers with optional base laod carriers
        if (dict && numberOfStacks && baseLoadCarrier && baseLoadCarrier.id) {
          baseLoadCarrierQuantity =
            numberOfStacks * dict[baseLoadCarrier.id].type.quantityPerEur;
        } else {
          baseLoadCarrierQuantity = 0;
        }
        baseLoadCarrierQuantityControl.patchValue(baseLoadCarrierQuantity);
      })
    );

    const formSync$ = combineLatest([
      loadConfig$,
      stackHeightMin$,
      stackHeightMax$,
      baseLoadCarrierQuantity$,
      requiresSpecialAmount$,
    ]).pipe(
      switchMap(() => EMPTY), // we do not wanne trigger new view data
      startWith(null) // needed so viewdata outputs values
    );

    this.viewData$ = combineLatest([
      selectedLoadCarrier$,
      loadCarriers$,
      formSync$,
    ]).pipe(
      map((result) => {
        const [selectedLoadCarrier, { list }] = result;

        const viewData: IViewData = {
          loadCarriers: list,
          baseLoadCarrierDisabled: selectedLoadCarrier
            ? selectedLoadCarrier.type.baseLoadCarrier ===
              BaseLoadCarrierInfo.None
            : true,
          baseLoadCarrierInfo:
            selectedLoadCarrier && selectedLoadCarrier.type.baseLoadCarrier
              ? selectedLoadCarrier.type.baseLoadCarrier
              : null,
          baseLoadCarriers: selectedLoadCarrier
            ? selectedLoadCarrier.type.baseLoadCarriers
            : [],
        };
        return viewData;
      })
    );
  }

  selectedLoadsPerDay(event) {
    this.selectedLoadsPerDayChanged.emit({ loads: event, index: this.index });
  }

  protected getFormControls(): Controls<OrderPosition> {
    return {
      loadCarrier: new SubFormGroup<number, WrappedControlForm<number>>(
        null,
        Validators.required
      ),
      loadCarrierQuantity: new FormControl({ value: null, disabled: true }),
      loadCarrierQuantityRange: new FormControl({
        value: null,
        disabled: true,
      }),
      loadConfiguration: new FormGroup({
        quantityPerEur: new FormControl(
          { value: '', disabled: true },
          Validators.required
        ),
        requiresSpecialAmount: new FormControl(false),
        numberOfStacks: new FormControl({ value: 33, disabled: true }, [
          Validators.required,
          Validators.max(37),
          Validators.min(15),
        ]),
        // TODO fill stackheight min/max based on loading location + potentially load carrier
        stackHeightMin: new FormControl(15, [
          Validators.required,
          Validators.max(30),
          Validators.min(1),
        ]),
        stackHeightMax: new FormControl(15, [
          Validators.required,
          Validators.max(30),
          Validators.min(1),
        ]),
      }),
      baseLoadCarrier: new FormControl(null),
      baseLoadCarrierQuantity: new FormControl(
        { value: null, disabled: true },
        Validators.required
      ),
      dateRange: new FormControl({
        startDate: null,
        endDate: null,
        numberOfLoadsRange: null,
      }),
    };
  }
}
