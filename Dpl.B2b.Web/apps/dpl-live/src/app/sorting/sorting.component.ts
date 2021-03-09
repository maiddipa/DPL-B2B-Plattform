import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import * as _ from 'lodash';
import { combineLatest, Observable } from 'rxjs';
import { map, publishReplay, refCount, switchMap } from 'rxjs/operators';
import {
  getTypedParams,
  LoadingService,
} from '../../../../../libs/dpl-lib/src';
import { DplApiService } from '../core';
import { LoadCarrierReceiptSortingOption } from '../core/services/dpl-api-services';
import { SortingService } from './services/sorting.service';

type SortingComponentRouteParams = {
  id: string;
};
type SortingComponentQueryParams = {};

type ViewData = {
  sortInput: LoadCarrierReceiptSortingOption;
};

@Component({
  selector: 'dpl-sorting',
  templateUrl: './sorting.component.html',
  styleUrls: ['./sorting.component.scss'],
})
export class SortingComponent implements OnInit {
  viewData$: Observable<ViewData>;

  constructor(private route: ActivatedRoute, private dpl: DplApiService) {}

  ngOnInit(): void {
    const params$ = getTypedParams<
      SortingComponentRouteParams,
      SortingComponentQueryParams
    >(this.route).pipe(publishReplay(1), refCount());

    const receiptInput$ = params$.pipe(
      switchMap((params) => {
        return this.dpl.loadCarrierReceipts.getSortingOptionsByReceiptId(
          parseInt(params.route.id)
        );
      })
    );

    this.viewData$ = combineLatest([receiptInput$]).pipe(
      map(([sortInput]) => {
        const orderedSortInput = {
          ...sortInput,
          sortingPositions: sortInput.sortingPositions.map((position) => {
            // HACK Demo -> filter possibleQualities clientside (EUR and DD)
            let sortingQualities = [];
            switch (position.loadCarrierId) {
              case 103:
              case 203:
                sortingQualities = position.possibleSortingQualities.filter(
                  (x) =>
                    x.id == 10 ||
                    x.id == 11 ||
                    x.id == 12 ||
                    x.id == 14 ||
                    x.id == 2 ||
                    x.id == 20
                );
                break;
              case 104:
              case 204:
                sortingQualities = position.possibleSortingQualities.filter(
                  (x) => x.id == 2 || x.id == 20
                );
                break;
              case 108:
              case 208:
                sortingQualities = position.possibleSortingQualities.filter(
                  (x) => x.id == 20
                );
                break;

              default:
                sortingQualities = position.possibleSortingQualities;
                break;
            }
            return {
              ...position,
              possibleSortingQualities: _.orderBy(sortingQualities, 'order'),
            };
          }),
        };

        const viewData: ViewData = {
          sortInput: orderedSortInput,
        };
        return viewData;
      })
    );
  }
}
