import { Component, OnInit } from '@angular/core';
import * as moment from 'moment';
import { ActivatedRoute } from '@angular/router';
import { getTypedParams } from '@dpl/dpl-lib';
import { Observable, combineLatest, of } from 'rxjs';
import { map, switchMap } from 'rxjs/operators';
import { TransportsService } from '../services/transports.service';
import { ITransport } from '../state/transport.model';
import {
  DayOfWeek,
  TransportOfferingStatus,
  TransportOffering,
} from '@app/api/dpl';

export interface TransportDetailsComponentParams {
  id: number;
}

type ViewData = {
  transport: ITransport;
};

export interface OfferElement {
  position: number;
  date: moment.Moment;
  weight: number;
  from: string;
  to: string;
  offer: number;
}

@Component({
  selector: 'app-transport',
  template: `
    <mat-toolbar fxLayoutAlign="end center">
      <span i18n="TransportBroker|Label Transport Broker@@TransportBroker"
        >Transport Broker</span
      >
    </mat-toolbar>
    <div
      *ngIf="viewData$ | async as data"
      fxLayout="column"
      fxLayoutGap="20px"
      style="margin:20px;"
    >
      <div fxLayout="row" fxLayoutAlign=" stretch" fxLayoutGap="10px">
        <dpl-transport-location-details
          fxFlex
          [location]="data.transport.supplyInfo"
          title="Beladestelle"
          title-i18n="Label Beladestelle@@PickupLoadingPoint"
        ></dpl-transport-location-details>
        <dpl-transport-details
          fxFlex
          [transport]="data.transport"
        ></dpl-transport-details>
        <dpl-transport-location-details
          fxFlex
          [location]="data.transport.demandInfo"
          title="Entladestelle"
          title-i18n="Label Entladestelle@@DeliveryLoadingPoint"
        ></dpl-transport-location-details>
      </div>
      <dpl-transport-bid-form
        *ngIf="
          data.transport.status === transportStatus.Available ||
          data.transport.status === transportStatus.BidPlaced
        "
        [transport]="data.transport"
      ></dpl-transport-bid-form>
      <dpl-transport-bids
        fxLayout="row"
        fxLayoutAlign="space-between center"
        fxLayouGap="20px"
        [transportId]="data.transport.id"
        [bids]="data.transport.bids"
      ></dpl-transport-bids>
      <div fxLayout="row" fxLayoutAlign="end">
        <a mat-raised-button [routerLink]="['/transports']" color="primary">
          <ng-container
            i18n="
              TransportOverview|Label Btn Zurück Transport
              Übersicht@@TransportOverview"
            >Transporte Übersicht</ng-container
          >
        </a>
      </div>
    </div>
  `,
  styleUrls: ['./transport.component.scss'],
})
export class TransportComponent implements OnInit {
  transportStatus = TransportOfferingStatus;
  viewData$: Observable<ViewData>;
  constructor(
    private route: ActivatedRoute,
    private transport: TransportsService
  ) {}

  ngOnInit() {
    const params$ = getTypedParams<TransportDetailsComponentParams>(this.route);

    const transport$ = params$.pipe(
      switchMap((params) => this.transport.getTransport(params.route.id)),
      map((transport) => {
        const earliestFulfillmentDateTime =
          transport.demandInfo.earliestFulfillmentDateTime >
          transport.supplyInfo.earliestFulfillmentDateTime
            ? transport.demandInfo.earliestFulfillmentDateTime
            : transport.supplyInfo.earliestFulfillmentDateTime;

        const demandInfo = {
          ...transport.demandInfo,
          earliestFulfillmentDateTime,
        };

        const updatedTransport: TransportOffering = {
          ...transport,
          demandInfo,
        };

        return updatedTransport;
      })
    );

    this.viewData$ = combineLatest(transport$).pipe(
      map(([transport]) => {
        const viewData: ViewData = {
          transport,
        };
        return viewData;
      })
    );
  }
}
