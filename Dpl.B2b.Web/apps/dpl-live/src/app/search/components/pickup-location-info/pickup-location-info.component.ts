import {
  Component,
  EventEmitter,
  Input,
  OnDestroy,
  OnInit,
  Output,
} from '@angular/core';
import { BusinessHours, DayOfWeek } from '@app/api/dpl';
import { getOffsetSinceStartOfWeek } from '@app/core';
import * as _ from 'lodash';
import { untilDestroyed } from 'ngx-take-until-destroy';
import { distinctUntilChanged, map } from 'rxjs/operators';

import { SearchBasketService } from '../../services/search-basket.service';
import { IDestination } from '../../services/search.service.types';

@Component({
  selector: 'app-pickup-location-info',
  templateUrl: './pickup-location-info.component.html',
  styleUrls: ['./pickup-location-info.component.scss'],
})
export class PickupLocationInfoComponent implements OnInit, OnDestroy {
  @Input() basketId: number;
  @Input() location: IDestination;
  @Output() addToBasket = new EventEmitter<IDestination>();
  @Output() zoomToggled = new EventEmitter<IDestination>();

  inBasket: boolean;
  isZoomed = false;

  get businessHoursGroupedByDay() {
    return _(this.location.info.data.businessHours)
      .groupBy((bh) => bh.dayOfWeek)
      .map((hours, dayOfWeek) => ({ hours, dayOfWeek: dayOfWeek as DayOfWeek }))
      .sortBy(({ dayOfWeek }) => getOffsetSinceStartOfWeek(dayOfWeek))
      .value();
  }

  constructor(private basketService: SearchBasketService) {}

  ngOnInit() {
    this.basketService
      .getBasket(this.basketId)
      .pipe(
        untilDestroyed(this),
        map((basket) => {
          return (
            basket.items.findIndex((i) => i.id === this.location.id) !== -1
          );
        }),
        distinctUntilChanged()
      )
      .subscribe((inBasket) => {
        this.inBasket = inBasket;
      });
  }

  ngOnDestroy() {}

  onAddToBasket(location: IDestination) {
    this.addToBasket.emit(location);
  }

  onRemoveFromBasket(location: IDestination) {
    this.basketService.removeItemFromBasket(this.basketId, location.id);
  }

  onZoomToggle() {
    this.isZoomed = !this.isZoomed;
    this.zoomToggled.emit(this.isZoomed ? this.location : null);
  }

  groupByDay(hours: BusinessHours[]) {}

  getMailBody() {
    return encodeURIComponent(this.location.googleRoutingLink);
  }
}
