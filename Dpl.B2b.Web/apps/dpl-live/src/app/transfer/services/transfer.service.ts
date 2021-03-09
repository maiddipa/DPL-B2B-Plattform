import { Injectable } from '@angular/core';
import { BalanceTransferCreateRequest } from '@app/api/dpl';

import { DplApiService } from '../../core/services';

@Injectable({
  providedIn: 'root',
})
export class TransferService {
  constructor(private dpl: DplApiService) {}

  //transfer object
  transfer(transferRequest: BalanceTransferCreateRequest) {
    return this.dpl.balanceTransfer.post(transferRequest);
  }
}
