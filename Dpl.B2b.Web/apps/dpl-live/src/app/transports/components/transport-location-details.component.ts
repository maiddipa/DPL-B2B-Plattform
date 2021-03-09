import {
  ChangeDetectionStrategy,
  Component,
  Input,
  OnChanges,
  SimpleChanges,
} from '@angular/core';
import { TransportOfferingInfo } from '@app/api/dpl';
import { getOffsetSinceStartOfWeek } from '@app/core';
import * as _ from 'lodash';

@Component({
  selector: 'dpl-transport-location-details',
  template: `
    <div fxFlex fxLayout="column" fxLayoutGap="10px">
      <mat-card fxLayout="column" fxLayouGap="20px" class="required">
        <mat-card-header>
          <mat-card-title>{{ title }}</mat-card-title>
        </mat-card-header>
        <mat-card-content fxLayout="column" fxLayoutGap="10px">
          <div fxLayout="row" fxLayoutGap="10px">
            <span fxFlex>Ort:</span>
            <span fxFlex>{{ location.address | address }}</span>
          </div>
          <div fxLayout="row" fxLayoutGap="10px">
            <span fxFlex>Zeitraum:</span>
            <div fxFlex fxLayout="column" fxLayoutGap="5px">
              <span
                >{{ location.earliestFulfillmentDateTime | date }} -
                {{ location.latestFulfillmentDateTime | date }}</span
              >
            </div>
          </div>
          <div fxLayout="row" fxLayoutGap="10px">
            <span
              fxFlex
              i18n="
                Label für Ladungsarten auf transport
                details@@TransportDetailsLoadingPropertiesLabel"
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
          </div>
        </mat-card-content>
      </mat-card>
      <mat-card fxFlex>
        <mat-card-header>
          <mat-card-title i18n="OpeningHours|Label Öffnungszeiten@@OpeningHours"
            >Öffnungszeiten</mat-card-title
          >
        </mat-card-header>
        <mat-card-content fxLayout="column">
          <dpl-business-hours
            [businessHours]="location.businessHours"
          ></dpl-business-hours>
        </mat-card-content>
      </mat-card>
    </div>
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
export class TransportLocationDetailsComponent implements OnChanges {
  @Input() title: string;
  @Input() location: TransportOfferingInfo;
  loadtingTypes: { label: string; checked: boolean }[];

  constructor() {}

  ngOnChanges(changes: SimpleChanges): void {
    const atLeatOne =
      this.location.supportsSideLoading ||
      this.location.supportsRearLoading ||
      this.location.supportsJumboVehicles;

    // Sort businessHours by weekname
    const businessHours = _(this.location.businessHours)
      .sortBy(({ dayOfWeek }) => getOffsetSinceStartOfWeek(dayOfWeek))
      .value();

    // Fix unsorted location.businessHours
    this.location = { ...this.location, ...{ businessHours } };

    if (changes.location && this.location) {
      this.loadtingTypes = [
        {
          label: $localize`:@@SideLoadingLabel:Seitenbeladung`,
          checked: this.location.supportsSideLoading || !atLeatOne,
        },
        {
          label: $localize`:@@RearLoadingLabel:Heckbeladung`,
          checked: this.location.supportsRearLoading || !atLeatOne,
        },
        {
          label: $localize`:@@JumboVehiclesLabel:Jumbobeladung`,
          checked: this.location.supportsJumboVehicles,
        },
      ];
    }
  }
}
