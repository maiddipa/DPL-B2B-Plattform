import { Injectable } from '@angular/core';
import {
  ActivatedRouteSnapshot,
  CanActivate,
  Router,
  RouterStateSnapshot,
} from '@angular/router';
import { from, Observable, of } from 'rxjs';
import { catchError, first, map } from 'rxjs/operators';

import { SearchBasketService } from '../services/search-basket.service';

@Injectable({
  providedIn: 'root',
})
export class BasketGuard implements CanActivate {
  constructor(private basket: SearchBasketService, private router: Router) {}

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): Observable<boolean> {
    return this.basket.getBasket(route.params.basketId).pipe(
      catchError(() => {
        return from(this.router.navigate(['/search/start'])).pipe(
          map(() => null)
        );
      }),
      map((basket) => !!basket)
    );
  }
}
