import {
  Component,
  OnInit,
  ChangeDetectionStrategy,
  Optional,
  Inject,
  ViewChild,
} from '@angular/core';
import CustomStore from 'devextreme/data/custom_store';
import * as AspNetData from 'devextreme-aspnet-data-nojquery';
import {
  AuthenticationService,
  DplApiService,
  LocalizationService,
} from '../../../core';
import { BehaviorSubject, combineLatest, forkJoin, Observable, of } from 'rxjs';
import { filter, map, tap } from 'rxjs/operators';
import {
  API_BASE_URL,
  LoadCarrierReceipt,
  LoadCarrierReceiptPosition,
  LoadCarrierReceiptType,
  ResourceAction,
} from '../../../core/services/dpl-api-services';
import { CustomerDivisionsService } from '../../../customers/services/customer-divisions.service';
import ArrayStore from 'devextreme/data/array_store';
import { MatDialog } from '@angular/material/dialog';
import {
  SortDialogData,
  SortDialogResult,
  SortingDialogComponent,
} from '../../../sorting/components/sorting-dialog/sorting-dialog.component';
import { UserService } from '../../../user/services/user.service';
import { LoadingService } from '../../../../../../../libs/dpl-lib/src';
import { SortingService } from '../../../sorting/services/sorting.service';
import { DxDataGridComponent } from 'devextreme-angular';
import { PrintService } from '../../../shared';

type ViewData = {
  dataSource: CustomStore;
  canUpdateSortRequired: boolean;
  typeLookupData: {
    id: number;
    value: string;
    display: string;
  }[];
};

enum FilterPreset {
  All = 'All',
  Completed = 'Completed',
  Open = 'Open',
}

