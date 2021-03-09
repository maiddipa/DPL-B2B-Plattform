import { Pipe, PipeTransform } from '@angular/core';
import { ResourceAction } from '../../core/services/dpl-api-services';
import { TranslatePipeBase, TranslatePipeEnumBase } from "@app/shared/pipes/translate-pipe.base";
import { LocalizationService } from "@app/core";

@Pipe({
  name: 'permissionAction',
})
export class PermissionActionPipe extends TranslatePipeEnumBase {
  name="ResourceAction";
  constructor(private ls: LocalizationService) {
    super(ls);
  }
  transform(value: ResourceAction) {
    return this.translate(value.toString(), this.name);

    switch (value) {
      case ResourceAction.Create:
        return 'erstellen';
      case ResourceAction.Update:
        return 'editieren';
      case ResourceAction.Read:
        return 'abrufen';
      case ResourceAction.Delete:
        return 'löschen';

      case ResourceAction.CreateCustomer:
        return 'Kunde erstellen';

      case ResourceAction.UpdateCustomer:
        return 'Kunde editieren';
      case ResourceAction.DeleteCustomer:
        return 'Kunde löschen';

      case ResourceAction.CreateDivision:
        return 'Abteilung erstellen';

      case ResourceAction.UpdateDivision:
        return 'Abteilung editieren';
      case ResourceAction.DeleteDivision:
        return 'Abteilung löschen';

      case ResourceAction.CreateVoucher:
        return 'Gutschrift erstellen';
      case ResourceAction.CancelVoucher:
        return 'Gutschrift stornieren';
      case ResourceAction.SubmitVoucher:
        return 'Gutschrift übermitteln';
      case ResourceAction.ReadVoucher:
        return 'Gutschrift abrufen';

      case ResourceAction.CreateLoadCarrierReceipt:
        return 'Quittung erstellen';
      case ResourceAction.CancelLoadCarrierReceipt:
        return 'Quittung stornieren';
      case ResourceAction.ReadLoadCarrierReceipt:
        return 'Quittung abrufen';

      case ResourceAction.CreateOrder:
        return 'Verfügbarkeit/Bedarf erstellen';
      case ResourceAction.UpdateOrder:
        return 'Verfügbarkeit/Bedarf editieren';
      case ResourceAction.CancelOrder:
        return 'Verfügbarkeit/Bedarf übermitteln';
      case ResourceAction.ReadOrder:
        return 'Verfügbarkeit/Bedarf abrufen';

      case ResourceAction.CancelOrderLoad:
        return 'Verfügbarkeit/Bedarf erstellen';
      case ResourceAction.ReadOrderLoad:
        return 'Verfügbarkeit/Bedarf editieren';

      case ResourceAction.CreateExpressCode:
        return 'DPG erstellen';
      case ResourceAction.ReadExpressCode:
        return 'DPG abrufen';
      case ResourceAction.CancelExpressCode:
        return 'DPG stornieren';

      case ResourceAction.CreateBalanceTransfer:
        return 'Umbuchung erstellen';
      case ResourceAction.ReadBalanceTransfer:
        return 'Umbuchung abrufen';
      case ResourceAction.CancelBalanceTransfer:
        return 'Umbuchung stornieren';
      case ResourceAction.CreateLivePoolingSearch:
        return 'Live Pooling';
      case ResourceAction.ReadLivePoolingOrders:
        return 'Live Pooling Aufträge';
      case ResourceAction.CreateSorting:
        return 'Sortierung';
      default:
        throw new Error(`Argument our of range: ${value}`);
    }
  }
}
