import {
  Component,
  EventEmitter,
  OnDestroy,
  OnInit,
  Output,
  ViewEncapsulation,
} from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';
import { LoadCarrierOfferingDetail, OrderGroup } from '@app/api/dpl';
import { DayOfWeekPipe, IBasketItem } from '@app/shared';
import { getTypedParams, ILabelValue, LoadingService } from '@dpl/dpl-lib';
import * as _ from 'lodash';
import * as moment from 'moment';
import { combineLatest, EMPTY, from, Observable, of, Subject } from 'rxjs';
import {
  distinctUntilChanged,
  distinctUntilKeyChanged,
  first,
  map,
  publishReplay,
  refCount,
  startWith,
  switchMap,
  tap,
} from 'rxjs/operators';

import { compareById, getOffsetSinceStartOfWeek } from '../core/utils';
import {
  AddToBasketDialogComponent,
  AddToBasketDialogComponentData,
  AddToBasketDialogComponentResponse,
} from './components/add-to-basket-dialog/add-to-basket-dialog.component';
import { SearchBasketService } from './services/search-basket.service';
import {
  ISearchBasketData,
  SearchBasket,
} from './services/search-basket.service.types';
import { SearchService } from './services/search.service';
import {
  IDestination,
  IMapClickPosition,
  ISearchInput,
  ISearchResponse,
  SearchInputType,
} from './services/search.service.types';

interface IViewData {
  basket: SearchBasket;
  responses: ISearchResponse[];
  selectedResponse: ISearchResponse;
  selectedDestinationId: number;
  selectedBasketItemId: number;
}

type SearchComponentComponentParams = {
  basketId: number;
};

@Component({
  selector: 'app-search',
  templateUrl: './search.component.html',
  styleUrls: ['./search.component.scss'],
  encapsulation: ViewEncapsulation.None,
})
export class SearchComponent implements OnInit, OnDestroy {
  @Output() checkout = new EventEmitter<SearchBasket>();
  showSidebar = true;
  showSearch = true;
  showResponses = true;
  showMap = true;

  viewData$: Observable<IViewData>;
  selectedResponse$: Subject<ISearchResponse> = new Subject();
  selectedDestinationId$: Subject<number> = new Subject();

  searchInput: ISearchInput;

  constructor(
    private search: SearchService,
    private basketService: SearchBasketService,
    private dialog: MatDialog,
    private dayOfWeekPipe: DayOfWeekPipe,
    private router: Router,
    private route: ActivatedRoute,
    private loadingService: LoadingService
  ) {}

  ngOnInit() {
    const params$ = getTypedParams<SearchComponentComponentParams>(this.route);

    const basket$ = params$.pipe(
      tap((params) => {
        if (!params.route.basketId) {
          this.search.clearSearchResponses();
        }
      }),
      switchMap((params) => {
        return params.route.basketId
          ? this.basketService.getBasket(params.route.basketId)
          : this.basketService.getNewBasket().pipe(
              switchMap((basket) => {
                return from(
                  this.router.navigate(['/search/start', basket.id])
                ).pipe(switchMap(() => EMPTY));
              })
            );
      }),
      publishReplay(1),
      refCount()
    );

    const responses$ = this.search.getSearchResponses().pipe(
      distinctUntilKeyChanged('length'),
      tap((responses) => {
        this.selectedResponse$.next(
          responses.length > 0 ? responses[responses.length - 1] : null
        );
      }),
      publishReplay(1),
      refCount()
    );

    const selectedResponse$ = this.selectedResponse$.pipe(
      startWith(null),
      distinctUntilChanged(compareById),
      tap((response) => {
        // when new result is selected reset selected destination
        this.selectedDestinationId$.next(-1);
      })
    );

    const selectedDestinationId$ = this.selectedDestinationId$.pipe(
      startWith(null)
    );

    const selectedBasketItemId$ = combineLatest([
      basket$,
      selectedDestinationId$,
    ]).pipe(
      map(([basket, destinationId]) => {
        const item = basket
          ? basket.items.find((i) => i.data.destination.id === destinationId)
          : null;
        if (!item) {
          return null;
        }
        return item.id;
      }),
      startWith(null)
    );

    this.viewData$ = combineLatest([
      basket$,
      responses$,
      selectedResponse$,
      selectedDestinationId$,
      selectedBasketItemId$,
    ]).pipe(
      map(
        ([
          basket,
          responses,
          selectedResponse,
          selectedDestinationId,
          selectedBasketItemId,
        ]) => {
          return <IViewData>{
            basket,
            responses,
            selectedResponse,
            selectedDestinationId,
            selectedBasketItemId,
          };
        }
      ),
      publishReplay(1),
      refCount()
    );
  }

