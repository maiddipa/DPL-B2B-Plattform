import { CustomerPartner } from '@app/api/dpl';

export interface IPartner extends CustomerPartner {
  directoryNames: string[]
}

export interface IPartnerUI {
  active: {
    [partnerPickerContext: string]: boolean;
  };
}
