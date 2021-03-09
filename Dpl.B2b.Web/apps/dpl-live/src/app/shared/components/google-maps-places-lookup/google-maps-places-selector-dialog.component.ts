import { Component, Inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'dpl-google-maps-selector-dialog',
  template: `
    <!-- <button *ngFor="let origin of origins" (click)="onSelected(origin)">{{origin.geometry.location.toString()}}</button> -->
    <mat-toolbar color="primary">
      <span
        class="fill-remaining-space"
        i18n="ChooseAdress|Label Addresse auswählen@@ChooseAdress"
        >Adresse auswählen</span
      >
    </mat-toolbar>
    <mat-action-list class="scrollable" *ngIf="places; else noResults">
      <button
        mat-list-item
        *ngFor="let place of places"
        (click)="onSelected(place)"
      >
        <h3 matLine>{{ place.formatted_address }}</h3>
      </button>
    </mat-action-list>
    <ng-template #noResults>
      <span
        i18n="
          searchResultSelectorDialogNoResults|search-result-selector-dialog
          Nachricht Kein Ergebniss@@searchResultSelectorDialogNoResults"
        >Keine Ergebnisse, bitte versuchen Sie es erneut</span
      >
    </ng-template>
  `,
  styles: [],
})
export class GoogleMapsPlacesSelectorDialogComponent implements OnInit {
  constructor(
    @Inject(MAT_DIALOG_DATA) public places: google.maps.places.PlaceResult[],
    public dialogRef: MatDialogRef<
      GoogleMapsPlacesSelectorDialogComponent,
      google.maps.places.PlaceResult
    >
  ) {}

  onSelected(place: google.maps.places.PlaceResult) {
    this.dialogRef.close(place);
  }

  ngOnInit() {}
}
