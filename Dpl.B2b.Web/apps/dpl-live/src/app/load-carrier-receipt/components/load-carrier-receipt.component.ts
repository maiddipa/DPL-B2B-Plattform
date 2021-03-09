import { ChangeDetectionStrategy, Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { LoadCarrierReceiptType } from '@app/api/dpl';
import { LoadCarrierPickerContext } from '@app/shared';
import { getTypedParams } from '@dpl/dpl-lib';
import { combineLatest, Observable, of } from 'rxjs';
import { map, publishReplay, refCount, switchMap } from 'rxjs/operators';

import { LoadCarrierReceipt, LoadCarrierReceiptService } from '../services';
import { LoadCarrierReceiptContext } from './load-carrier-receipt-form.component';


interface LoadCarrierReceiptComponentRouteParams {
  digitalCode: string;
}

export type LoadCarrierReceiptAction = Exclude<
  LoadCarrierPickerContext,
  'voucher' | 'transfer'
>;

interface LoadCarrierReceiptComponentQueryParams {
  action: LoadCarrierReceiptType;
  context: LoadCarrierReceiptContext
}

type ViewData = {
  action: LoadCarrierReceiptType;
  context: LoadCarrierReceiptContext
  receipt: LoadCarrierReceipt;
};

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'app-load-carrier-receipt',
  template: `
    <ng-container *ngIf="viewData$ | async as data">
      <mat-toolbar [ngSwitch]="data.action" fxLayoutAlign="end center">
        <span
          *ngSwitchCase="loadCarrierReceiptType.Pickup"
          i18n="
            LoadCarrierReceiptPickupActionLabel|Action Label
            pickup@@LoadCarrierReceiptPickupActionLabel"
          >Ausgabe-Quittung erstellen</span
        >
        <span
          *ngSwitchCase="loadCarrierReceiptType.Delivery"
          i18n="
            LoadCarrierReceiptActionDeliveryLabel|Action Label delivery
            @@LoadCarrierReceiptActionDeliveryLabel"
          >Annahme-Quittung erstellen</span
        >
        <span
          *ngSwitchCase="loadCarrierReceiptType.Exchange"
          i18n="
            LoadCarrierReceiptActionExchangeLabel|Action Label
            exchange@@LoadCarrierReceiptActionExchangeLabel"
          >Tausch-Quittung erstellen</span
        >
        <span
          *ngSwitchDefault
          i18n="
            LoadCarrierReceiptActionDefaultLabel|Action Label
            Default@@LoadCarrierReceiptActionDefaultLabel"
          >Quittung erstellen</span
        >
      </mat-toolbar>
      <app-load-carrier-receipt-form
        [context]="data.context"
        [action]="data.action"
        [receipt]="data.receipt"
      ></app-load-carrier-receipt-form>
    </ng-container>
  `,
  styles: [],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class LoadCarrierReceiptComponent implements OnInit {
  viewData$: Observable<ViewData>;
  loadCarrierReceiptType = LoadCarrierReceiptType;

  constructor(
    private route: ActivatedRoute,
    private receiptService: LoadCarrierReceiptService
  ) {}

  ngOnInit() {
    const params$ = getTypedParams<
      LoadCarrierReceiptComponentRouteParams,
      LoadCarrierReceiptComponentQueryParams
    >(this.route).pipe(publishReplay(1), refCount());

    const receipt$ = params$.pipe(
      switchMap((params) => {
        if (!(params && params.route && params.route.digitalCode)) {
          return of(null);
        }
        return this.receiptService.generateReceiptFromDigitalCode(
          params.query.action,
          params.route.digitalCode
        );
      })
    );

    this.viewData$ = combineLatest([params$, receipt$]).pipe(
      map(([params, receipt]) => {
        return {
          action: params.query.action,
          context: params.query.context,
          receipt,
        };
      })
    );
  }
}
