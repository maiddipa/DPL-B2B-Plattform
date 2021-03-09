import {
  AfterViewInit,
  Component,
  ElementRef,
  Input,
  OnChanges,
  OnDestroy,
  OnInit,
  QueryList,
  SimpleChanges,
  ViewChildren,
} from '@angular/core';
import { FormControl, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { NgxSingleFieldSubFormComponent } from '@dpl/dpl-lib';
import { subformComponentProviders } from 'ngx-sub-form';
import { untilDestroyed } from 'ngx-take-until-destroy';
import { EMPTY, of } from 'rxjs';
import { switchMap, tap } from 'rxjs/operators';

import { GoogleMapsService } from '../../../search/services/google-maps.service';
import { CustomValidators } from '../../../validators';
import { IGeoPoint } from '../../types';
import { GoogleMapsPlacesSelectorDialogComponent } from './google-maps-places-selector-dialog.component';

export interface GoogleMapsPlacesLookupComponentForm {
  searchText: string;
  geoPoint: IGeoPoint<google.maps.places.PlaceResult>;
}

@Component({
  selector: 'dpl-google-maps-places-lookup',
  template: `
    <mat-form-field *ngIf="formGroup" fxLayout="column">
      <input
        #searchInput
        [formControl]="searchInputControl"
        matInput
        type="text"
        [placeholder]="placeholder"
      />
      <mat-error>Bitte aus Liste wählen</mat-error>
    </mat-form-field>
  `,
  providers: subformComponentProviders(GoogleMapsPlacesLookupComponent),
})
export class GoogleMapsPlacesLookupComponent
  extends NgxSingleFieldSubFormComponent<GoogleMapsPlacesLookupComponentForm>
  implements OnInit, OnChanges, OnDestroy, AfterViewInit {
  @Input() initalSearchText: string;
  @Input() placeholder: string;
  @Input() required: boolean;
  @Input() adressType: string | string[]; // 'establishment' / 'address' / 'geocode'

  //@ViewChild('searchInput', { static: true }) searchInput: any;

  @ViewChildren('searchInput') searchInputChildren: QueryList<ElementRef>;

  searchInputControl = new FormControl(null);

  constructor(
    private google: GoogleMapsService,
    private dialog: MatDialog,
    private snackBar: MatSnackBar
  ) {
    super();
  }

  ngOnInit() {
    this.searchInputControl.setValue(this.initalSearchText || null, {
      emitEvent: false,
    });

    const validator = () => {
      if (this.formGroup.controls.innerControl.valid) {
        return null;
      }

      return {
        invalid: true,
      };
    };
    this.searchInputControl.setValidators(validator);
    this.searchInputControl.updateValueAndValidity();

    this.searchInputControl.valueChanges
      .pipe(
        untilDestroyed(this),
        tap(() => {
          if (!this.formGroup.controls.innerControl.value) {
            return;
          }

          this.formGroup.controls.innerControl.reset(null);
        })
      )
      .subscribe();
  }

  ngOnChanges(changes: SimpleChanges): void {
    super.ngOnChanges(changes);
    if (changes.required) {
      this.formGroup.controls.innerControl.setValidators(
        this.required ? [Validators.required, CustomValidators.isObject] : []
      );
      this.formGroup.controls.innerControl.updateValueAndValidity();
      this.searchInputControl.updateValueAndValidity({ emitEvent: false });
    }
  }

  ngAfterViewInit() {
    this.google.initialized$
      .pipe(
        untilDestroyed(this),
        tap(() => {
          this.enableSearchInputAutocomplete(this.searchInputChildren.first);
        })
      )
      .subscribe();
  }

  ngOnDestroy(): void {}

  private enableSearchInputAutocomplete(element: ElementRef) {
    const autocomplete = new google.maps.places.SearchBox(
      element.nativeElement,
      {
        // Bounds set to FAVOR results from germany (but does not exclude others)
        // https://gist.github.com/graydon/11198540
        bounds: <google.maps.LatLngBoundsLiteral>{
          north: 54.983104153,
          west: 5.98865807458,
          south: 15.0169958839,
          east: 47.3024876979,
        },
      }
    );

    google.maps.event.addListener(autocomplete, 'places_changed', () => {
      const places = autocomplete.getPlaces();
      this.handleResponse(places);
    });
  }

  handleResponse(places: google.maps.places.PlaceResult[]) {
    const getPlace = () => {
      if (!places || places.length === 0) {
        this.snackBar.open(
          $localize`:SearchEmptyStartpointMessage|Hinweis Leere Suchparameter@@SearchEmptyStartpointMessage:Es wurde kein Startpunkt zu den von Ihnen eingegeben Suchparametern gefunden.`,
          null,
          {
            duration: 5000,
          }
        );

        return EMPTY;
      } else if (places.length === 1) {
        return of(places[0]);
      }

      return this.dialog
        .open<
          GoogleMapsPlacesSelectorDialogComponent,
          google.maps.places.PlaceResult[],
          google.maps.places.PlaceResult
        >(GoogleMapsPlacesSelectorDialogComponent, {
          data: places,
        })
        .afterClosed()
        .pipe(
          tap((place) => {
            this.searchInputControl.patchValue(place.formatted_address);
          })
        );
    };

    getPlace()
      .pipe(
        switchMap((place) => {
          // we need the address components
          // since autocomplete allows for returning
          return place.address_components
            ? of(place)
            : this.google.getPlaceDetails(place.place_id);
        }),
        tap((place) => {
          const geoPoint = this.google.mapToGeoPoint(place);
          this.formGroup.controls.innerControl.patchValue({
            searchText: place.formatted_address,
            geoPoint,
          });
          this.searchInputControl.updateValueAndValidity({ emitEvent: false });
        })
      )
      .subscribe();
  }
}
