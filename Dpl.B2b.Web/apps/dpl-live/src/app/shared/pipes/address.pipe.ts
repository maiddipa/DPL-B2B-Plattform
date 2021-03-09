import { Pipe, PipeTransform } from '@angular/core';
import { Address } from '@app/api/dpl';
import { CountryPipe } from './country.pipe';

export type AddressPipeFormat = 'short' | 'long';
@Pipe({
  name: 'address',
})
export class AddressPipe implements PipeTransform {
  constructor(private countryPipe: CountryPipe) {}

  transform(address: Address, format: AddressPipeFormat = 'short'): any {
    if (!address) {
      return undefined;
    }

    switch (format) {
      case 'short':
        return `${this.countryPipe.transform(
          address.country,
          'licensePlate'
        )}-${address.postalCode} ${address.city}`;

      case 'long':
        return `${address.street1}, ${this.countryPipe.transform(
          address.country,
          'licensePlate'
        )}-${address.postalCode} ${address.city}`;
      default:
        break;
    }
  }
}
