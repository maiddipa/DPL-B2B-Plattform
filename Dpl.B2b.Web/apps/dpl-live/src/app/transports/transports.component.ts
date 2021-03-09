import { Component, OnInit } from '@angular/core';
import { PageEvent } from '@angular/material/paginator';
import { Sort } from '@angular/material/sort';
import { Router } from '@angular/router';
import { TransportOffering, TransportOfferingSortOption } from '@app/api/dpl';
import { PaginatedTableConfig } from '@dpl/dpl-lib';
import { combineLatest, Observable, Subject } from 'rxjs';
import {
  debounceTime,
  map,
  publishReplay,
  refCount,
  startWith,
  switchMap,
} from 'rxjs/operators';

import { FilterService } from '../filters/services/filter.service';
import { FilterContext } from '../filters/services/filter.service.types';
import { TransportsService } from './services/transports.service';
import { ITransport } from './state/transport.model';
import { convertToDplApiSort } from '@app/core';
import { ChatLinkData } from '../chat/components/chat-link.directive';
import { ChannelType } from '../chat/services/chat.service.types';

interface ViewData {
  rows: ITransport[];
  listFiltered: boolean;
}

@Component({
  selector: 'app-transports',
  templateUrl: './transports.component.html',
  styleUrls: ['./transports.component.scss'],
})
export class TransportsComponent implements OnInit {
  sortOption = TransportOfferingSortOption;
  config: PaginatedTableConfig<ITransport[]>;
  filterContext: FilterContext = 'transports';
  displayedColumns: string[] = [
    'referenceNumber',
    'status',
    'earliestPickupDate',
    'latestDropoffDate',
    'country',
    'zip',
    'city',
    'targetCountry',
    'targetZip',
    'targetCity',
    // 'amount',
    // 'length',
    // 'weight',
    // 'type',
    'distance',
    'chat',
  ];

  transportsSubject$ = new Subject<ITransport[]>();
  viewData$: Observable<ViewData>;

  constructor(
    private router: Router,
    private transport: TransportsService,
    private filterService: FilterService
  ) {}

  ngOnInit() {
    const filters$ = this.filterService
      .getAppliedFilters(this.filterContext)
      .pipe(debounceTime(100), publishReplay(1), refCount());

    const getData = (sort: Sort, page: PageEvent) => {
      return filters$.pipe(
        switchMap((filters) => {
          return this.transport.getTransports({
            filters,
            page: page.pageIndex + 1,
            limit: page.pageSize,
            sort: convertToDplApiSort<TransportOfferingSortOption>(sort),
          });
        })
      );
    };

    this.config = {
      getData,
      defaultSort: {
        active: TransportOfferingSortOption.ReferenceNumber,
        direction: 'asc',
      },
      resetTrigger$: filters$,
      defaultPage: {
        pageSize: 10,
        pageIndex: 0,
      },
    };

    const transports$ = this.transportsSubject$
      .asObservable()
      .pipe(startWith(null));

    const listFiltered$ = filters$.pipe(
      map((filters) => {
        return (
          filters &&
          filters.filter((x) => x.value && x.value.length > 0).length > 0
        );
      })
    );

    this.viewData$ = combineLatest(transports$, listFiltered$).pipe(
      map(([transports, listFiltered]) => {
        return <ViewData>{
          rows: transports,
          listFiltered,
        };
      })
    );
  }

  onData(data: ITransport[]) {
    this.transportsSubject$.next(data);
  }

  onTransportTapped(transport: TransportOffering) {
    this.router.navigate(['/transports', transport.id]);
  }
}
