import { Component, Inject, OnInit, ViewEncapsulation } from '@angular/core';
import { combineLatest, Observable, of } from 'rxjs';
import { map, tap, switchMap } from 'rxjs/operators';
import { API_BASE_URL } from 'apps/dpl-live/src/app/core/services/dpl-api-services';
import { CustomersService } from 'apps/dpl-live/src/app/customers/services/customers.service';
import { ActivatedRoute } from '@angular/router';

interface IViewData {
  host: string;
  path: string;
  report: string;
}

const getDesignerModelAction = '/reporting/designer-model';

@Component({
  selector: 'report-designer',
  encapsulation: ViewEncapsulation.None,
  templateUrl: './report-designer.component.html',
  styleUrls: ['./report-designer.component.scss'],
})
export class ReportDesignerComponent implements OnInit {
  viewData$: Observable<IViewData>;
  constructor(
    @Inject(API_BASE_URL) public apiBaseUrl: string,
    private customerService: CustomersService,
    private route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    const params$ = this.route.queryParams.pipe(
      map((params) => ({
        customerId: params['customerId'] as string,
        printType: params['printType'] as string,
        languageId: params['languageId'] as string,
      }))
    );

    const customerId$ = params$.pipe(
      map((i) => i.customerId),
      switchMap((customerId) => {
        if (customerId) {
          return of(customerId);
        }
        return this.customerService
          .getCustomers()
          .pipe(map((customers) => customers[0].id));
      })
    );

    this.viewData$ = combineLatest(params$, customerId$).pipe(
      map(([params, customerId]) => {
        const printType = params.printType || 1;
        const languageId = params.languageId || 1;
        return <IViewData>{
          host: this.apiBaseUrl,
          path: getDesignerModelAction,
          report: `mode=edit&customerId=${customerId}&printType=${printType}&languageId=${languageId}`,
        };
      })
    );
  }
}