@Component({
  selector: 'dpl-load-carrier-receipts-list',
  templateUrl: './load-carrier-receipts-list.component.html',
  styleUrls: ['./load-carrier-receipts-list.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class LoadCarrierReceiptsListComponent implements OnInit {
  @ViewChild(DxDataGridComponent, { static: false })
  dataGrid: DxDataGridComponent;
  baseUrl: string;
  viewData$: Observable<ViewData>;
  reloadSub = new BehaviorSubject<boolean>(false);
  resourceAction = ResourceAction;

  filterPreset = FilterPreset;
  selectedFilter = FilterPreset.All;
  loadCarrierReceiptType = LoadCarrierReceiptType;

  constructor(
    private dpl: DplApiService,
    private authenticationService: AuthenticationService,
    private customerDivisionService: CustomerDivisionsService,
    private dialog: MatDialog,
    private userService: UserService,
    private loadingService: LoadingService,
    private printService: PrintService,
    private localizationService: LocalizationService,
    @Optional() @Inject(API_BASE_URL) baseUrl?: string
  ) {
    this.baseUrl = baseUrl;
  }

  ngOnInit(): void {
    const activeDivision$ = this.customerDivisionService
      .getActiveDivision()
      .pipe(filter((x) => !!x));

    const canUpdateSortRequired$ = this.userService.getIsDplEmployee();

    const reload$ = this.reloadSub.asObservable();

    const typeLookupData$ = this.getReceiptTypeData();

    this.viewData$ = combineLatest([
      activeDivision$,
      canUpdateSortRequired$,
      typeLookupData$,
      reload$,
    ]).pipe(
      map(([division, canUpdateSortRequired, typeLookupData]) => {
        const dataSource = AspNetData.createStore({
          key: 'id',
          loadUrl: this.baseUrl + '/loadcarrierreceipts',
          loadParams: {
            customerDivisionId: division.id,
          },
          onBeforeSend: async (method, ajaxOptions) => {
            return await this.authenticationService
              .getTokenForRequestUrl(this.baseUrl)
              .pipe(
                map((token) => {
                  ajaxOptions.xhrFields = { withCredentials: false };
                  ajaxOptions.headers = { Authorization: 'Bearer ' + token };
                })
              )
              .toPromise();
          },
          onLoaded: async (result: LoadCarrierReceipt[]) => {
            console.log(result);
            const receipts$ = result.map((receipt) => of(receipt));
            return forkJoin(receipts$).toPromise();
          },
        });

        const viewData: ViewData = {
          dataSource,
          canUpdateSortRequired,
          typeLookupData,
        };
        return viewData;
      })
    );
  }

  getPositionsDataSource(
    loadCarrierReceiptId: number,
    sortingCompleted: boolean,
    positions: LoadCarrierReceiptPosition[]
  ) {
    console.log('getPositionsDataSource', positions);
    if (sortingCompleted) {
      // CustomStore match Position and Output
      const customStore = {
        store: new CustomStore({
          key: 'id',
          loadMode: 'raw',
          load: async () => {
            return this.dpl.loadCarrierSortingService
              .getByLoadCarrierReceiptId({ loadCarrierReceiptId })
              .pipe(
                map((sortingOutput) => {
                  return positions.map((position) => {
                    const outputPosition = sortingOutput.positions.find(
                      (x) => x.loadCarrierId === position.loadCarrierId
                    );
                    console.log({
                      ...position,
                      outputs: outputPosition.outputs,
                    });
                    return { ...position, outputs: outputPosition.outputs };
                  });
                })
              )
              .toPromise();
          },
        }),
        sort: 'name',
      };
      customStore.store.load();
      return customStore;
    }
    return {
      store: new ArrayStore({
        data: positions,
        key: 'id',
      }),
    };
  }

  openSortDialog(receiptId: number) {
    return this.dialog
      .open<SortingDialogComponent, SortDialogData, SortDialogResult>(
        SortingDialogComponent,
        {
          width: '1000px',
          data: {
            receiptId,
          },
        }
      )
      .afterClosed()
      .pipe(tap(() => this.reloadSub.next(true)))
      .subscribe();
  }

  enableSortRequired(receiptId: number) {
    this.dpl.loadCarrierReceipts
      .patchLoadCarrierReceiptIsSortingRequired(receiptId, {
        isSortingRequired: true,
      })
      .pipe(
        this.loadingService.showLoadingWhile(),
        tap(() => this.reloadSub.next(true))
      )
      .subscribe();
  }

  disableSortRequired(receiptId: number) {
    this.dpl.loadCarrierReceipts
      .patchLoadCarrierReceiptIsSortingRequired(receiptId, {
        isSortingRequired: false,
      })
      .pipe(
        this.loadingService.showLoadingWhile(),
        tap(() => this.reloadSub.next(true))
      )
      .subscribe();
  }

  filterGrid() {
    switch (this.selectedFilter) {
      case FilterPreset.Completed:
        this.dataGrid.instance.filter([
          ['isSortingRequired', true],
          'and',
          ['isSortingCompleted', true],
        ]);
        break;
      case FilterPreset.Open:
        this.dataGrid.instance.filter([
          ['isSortingRequired', true],
          'and',
          ['isSortingCompleted', false],
        ]);
        break;

      default:
        this.dataGrid.instance.clearFilter();
        break;
    }
  }

  getReceiptTypeData() {
    const r = Object.keys(LoadCarrierReceiptType)
      .filter((x) => x !== LoadCarrierReceiptType.Exchange)
      .map((value, id) => {
        value = LoadCarrierReceiptType[value as any];
        const display = this.localizationService.getTranslation(
          'LoadCarrierReceiptType',
          value
        );
        return { id, value, display };
      });

    return of(r);
  }

  // set a filter Expression
  getFilterExpressionReceiptType(value) {
    let column = this as any;
    var filterExpression = [[column.dataField, 'contains', value]];
    return filterExpression;
  }

  // DisplayExpression for Lookups
  lookUpDisplayExpression(item) {
    return item ? `${item.display}` : ``;
  }

  // ValueExpression for Lookups
  lookUpValueExpression(item) {
    return item ? `${item.value}` : ``;
  }

  openDocument(e: Event, documentId: number) {
    e.preventDefault();
    this.dpl.documents.getDocumentDownload(documentId).subscribe((url) => {
      this.printService.printUrl(url, false);
    });
  }
}
