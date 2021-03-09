import { PartnerDirectory } from '../../core/services/dpl-api-services';
import { IPartner } from './partner.model';

export interface IPartnerDirectory extends PartnerDirectory {}

export interface IPartnerDirectoryUI {
  active: {
    [partnerPickerContext: string]: boolean;
  };
}
