import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { OrderLoad } from '@app/api/dpl';
import { OrderLoadsViewType } from '@app/shared';
import { getTypedParams } from '@dpl/dpl-lib';
import * as _ from 'lodash';
import { combineLatest, Observable } from 'rxjs';
import { map, publishReplay, refCount, switchMap } from 'rxjs/operators';

import { OrderLoadsComponentQueryParams } from '../../../order-loads/order-loads.component';
import { SearchBasketService } from '../../services/search-basket.service';
import {
  SearchBasket,
  SearchBasketItem,
} from '../../services/search-basket.service.types';

type ViewData = {
  basket: SearchBasket;
  basketItemOrderLoads: {
    basketItem: SearchBasketItem;
    orderLoad: OrderLoad;
  }[];
};

type OrderConfirmationComponentParams = {
  basketId: number;
};

@Component({
  selector: 'app-order-confirmation',
  templateUrl: './order-confirmation.component.html',
  styleUrls: ['./order-confirmation.component.scss'],
  //changeDetection: ChangeDetectionStrategy.OnPush,
})
export class OrderConfirmationComponent implements OnInit {
  viewData$: Observable<ViewData>;
  constructor(
    private basketService: SearchBasketService,
    private router: Router,
    private route: ActivatedRoute
  ) {}

  ngOnInit() {
    const params$ = getTypedParams<OrderConfirmationComponentParams>(
      this.route
    );

    const basket$ = params$.pipe(
      switchMap((params) =>
        this.basketService.getBasket(params.route.basketId)
      ),
      publishReplay(1),
      refCount()
    );

    const basketItemOrderLoads$ = basket$.pipe(
      map((basket) => {
        return _(basket.items)
          .map((basketItem) => {
            if (!basketItem.response.data) {
              return { basketItem, orderLoad: {} as OrderLoad };
            }
            return basketItem.response.data.map((orderLoad) => {
              return { basketItem, orderLoad };
            });
          })
          .flatten()
          .value();
      })
    );

    this.viewData$ = combineLatest(basket$, basketItemOrderLoads$).pipe(
      map(([basket, basketItemOrderLoads]) => {
        return {
          basket,
          basketItemOrderLoads,
        };
      })
    );
  }

  onPrint() {}

  getQueryParams(
    basketItemOrders: ViewData['basketItemOrderLoads']
  ): OrderLoadsComponentQueryParams {
    return {
      type: OrderLoadsViewType.LivePooling,
      highlightId:
        basketItemOrders && basketItemOrders.length > 0
          ? basketItemOrders.map((item) => item.orderLoad.id)
          : null,
    };
  }
}
