import { Component, OnInit } from '@angular/core';
import { combineLatest, Observable } from 'rxjs';
import { map, pluck } from 'rxjs/operators';
import { AccountsService } from '../accounts/services/accounts.service';
import {
  PostingAccountCondition,
  VoucherType,
} from '../core/services/dpl-api-services';
import { VoucherForm } from './voucher-form/voucher-form.component';

type ViewData = {
  defaultValues: Partial<VoucherForm>;
};
@Component({
  selector: 'dpl-voucher',
  templateUrl: './voucher.component.html',
  styleUrls: ['./voucher.component.scss'],
})
export class VoucherComponent implements OnInit {
  viewData$: Observable<ViewData>;
  constructor(private accountService: AccountsService) {}

  ngOnInit(): void {
    const loadCarrierIds$ = this.accountService.getActiveAccount().pipe(
      pluck('voucherConditions'),
      map((data: PostingAccountCondition[]) => data.map((i) => i.loadCarrierId))
    );

    this.viewData$ = combineLatest([loadCarrierIds$]).pipe(
      map(([loadCarrierIds]) => {
        const viewData: ViewData = {
          defaultValues: {
            note: '',
            recipientSelection: {
              shipper: true,
              supplier: false,
            },
            voucherType: VoucherType.Original,
            loadCarriers: loadCarrierIds.map((id) => {
              return {
                id,
                quantity: null,
              };
            }),
          },
        };

        return viewData;
      })
    );
  }
}
