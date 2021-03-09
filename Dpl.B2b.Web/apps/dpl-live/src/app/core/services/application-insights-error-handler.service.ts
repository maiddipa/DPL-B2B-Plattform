import { ErrorHandler, Injectable, Injector } from '@angular/core';
import { AppInsightsService } from '@markpieszak/ng-application-insights';
import { HttpErrorResponse } from '@angular/common/http';

class WrappedError extends Error {
  constructor(private error: any) {
    super(
      'This is a wrapped error, more details can be found in the error property'
    );
  }
}

@Injectable()
export class ApplicationInsightsErrorHandlerService extends ErrorHandler {
  _insights: AppInsightsService;
  constructor(private injector: Injector) {
    super();
  }

  get insights(): AppInsightsService {
    if (!this._insights) {
      this._insights = this.injector.get(AppInsightsService);
    }

    return this._insights;
  }

  handleError(error: HttpErrorResponse): void {
    const parsedError =
      error instanceof Error
        ? error
        : typeof error === 'string'
        ? new Error(error)
        : new WrappedError(error);

    if (this.insights) {
      this.insights.trackException(parsedError);
    }

    super.handleError(error);
  }
}
