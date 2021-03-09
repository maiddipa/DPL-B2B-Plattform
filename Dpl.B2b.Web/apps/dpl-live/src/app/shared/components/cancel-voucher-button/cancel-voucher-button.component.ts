import {
  ChangeDetectionStrategy,
  Component,
  Input,
  OnInit,
} from '@angular/core';
import { ResourceAction, Voucher, VoucherStatus, VoucherType } from '@app/api/dpl';
import * as moment from 'moment';
import { Observable } from 'rxjs';
import { delay, filter, map, switchMap, tap } from 'rxjs/operators';
import { CustomerDivisionsService } from '../../../customers/services/customer-divisions.service';
import { VoucherService } from '../../../voucher/services/voucher.service';
import { UserService } from '../../../user/services/user.service';
import { CustomersService } from '../../../customers/services/customers.service';
import { ICustomer } from '../../../customers/state/customer.model';
import { voucherTypeToDocumentTypeId } from '@app/core';

type ViewData = {
  disabled: boolean;
};

@Component({
  selector: 'dpl-cancel-voucher-button',
  templateUrl: './cancel-voucher-button.component.html',
  styleUrls: ['./cancel-voucher-button.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class CancelVoucherButtonComponent implements OnInit {
  @Input() voucher: Voucher;
  viewData$: Observable<ViewData>;
  resourceAction = ResourceAction;

  constructor(
    private voucherService: VoucherService,
    private divisionService: CustomerDivisionsService,
    private userService: UserService,
    private customerService: CustomersService
  ) {}

  ngOnInit(): void {
    this.viewData$ = this.customerService.getActiveCustomer().pipe(
      switchMap((customer) =>
        this.divisionService.getActiveDivision().pipe(
          filter((x) => !!x),
          map((division) => {
            let disabled = true;

            const timeSpan = moment(moment()).diff(
              this.voucher.issuedDate,
              'hours',
              true
            );

            //check voucher is issued by active division
            if (
              this.voucher.status !== VoucherStatus.Canceled &&
              division.id === this.voucher.divisionId
            ) {
              if (
                this.voucher.type === VoucherType.Original &&
                timeSpan <=
                  this.getCancellationTimeSpan(
                    customer,
                    voucherTypeToDocumentTypeId(this.voucher.type),
                    24
                  )
              ) {
                disabled = false;
              }
              if (
                this.voucher.type === VoucherType.Digital &&
                timeSpan <=
                  this.getCancellationTimeSpan(
                    customer,
                    voucherTypeToDocumentTypeId(this.voucher.type),
                    1
                  )
              ) {
                disabled = false;
              }
            }
            const viewData: ViewData = { disabled };
            return viewData;
          })
        )
      )
    );
  }

  onCancel() {
    return this.voucherService.cancel(this.voucher, 'grid').pipe(delay(200));
  }

  private getCancellationTimeSpan(
    customer: ICustomer<number>,
    documentTypeId: number,
    defaultCancellationTimeSpan: number
  ): number {
    return (
      customer.documentSettings.find(
        (c) => c.cancellationTimeSpan > 0 && c.documentTypeId == documentTypeId
      )?.cancellationTimeSpan ?? defaultCancellationTimeSpan
    );
  }
}
