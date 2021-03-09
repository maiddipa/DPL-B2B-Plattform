import {
  ChangeDetectorRef,
  Component,
  EventEmitter,
  Input,
  OnChanges,
  OnInit,
  Output,
  SimpleChanges,
  ViewChild,
} from '@angular/core';
import {
  AbstractControl,
  FormArray,
  FormControl,
  FormGroup,
  Validators,
} from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import {
  DriverInfo,
  DynamicConfirmationDialogComponent,
  DynamicConfirmationDialogData,
  DynamicConfirmationDialogResult,
  ExpressCodePreset,
  LicensePlate,
  LoadCarrierQuantity,
  OnBehalfOfService,
  PartnerLookupComponent,
  PartnerTypePipe,
  PrintSettings,
  VoucherExpressCode,
} from '@app/shared';
import { DocumentTypesService } from '@app/shared/services/document-types.service';
import { ValidationDataService } from '@app/shared/services/validation-data.service';
import { filterNil } from '@datorama/akita';
import { Controls, NgxRootFormComponent, SubFormGroup } from 'ngx-sub-form';
import {
  combineLatest,
  EMPTY,
  Observable,
  of,
  ReplaySubject,
  Subject,
} from 'rxjs';
import {
  distinctUntilChanged,
  first,
  map,
  pluck,
  publishReplay,
  refCount,
  startWith,
  switchMap,
  tap,
} from 'rxjs/operators';

import {
  CustomerPartner,
  EmployeeNoteType,
  PartnerType,
  PrintType,
  VouchersCreateRequest,
  VouchersCreateRequestPosition,
  VoucherStatus,
  VoucherType,
} from '../../core/services/dpl-api-services';
import { CustomersService } from '../../customers/services/customers.service';
import { LoadCarriersService } from '../../master-data/load-carriers/services/load-carriers.service';
import { VoucherReasonTypesService } from '../../master-data/voucher-reason-types/services/voucher-reason-types.service';
import { IPartner } from '../../partners/state/partner.model';
import { CustomValidators } from '../../validators';
import { VoucherService } from '../services/voucher.service';
import { CustomerDivisionsService } from '../../customers/services/customer-divisions.service';

interface IViewData {
  voucherReasonTypes: {
    value: number;
    disabled: boolean;
  }[];
  voucherTypes: {
    value: VoucherType;
    disabled: boolean;
  }[];
  customLabelsDict: any[];
  supplierRequired: boolean;
  customerReferenceRequired: boolean;
  driverRequired: boolean;
}

type RecipientSelection = {
  shipper: boolean;
  supplier: boolean;
};

export interface VoucherForm {
  expressCode: string;
  loadCarriers: LoadCarrierQuantity[];
  voucherReasonTypeId: number;
  note: string;
  recipientSelection: {
    shipper: boolean;
    supplier: boolean;
  };
  supplierIsShipper: boolean;
  supplier: IPartner;
  shipper: IPartner;
  driver: DriverInfo;
  voucherType: VoucherType;
  print: PrintSettings;
  procurementLogistics: boolean;
  customerReference: string;
}

export interface IPalletAccptanceFormState {
  palletAcceptanceForm: VoucherForm;
}

