import { Injectable } from '@angular/core';
import { switchMap } from 'rxjs/operators';

import { VoucherReasonTypesQuery } from '../state/voucher-reason-types.query';

@Injectable({ providedIn: 'root' })
export class VoucherReasonTypesService {
  constructor(private voucherReasonTypesQuery: VoucherReasonTypesQuery) {}

  getVoucherReasonTypes() {
    return this.voucherReasonTypesQuery.voucherReasonTypes$;
  }
}
