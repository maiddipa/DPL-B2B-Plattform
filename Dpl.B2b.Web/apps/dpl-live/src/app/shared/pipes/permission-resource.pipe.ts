import { Pipe, PipeTransform } from '@angular/core';
import { PermissionResourceType } from '../../core/services/dpl-api-services';
import { LocalizationService } from "@app/core";
import { TranslatePipeBase, TranslatePipeEnumBase } from "@app/shared/pipes/translate-pipe.base";


@Pipe({
  name: 'permissionResource',
})
export class PermissionResourcePipe extends TranslatePipeEnumBase
{
  name="PermissionResourceType";
  constructor(private ls: LocalizationService) {
    super(ls);
  }

  // TODO: Add translations
  transform(value: PermissionResourceType): unknown {
    return this.translate(value.toString(), this.name);

    switch (value) {
      case PermissionResourceType.Organization:
        return 'Organisation';
      case PermissionResourceType.Customer:
        return 'Kunde';
      case PermissionResourceType.Division:
        return 'Abteilung';
      case PermissionResourceType.PostingAccount:
        return 'Konto';
      default:
        throw new Error(`Argument our of range: ${value}`);
    }
  }
}
