import { BehaviorSubject, Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { SessionService } from 'apps/dpl-live/src/app/core/services/session.service';

import { IBasket, IBasketItem } from './basket.service.types';

// @Injectable({
//   providedIn: "root"
// })
export abstract class BasketService<T, R> {
  private baskets: BehaviorSubject<IBasket<T, R>[]>;
  constructor(private session: SessionService) {
    this.baskets = new BehaviorSubject([]);
  }

  getNewBasket() {
    const basket = <IBasket<T, R>>{
      id: this.session.getNextId(),
      items: [],
    };

    const baskets = this.baskets.value;
    baskets.push(basket);
    this.baskets.next(baskets);

    return this.getBasket(basket.id);
  }

  getBasket(basketId: number) {
    return this.baskets.pipe(
      map((baskets) => {
        const basket = baskets.find((b) => b.id == basketId);
        if (!basket) {
          throw `No basket found for basketId ${basketId}`;
        }
        return basket;
      })
    );
  }

  removeBasket(basketId: number) {
    const baskets = this.baskets.value;
    const basketIndex = baskets.findIndex((b) => b.id !== basketId);

    if (basketIndex !== -1) {
      baskets.splice(basketIndex, 1);
    }
    this.baskets.next(baskets);
  }

  addItemToBasket(basketId: number, item: IBasketItem<T, R>) {
    const baskets = this.baskets.value;
    const basket = baskets.find((b) => b.id === basketId);
    if (!basket) {
      throw `No basket found for basketId ${basketId}`;
    }

    if (basket.items.find((i) => i.id === item.id)) {
      return;
    }

    basket.items = [...basket.items, item];

    // this is neccessary to trigger changes
    this.baskets.next(baskets);
  }

  removeItemFromBasket(basketId: number, itemId: number) {
    const baskets = this.baskets.value;
    const basket = baskets.find((b) => b.id == basketId);
    if (!basket) {
      throw `No basket found for basketId ${basketId}`;
    }

    basket.items = basket.items.filter((i) => i.id !== itemId);

    // this is neccessary to trigger changes
    this.baskets.next(baskets);
  }

  removeItemsFromBasket(basketId: number, itemIds: number[]) {
    const baskets = this.baskets.value;
    const basket = baskets.find((b) => b.id == basketId);
    if (!basket) {
      throw `No basket found for basketId ${basketId}`;
    }

    basket.items = basket.items.filter(
      (item) => !itemIds.some((id) => item.id === id)
    );
    // itemIds.forEach(itemId => {
    //   const itemIndex = basket.items.findIndex(i => i.id === itemId);
    //   if (itemIndex !== -1) {
    //     basket.items.splice(itemIndex, 1);
    //   }
    // });

    // this is neccessary to trigger changes
    this.baskets.next(baskets);
  }

  removeAllItemsFromBasket(basketId: number) {
    const baskets = this.baskets.value;
    const basket = baskets.find((b) => b.id == basketId);
    if (!basket) {
      throw `No basket found for basketId ${basketId}`;
    }

    basket.items = [];

    // this is neccessary to trigger changes
    this.baskets.next(baskets);
  }

  abstract checkout(basketId: number): Observable<IBasket<T, R>>;
}
