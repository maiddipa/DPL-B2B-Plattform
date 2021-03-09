import {
  ChangeDetectionStrategy,
  Component,
  Input,
  OnChanges,
  SimpleChanges,
} from '@angular/core';
import { TransportLoadType, TransportOfferingStatus } from '@app/api/dpl';

import { TransportsService } from '../services/transports.service';
import { ITransport } from '../state/transport.model';

@Component({
  selector: 'dpl-transport-details',
  template: `
    <mat-card fxFlex>
      <mat-card-header>
        <mat-card-title>Transportdetails</mat-card-title>
      </mat-card-header>
      <mat-card-content fxLayout="column" fxLayoutGap="10px">
        <div fxFlex fxLayout="column" fxLayoutGap="10px">
          <div fxLayout="row" fxLayoutGap="10px">
            <span
              fxFlex
              i18n="
                Label für Ladungstypen auf transport
                details@@TransportDetailsLoadTypeLabel"
              >Ladungsgut:</span
            >
            <span fxFlex>Ladungsträger</span>
          </div>
          <div
            *ngIf="transport.loadType === loadType.LoadCarrier"
            fxLayout="row"
            fxLayoutGap="10px"
          >
            <span
              fxFlex
              i18n="
                Label für Ladung auf transport
                details@@TransportDetailsLoadLabel"
              >Ladung:</span
            >
            <span
              *ngIf="
                transport.status === transportStatus.Won;
                else notYetWonLoad
              "
              fxFlex
              >{{ transport.loadCarrierLoad.loadCarrierQuantity }} x
              {{ transport.loadCarrierLoad.loadCarrierId | loadCarrier }}</span
            >
            <ng-template #notYetWonLoad>
              <span fxFlex
                >{{ transport.loadCarrierLoad.loadCarrierQuantity }} x
                {{
                  transport.loadCarrierLoad.loadCarrierTypeId
                    | loadCarrier: 'type'
                }}</span
              >
            </ng-template>
          </div>

          <!-- <div fxLayout="row" fxLayoutGap="10px">
            <span
              fxFlex
              i18n="Label für Ladungsarten auf transport details@@TransportDetailsLoadingPropertiesLabel"
              >Ladungsarten:</span
            >
            <div fxFlex fxLayout="column" fxLayoutGap="5px">
              <ng-container *ngFor="let loadingType of loadtingTypes">
                <ng-container
                  *ngTemplateOutlet="check; context: loadingType"
                ></ng-container>
              </ng-container>
            </div>
            <ng-template #check let-checked="checked" let-label="label">
              <div fxLayout="row" fxLayoutAlign=" center" fxLayoutGap="5px">
                <mat-icon [ngClass]="{ checked: checked, crossed: !checked }">{{
                  checked ? 'check' : 'close'
                }}</mat-icon>
                <span>{{ label }}</span>
              </div>
            </ng-template>
          </div> -->
        </div>
        <div fxLayout="row" fxLayoutGap="10px">
          <span
            fxFlex
            i18n="
              Label für Ladung auf transport
              details@@TransportDetailsDistanceLabel"
            >Entfernung:</span
          >
          <span fxFlex>ca. {{ transport.distance | distance }}</span>
        </div>
        <div fxLayout="row" fxLayoutAlign="center">
          <button
            mat-stroked-button
            color="primary"
            title="Premium Feature"
            (click)="onRoutingTapped()"
            [disabled]="!wonOrAccepted"
          >
            <mat-icon>map</mat-icon>
            <span
              i18n="
                TransportBrokerDetailsRouteInformationBtn|Schaltfläche
                Routeninformationen
                (Premium)@@TransportBrokerDetailsRouteInformationBtn"
              >Routeninformationen (Premium)</span
            >
          </button>
        </div>
      </mat-card-content>
    </mat-card>
  `,
  styles: [
    `
      mat-icon.checked {
        color: green;
      }
      mat-icon.crossed {
        color: red;
      }
    `,
  ],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class TransportDetailsComponent implements OnChanges {
  @Input() transport: ITransport;

  transportStatus = TransportOfferingStatus;
  loadType = TransportLoadType;

  loadtingTypes: { label: string; checked: boolean }[];

  constructor(private transportService: TransportsService) {}

  ngOnChanges(changes: SimpleChanges): void {
    if (changes.transport && this.transport) {
      this.loadtingTypes = [
        {
          label: $localize`:@@SideLoadingLabel:Seitenbeladung`,
          checked: this.transport.supportsSideLoading,
        },
        {
          label: $localize`:@@RearLoadingLabel:Heckbeladung`,
          checked: this.transport.supportsRearLoading,
        },
        {
          label: $localize`:@@JumboVehiclesLabel:Jumbobeladung`,
          checked: this.transport.supportsJumboVehicles,
        },
      ];
    }
  }

  onRoutingTapped() {
    const url = this.transportService.getRoutingLink(
      this.transport.supplyInfo.address,
      this.transport.demandInfo.address
    );
    window.open(url, 'transport-routing');
  }

  get wonOrAccepted() {
    return (
      this.transport.status === TransportOfferingStatus.Won ||
      this.transport.status === TransportOfferingStatus.Accepted
    );
  }
}
