import { Component, Inject, OnInit } from '@angular/core';
import { LoadingService } from '@dpl/dpl-lib';
import { Observable, timer } from 'rxjs';
import { map } from 'rxjs/operators';

import { APP_CONFIG, DplLiveConfiguration } from '../../../../config';

@Component({
  selector: 'app-loading-spinner',
  templateUrl: './loading-spinner.component.html',
  styleUrls: ['./loading-spinner.component.scss'],
})
export class LoadingSpinnerComponent implements OnInit {
  showCancel$: Observable<boolean>;
  constructor(
    @Inject(APP_CONFIG) private config: DplLiveConfiguration,
    private loadingService: LoadingService
  ) {}

  ngOnInit() {
    this.showCancel$ = timer(
      this.config.app.ui.loading.durationUntilUserCanHide
    ).pipe(map(() => true));
  }

  hideSpinner() {
    this.loadingService.setLoading(false);
  }
}
