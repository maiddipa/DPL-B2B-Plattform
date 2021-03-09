import { Injectable, Injector } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ToastrService } from 'ngx-toastr';
import { IndividualConfig } from 'ngx-toastr/toastr/toastr-config';

@Injectable()
export class NotificationService {
  constructor(private injector: Injector) {}

  showSuccess(
    message: string,
    title: string | null = null,
    override?: Partial<IndividualConfig> | null
  ): void {
    this.show(NotificationType.success, message, title, override);
  }

  showWarning(
    message: string,
    title: string | null = null,
    override?: Partial<IndividualConfig> | null
  ): void {
    this.show(NotificationType.warning, message, title, override);
  }

  showInfo(
    message: string,
    title: string | null = null,
    override?: Partial<IndividualConfig> | null
  ): void {
    this.show(NotificationType.info, message, title, override);
  }

  showError(
    message: string,
    title: string | null = null,
    override?: Partial<IndividualConfig> | null
  ): void {
    this.show(NotificationType.error, message, title, override);
  }

  showSnackBarMessage(message: string, type: NotificationType): void {
    const snackBar = this.injector.get(MatSnackBar);

    const actionText = 'X';

    const config = {
      panelClass: [type],
    };

    // The second parameter is the text in the button.
    // In the third, we send in the css class for the snack bar.
    snackBar.open(message, actionText, config);
  }

  show(
    type: NotificationType,
    message: string,
    title: string | null = null,
    override?: Partial<IndividualConfig> | null
  ): void {
    const toastr = this.injector.get(ToastrService);

    switch (type) {
      case NotificationType.error:
        toastr.error(message, title, override);
        break;
      case NotificationType.info:
        toastr.info(message, title, override);
        break;
      case NotificationType.success:
        toastr.success(message, title, override);
        break;
      case NotificationType.warning:
        toastr.warning(message, title, override);
        break;
    }
  }
}

export enum NotificationType {
  error = 'error',
  info = 'info',
  success = 'success',
  warning = 'warning',
}
