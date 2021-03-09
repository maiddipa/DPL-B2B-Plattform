import { ErrorHandler, Injectable, Injector } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';
import { LoggingService, LogType } from './logging.service';

import { NotificationService, NotificationType } from './notification.service';
import {
  ApiException,
  DplProblemDetails,
  ProblemDetails,
  StateItem,
} from './dpl-api-services';
import { IndividualConfig } from 'ngx-toastr/toastr/toastr-config';
import * as _ from 'lodash';
import { LocalizationService } from './localization.service';
import { AuthenticationService } from './authentication.service';

export class WrappedError extends Error {
  constructor(
    private error: Error | string,
    private problemDetails?: ProblemDetails | null
  ) {
    super(
      'This is a wrapped error, more details can be found in the error property'
    );
    if (typeof error === 'string') {
      this._error = new Error(error);
    } else {
      this._error = error;
    }

    this._problemDetails = problemDetails;
  }

  get errorType(): ErrorType {
    if (WrappedError.isDplProblemDetails(this.ProblemDetails)) {
      return 'dplProblemDetails';
    }
    if (this.Error instanceof ApiException) {
      return 'api';
    } else return 'undefined';
  }

  private readonly _error: any;

  get Error(): Error {
    return this._error;
  }

  private readonly _problemDetails: ProblemDetails;

  get ProblemDetails(): ProblemDetails {
    return this._problemDetails;
  }

  public static isDplProblemDetails(
    dplProblemDetails: DplProblemDetails
  ): boolean {
    return (
      _.isObject(dplProblemDetails) &&
      (_.isArray(dplProblemDetails.ruleStates) ||
        _.isArray(dplProblemDetails.serviceStates))
    );
  }

  public static parse(error: Error | string): WrappedError {
    // ErrorText is not wrapped inside Error
    if (typeof error === 'string') {
      return new WrappedError(error);
    }
    // Error is HttpErrorResponse - Concurrent Api request
    else if (error instanceof HttpErrorResponse) {
      return new WrappedError(error);
    }
    // Error is ApiException -  Comes from the Api
    else if (ApiException.isApiException(error)) {
      if (error.status === 400 || error.status === 500) {
        return new WrappedError(error, error.result as DplProblemDetails);
      } else return new WrappedError(error, error.result as ProblemDetails);
    }

    // If none of a defined Error occured
    return new WrappedError(error);
  }
}

export type ErrorType = 'dplProblemDetails' | 'api' | 'undefined';

@Injectable()
export class AppErrorHandlerService extends ErrorHandler {
  // Style override Default Values
  private override = { closeButton: true, tapToDismiss: true } as Partial<
    IndividualConfig
  >;

  constructor(
    private logger: LoggingService,
    private notification: NotificationService,
    private injector: Injector,
    private authService: AuthenticationService
  ) {
    super();
  }

  handleError(error: Error): void {
    const parsedError = WrappedError.parse(error);

    switch (parsedError.errorType) {
      case 'dplProblemDetails':
        return this.handleErrorDplProblemDetails(parsedError);
      case 'api':
        return this.handleErrorApi(parsedError.Error as ApiException); // From ApiException without details
      case 'undefined':
        return this.handleErrorUndefined(parsedError.Error); // Default handling, just console message
      default:
        return super.handleError(parsedError);
    }
  }

  private handleErrorDplProblemDetails(error: WrappedError): void {
    // Argument error
    if (error == null) {
      throw new ReferenceError('Not an error');
    }

    const dplProblemDetails = error.ProblemDetails as DplProblemDetails;

    // Do nothing if there are no Details or ruleStates
    if (!WrappedError.isDplProblemDetails(dplProblemDetails)) {
      return;
    }

    // Log message
    this.logger.message(
      LogType.error,
      error.message,
      error.message,
      dplProblemDetails
    );

    // Get All RuleStates
    const items: StateItem[] = [];
    if (dplProblemDetails.ruleStates) {
      dplProblemDetails.ruleStates.forEach((ruleState) =>
        items.push(...ruleState)
      );
    }
    if (dplProblemDetails.serviceStates) {
      dplProblemDetails.serviceStates.forEach((serviceState) =>
        items.push(...serviceState)
      );
    }

    // Show Message for each RuleState
    items.forEach((item) => {
      // i18n macht probleme in bezug auf das extrahieren wenn value zur laufzeit bekanntgemacht wird
      //const translateMessage = $localize`:StatusMeaning|Descr@@${item.messageId}:Server`

      const s = this.injector.get(LocalizationService);
      const translateMessage = s.getTranslationById(item.messageId);
      const type = item.type.toLowerCase() as NotificationType;

      this.notification.show(type, translateMessage, '', this.override);
    });
  }

