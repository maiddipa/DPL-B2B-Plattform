import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';
import { PricingComponent, TermsComponent } from '@app/shared';
import { getTypedParams, LoadingService } from '@dpl/dpl-lib';
import { combineLatest, Observable } from 'rxjs';
import { first, map, switchMap } from 'rxjs/operators';

import { CustomersService } from '../../../customers/services/customers.service';
import { ICustomer } from '../../../customers/state/customer.model';
import { SearchBasketService } from '../../services/search-basket.service';
import { SearchBasket } from '../../services/search-basket.service.types';

interface IViewData {
  basket: SearchBasket;
  customer: ICustomer<number>;
}

type SearchCheckoutComponentParams = {
  basketId: number;
};

@Component({
  selector: 'app-search-checkout',
  templateUrl: './search-checkout.component.html',
  styleUrls: ['./search-checkout.component.scss'],
})
export class SearchCheckoutComponent implements OnInit {
  @Output() complete = new EventEmitter<SearchBasket>();
  @Output() cancel = new EventEmitter<void>();

  viewData$: Observable<IViewData>;
  constructor(
    private basketService: SearchBasketService,
    private dialog: MatDialog,
    private customer: CustomersService,
    private router: Router,
    private route: ActivatedRoute,
    private loadingService: LoadingService
  ) {}

  ngOnInit() {
    const params$ = getTypedParams<SearchCheckoutComponentParams>(this.route);

    const basket$ = params$.pipe(
      switchMap((params) => this.basketService.getBasket(params.route.basketId))
    );

    const customer$ = this.customer.getActiveCustomer();

    this.viewData$ = combineLatest(basket$, customer$).pipe(
      map(([basket, customer]) => {
        return {
          basket,
          customer,
        };
      })
    );
  }

  onCancel() {
    this.cancel.next();
  }

  onShowPrices() {
    this.dialog
      .open(PricingComponent, {
        width: '100%',
        height: '100%',
      })
      .afterClosed()
      .subscribe();
  }
  onShowTerms() {
    this.dialog
      .open(TermsComponent, {
        width: '100%',
        height: '100%',
      })
      .afterClosed()
      .subscribe();
  }

  onCheckout() {
    this.viewData$
      .pipe(
        switchMap((data) => this.basketService.checkout(data.basket.id)),
        first(),
        switchMap((basket) =>
          this.router.navigate(['search/confirmation', basket.id])
        ),
        this.loadingService.showLoadingWhile()
      )
      .subscribe();
  }
}
