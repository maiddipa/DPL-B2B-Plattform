import { Injectable } from '@angular/core';
import {
  OrderGroup,
  OrderLoad,
  OrderQuantityType,
  OrderTransportType,
  EmployeeNoteType,
  DplProblemDetails,
  StateItemType,
} from '@app/api/dpl';
import { DplApiService, LocalizationService, SessionService } from '@app/core';
import {
  CalendarWeekPipe,
  CountryPipe,
  DayOfWeekPipe,
  LoadCarrierPipe,
  OrderTypePipe,
} from '@app/shared/pipes';
import {
  BasketService,
  IBasketCheckoutResponseItem,
  OnBehalfOfService,
} from '@app/shared/services';
import * as _ from 'lodash';
import * as moment from 'moment';
import { combineLatest, forkJoin, of, throwError } from 'rxjs';
import {
  catchError,
  first,
  map,
  pluck,
  publishReplay,
  refCount,
  switchMap,
} from 'rxjs/operators';
import { WrappedError } from '../../core/services/app-error-handler.service';

import { CustomerDivisionsService } from '../../customers/services/customer-divisions.service';
import { AddToBasketDialogComponentResponse } from '../components/add-to-basket-dialog/add-to-basket-dialog.component';
import {
  ISearchBasketData,
  SearchBasketItem,
} from './search-basket.service.types';
import { IDestination, ISearchResponse } from './search.service.types';

const RETRY_ATTEMPTS = 2;
@Injectable({
  providedIn: 'root',
})
export class SearchBasketService extends BasketService<
  ISearchBasketData,
  OrderLoad[]