@Component({
  selector: 'app-voucher-form',
  templateUrl: './voucher-form.component.html',
  styleUrls: ['./voucher-form.component.scss'],
})
export class VoucherFormComponent
  extends NgxRootFormComponent<VouchersCreateRequest, VoucherForm>
  implements OnInit, OnChanges {
  @Input() defaultValues: Partial<VoucherForm>;
  @Input('voucher') dataInput: Required<VouchersCreateRequest>;
  @Output('valueChange') dataOutput = new EventEmitter<VouchersCreateRequest>();

  @ViewChild('shipperLookup')
  shipperLookup: PartnerLookupComponent;

  @ViewChild('supplierLookup')
  supplierLookup: PartnerLookupComponent;

  documentTypeId: number;
  expressCodePreset: ExpressCodePreset = 'voucherPresets';
  printType: PrintType = PrintType.VoucherCommon;
  viewData$: Observable<IViewData>;

  expressCode$ = new Subject<VoucherExpressCode | undefined>();

  ValidationDataService = ValidationDataService;

  validExpressCode: VoucherExpressCode;

  formResetSub = new ReplaySubject<boolean>();

  constructor(
    private voucherReasonTypeService: VoucherReasonTypesService,
    private palletAcceptanceService: VoucherService,
    public customersService: CustomersService,
    private router: Router,
    private loadCarrierService: LoadCarriersService,
    private dialog: MatDialog,
    public validationData: ValidationDataService,
    private onBehalfOfService: OnBehalfOfService,
    cd: ChangeDetectorRef,
    private partnerType: PartnerTypePipe,
    private documentTypes: DocumentTypesService
  ) {
    super(cd);
  }

  ngOnChanges(data: SimpleChanges) {
    super.ngOnChanges(data);
    this.formResetSub.next(true);
  }

  ngOnInit() {
    super.ngOnInit();
    this.formResetSub.next(true);

    this.documentTypeId = this.documentTypes.getDocumentTypeIdForLoadCarrierPickerContext(
      'voucher'
    );

    const customLabelsDict$ = this.customersService.getActiveCustomer().pipe(
      map((customer) => {
        let customLabelsDict = [];
        if (customer.customDocumentLabels?.length > 0) {
          customer.customDocumentLabels.forEach((cdl) => {
            customLabelsDict[cdl.uiLabel] = cdl.text;
          });
        }
        return customLabelsDict;
      })
    );

    const formReset$ = this.formResetSub.asObservable();

    const customerVoucherRequiredFields$ = formReset$.pipe(
      switchMap(() => {
        return this.customersService.getActiveCustomer().pipe(
          map((customer) => {
            return customer.settings?.voucher?.required
              ? customer.settings.voucher.required
              : [];
          }),
          tap((fieldNames) => {
            for (const fieldName of fieldNames) {
              if (this.formGroup.get(fieldName)) {
                if (fieldName === 'supplier') {
                  // handled by View Data
                } else if (fieldName === 'driver') {
                  // handled by View Data
                } else if (fieldName === 'customerReference') {
                  this.formGroup
                    .get(fieldName)
                    .setValidators([
                      Validators.maxLength(
                        ValidationDataService.maxLength.customerReference
                      ),
                      Validators.required,
                    ]);
                  this.formGroup.get(fieldName).updateValueAndValidity();
                } else {
                  // logic for additonal custom required fields
                }
              }
            }
          })
        );
      })
    );

    const voucherReasonTypes$ = this.voucherReasonTypeService
      .getVoucherReasonTypes()
      .pipe(
        map((reasons) => {
          return reasons
            .sort((a, b) => {
              return a.order > b.order ? 1 : -1;
            })
            .map((reason) => {
              return {
                value: reason.id,
                disabled: false,
              };
            });
        })
      );

    const recipientSelection$ = this.formGroup.controls.recipientSelection.valueChanges.pipe(
      map((value: VoucherForm['recipientSelection']) => value),
      startWith(this.formGroup.value?.recipientSelection),
      distinctUntilChanged(
        (prev, current) => !prev && !current && prev.shipper === current.shipper
      ),
      publishReplay(1),
      refCount()
    );

    const shipper$ = this.formGroup.controls.shipper.valueChanges.pipe(
      map((value: IPartner) => value),
      startWith(this.formGroup.controls.shipper.value),
      publishReplay(1),
      refCount()
    );

    const supplier$ = this.formGroup.controls.supplier.valueChanges.pipe(
      map((value: IPartner) => value),
      startWith(this.formGroup.controls.supplier.value),
      tap((value) => {
        if (value) {
          if (this.validExpressCode?.voucherPresets?.shipper) {
            return;
          }

          this.formGroup.controls.supplierIsShipper.enable();
        } else {
          this.formGroup.controls.supplierIsShipper.disable();
          this.formGroup.controls.supplierIsShipper.patchValue(false);
        }
      }),
      publishReplay(1),
      refCount()
    );

    const recipientPostingAccountId$ = combineLatest([
      recipientSelection$,
      shipper$,
      supplier$,
    ]).pipe(
      map(([recipientSelection, shipper, supplier]) => {
        if (!recipientSelection) {
          return null;
        }

        if (recipientSelection.shipper && shipper) {
          return shipper.postingAccountId;
        } else if (supplier) {
          return supplier.postingAccountId;
        }

        return null;
      }),
      distinctUntilChanged()
    );

    const voucherTypes$ = combineLatest([
      recipientPostingAccountId$,
      this.expressCode$.pipe(startWith(null)),
    ]).pipe(
      map(([postingAccountId, expressCode]) => {
        const voucherType = expressCode?.voucherType || VoucherType.Original;

        const voucherOptions: IViewData['voucherTypes'] = [
          {
            value: VoucherType.Direct,
            //disabled: true,
            disabled: VoucherType.Direct !== voucherType,
          },
          // commented out for demo
          // {
          //   value: VoucherType.Digital,
          //   disabled: VoucherType.Digital !== voucherType,
          // },
          {
            value: VoucherType.Original,
            disabled: VoucherType.Original !== voucherType,
          },
        ];
        return voucherOptions;
      }),
      tap((voucherTypes) => {
        // if current voucher type is disabled
        // update voucher type to fist that is not disabled
        const currentValue = this.formGroup.controls.voucherType.value;
        if (voucherTypes.find((i) => i.value === currentValue && i.disabled)) {
          const firstEnabled = voucherTypes.find((i) => !i.disabled).value;
          this.formGroup.controls.voucherType.patchValue(firstEnabled);
        }
      })
    );

    const voucherType$ = this.formGroup.controls.voucherType.valueChanges.pipe(
      startWith(this.formGroup.controls.voucherType.value)
      // tap((voucherType: VoucherType) => {
      //   console.log('VOUCHER TYPE', voucherType);
      //   this.defaultValues = {
      //     ...this.defaultValues,
      //     voucherType,
      //   };
      // })
    );

    const supplierIsShipper$ = this.formGroup.controls.supplierIsShipper.valueChanges.pipe(
      tap((supplierIsShipper) => {
        if (supplierIsShipper) {
          this.onRecipientChanged('supplier');
          this.formGroup.controls.shipper.get('option').disable();
          this.formGroup.controls.recipientSelection.disable();
        } else {
          if (this.validExpressCode?.voucherPresets?.shipper) {
            // if (
            //   this.validExpressCode?.voucherPresets?.shipper ||
            //   this.validExpressCode?.voucherPresets?.recipient
            // ) {
            // don't enable recipient checkboxes if valid express code is set
            return;
          }
          this.formGroup.controls.shipper.get('option').enable();
          if (this.validExpressCode?.voucherPresets?.recipient) {
            return;
          }
          this.formGroup.controls.recipientSelection.enable();
        }
      }),
      startWith(this.formGroup.controls.supplierIsShipper.value),
      distinctUntilChanged(),
      switchMap((supplierIsShipper) => {
        return !supplierIsShipper
          ? of(supplierIsShipper)
          : supplier$.pipe(
              tap((supplier) => {
                const shipper = this.formGroup.controls.shipper;
                shipper.patchValue({
                  ...supplier,
                  type: this.partnerType.transform(supplier.type),
                });
              }),
              switchMap(() => EMPTY),
              startWith(supplierIsShipper)
            );
      })
    );

    const supplierIsRecipient$ = combineLatest([
      this.formGroup.controls.recipientSelection.get('supplier').valueChanges,
      supplierIsShipper$,
    ]).pipe(
      map(([supplierIsRecipient, supplierIsShipper]) => {
        return supplierIsRecipient || supplierIsShipper;
      }),
      distinctUntilChanged(),
      tap((supplierIsRecipient) => {
        const required = supplierIsRecipient;

        const supplierControl = this.formGroup.controls.supplier;

        // TODO check if we still need these validtors on parent
        supplierControl.setValidators(
          required
            ? [Validators.required, CustomValidators.isObject]
            : [CustomValidators.isObject]
        );

        // TODO check if this actually works as expected
        if (this.supplierLookup) {
          this.supplierLookup.setRequiredState(required);
        }

        supplierControl.updateValueAndValidity();
      })
    );

    const destinationLoadCarrier$ = this.expressCode$.pipe(
      filterNil,
      pluck('destinationAccountPreset', 'loadCarrierId'),
      tap((loadCarrierId) => {
        if (!loadCarrierId) {
          return;
        }

        const formArray = this.formGroup.controls.loadCarriers.get(
          'innerControl.innerControl'
        ) as FormArray;

        formArray.controls.forEach((control, i) => {
          if (control.value.id !== loadCarrierId) {
            formArray.removeAt(i);
            return;
          }

          control.get('id').disable();
        });
      })
    );

    const formSync$ = combineLatest([
      voucherType$,
      supplierIsRecipient$,
      destinationLoadCarrier$,
    ]).pipe(
      switchMap(() => EMPTY),
      startWith(null)
    );

    this.viewData$ = combineLatest([
      voucherReasonTypes$,
      voucherTypes$,
      customLabelsDict$,
      customerVoucherRequiredFields$,
      formSync$,
    ]).pipe(
      //delay(0), // neccessary as some obs are based on value changes
      map(
        ([
          voucherReasonTypes,
          voucherTypes,
          customLabelsDict,
          customRequiredFields,
        ]) => {
          const viewData: IViewData = {
            voucherReasonTypes,
            voucherTypes,
            customLabelsDict,
            supplierRequired: customRequiredFields.indexOf('supplier') > -1,
            customerReferenceRequired:
              customRequiredFields.indexOf('customerReference') > -1,
            driverRequired: customRequiredFields.indexOf('driver') > -1,
          };
          return viewData;
        }
      ),
      tap((viewData) => console.log('ViewData', viewData)),
      publishReplay(1),
      refCount()
    );
  }

  onRecipientChanged(recipient: 'shipper' | 'supplier') {
    const recipientSelection = this.formGroup.controls.recipientSelection;

    const selection: VoucherForm['recipientSelection'] = {
      shipper: recipient === 'shipper',
      supplier: recipient === 'supplier',
    };

    recipientSelection.patchValue(selection);

    this.defaultValues = {
      ...this.defaultValues,
      recipientSelection: selection,
    };
  }

  onExpressCode(expressCode: VoucherExpressCode) {
    if (!expressCode) {
      this.validExpressCode = undefined;
      return;
    }

    // reset form before applying express code
    this.reset();

    this.formGroup.controls.expressCode.patchValue(expressCode.digitalCode, {
      emitEvent: false,
    });

    if (expressCode.demoProperties) {
      this.validExpressCode = expressCode;
      // HACK Demo Express Code CodeZZ
      this.patch(
        this.formGroup.controls.supplier,
        expressCode.demoProperties.supplier,
        !!expressCode.demoProperties.supplier
      );
      this.formGroup.controls.supplier.disable();

      this.patch(
        this.formGroup.controls.shipper,
        expressCode.demoProperties.shipper,
        !!expressCode.demoProperties.shipper
      );
      this.formGroup.controls.shipper.disable();

      this.patch(
        this.formGroup.controls.voucherReasonTypeId,
        expressCode.demoProperties.ntg
      );
      this.formGroup.controls.voucherReasonTypeId.disable();

      this.patch(
        this.formGroup.controls.voucherType,
        expressCode.demoProperties.voucherType
      );
      this.formGroup.controls.voucherType.disable();

      this.patch(
        this.formGroup.controls.loadCarriers,
        expressCode.demoProperties.loadCarrier
      );
      this.formGroup.controls.loadCarriers.disable();

      this.formGroup.controls.recipientSelection.disable();
      this.formGroup.controls.supplierIsShipper.disable();
    } else {
      this.validExpressCode = expressCode;

      this.expressCode$.next(expressCode);
      this.patch(
        this.formGroup.controls.voucherReasonTypeId,
        expressCode.voucherReasonTypeId
      );

      if (!expressCode.voucherPresets) {
        return;
      }

      const localizePartner = (partner: CustomerPartner) => {
        return {
          ...partner,
          type: this.partnerType.transform(partner.type),
        };
      };

      //Supplier check
      if (expressCode.voucherPresets.supplier) {
        this.patch(
          this.formGroup.controls.supplier,
          localizePartner(expressCode.voucherPresets.supplier),
          !!expressCode.voucherPresets.supplier
        );
        this.formGroup.controls.supplier.disable();
      }

      // Shipper check
      if (expressCode.voucherPresets.shipper) {
        this.patch(
          this.formGroup.controls.shipper,
          localizePartner(expressCode.voucherPresets.shipper),
          !!expressCode.voucherPresets.shipper
        );
        this.formGroup.controls.shipper.disable();
        this.formGroup.controls.supplierIsShipper.disable();
      }

      //Recipient check
      if (
        expressCode.voucherPresets.recipient &&
        (expressCode.voucherPresets.recipient?.id ===
          expressCode.voucherPresets.shipper?.id ||
          expressCode.voucherPresets.recipient?.id ===
            expressCode.voucherPresets.supplier?.id)
      ) {
        this.patch(this.formGroup.controls.recipientSelection, {
          shipper:
            expressCode.voucherPresets.recipient?.id ===
            expressCode.voucherPresets.shipper?.id,
          supplier:
            expressCode.voucherPresets.recipient?.id ===
            expressCode.voucherPresets.supplier?.id,
        });
        this.formGroup.controls.recipientSelection.disable();
      }
    }
  }

  private patch(control: AbstractControl, value: any, update: boolean = true) {
    if (!value || !update) {
      return;
    }

    control.patchValue(value);
  }

  onCreateVoucher() {
    if (!this.formGroup.valid) {
      this.formGroup.markAllAsTouched();
      return;
    }

    this.manualSave();

    this.onBehalfOfService
      .openOnBehalfofDialog(EmployeeNoteType.Create)
      .pipe(
        switchMap((dplNote) => {
          return this.getConfirmation(
            this.dataValue.type || VoucherType.Direct
          ).pipe(
            switchMap((result) => {
              if (!result || !result.confirmed) {
                return EMPTY;
              }

              return of(result);
            }),
            switchMap((result) => {
              const request = this.formGroup.getRawValue();
              return this.palletAcceptanceService
                .createVoucher(request, result.print, dplNote)
                .pipe(
                  map((voucher) => {
                    // check voucher is canceled -> redirect DOR
                    if (voucher.status === VoucherStatus.Canceled) {
                      const canceledResult = { ...result, redirect: true };
                      return { voucher, result: canceledResult };
                    }
                    return { voucher, result };
                  })
                );
            }),
            switchMap(({ voucher, result }) => {
              if (!result.redirect) {
                this.reset();
                return EMPTY;
              }

              return this.loadCarrierService
                .getLoadCarrierById(voucher.positions[0].loadCarrierId)
                .pipe(
                  map((loadCarrier) =>
                    this.router.navigate(['voucher-register/start'], {
                      queryParams: {
                        carrierType: loadCarrier.type,
                        highlightId: voucher.id,
                      },
                    })
                  )
                );
            })
          );
        })
      )
      .subscribe();
  }

  private getConfirmation(voucherType: VoucherType) {
    const { title, description } = (() => {
      switch (voucherType) {
        case VoucherType.Direct:
          return {
            title: $localize`:Titel für Dialog zur Bestätigung der erstellung von Gutschrift (Direkt-Buchung)@@VoucherDirectConfirmationDialogTitle:Digitale Pooling Gutschrift erstellen (Direkt-Buchung)`,
            description: $localize`:Beschreibung für Dialog zur Bestätigung von Gutschriften (Direkt-Buchung)@@VoucherDirectConfirmationDialogDescription:Dieser Vorgang erzeugt eine Buchung.`,
          };
        case VoucherType.Digital:
          return {
            title: $localize`:Titel für Dialog zur Bestätigung der erstellung von Gutschrift (Digitale Einreichung)@@VoucherDigitalConfirmationDialogTitle:Digitale Pooling Gutschrift erstellen (Digitale Einreichung)`,
          };
        case VoucherType.Original:
          return {
            title: $localize`:Titel für Dialog zur Bestätigung der erstellung von Gutschrift (Original Einreichung)@@VoucherOriginalConfirmationDialogTitle:Digitale Pooling Gutschrift erstellen (Original Einreichung)`,
          };
        default:
          throw new Error(`Unknown VoucherType : ${voucherType}`);
      }
    })();

    return this.dialog
      .open<
        DynamicConfirmationDialogComponent,
        DynamicConfirmationDialogData,
        DynamicConfirmationDialogResult
      >(DynamicConfirmationDialogComponent, {
        data: {
          labels: {
            title,
            description,
          },
          print: {
            show: true,
            disabled: voucherType !== VoucherType.Direct,
          },
          redirect: true,
          defaultValues: {
            print: true, //voucherType !== VoucherType.Direct,
            redirect: false,
          },
        },
        disableClose: true,
        autoFocus: false,
      })
      .afterClosed();
  }

  reset() {
    this.expressCode$.next();
    this.formResetSub.next(true);
    this.formGroup.reset();
    this.formGroup.controls.shipper.get('option').enable();
    this.formGroup.controls.supplier.get('option').enable();
    this.formGroup.controls.loadCarriers.enable();
    this.formGroup.controls.supplierIsShipper.enable();
    this.formGroup.controls.recipientSelection.enable();
  }

  protected getDefaultValues() {
    return this.defaultValues;
  }

  protected transformFromFormGroup(value: VoucherForm): VouchersCreateRequest {
    // if (value.expressCode === 'CODEZZZ') {
    //   value.voucherType = VoucherType.Direct;
    //   value.voucherReasonTypeId = 1;
    //   value.loadCarriers = [{ id: 203, quantity: 33 }];
    //   value.shipper = {
    //     id: 9,
    //     guid: 'b2649443-5a1d-45be-b08e-6183eeb00327',
    //     type: PartnerType.Shipper,
    //     companyName: 'Spedition #2',
    //     address: {
    //       id: 56,
    //       street1: 'Schillerstrasse 88',
    //       postalCode: '86568',
    //       city: 'Hollenbach',
    //       state: 16,
    //       country: 1
    //     },
    //     postingAccountId: 9,
    //     directoryIds: [5],
    //     directoryNames: ['Standard']
    //   };
    //   (value.supplier = {
    //     id: 19,
    //     guid: 'df3a2864-9017-4371-9c35-a905461fd219',
    //     type: PartnerType.Default,
    //     companyName: 'Lieferant #2',
    //     address: {
    //       id: 43,
    //       street1: 'Brandenburgische Strasse 89',
    //       postalCode: '76857',
    //       city: 'Waldrohrbach',
    //       state: 1,
    //       country: 1
    //     },
    //     postingAccountId: 14,
    //     directoryIds: [],
    //     directoryNames: []
    //   }),
    //     (value.supplierIsShipper = false);
    //   value.recipientSelection = { shipper: true, supplier: false };
    // }

    const recipient =
      value.recipientSelection && value.recipientSelection.shipper
        ? value.shipper
        : value.supplier;

    const driver =
      value.driver ||
      ({
        licensePlate: {},
      } as DriverInfo);

    const licensePlate =
      value.driver && value.driver.licensePlate
        ? value.driver.licensePlate
        : ({} as LicensePlate);
    const print = value.print || ({} as PrintSettings);

    return {
      positions: (value.loadCarriers || []).map(
        (i) =>
          <VouchersCreateRequestPosition>{
            loadCarrierId: i.id,
            quantity: i.quantity,
          }
      ),
      recipientGuid: recipient?.guid,
      recipientType: value.recipientSelection?.shipper
        ? PartnerType.Shipper
        : PartnerType.Supplier,
      supplierGuid: value.supplier ? value.supplier.guid : null,
      shipperGuid: value.shipper ? value.shipper.guid : null,
      reasonTypeId: value.voucherReasonTypeId,
      note: value.note,
      licensePlate: licensePlate.number,
      licensePlateCountryId: licensePlate.countryId,
      truckDriverName: driver.driverName,
      truckDriverCompanyName: driver.companyName,
      type: value.voucherType,
      customerReference: value.customerReference,
      procurementLogistics: value.procurementLogistics,
      printLanguageId: print.languageId,
      printCount: print.count,
      expressCode: this.validExpressCode ? value.expressCode : undefined,
    };
  }

  transformToFormGroup(
    request: VouchersCreateRequest,
    defaultValues: Partial<VoucherForm>
  ): VoucherForm {
    const {
      type,
      expressCode,
      note,
      positions,
      truckDriverName,
      truckDriverCompanyName,
      licensePlate,
      licensePlateCountryId,
      customerReference,
      procurementLogistics,
      printLanguageId,
      printCount,
    } = request || {};

    const print =
      !printCount || !printLanguageId
        ? null
        : {
            count: printCount,
            languageId: printLanguageId,
          };

    return {
      expressCode,
      voucherReasonTypeId: null,
      voucherType: type,
      recipientSelection: {
        shipper: true,
        supplier: false,
      },
      shipper: null,
      supplier: null,
      supplierIsShipper: false,
      loadCarriers: (positions || []).map((p) => ({
        id: p.loadCarrierId,
        quantity: p.quantity,
      })),
      driver: {
        driverName: truckDriverName,
        companyName: truckDriverCompanyName,
        licensePlate: {
          countryId: licensePlateCountryId,
          number: licensePlate,
        },
      },
      customerReference,
      procurementLogistics,
      note,
      print,
    };
  }

  protected getFormControls(): Controls<VoucherForm> {
    return {
      expressCode: new SubFormGroup(
        { value: null, disabled: false },
        {
          updateOn: 'change',
          validators: [
            Validators.minLength(ValidationDataService.minLength.expressCode),
            Validators.maxLength(ValidationDataService.maxLength.expressCode),
          ],
        } // AbstractControlOptions
      ),
      loadCarriers: new SubFormGroup(
        null,
        Validators.minLength(ValidationDataService.minLength.loadCarriers)
      ),
      voucherReasonTypeId: new FormControl(null, Validators.required),
      note: new FormControl(
        '',
        Validators.maxLength(ValidationDataService.maxLength.note)
      ),
      recipientSelection: new FormGroup({
        shipper: new FormControl(null, undefined),
        supplier: new FormControl(null, undefined),
      }),
      supplierIsShipper: new FormControl(null, undefined),
      shipper: new SubFormGroup(null),
      supplier: new SubFormGroup(null),
      driver: new SubFormGroup(null),
      voucherType: new FormControl(null, Validators.required),
      customerReference: new FormControl(
        null,
        Validators.maxLength(ValidationDataService.maxLength.customerReference)
      ),
      procurementLogistics: new FormControl(null),
      print: new SubFormGroup(null),
    };
  }
}
