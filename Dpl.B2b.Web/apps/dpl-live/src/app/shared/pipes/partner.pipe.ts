import { Pipe, PipeTransform } from '@angular/core';
import { CustomerPartner } from '@app/api/dpl';
import { CountryPipe } from './country.pipe';

export type PartnerPipeFormat = 'format1' | 'format2';

@Pipe({
  name: 'partnerToString',
})
export class PartnerPipe implements PipeTransform {
  constructor(private countryPipe: CountryPipe) {}

  transform(value: CustomerPartner, format: PartnerPipeFormat = 'format1'): string {
    switch (format) {
      case 'format1': {
        return this.format1(value);
      }
      case 'format2': {
        return this.format2(value);
      }
      default:
        return 'n/a';
    }
  }

  /**
   * Erzeugt einen Term der wie folgt aufgebaut ist 'LK-PLZ Stadt'.
   * Wenn Teile fehlen werden verbindende Zeichen weggelassen
   * @param country
   * @param postalCode
   * @param city
   * @returns {string}
   */
  private buildPostalTerm(country: number, postalCode: string, city: string) {
    let term = '';
    let licensePlate = '';

    if (country != null && !Number.isNaN(country)) {
      licensePlate = this.countryPipe.transform(country, 'licensePlate');
    }

    if (postalCode?.length > 0) {
      if (licensePlate?.length > 0) {
        term = `${licensePlate}-${postalCode}`;
      } else {
        term = `${postalCode}`;
      }
    }

    if (city?.length > 0) {
      term = `${term} ${city}`;
    }
    return term;
  }

  format1(partner: CustomerPartner): string {
    let text = partner.companyName;

    if (partner?.address && typeof partner.address === 'object') {
      const {
        address: {
          street1 = '',
          city = '',
          postalCode = '',
          country = null,
        } = {},
      } = partner;

      if (street1?.length > 0) {
        text = `${text}, ${street1}`;
      }

      const townToken = this.buildPostalTerm(country, postalCode, city);

      if (townToken?.length > 0) {
        text = `${text}, ${townToken}`;
      }
    }

    return text;
  }

  format2(partner: CustomerPartner): string {
    let text = partner.companyName;

    if (partner.address && typeof partner.address === 'object') {
      const {
        address: {
          street1 = '',
          city = '',
          postalCode = '',
          country = null,
        } = {},
      } = partner;
      let town = '';

      if (country != null && !Number.isNaN(country)) {
        const iso2 = this.countryPipe.transform(country, 'iso2');
        town = `${town}${iso2}`;
      }

      if (city?.length > 0) {
        town = `${town}, ${city}`;
      }

      if (town?.length > 0) {
        text = `${text}, ${town}`;
      }
    }

    return text;
  }
}