> {
  constructor(
    session: SessionService,
    private dpl: DplApiService,
    private countryPipe: CountryPipe,
    private orderTypePipe: OrderTypePipe,
    private calendarWeekPipe: CalendarWeekPipe,
    private dayOfWeekPipe: DayOfWeekPipe,
    private loadCarrierPipe: LoadCarrierPipe,
    private divisions: CustomerDivisionsService,
    private onBehalfOfService: OnBehalfOfService,
    private localization: LocalizationService
  ) {
    super(session);
  }

  checkout(basketId: number) {
    const basket$ = this.getBasket(basketId).pipe(
      first(),
      publishReplay(1),
      refCount()
    );

    const divisionsId$ = this.divisions
      .getActiveDivision()
      .pipe(first(), pluck('id'));

    const dplNote$ = this.onBehalfOfService.openOnBehalfofDialog(
      EmployeeNoteType.Create
    );

    const orderGroups$ = combineLatest([basket$, divisionsId$, dplNote$]).pipe(
      map(([basket, divisionId, dplNote]) =>
        basket.items.map((basketItem) => {
          const orderDate = moment(basketItem.data.response.input.calendarWeek)
            .add(basketItem.data.dayOfWeek, 'days')
            .toDate();

          // if its load carrier quantity input + qty is smaller than stack height use qty as stack height
          const stackHeight =
            basketItem.data.response.input.quantityType ===
              OrderQuantityType.LoadCarrierQuantity &&
            basketItem.data.response.input.quantity <
              basketItem.data.response.input.stackHeight
              ? basketItem.data.response.input.quantity
              : basketItem.data.response.input.stackHeight;

          // TODO implement other quantity types than load carrier quantity
          // TODO add triggering emails
          return this.dpl.orderGroups
            .post({
              divisionId,
              type: basketItem.data.response.input.orderType,
              transportType: OrderTransportType.Self,
              postingAccountId:
                basketItem.data.response.input.postingAccount.id,

              matchLmsOrderGroupRowGuid: basketItem.data.orderGroupGuid,

              loadCarrierId: basketItem.data.response.input.palletId,
              baseLoadCarrierId: basketItem.data.baseLoadCarrierId,

              quantityType: basketItem.data.response.input.quantityType,

              numberOfLoads:
                basketItem.data.response.input.quantityType ===
                OrderQuantityType.Load
                  ? basketItem.data.response.input.quantity
                  : undefined,

              loadCarrierQuantity:
                basketItem.data.response.input.quantityType ===
                OrderQuantityType.LoadCarrierQuantity
                  ? basketItem.data.response.input.quantity
                  : undefined,

              stackHeightMin: stackHeight,
              stackHeightMax: stackHeight,

              earliestFulfillmentDateTime: orderDate,
              latestFulfillmentDateTime: orderDate,

              dplNote,
            })
            .pipe(
              map((orderGroup) => {
                const loads = _(orderGroup.orders)
                  .map((i) => i.loads)
                  .flatten()
                  .value();

                const response: IBasketCheckoutResponseItem<OrderLoad[]> = {
                  status: 'ok',
                  data: loads,
                };

                return response;
              }),
              catchError((error, obs) => {
                // check if this is an error with one of the specified message ids
                // if so: translate the errors and return a response object
                // if not: rethrow the exception so that the error is handled by thedefault global error handling

                const parsedError = WrappedError.parse(error);

                if (!parsedError.ProblemDetails) {
                  return throwError(error);
                }

                const details = parsedError.ProblemDetails as DplProblemDetails;
                if (details.ruleStates && details.ruleStates.length === 0) {
                  return throwError(error);
                }

                const actionableRuleStates = _(details.ruleStates)
                  .flatten()
                  .filter((ruleState) => {
                    return (
                      ruleState.messageId ===
                        'Error|OrderMatches|OrdersNotMatchError' ||
                      ruleState.messageId ===
                        'Error|OrderMatches|AvailableError' ||
                      ruleState.messageId ===
                        'Error|OrderMatches|QuantityExceededDemandError' ||
                      ruleState.messageId ===
                        'Error|OrderMatches|QuantityExceededSupplyError'
                    );
                  })
                  .value();

                if (actionableRuleStates.length === 0) {
                  return throwError(error);
                }

                const groupedMessages = _(actionableRuleStates)
                  .groupBy((i) => i.type)
                  .mapValues((stateItems) => {
                    return stateItems.map((stateItem) => {
                      return this.localization.getTranslationById(
                        stateItem.messageId
                      );
                    });
                  })
                  .value();

                const response: IBasketCheckoutResponseItem<OrderLoad[]> = {
                  status: 'failed',
                  errorMessages: groupedMessages[StateItemType.Error] || [],
                  warningMessages: groupedMessages[StateItemType.Warning] || [],
                };

                return of(response);
              }),
              map((response) => {
                return { basketItem, response };
              })
            );
        })
      ),
      switchMap((orderGroupObsArray) => forkJoin(orderGroupObsArray))
    );

    // grab all orders from all order groups
    // in case of self service there should be only one order per order group
    // but we need to deal with array of order ids as this is what the api returns
    const orders$ = orderGroups$.pipe(
      map((groups) => {
        const items = _(groups).map(({ basketItem, response }) => {
          basketItem.response = response;
          return basketItem;
        });

        return items;
      })
    );

    return combineLatest([basket$, orders$]).pipe(
      map(([basket, orders]) => {
        let status;
        if (orders.every((o) => o.response.status === 'ok')) {
          status = 'ok';
        } else if (orders.some((o) => o.response.status === 'ok')) {
          status = 'partial';
        } else {
          status = 'failed';
        }

        basket.response = {
          status,
        };

        return basket;
      })
    );
  }

  public createSearchBasketItem(
    response: ISearchResponse,
    destination: IDestination,
    addToBasketResponse: AddToBasketDialogComponentResponse
  ) {
    const item: SearchBasketItem = {
      id: destination.id,
      title: null, // HACK remove title, was company name previously but we are now using address only
      subTitles: [
        `${this.countryPipe.transform(
          destination.info.address.country,
          'iso2'
        )}-${destination.info.address.details}`,
        `${this.orderTypePipe.transform(response.input.orderType)} ${
          response.input.calculatedQuantity
        } ${this.loadCarrierPipe.transform(response.input.palletId)}`,
        `${this.calendarWeekPipe.transform(
          response.input.calendarWeek
        )} - ${this.dayOfWeekPipe.transform(
          addToBasketResponse.dayOfWeek,
          response.input.calendarWeek
        )}`,
      ],
      data: {
        dayOfWeek: addToBasketResponse.dayOfWeek,
        orderGroupGuid: addToBasketResponse.orderGroupGuid,
        baseLoadCarrierId: addToBasketResponse.baseLoadCarrierId,
        response,
        destination,
      },
    };

    return item;
  }
}
