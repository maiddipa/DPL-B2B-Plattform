import { Injectable, Optional } from '@angular/core';
import { MatSnackBar, MatSnackBarConfig } from '@angular/material/snack-bar';
import { ApiException } from '@app/api/dpl';
import { AppInsightsService } from '@markpieszak/ng-application-insights';
import { EMPTY, Observable, of, throwError } from 'rxjs';

export interface HttpCodeErrorHandlingInfo<TReturn> {
  [code: number]: ErrorHandlingInfo<TReturn>;
  default?: ErrorHandlingInfo<TReturn>;
  rethrowNonApi?: boolean;
}

export interface ErrorHandlingInfo<TReturn> {
  snackBar?: {
    text: string;
  } & MatSnackBarConfig;
  return?: TReturn | Observable<TReturn>;
}

@Injectable({
  providedIn: 'root',
})
export class ErrorService {
  defaults = {
    snackBar: {
      duration: 2000,
    },
  };

  constructor(
    private snackBar: MatSnackBar,
    // only exists in production
    @Optional() private appInsights: AppInsightsService
  ) {}

  getUnknownApiError() {
    return $localize`:Text für unbekannten API Fehler@@UnkownApiError:Unbekannter Fehler, bitte versuchen Sie es erneut.`;
  }

  handleApiError<TReturn>(
    error: any,
    handlingInfo: HttpCodeErrorHandlingInfo<TReturn>
  ) {
    if (error instanceof ApiException) {
      if (this.appInsights) {
        this.appInsights.trackException(error);
      }

      const info = handlingInfo[error.status];
      if (!info) {
        if (!handlingInfo.default) {
          return EMPTY;
        }

        return this.handle(handlingInfo.default);
      }

      return this.handle(info);
    }

    if (handlingInfo.rethrowNonApi) {
      // rethrow the error
      return throwError(error);
    }

    if (this.appInsights) {
      this.appInsights.trackException(error);
    }

    return EMPTY;
  }

  private handle<TReturn>(info: ErrorHandlingInfo<TReturn>) {
    if (info.snackBar) {
      this.snackBar.open(info.snackBar.text, null, {
        duration: info.snackBar.duration || this.defaults.snackBar.duration,
      });
    }

    if (!info.return) {
      return EMPTY;
    }

    if (info.return instanceof Observable) {
      return info.return;
    }

    return of(info.return);
  }

  public trackException(error: any) {
    if (this.appInsights) {
      this.appInsights.trackException(error);
    }
  }
}