  ngOnDestroy() {}

  onSearch(search: ISearchInput) {
    this.search
      .search(search)
      .pipe(this.loadingService.showLoadingWhile())
      .subscribe();
  }

  onSearchChanged(search: ISearchInput) {
    this.searchInput = search;
  }

  onMapSearch(position: IMapClickPosition) {
    const search = {
      ...this.searchInput,
      ...{ type: 'map' as SearchInputType, isZipOnlySearch: false, position },
    };
    this.onSearch(search);
  }

  onRadiusChanged(radius: number) {
    this.viewData$.pipe(first()).subscribe((data) => {
      const search = {
        // neccessary to handle radius change for map click based searches
        ...(data.selectedResponse
          ? data.selectedResponse.input
          : this.searchInput),
        ...{ radius },
      };
      this.onSearch(search);
    });
  }

  onResponseSelected(response: ISearchResponse) {
    this.selectedResponse$.next(response);
  }

  onDestinationSelected(id: number) {
    this.selectedDestinationId$.next(id);
  }

  onAddToBasket(destination: IDestination) {
    const addToBasketResponse$ = this.viewData$.pipe(
      first(),
      switchMap((data) => {
        const searchDaysOfWeek = new Set(
          data.selectedResponse.input.daysOfWeek
        );

        const daysOfWeek = _(destination.info.data.businessHours)
          .map((i) => i.dayOfWeek)
          .uniq()
          .map((dayOfWeek) => getOffsetSinceStartOfWeek(dayOfWeek))
          // only include days that were selected during search selected days
          .filter((i) => searchDaysOfWeek.has(i))
          .sortBy((i) => i)
          .value();

        const startDate = moment(
          data.selectedResponse.input.calendarWeek
        ).startOf('day');

        const options = daysOfWeek
          .map((dayOfWeek) => {
            const dayOfWeekOption: ILabelValue<number> = {
              value: dayOfWeek,
              label: `${this.dayOfWeekPipe.transform(
                dayOfWeek,
                data.selectedResponse.input.calendarWeek
              )}`,
            };

            // get offering details that are valid for the current dayOfWeek
            // only return one per baseLoadCarrierId
            const offeringDetails: LoadCarrierOfferingDetail[] = _(
              destination.info.data.details
            )
              .filter((offeringDetail) => {
                const dateInWeek = startDate
                  .clone()
                  .add(dayOfWeek, 'days')
                  .toDate();

                const fromDate = moment(offeringDetail.availabilityFrom)
                  .startOf('day')
                  .toDate();
                const toDate = moment(offeringDetail.availabilityTo)
                  .endOf('day')
                  .toDate();

                return fromDate <= dateInWeek && dateInWeek <= toDate;
              })
              .uniqBy((i) => i.baseLoadCarrierId)
              .value();

            return {
              dayOfWeekOption,
              offeringDetails,
            };
          })
          // only include options (days) with offerings
          .filter((option) => option.offeringDetails.length > 0);

        // only auto select when only single day was selected in search
        if (
          data.selectedResponse.input.daysOfWeek.length === 1 &&
          options.length === 1 &&
          options[0].offeringDetails &&
          options[0].offeringDetails.length === 1
        ) {
          const option = options[0];
          const details = option.offeringDetails[0];
          const response: AddToBasketDialogComponentResponse = {
            orderGroupGuid: details.guid,
            dayOfWeek: option.dayOfWeekOption.value,
            baseLoadCarrierId: details.baseLoadCarrierId,
          };
          return of(response);
        }

        const addToBasketDialogData: AddToBasketDialogComponentData = {
          options,
        };

        return this.dialog
          .open<
            AddToBasketDialogComponent,
            AddToBasketDialogComponentData,
            AddToBasketDialogComponentResponse
          >(AddToBasketDialogComponent, {
            data: addToBasketDialogData,
          })
          .afterClosed();
      })
    );

    combineLatest(this.viewData$, addToBasketResponse$)
      .pipe(first())
      .subscribe(([data, addToBasketResponse]) => {
        if (!addToBasketResponse) {
          return;
        }

        const item = this.basketService.createSearchBasketItem(
          data.selectedResponse,
          destination,
          addToBasketResponse
        );
        this.basketService.addItemToBasket(data.basket.id, item);
      });
  }

  onBasketItemSelectionChanged(
    item: IBasketItem<ISearchBasketData, OrderGroup>
  ) {
    this.onResponseSelected(item.data.response);
    this.onDestinationSelected(item.data.destination.id);
  }

  onCheckout() {
    this.viewData$
      .pipe(
        first(),
        switchMap((data) =>
          this.router.navigate(['search/checkout', data.basket.id])
        )
      )
      .subscribe();
  }
}
