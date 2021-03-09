import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
} from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable()
export class TimezoneOffsetHeaderInterceptor implements HttpInterceptor {
  constructor() {}

  intercept(
    request: HttpRequest<unknown>,
    next: HttpHandler
  ): Observable<HttpEvent<unknown>> {
    const reqWithTimeZoneOffsetHeader = request.clone({
      setHeaders: {
        'x-timezone-offset': new Date().getTimezoneOffset().toString(),
      },
    });
    return next.handle(reqWithTimeZoneOffsetHeader);
  }
}
