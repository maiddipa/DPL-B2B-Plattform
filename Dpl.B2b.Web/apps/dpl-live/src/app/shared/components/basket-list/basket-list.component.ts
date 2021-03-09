import {
  ChangeDetectionStrategy,
  Component,
  EventEmitter,
  Input,
  OnInit,
  Output,
} from '@angular/core';
import { combineLatest, Observable } from 'rxjs';
import { map, publishReplay, refCount, startWith } from 'rxjs/operators';
import { SearchBasketService } from 'apps/dpl-live/src/app/search/services/search-basket.service';
import { IBasket, IBasketItem } from '../../services';

interface IViewData {
  basket: IBasket<any, any>;
}

@Component({
  selector: 'app-basket-list',
  templateUrl: './basket-list.component.html',
  styleUrls: ['./basket-list.component.scss'],
})
export class BasketListComponent implements OnInit {
  @Input() basketId: number;
  @Input() selected: number;
  @Output() selectionChanged = new EventEmitter<IBasketItem<any, any>>();
  @Output() checkout = new EventEmitter<IBasket<any, any>>();

  viewData$: Observable<IViewData>;

  constructor(private basketService: SearchBasketService) {}

  ngOnInit() {
    const basket$ = this.basketService.getBasket(this.basketId);

    this.viewData$ = combineLatest(basket$).pipe(
      map(([basket]) => {
        return <IViewData>{
          basket,
        };
      }),
      publishReplay(1),
      refCount()
    );
  }

  onRemoveFromBasket(item: IBasketItem<any, any>) {
    this.basketService.removeItemFromBasket(this.basketId, item.id);
  }

  onClearAll() {
    this.basketService.removeAllItemsFromBasket(this.basketId);
  }

  onCheckout(basket: IBasket<any, any>) {
    this.checkout.emit(basket);
  }

  onSelected(item: IBasketItem<any, any>) {
    this.selected = item.id;
    this.selectionChanged.emit(item);
  }
}
