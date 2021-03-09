import {Injectable} from '@angular/core';
import {MatDialog} from '@angular/material/dialog';
import {ExpressCode, LoadCarrierReceiptsCreateRequest, LoadCarrierReceiptType, PrintType,} from '@app/api/dpl';
import {DplApiService} from '@app/core';
import {OnBehalfOfService, PrintService} from '@app/shared';
import {LoadingService} from '@dpl/dpl-lib';
import {combineLatest, of} from 'rxjs';
import {first, map, pluck, switchMap} from 'rxjs/operators';

import { CustomerDivisionsService } from '../../customers/services/customer-divisions.service';
import { OrdersService } from '../../orders/services';
import { UserService } from '../../user/services/user.service';
import { LoadCarrierReceiptAction } from '../components';
import {
  LoadCarrierReceipt,
  ReceiptPositionPositionType,
} from './load-carrier-receipt.service.types';

@Injectable({
  providedIn: 'root',
})
export class LoadCarrierReceiptService {
  constructor(
    private dpl: DplApiService,
    private user: UserService,
    private divisionService: CustomerDivisionsService,

    private print: PrintService,
    public dialog: MatDialog,
    private orderService: OrdersService,
    private onBehalfOfService: OnBehalfOfService,
    private loadingService: LoadingService
  ) {}

  findDigitalCode(digitalCode : string, action: LoadCarrierReceiptType) {
    let printType = PrintType.LoadCarrierReceiptDelivery;
    if(action === LoadCarrierReceiptType.Pickup){
      printType = PrintType.LoadCarrierReceiptPickup;
    }
  return this.dpl.expressCodes.getByCode({expressCode: digitalCode, printType: printType})
    .pipe(map((i) => i));
  }

  generateReceiptFromExpressCode(
    action: LoadCarrierReceiptType,
    expressCode: ExpressCode
  ) {
    if (!expressCode) {
      throw new Error(
        'order cannot be null when trying to create a load carrier receipt position.'
      );
    }

    const type = expressCode.loadCarrierReceiptPreset.type;
    const quantityMultiplicationFactor =
      action === LoadCarrierReceiptType.Exchange && type === LoadCarrierReceiptType.Pickup ? -1 : 1;

    let loadCarriers = [
      {
        id: expressCode.loadCarrierReceiptPreset.loadCarrierId,
        quantity: expressCode.loadCarrierReceiptPreset.loadCarrierQuantity * quantityMultiplicationFactor,
      },
    ];
    if (expressCode.loadCarrierReceiptPreset.baseLoadCarrierId) {
      loadCarriers = [
        {
          id: expressCode.loadCarrierReceiptPreset.baseLoadCarrierId,
          quantity:
            expressCode.loadCarrierReceiptPreset.baseLoadCarrierQuantity * quantityMultiplicationFactor,
        },
        ...loadCarriers,
      ];
    }

    const receipt: LoadCarrierReceipt = {
      type,
      generated: true,
      depoPresetId: null,
      sourcePostingAccountId: expressCode.loadCarrierReceiptPreset.postingAccountId,
      targetPostingAccountId: null,
      digitalCode: expressCode.digitalCode,
      date: expressCode.loadCarrierReceiptPreset.plannedFulfillmentDateTime,
      deliveryNoteNumber: expressCode.loadCarrierReceiptPreset.deliveryNoteNumber,
      deliveryNoteDocument: null,
      pickupNoteNumber: expressCode.loadCarrierReceiptPreset.pickupNoteNumber,
      pickupNoteDocument: null,
      referenceNumber: null,
      instructions: $localize`:LoadCarrierReceiptInstructions|LoadCarrierReceipt Instructions@@LoadCarrierReceiptInstructions:Reihenfolge beachten!`,
      note: null,
      loadCarriers,
      driver: null,
      shipper: null,
      print: null,
      isSortingRequired: false
    };

    return receipt;
  }

  generateReceiptFromDigitalCode(
    action: LoadCarrierReceiptType,
    digitalCode: string
  ) {
    if (!digitalCode) {
      throw new Error(
        'digitalCode cannot be null when trying to create a load carrier receipt.'
      );
    }

    return this.findDigitalCode(digitalCode, action).pipe(
      map((expressCode) => {
        if (!expressCode) {
          return null;
        }
        return this.generateReceiptFromExpressCode(action, expressCode);
      })
    );
  }

  createReceipt(
    print: boolean,
    request: Omit<
      LoadCarrierReceiptsCreateRequest,
      'customerDivisionId' | 'printDateTimeOffset'
    >
  ) {
    const divisionId$ = this.divisionService
      .getActiveDivision()
      .pipe(pluck('id'));

    return combineLatest([divisionId$]).pipe(
      first(),
      switchMap(([customerDivisionId]) => {
        return this.dpl.loadCarrierReceipts.post({
          ...request,
          ...{
            customerDivisionId,
            printDateTimeOffset: new Date().getTimezoneOffset(),
          },
        });
      }),
      this.loadingService.showLoadingWhile(),
      switchMap((receipt) => {
        return print
          ? this.print
              .printUrl(
                receipt.downloadLink,
                true,
                this.print.getPrintContextLoadCarrierReceipt(receipt),
                receipt
              )
              .pipe(
                switchMap((result) => {
                  if (result && result.cancel) {
                    return this.dpl.loadCarrierReceipts.patchLoadCarrierReceiptCancel(
                      receipt.id,
                      { reason: result.cancelReason }
                    );
                  }
                  return of(receipt);
                })
              )
          : of(receipt);
      })
    );
  }
}
