import {
  ControlPosition,
  StreetViewControlOptions,
  ZoomControlOptions,
  ZoomControlStyle,
} from '@agm/core';
import {
  ChangeDetectionStrategy,
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
import { untilDestroyed } from 'ngx-take-until-destroy';
import { combineLatest, Subject } from 'rxjs';
import { distinctUntilChanged, tap } from 'rxjs/operators';

import { GoogleMapsService } from '../../services/google-maps.service';
import { SearchBasketService } from '../../services/search-basket.service';
import {
  IDestination,
  IMapClickPosition,
  ISearchResponse,
  SearchInputOrderType,
} from '../../services/search.service.types';
import { mapStyles } from './search-result-map.styles';

interface IViewData {
  // google maps zoom level
  zoom: number;
  lat: number;
  lng: number;
  geoJson: any;
}

@Component({
  selector: 'app-search-result-map',
  templateUrl: './search-result-map.component.html',
  styleUrls: ['./search-result-map.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class SearchResultMapComponent implements OnInit, OnDestroy, OnChanges {
  @Input() response: ISearchResponse;
  @Input() selected: number;
  @Input() basketId: number;

  @Output() selectionChanged: EventEmitter<number> = new EventEmitter();
  @Output() search: EventEmitter<IMapClickPosition> = new EventEmitter();
  @Output() radiusChanged: EventEmitter<number> = new EventEmitter();
  @Output() addToBasket: EventEmitter<IDestination> = new EventEmitter();

  radiusChanged$ = new Subject<number>();
  radiusCenterChanged$ = new Subject<IMapClickPosition>();

  radiuses: {
    lat: number;
    lng: number;
    radius: number;
  }[];

  mapData: {
    zoom: number;
    lat: number;
    lng: number;
    styles: any;

    geoJson?: any;
  };

  // this is used to switch back to the overview when returning from a destination zoom
  prevMapData: SearchResultMapComponent['mapData'];

  zoomControlOptions: ZoomControlOptions = {
    position: ControlPosition.TOP_RIGHT,
    style: ZoomControlStyle.SMALL,
  };

  streetViewControlOptions: StreetViewControlOptions = {
    position: ControlPosition.TOP_CENTER,
  };

  basketDict: {
    [itemId: number]: boolean;
  } = {};

  searchAction = SearchInputOrderType;
  clickedPosition: IMapClickPosition;

  constructor(
    private googleMaps: GoogleMapsService,
    private basketService: SearchBasketService,
    private cd: ChangeDetectorRef
  ) {}

  ngOnInit() {
    const radiusChanged$ = this.radiusChanged$.asObservable().pipe(
      untilDestroyed(this),
      distinctUntilChanged(),
      tap((radius) => this.radiusChanged.emit(radius))
    );

    const radiusCenterChanged$ = this.radiusCenterChanged$.asObservable().pipe(
      untilDestroyed(this),
      distinctUntilChanged((prev, current) => {
        return prev.lat === current.lat && prev.lng === current.lng;
      }),
      tap((position) => this.search.emit(position))
    );

    const basketDict$ = this.basketService.getBasket(this.basketId).pipe(
      untilDestroyed(this),
      tap((basket) => {
        this.basketDict = basket.items.reduce((prev, current) => {
          prev[current.id] = true;
          return prev;
        }, {});

        this.cd.markForCheck();
      })
    );

    combineLatest(
      radiusChanged$,
      radiusCenterChanged$,
      basketDict$
    ).subscribe();
  }

  ngOnDestroy() {}

  ngOnChanges(changes: SimpleChanges) {
    this.clickedPosition = null;

    if (!this.response) {
      // zoom out with map center being center of germany
      this.prevMapData = this.mapData = {
        zoom: 6,
        lat: 51.1657,
        lng: 10.4515,
        styles: mapStyles,
      };
      this.radiuses = [];
      return;
    }

    this.prevMapData = this.mapData = {
      zoom: 9,
      lat: this.response.origin.lat,
      lng: this.response.origin.lng,
      styles: mapStyles,
    };

    this.radiuses = [
      {
        lat: this.response.origin.lat,
        lng: this.response.origin.lng,
        radius: this.response.input.radius,
      },
    ];
  }

  mapReady(map: google.maps.Map) {
    // the google maps places api needs a reference to the maps object to instanciate
    this.googleMaps.setMap(map);
  }

  onSelectionChanged(id: number) {
    this.selected = id;
    this.selectionChanged.emit(this.selected);
    this.cd.markForCheck();
  }

  onMapClicked(data: { coords: IMapClickPosition }) {
    this.prevMapData = this.mapData = {
      ...this.mapData,
      ...data.coords,
      ...{ zoom: 9 },
    };
    this.clickedPosition = data.coords;
    this.cd.markForCheck();
  }

  onMapClickClose() {
    this.clickedPosition = null;
    this.cd.markForCheck();
  }

  onRadiusChange(radius: number) {
    this.radiusChanged$.next(radius);
  }

  onRadiusCenterChanged(position: IMapClickPosition) {
    this.mapData = { ...this.mapData, ...position, ...{ zoom: 9 } };
    this.radiusCenterChanged$.next(position);
    this.cd.markForCheck();
  }

  onSearch(position: IMapClickPosition) {
    this.search.emit(position);
    this.clickedPosition = null;
    this.cd.markForCheck();
  }

  onAddToBasket(location: IDestination) {
    this.addToBasket.emit(location);
  }

  onPickupLocationZoomToggled(destination: IDestination | null) {
    if (!destination) {
      this.mapData = { ...this.prevMapData };
      this.cd.detectChanges();
      return;
    }

    this.prevMapData = this.mapData;

    this.mapData = {
      zoom: 17,
      lat: destination.info.lat,
      lng: destination.info.lng,
      styles: mapStyles,
    };
    this.cd.detectChanges();
  }

  dataStyleFunc(feature) {
    return {
      clickable: false,
      fillOpacity: 0,
    };
  }
}
