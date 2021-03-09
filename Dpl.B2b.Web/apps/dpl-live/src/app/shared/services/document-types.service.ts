import { Injectable } from '@angular/core';
import { LoadCarrierPickerContext } from '../components/load-carrier-picker/load-carrier-picker.component';

@Injectable({
  providedIn: 'root',
})
export class DocumentTypesService {
  constructor() {}

  getDocumentTypeIdForLoadCarrierPickerContext(
    context: LoadCarrierPickerContext
  ) {
    switch (context) {
      case 'voucher':
        return 3;
      case 'delivery':
      case 'pickup':
      case 'exchange':
        return 6
      default:
        return null;
    }
  }
}