  private handleErrorApi(error: ApiException) {
    if (!ApiException.isApiException(error)) {
      throw new Error('Not an ApiException');
    }

    this.logger.error('Api Error', error);

    const title = $localize`:@@ApiError:Server Api Fehler`;

    const problemDetails = error.result as ProblemDetails;

    switch (error.status) {
      case 400: {
        const message = $localize`:BadRequest|BadRequest@@BadRequest:Die Anfrage-Nachricht war fehlerhaft aufgebaut.`;

        this.notification.showError(message, title, this.override);
        break;
      }
      case 401: {
        const message = $localize`:@@UnauthorizedError:Die Anfrage kann nicht ohne gültige Authentifizierung gestellt werden`;

        this.notification.showError(message, title, this.override);
        break;
      }
      case 403: {
        const message = $localize`:ForbiddenError|ForbiddenError@@ForbiddenError:Die Anfrage wurde aufgrund fehlender Autorisierung durch den Kunden, z.B. weil der authentifizierte Benutzer nicht autorisiert ist oder eine als HTTPS konfigurierte URL nur mit HTTP aufgerufen wurde.`;

        this.notification.showError(message, title, this.override);

        break;
      }
      case 404: {
        const message = $localize`:NotFoundError|NotFoundError@@NotFoundError:Die angeforderte Ressource wurde nicht gefunden.`;

        this.notification.showError(message, title, this.override);

        break;
      }
      case 405: {
        const message = $localize`:@@MethodNotAllowedError:Die Anforderung kann nur mit anderen HTTP-Methoden erfolgen.`;

        this.notification.showError(message, title, this.override);
        break;
      }
      case 408: {
        const message = $localize`@@RequestTimeoutError:Innerhalb der vom Server erlaubten Zeitspanne wurde keine vollständige Anfrage des Clients empfangen.`;

        this.notification.showError(message, title, this.override);
        break;
      }

      case 500:
      default: {
        const message = $localize`:@@UnexpectedServerError :Ein unerwarteter Serverfehler ist aufgetreten.`;

        this.notification.showError(message, title, this.override);
        break;
      }
    }
  }

  private handleErrorUndefined(error: Error) {
    if (error == null) {
      throw new ReferenceError('Error cannot be null');
    }

    this.logger.error('Undefined error', error);
    const title = $localize`:@@UndefinedError:Undefinierter Fehler`;
    const chatErrorTitle = $localize`:@@ChatError:Chat Fehler`;
    let chatErrorMessage = $localize`:@@ChatErrorMessage:Der Chat ist zurzeit nicht verfügbar.`;
    const defaultChatError = $localize`:@@ChatErrorMessage:Der Chat ist zurzeit nicht verfügbar.`;

    if (
      error.message.includes('(in promise)') &&
      error.message.includes('dpl-chat-plugin')
    ) {
      // error chat update (throw on delete channels or new user)
      if (
        error.message.includes('(in promise)') &&
        error.message.includes('data') &&
        error.message.includes('dpl-chat-plugin') &&
        error.message.includes('Generator.forEach')
      ) {
        // console.log('update error');
        chatErrorMessage = $localize`:@@ChatUpdateErrorMessage:Der Chat hat ein Änderung nicht mit bekommen. Bitte laden sie die Seite mit leerem Cache erneut`;
      }
      // hack for a chat error - maybe blocked cookies or urls - not tested
      if (
        error.message.includes('(in promise)') &&
        error.message.includes('me') &&
        error.message.includes('of undefined') &&
        error.message.includes('dpl-chat-plugin') &&
        error.message.includes('Object.onInvoke')
      ) {
        //  console.log('Chat Error: Cannot read Property "me" of undefined');
        chatErrorMessage = $localize`:@@ChatMeErrorMessage:Der Chat kann Sie nicht kennen lernen. Bitte überprüfen Sie Ihre Firewall- & Browsereinstellungen`;
      }
      // hack for a chat error - no api key
      if (
        error.message.includes('(in promise)') &&
        error.message.includes('Connect failed with error') &&
        error.message.includes('api_key') &&
        error.message.includes('not provided') &&
        error.message.includes('dpl-chat-plugin') &&
        error.message.includes('401')
      ) {
        //  console.log('Chat Error: Connection Error: Key not provided');
        chatErrorMessage = $localize`:@@ChatNoKeyErrorMessage:Es wurde kein API-Key für den Chat gesetzt`;
      }
      // hack for a chat error - wrong api key
      if (
        error.message.includes('(in promise)') &&
        error.message.includes('Connect failed with error') &&
        error.message.includes('api_key') &&
        error.message.includes('not found') &&
        error.message.includes('dpl-chat-plugin') &&
        error.message.includes('401')
      ) {
        //  console.log('Chat Error: Connection Error: Key not found');
        chatErrorMessage = $localize`:@@ChatWrongKeyErrorMessage:Der API-Key für den Chat wurde nicht gefunden`;
      }
      this.notification.showError(
        defaultChatError,
        chatErrorTitle,
        this.override
      );
    }

    // hack for 'AADSTS50058%3a+A+silent+sign-in+request+was+sent+but+no'
    else if (error && (error as any).errorCode === 'login_required') {
      // this.authService.logout();
      localStorage.clear();
      window.location.reload();
    } else {
      this.notification.showError(error.message, title, this.override);
    }
  }
}
