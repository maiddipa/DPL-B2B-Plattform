import { ChangeDetectionStrategy, Component, Input } from '@angular/core';
import { TransportBidStatus } from '@app/api/dpl';
import { LoadingService } from '@dpl/dpl-lib';

import { TransportsService } from '../services/transports.service';
import { ITransport } from '../state/transport.model';

@Component({
  selector: 'dpl-transport-bids',
  template: `
    <mat-card fxFlex>
      <mat-card-header>
        <mat-card-title
          i18n="Label für Liste von Geboten@@TransportBidsListLabel"
          >Meine Gebote</mat-card-title
        >
      </mat-card-header>
      <table
        style="width:100%"
        mat-table
        [dataSource]="bids"
        class="mat-elevation-z8"
      >
        <ng-container matColumnDef="position">
          <th
            mat-header-cell
            *matHeaderCellDef
            i18n="Label für Gebotsnummer@@TransportBidNumber"
            class="position"
          >
            Nr.
          </th>
          <td mat-cell *matCellDef="let element" class="position">
            {{ element.id }}
          </td>
        </ng-container>
        <ng-container matColumnDef="status">
          <th
            mat-header-cell
            *matHeaderCellDef
            i18n="Label für Gebotsstatus@@TransportBidStatus"
            class="status"
          >
            Status
          </th>
          <td mat-cell *matCellDef="let element" class="status">
            {{ element.status | transportBidStatus }}
          </td>
        </ng-container>
        <ng-container matColumnDef="note">
          <th
            mat-header-cell
            *matHeaderCellDef
            i18n="Label für Transportgebots Notiz@@TransportBidNote"
            class="note"
          >
            Notiz
          </th>
          <td mat-cell *matCellDef="let element" class="note">
            {{ element.note }}
          </td>
        </ng-container>
        <ng-container matColumnDef="pickupDate">
          <th
            mat-header-cell
            *matHeaderCellDef
            i18n="Label für Transport Abholdatum@@TransportBidPickupDate"
            class="pickupDate"
          >
            Abholdatum
          </th>
          <td mat-cell *matCellDef="let element" class="pickupDate">
            {{ element.pickupDate | date: 'shortDate' }}
          </td>
        </ng-container>
        <ng-container matColumnDef="deliveryDate">
          <th
            mat-header-cell
            *matHeaderCellDef
            i18n="Label für Transport Lieferdatum@@TransportBidDeliveryDate"
            class="deliveryDate"
          >
            Lieferdatum
          </th>
          <td mat-cell *matCellDef="let element" class="deliveryDate">
            {{ element.deliveryDate | date: 'shortDate' }}
          </td>
        </ng-container>

        <ng-container matColumnDef="price">
          <th
            mat-header-cell
            *matHeaderCellDef
            i18n="Label Preis für Gebot@@TransportBidPrice"
            class="price"
          >
            Angebot (€)
          </th>
          <td mat-cell *matCellDef="let element" class="price">
            {{ element.price | number }} €
          </td>
        </ng-container>
        <ng-container matColumnDef="action">
          <th
            mat-header-cell
            *matHeaderCellDef
            i18n="
              Label für Aktionen die auf Geboten ausgeführt werden
              können@@TransportBidAction"
            class="action"
          >
            Aktion
          </th>
          <td mat-cell *matCellDef="let element" class="action">
            <button
              *ngIf="element.status === transportBidStatus.Active"
              mat-icon-button
              (click)="onCancelBid(element.id)"
            >
              <mat-icon>cancel</mat-icon>
            </button>
            <ng-container *ngIf="element.status === transportBidStatus.Won">
              <button mat-icon-button (click)="onDeclineTransport(element.id)">
                <mat-icon>cancel</mat-icon>
              </button>
              <button mat-icon-button (click)="onAcceptTransport(element.id)">
                <mat-icon>check_circle</mat-icon>
              </button>
            </ng-container>
          </td>
        </ng-container>
        <!-- <ng-container matColumnDef="weight">
          <th
            mat-header-cell
            *matHeaderCellDef
            i18n="Weight|Label Gewicht@@Weight"
          >
            Gewicht
          </th>
          <td mat-cell *matCellDef="let element">
            {{ element.weight | number }}
          </td>
        </ng-container>
        <ng-container matColumnDef="from">
          <th mat-header-cell *matHeaderCellDef i18n="From|Label Von@@From">
            von
          </th>
          <td mat-cell *matCellDef="let element">{{ element.from }}</td>
        </ng-container>
        <ng-container matColumnDef="to">
          <th mat-header-cell *matHeaderCellDef i18n="To|Label Nach@@To">
            nach
          </th>
          <td mat-cell *matCellDef="let element">{{ element.to }}</td>
        </ng-container> -->
        <tr mat-header-row *matHeaderRowDef="displayedColumns"></tr>
        <tr
          mat-row
          *matRowDef="let row; columns: displayedColumns"
          [ngClass]="{
            active: row.status === transportBidStatus.Active,
            won: row.status === transportBidStatus.Won,
            lost: row.status === transportBidStatus.Lost,
            canceled: row.status === transportBidStatus.Canceled
          }"
        ></tr>
      </table>
    </mat-card>
  `,
  styleUrls: ['./transport-bids.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class TransportBidsComponent {
  @Input() transportId: number;
  @Input() bids: ITransport['bids'];

  transportBidStatus = TransportBidStatus;
  displayedColumns: string[] = [
    'position',
    'status',
    'pickupDate',
    'deliveryDate',
    'note',
    // 'weight',
    // 'from',
    // 'to',
    'price',
    'action',
  ];

  constructor(
    private transportService: TransportsService,
    private loadingService: LoadingService
  ) {}

  onCancelBid(id: number) {
    this.transportService
      .cancelBid(this.transportId, id)
      .pipe(this.loadingService.showLoadingWhile())
      .subscribe();
  }

  onAcceptTransport(id: number) {
    this.transportService
      .acceptTransport(this.transportId)
      .pipe(this.loadingService.showLoadingWhile())
      .subscribe();
  }

  onDeclineTransport(id: number) {
    this.transportService
      .declineTransport(this.transportId)
      .pipe(this.loadingService.showLoadingWhile())
      .subscribe();
  }
}
