import {
  Component,
  EventEmitter,
  Input,
  OnDestroy,
  OnInit,
  Output,
} from '@angular/core';
import { untilDestroyed } from 'ngx-take-until-destroy';

import { SearchBasketService } from '../../services/search-basket.service';
import { ISearchResponse } from '../../services/search.service.types';

type IDestination = ISearchResponse['destinations'][0];

@Component({
  selector: 'app-search-result-list',
  templateUrl: './search-result-list.component.html',
  styleUrls: ['./search-result-list.component.scss'],
})
export class SearchResultListComponent implements OnInit, OnDestroy {
  @Input() basketId: number;
  @Input() response: ISearchResponse;
  @Input() selected: number;
  @Output() selectionChanged: EventEmitter<number> = new EventEmitter();
  @Output() addToBasket: EventEmitter<IDestination> = new EventEmitter();

  basketDict: {
    [itemId: number]: boolean;
  } = {};
  constructor(private basketService: SearchBasketService) {}

  ngOnInit() {
    this.basketService
      .getBasket(this.basketId)
      .pipe(untilDestroyed(this))
      .subscribe((basket) => {
        this.basketDict = basket.items.reduce((prev, current) => {
          prev[current.id] = true;
          return prev;
        }, {});
      });
  }

  ngOnDestroy() {}

  onSelectionChanged(id: number) {
    this.selected = id;
    this.selectionChanged.emit(this.selected);
  }

  onAddToBasket(destination: IDestination) {
    this.addToBasket.emit(destination);
  }

  onRemoveFromBasket(destination: IDestination) {
    this.basketService.removeItemFromBasket(this.basketId, destination.id);
  }
}
