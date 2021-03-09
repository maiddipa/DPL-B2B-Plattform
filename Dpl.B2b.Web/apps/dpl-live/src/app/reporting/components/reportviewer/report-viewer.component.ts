import { Component, Inject, ViewEncapsulation } from '@angular/core';
import { API_BASE_URL } from 'apps/dpl-live/src/app/core/services/dpl-api-services';
import { Observable, combineLatest } from 'rxjs';
import { CustomersService } from 'apps/dpl-live/src/app/customers/services/customers.service';
import { map } from 'rxjs/operators';
import { ActivatedRoute } from '@angular/router';

interface IViewData {
  host: string;
  path: string;
  report: string;
}

const viewDocumentPath = '/DXXRDV';

@Component({
  selector: 'report-viewer',
  encapsulation: ViewEncapsulation.None,
  templateUrl: './report-viewer.component.html',
  styleUrls: ['./report-viewer.component.scss'],
})
export class ReportViewerComponent {
  reportUrl: string = 'XtraReport';
  invokeAction: string = '/DXXRDV';

  viewData$: Observable<IViewData>;

  constructor(
    @Inject(API_BASE_URL) public apiBaseUrl: string,
    private customerService: CustomersService,
    private route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    const params$ = this.route.queryParams.pipe(
      map((params) => ({
        documentId: params['documentId'] as string,
        languageId: params['languageId'] as string,
      }))
    );

    this.viewData$ = combineLatest(params$).pipe(
      map(([params]) => {
        const documentId = params.documentId || 1;
        const languageId = params.languageId || 2;
        return <IViewData>{
          host: this.apiBaseUrl,
          path: viewDocumentPath,
          report: `mode=view&documentId=${documentId}&languageId=${languageId}`,
        };
      })
    );
  }
}
