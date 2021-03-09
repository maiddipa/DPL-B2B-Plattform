import { CdkDragDrop, moveItemInArray } from '@angular/cdk/drag-drop';
import {
  Component,
  EventEmitter,
  Input,
  OnChanges,
  OnInit,
  Output,
  SimpleChanges,
} from '@angular/core';
import { OrderType } from '@app/api/dpl';
import { flatten } from 'lodash';

import { SearchBasketService } from '../../services/search-basket.service';
import {
  ISearchResponse,
  SearchInputType,
} from '../../services/search.service.types';

@Component({
  selector: 'app-search-list',
  templateUrl: './search-list.component.html',
  styleUrls: ['./search-list.component.scss'],
})
export class SearchListComponent implements OnInit, OnChanges {
  @Input() basketId: number;
  @Input() responses: ISearchResponse[];
  @Input() selected: ISearchResponse;
  @Output() selectionChanged = new EventEmitter<ISearchResponse>();

  orderType = OrderType;
  selectedResponseIndex: number;

  constructor(private basket: SearchBasketService) {
    this.selectedResponseIndex = 0;
  }

  ngOnInit() {
    if (this.responses?.length > 0) {
      this.selected = this.responses[this.responses.length - 1];
      this.selectionChanged.emit(this.selected);
    }
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (this.selected) {
      // handles case when switching from basket and component gets reloaded
      if (changes.response && !changes.response.previousValue) {
        this.selectedResponseIndex = this.responses.findIndex(
          (r) => r.id == this.selected.id
        );
      }
      // handles case when new response is added
      else if (
        changes.responses &&
        (changes.responses.firstChange ||
          changes.responses.currentValue.length >
            changes.responses.previousValue.length)
      ) {
        this.selectedResponseIndex = this.responses.length - 1;
      }
      // handle when selection is changed
      else if (changes.selected) {
        this.selectedResponseIndex = this.responses.findIndex(
          (r) => r.id == this.selected.id
        );
      }
    }
  }

  selectResponse(index) {
    this.selectedResponseIndex = index;
    this.selectionChanged.emit(this.responses[this.selectedResponseIndex]);
  }

  removeResponse(index: number) {
    const itemIds = this.responses[index].destinations.map((d) => d.id);
    this.basket.removeItemsFromBasket(this.basketId, itemIds);

    this.responses.splice(index, 1);
    if (this.selectedResponseIndex === index) {
      if (index !== 0) {
        this.selectedResponseIndex--;
      }
      this.selectionChanged.emit(this.responses[this.selectedResponseIndex]);
    } else if (this.selectedResponseIndex > index) {
      this.selectedResponseIndex--;
    }
  }

  onClearAll() {
    const itemIds = flatten(
      this.responses.map((r) => r.destinations.map((d) => d.id))
    );
    this.basket.removeItemsFromBasket(this.basketId, itemIds);

    this.responses.splice(0, this.responses.length);
    this.selectedResponseIndex = -1;
    this.selectionChanged.emit(null);
  }
}
