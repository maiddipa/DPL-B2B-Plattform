import { Injectable } from '@angular/core';
/**
 * NOTE: Service steht in den akita getControls Methode nicht zur verfügung
 */
@Injectable({
  providedIn: 'root',
})
export class ValidationDataService {
  // Abhilfe über static definition um Service Problem zu umgehen
  public static maxLength = {
    note: 200,
    expressCode: 7,
    street: 75,
    postalCode: 10,
    city: 75,
    state: 100,
    country: 75,
    driverName: 75,
    companyName: 75,
    licensePlate: 14,
    deliveryNoteNumber: 50,
    pickupNoteNumber: 50,
    referenceNumber: 50,
    customerReference: 10
  };

  // Abhilfe über static definition um Service Problem zu umgehen
  public static minLength = {
    loadCarriers: 1,
    expressCode: 6,
  };

  get(name: string) {
    return ValidationDataService[name];
  }

  constructor() {}
}
