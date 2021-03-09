import {
  ChangeDetectorRef,
  Component,
  EventEmitter,
  Input,
  OnChanges,
  OnDestroy,
  OnInit,
  Output,
  SimpleChanges,
} from '@angular/core';
import { FormControl, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import {
  EmployeeNoteType,
  ExpressCode,
  LoadCarrierReceiptDepotPreset,
  LoadCarrierReceiptDepotPresetCategory,
  LoadCarrierReceiptPreset,
  LoadCarrierReceiptsCreateRequest,
  LoadCarrierReceiptType,
  OrderType, PrintType,
} from '@app/api/dpl';
import {
  DriverInfo,
  DynamicConfirmationDialogComponent,
  DynamicConfirmationDialogData,
  DynamicConfirmationDialogResult,
  ExpressCodePreset,
  LoadCarrierPickerContext,
  LoadCarrierQuantity,
  OnBehalfOfService,
  PrintSettings,
  Shipper,
  ShipperForm,
  ValidationDataService,
} from '@app/shared';
import { DocumentTypesService } from '@app/shared/services/document-types.service';
import { checkResetRadio } from '@dpl/dpl-lib';
import {
  Controls,
  NgxAutomaticRootFormComponent,
  SubFormGroup,
} from 'ngx-sub-form';
import {
  BehaviorSubject,
  combineLatest,
  EMPTY,
  Observable,
  of,
  ReplaySubject,
} from 'rxjs';
import {
  first,
  map,
  publishReplay,
  refCount,
  switchMap,
  takeUntil,
  tap,
} from 'rxjs/operators';

import { compareById } from '../../core';
import { CustomersService } from '../../customers/services/customers.service';
import {
  SortDialogData,
  SortDialogResult,
  SortingDialogComponent,
} from '../../sorting/components/sorting-dialog/sorting-dialog.component';
import { SortingService } from '../../sorting/services/sorting.service';
import { LoadCarrierReceipt, LoadCarrierReceiptService } from '../services';
import {CustomerDivisionsService} from "../../customers/services/customer-divisions.service";
import {AccountsService} from "../../accounts/services/accounts.service";

export type LoadCarrierReceiptContext = 'depot' | undefined;

type ViewData = {
  generated: boolean;
  depoPresets: LoadCarrierReceiptDepotPreset[];
};

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'app-load-carrier-receipt-form',
  templateUrl: './load-carrier-receipt-form.component.html',
  styles: [],
})
export class LoadCarrierReceiptFormComponent
  extends NgxAutomaticRootFormComponent<
    LoadCarrierReceiptsCreateRequest,
    LoadCarrierReceipt
  >
  implements OnInit, OnChanges, OnDestroy {
  @Input() action: LoadCarrierReceiptType;
  @Input() context: LoadCarrierReceiptContext;
  @Input('receipt') dataInput: Required<LoadCarrierReceiptsCreateRequest>;
  @Output('valueChange') dataOutput = new EventEmitter<
    LoadCarrierReceiptsCreateRequest
  >();
  loadCarrierReceiptType = LoadCarrierReceiptType;
  loadCarrierPickerContext: LoadCarrierPickerContext;
  documentTypeId: number;
  expressCodePreset: ExpressCodePreset = 'loadCarrierReceiptPreset';
  compareById = compareById;
  loadCarrierReceiptPreset: LoadCarrierReceiptPreset;
  checkResetRadio = checkResetRadio;
  possibleOrderTypes$ = new ReplaySubject<OrderType[]>();
  viewData$: Observable<ViewData>;
  generated$ = new BehaviorSubject<boolean>(false);

  getPrintType() {
    switch (this.action){
      case LoadCarrierReceiptType.Pickup:
        return PrintType.LoadCarrierReceiptPickup;
      case LoadCarrierReceiptType.Delivery:
        return PrintType.LoadCarrierReceiptDelivery;
      case LoadCarrierReceiptType.Exchange:
        return PrintType.LoadCarrierReceiptExchange;
    }
  }

  dataInput$ = new ReplaySubject<LoadCarrierReceiptsCreateRequest>();

  // HACK this is not used as of today and should be set servside rather than clientside
  // we should likely have a mode that tells us if we should fall back the "Intermediate account"
  // removed unkownTargetPostingAccountId = 3;
  unkownTargetPostingAccountId = 1;

  digitalCodeSet = false;

  ValidationDataService = ValidationDataService;

  minDate = new Date();

  constructor(
    private receiptService: LoadCarrierReceiptService,
    private customerDivisionsService: CustomerDivisionsService,
    private accountsService: AccountsService,
    private dialog: MatDialog,
    private router: Router,
    private onBehalfOfService: OnBehalfOfService,
    private documentTypes: DocumentTypesService,
    private customersService: CustomersService,
    private sortingService: SortingService,
    cd: ChangeDetectorRef
  ) {
    super(cd);
  }

  ngOnChanges(changes: SimpleChanges) {
    super.ngOnChanges(changes);
    // this is neccessary as when switching between actions there is no navigation event hence the form would not be cleared
    if (changes.action) {
      this.loadCarrierPickerContext = this.getLoadCarrierPickerContext(
        this.action
      );
      this.documentTypeId = this.documentTypes.getDocumentTypeIdForLoadCarrierPickerContext(
        this.loadCarrierPickerContext
      );
      this.possibleOrderTypes$
        .next
        //this.receiptService.getPossibleOrderTypes(this.action)
        ();
    }

    if (!this.dataInput && (changes.context || changes.action)) {
      this.reset();
    }

    if (changes['dataInput']) {
      this.dataInput$.next(this.dataInput);
    }
  }

  ngOnInit(): void {
    super.ngOnInit();

    // this.formsManager.upsert(
    //   'loadCarrierReceipt',
    //   (this.formGroup as unknown) as FormGroup,
    //   {
    //     persistForm: true,
    //   }
    // );

    const depoPresets$ = this.customersService.getActiveCustomer().pipe(
      map((customer) => customer.loadCarrierReceiptDepotPresets),
      publishReplay(1),
      refCount()
    );

    const depoPreset$ = combineLatest([
      this.formGroup.controls.depoPresetId.valueChanges,
      depoPresets$,
    ]).pipe(
      tap(([presetId, presets]) => {
        if (!presetId) {
          return;
        }

        const preset = presets.find((i) => i.id === presetId);
        if (!preset) {
          return;
        }

        const loadCarrierQuantites = preset.loadCarriersIds.map((id) => {
          const loadCarrierQuantity: LoadCarrierQuantity = {
            id,
            quantity: null,
          };

          return loadCarrierQuantity;
        });

        const instructions = this.getInstructions(preset.category);
        this.formGroup.controls.instructions.setValue(instructions);
        this.formGroup.controls.instructions.disable();

        this.formGroup.controls.digitalCode.disable();
        this.formGroup.controls.loadCarriers.setValue(loadCarrierQuantites);
        this.formGroup.controls.isSortingRequired.setValue(
          preset.isSortingRequired || false
        );
      })
    );

    // HACK der untere code block hat keinerlei funktion mehr
    // nach einführung der express code componente für das durchsuchen der orders
    // wird possibleOrderTypes nicht mehr mit werten gefüttert
    // daher emittet combinedLatest nie
    const digitalCode$ = combineLatest([
      this.dataInput$,
      this.possibleOrderTypes$,
    ]).pipe(
      tap(([data]) => {
        if (!data || !data.digitalCode) {
          this.formGroup.controls.digitalCode.patchValue(null);
          return;
        }

        this.formGroup.controls.digitalCode.patchValue(data.digitalCode);

        // TODO anschauen Niko - setzen der Werte...
        this.receiptService
          .findDigitalCode(data.digitalCode, this.action)
          .pipe(first())
          .subscribe((expressCode) => this.onDigitalCode(expressCode));

        this.generated$.next(true);
      })
    );

    //NOTE: required wird ungültig wenn weitere Validatoren exitieren, kann aber auch möglicherweise gewollt sein.
    // not needed anymore
    // this.formGroup.setValidators([
    //   atLeastOneValidator(
    //     [
    //       this.formGroup.controls.pickupNoteNumber,
    //       this.formGroup.controls.deliveryNoteNumber,
    //     ],
    //     [Validators.required]
    //   ),
    // ]);
    this.setNoteNumberValidators();

    const deliveryNoteNumber$ = this.formGroup.controls.deliveryNoteNumber.valueChanges.pipe(
      tap((value: string) => {
        const validators = [
          Validators.maxLength(
            ValidationDataService.maxLength.deliveryNoteNumber
          ),
        ];

        if (value && value.length > 0) {
          validators.push(Validators.required);
        }

        this.formGroup.controls.deliveryNoteDocument.setValidators(validators);
        this.formGroup.controls.deliveryNoteDocument.updateValueAndValidity();
      })
    );

    const pickupNoteNumber$ = this.formGroup.controls.pickupNoteNumber.valueChanges.pipe(
      tap((value: string) => {
        const validators = [
          Validators.maxLength(
            ValidationDataService.maxLength.pickupNoteNumber
          ),
        ];

        if (value && value.length > 0) {
          validators.push(Validators.required);
        }

        this.formGroup.controls.pickupNoteDocument.setValidators(validators);
        this.formGroup.controls.pickupNoteDocument.updateValueAndValidity();
      })
    );

    const generated$ = this.generated$.pipe(
      tap((generated) => {
        if (generated) {
          // this.formGroup.controls.deliveryNoteNumber.disable();
          // this.formGroup.controls.pickupNoteNumber.disable();
          this.formGroup.controls.date.disable();
          this.formGroup.controls.sourcePostingAccountId.disable();
          this.formGroup.controls.targetPostingAccountId.disable();
          this.formGroup.controls.digitalCode.disable();
        } else {
          this.formGroup.controls.digitalCode.reset();
          this.formGroup.controls.digitalCode.enable();
          this.formGroup.controls.deliveryNoteNumber.enable();
          this.formGroup.controls.pickupNoteNumber.enable();
          this.formGroup.controls.date.enable();
          this.formGroup.controls.sourcePostingAccountId.enable();

          // HACK for now always disabled
          this.formGroup.controls.targetPostingAccountId.setValue(
            this.unkownTargetPostingAccountId
          );
          //this.formGroup.controls.targetPostingAccountId.enable();
        }
      })
    );

    const formSync$ = combineLatest([
      depoPreset$,
      digitalCode$,
      deliveryNoteNumber$,
      pickupNoteNumber$,
    ]).pipe(
      switchMap(() => EMPTY) // we do not wanne trigger new view data
    );

    this.viewData$ = combineLatest([depoPresets$, generated$]).pipe(
      takeUntil(formSync$),
      map(([depoPresets, generated]) => ({ depoPresets, generated }))
    );
  }

  onDigitalCode(expressCode: ExpressCode) {
    if (!expressCode) {
      this.digitalCodeSet = true;
      this.generated$.next(false);
      return;
    }
    this.digitalCodeSet = true;
    this.loadCarrierReceiptPreset = expressCode.loadCarrierReceiptPreset;
    const receiptData = this.receiptService.generateReceiptFromExpressCode(
      this.action,
      expressCode
    );
    this.formGroup.patchValue(this.transformFromFormGroup(receiptData));

    // hack, patch fullfillment date on form (date not exists on createRequest object)
    this.formGroup.controls.date.patchValue(receiptData.date);

    // set validators for pickupnotenumber and deliverynotenumber
    this.setNoteNumberValidators();

    this.generated$.next(true);
  }

  setNoteNumberValidators() {
    if (this.digitalCodeSet) {
      this.formGroup.controls.pickupNoteNumber.setValidators([
        Validators.maxLength(ValidationDataService.maxLength.pickupNoteNumber),
      ]);
      this.formGroup.controls.deliveryNoteNumber.setValidators([
        Validators.maxLength(
          ValidationDataService.maxLength.deliveryNoteNumber
        ),
      ]);
    } else {
      if (this.action === LoadCarrierReceiptType.Pickup) {
        this.formGroup.controls.pickupNoteNumber.setValidators([
          Validators.required,
          Validators.maxLength(
            ValidationDataService.maxLength.pickupNoteNumber
          ),
        ]);
        this.formGroup.controls.deliveryNoteNumber.setValidators([
          Validators.maxLength(
            ValidationDataService.maxLength.deliveryNoteNumber
          ),
        ]);
      } else {
        this.formGroup.controls.pickupNoteNumber.setValidators([
          Validators.maxLength(
            ValidationDataService.maxLength.pickupNoteNumber
          ),
        ]);
        this.formGroup.controls.deliveryNoteNumber.setValidators([
          Validators.required,
          Validators.maxLength(
            ValidationDataService.maxLength.deliveryNoteNumber
          ),
        ]);
      }
    }
    this.formGroup.controls.deliveryNoteNumber.updateValueAndValidity();
    this.formGroup.controls.pickupNoteNumber.updateValueAndValidity();
  }

  reset() {
    this.digitalCodeSet = false;
    this.formGroup.controls.digitalCode.enable();
    this.formGroup.reset();
    this.setNoteNumberValidators();
    // todo set validators based on action
    this.generated$.next(false);
  }

  ngOnDestroy(): void {
    this.generated$.next(false);
    this.formGroup.reset();

    super.ngOnDestroy();
  }

  onSubmit() {
    if (this.formGroup.valid) {
      this.onBehalfOfService
        .openOnBehalfofDialog(EmployeeNoteType.Create)
        .pipe(
          switchMap((dplNote) => {
            return this.getConfirmation().pipe(
              switchMap((result) => {
                if (!result || !result.confirmed) {
                  return EMPTY;
                }

                return of(result);
              }),
              switchMap((result) => {
                return this.receiptService
                  .createReceipt(result.print, { ...this.dataValue, dplNote })
                  .pipe(
                    map((response) => {
                      return { result, response };
                    })
                  );
              }),
              tap((data) => {
                if (!data.result.redirect) {
                  this.reset();
                }
              }),

              switchMap((data) => {
                // open sort dialog if required
                if (data.response.isSortingRequired) {
                  //open sort confirmation dialog
                  return this.sortingService
                    .sortDisplayConfirmationDialog()
                    .pipe(
                      switchMap((displayConfirmationResult) => {
                        return displayConfirmationResult?.confirmed
                          ? this.dialog
                              .open<
                                SortingDialogComponent,
                                SortDialogData,
                                SortDialogResult
                              >(SortingDialogComponent, {
                                width: '1000px',
                                data: {
                                  receiptId: data.response.id,
                                },
                                hasBackdrop: false,
                                autoFocus: false,
                              })
                              .afterClosed()
                              .pipe(map((sortResult) => data))
                          : of(data);
                      })
                    );
                }
                return of(data);
              }),
              map((data) => {
                if (data.result.redirect) {
                  //data.response.positions[""0""].loadCarrierId
                  // TODO get load carrier type from load carrier id to add as query param
                  return this.router.navigate(['/load-carrier-receipts'], {
                    queryParams: {
                      loadCarrierId:
                        data.response.positions[0].loadCarrierId || 0,
                    },
                  });
                }
                return data.response;
              })
            );
          })
        )

        .subscribe({
          complete: () => {},
        });
    }
  }

  private getConfirmation() {
    const title = (() => {
      switch (this.action) {
        case LoadCarrierReceiptType.Delivery:
          return $localize`:Titel für Dialog zur Bestätigung von Annahme-Quittung@@LoadCarrierReceiptDeliveryConfirmationDialogTitle:Annahme-Quittung erstellen`;

        case LoadCarrierReceiptType.Pickup:
          return $localize`:Titel für Dialog zur Bestätigung von Ausgabe-Quittung@@LoadCarrierReceiptPickupConfirmationDialogTitle:Ausgabe-Quittung erstellen`;

        case LoadCarrierReceiptType.Exchange:
          return $localize`:Titel für Dialog zur Bestätigung von Tausch-Quittung@@LoadCarrierReceiptExchangeConfirmationDialogTitle:Tausch-Quittung erstellen`;

        default:
          throw new Error(`Action is not supported: ${this.action}`);
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
          },
          print: {
            show: true,
            disabled:
              this.action === LoadCarrierReceiptType.Pickup ||
              this.action === LoadCarrierReceiptType.Exchange,
          },
          redirect: true,
          defaultValues: {
            print: true,
            redirect: false,
          },
        },
        disableClose: true,
        autoFocus: false,
      })
      .afterClosed();
  }

  protected getFormControls(): Controls<LoadCarrierReceipt> {
    return {
      type: new FormControl(null),
      generated: new FormControl(null),
      depoPresetId: new FormControl(null),
      sourcePostingAccountId: new SubFormGroup(null),
      targetPostingAccountId: new SubFormGroup(null),
      digitalCode: new SubFormGroup(null),
      date: new FormControl({ value: null, disabled: true }),
      deliveryNoteNumber: new FormControl(null, [
        Validators.maxLength(
          ValidationDataService.maxLength.deliveryNoteNumber
        ),
      ]),
      deliveryNoteDocument: new FormControl(null),
      pickupNoteNumber: new FormControl(null, [
        Validators.maxLength(ValidationDataService.maxLength.pickupNoteNumber),
      ]),
      pickupNoteDocument: new FormControl(null),
      referenceNumber: new FormControl(null, [
        Validators.maxLength(ValidationDataService.maxLength.referenceNumber),
      ]),
      instructions: new FormControl({ value: null, disabled: true }),
      note: new FormControl(null, [
        Validators.maxLength(ValidationDataService.maxLength.note),
      ]),
      loadCarriers: new SubFormGroup(null),
      driver: new SubFormGroup<DriverInfo>(null),
      shipper: new SubFormGroup<Shipper, ShipperForm>(null),
      print: new SubFormGroup<PrintSettings>(null),
      isSortingRequired: new FormControl(null),
    };
  }

  public getDefaultValues(): Partial<LoadCarrierReceipt> {
    return {
      type: null,
      generated: false,
      depoPresetId: null,
      sourcePostingAccountId: null,
      targetPostingAccountId: this.unkownTargetPostingAccountId,
      digitalCode: null,
      date: new Date(),
      deliveryNoteNumber: null,
      deliveryNoteDocument: null,
      pickupNoteNumber: null,
      pickupNoteDocument: null,
      referenceNumber: null,
      instructions: null,
      note: null,
      loadCarriers: [{ id: null, quantity: null }],
      // driver: undefined,
      // shipper: undefined,
      print: null,
    };
  }

  protected transformFromFormGroup(
    formValue: LoadCarrierReceipt
  ): LoadCarrierReceiptsCreateRequest {
    return {
      digitalCode: formValue.digitalCode,
      depoPresetId: formValue.depoPresetId,
      isSortingRequired: formValue.isSortingRequired,
      targetPostingAccountId: formValue.targetPostingAccountId,
      refLmsBusinessTypeId: this.loadCarrierReceiptPreset?.refLmsBusinessTypeId,
      refLtmsTransactionRowGuid: this.loadCarrierReceiptPreset?.refLtmsTransactionRowGuid,
      deliveryNoteNumber: formValue.deliveryNoteNumber,
      deliveryNoteShown: formValue.deliveryNoteDocument,
      pickUpNoteNumber: formValue.pickupNoteNumber,
      pickUpNoteShown: formValue.pickupNoteDocument,
      customerReference: formValue.referenceNumber,
      type: this.action,
      truckDriverName: formValue.driver ? formValue.driver.driverName : null,
      truckDriverCompanyName: formValue.driver
        ? formValue.driver.companyName
        : null,
      licensePlate:
        formValue.driver && formValue.driver.licensePlate
          ? formValue.driver.licensePlate.number
          : null,
      licensePlateCountryId:
        formValue.driver && formValue.driver.licensePlate
          ? formValue.driver.licensePlate.countryId
          : null,
      shipperCompanyName: formValue.shipper ? formValue.shipper.name : null,
      shipperAddress: formValue?.shipper?.address?.country
        ? formValue.shipper.address
        : null,
      note: formValue.note,
      positions: formValue.loadCarriers
        ? formValue.loadCarriers.map((i) => ({
            loadCarrierId: i.id,
            quantity: i.quantity,
          }))
        : [],
      printLanguageId: formValue.print ? formValue.print.languageId : null,
      printCount: formValue.print ? formValue.print.count : null,
    };
  }

  transformToFormGroup(
    request: LoadCarrierReceiptsCreateRequest,
    defaultValues: Partial<LoadCarrierReceipt>
  ): LoadCarrierReceipt {
    const {
      digitalCode,
      targetPostingAccountId,
      customerReference,
      deliveryNoteNumber,
      deliveryNoteShown,
      pickUpNoteNumber,
      pickUpNoteShown,
      note,
      positions,
      truckDriverName,
      truckDriverCompanyName,
      licensePlate,
      licensePlateCountryId,
      shipperCompanyName,
      shipperAddress,
      printLanguageId,
      printCount,
      depoPresetId,
      isSortingRequired,
    } = request;

    const print =
      !printCount || !printLanguageId
        ? null
        : {
            count: printCount,
            languageId: printLanguageId,
          };

    return {
      digitalCode,
      targetPostingAccountId,
      deliveryNoteNumber,
      deliveryNoteDocument: deliveryNoteShown,
      pickupNoteNumber: pickUpNoteNumber,
      pickupNoteDocument: pickUpNoteShown,
      note,
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
      shipper: {
        name: shipperCompanyName,
        address: shipperAddress,
      },
      print,
      referenceNumber: customerReference,
      depoPresetId,
      isSortingRequired,
    } as LoadCarrierReceipt;
  }

  private getLoadCarrierPickerContext(
    type: LoadCarrierReceiptType
  ): LoadCarrierPickerContext {
    switch (type) {
      case LoadCarrierReceiptType.Delivery:
        return 'delivery';

      case LoadCarrierReceiptType.Pickup:
        return 'pickup';
      case LoadCarrierReceiptType.Exchange:
        return 'exchange';
      default:
        break;
    }
  }

  get isDepoContext() {
    return this.context === 'depot';
  }

  private getInstructions(category: LoadCarrierReceiptDepotPresetCategory) {
    switch (category) {
      case LoadCarrierReceiptDepotPresetCategory.External:
        return `1. Ladungsträger die nicht mindestens in der Maske vorgesehenen Qualitätsstufen entsprechen (z. B. Schrott) sind ohne Erfassung abzuweisen.
2. Gleiches gilt für andersartige Ladungsträger (z. B. Einweg-Paletten, Eigentums-Pool-Paletten etc.).
3. Depot-Annahmen mit mehr als 250 Ladungsträgern sind nur dann erlaubt wenn Sie vorgenanntes Prozedere einhalten.`;
      case LoadCarrierReceiptDepotPresetCategory.Internal:
        return `1. Bitte sofortige vorab Bewertung mit Mengen pro Ladungsträger-Art /-Qualität durchführen und an DPL mailen.
2. Ladungsträger pro Annahmevorgang nachvollziehbar gesondert einlagern.
3. Schnellstmöglich Sortierung durchführen und Mengen pro Ladungsträger-Art / -Qualität mittels DPL-Sortierprotokoll dieses Systems an DPL melden.`;
      default:
        return null;
    }
  }
}
