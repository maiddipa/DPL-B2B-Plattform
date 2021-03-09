import { LoadCarrier } from '../../core/services/dpl-api-services';

export type dummyReceiptInput = {
    id:number;
    positions: dummyPosition[];
  }
  
  export type dummyPosition = {
    id: number;
    inLoadCarrierId: number;
    quantity: number;
    possibleOutLoadCarriers: dummyOutput[];
  }

  export type dummyOutput = {
    loadCarrierId: number;
    quantity?: number
  }