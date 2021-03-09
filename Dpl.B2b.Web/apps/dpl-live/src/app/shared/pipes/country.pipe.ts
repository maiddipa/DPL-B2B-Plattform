import { Pipe, PipeTransform } from '@angular/core';
import { LocalizationService } from 'apps/dpl-live/src/app/core/services/localization.service';
import { CountriesService } from 'apps/dpl-live/src/app/master-data/countries/services/countries.service';
import { ICountry } from 'apps/dpl-live/src/app/master-data/countries/state/country.model';
import { map, tap } from 'rxjs/operators';
import * as _ from 'lodash';
import { CountriesQuery } from '../../master-data/countries/state/countries.query';

export type CountryPipeFormat = 'long' | 'licensePlate' | 'iso2' | 'iso3';

@Pipe({
  name: 'country',
})
export class CountryPipe implements PipeTransform {
  countriesDict: {
    [id: number]: ICountry<number>;
  };

  countryCodesToIdDict: {
    [id: string]: number;
  };

  constructor(
    private localizationService: LocalizationService,
    private countryService: CountriesService,
    private countryQuery: CountriesQuery
  ) {
    // // cache countries
    // this.countryService
    //   .getCountries()
    //   .pipe(
    //     tap(countries => {
    //       this.countriesDict = _(countries)
    //         .keyBy(i => i.id)
    //         .value();
    //       // HACK this is likely mpot correct, what we need here is grab google country names (loacalized) and map them to our ids
    //       this.countryNameToIdDict = _([...countries])
    //         .push({ name: 'Deutschland', id: 1 } as any)
    //         .keyBy(i => i.name)
    //         .mapValues(i => i.id)
    //         .value();
    //     })
    //   )
    //   .subscribe();
  }

  ensureInitialized() {
    if (!this.countriesDict) {
      const countries = this.countryQuery.getAll();
      this.countriesDict = _(countries)
        .keyBy((i) => i.id)
        .value();

      // HACK this is likely mpot correct, what we need here is grab google country names (loacalized) and map them to our ids
      this.countryCodesToIdDict = _([...countries])
        .push({ name: 'DE', id: 1 } as any)
        .keyBy((i) => i.name)
        .mapValues((i) => i.id)
        .value();
    }
  }

  transform(
    idOrName: number | string,
    format: CountryPipeFormat = 'long'
  ): any {
    if (!idOrName) {
      throw 'idOrName cannot be null';
    }

    this.ensureInitialized();

    // we need to support passing in a string name as google reverse geocode does only give us a country name
    const id =
      typeof idOrName === 'number'
        ? idOrName
        : this.countryCodesToIdDict[idOrName];

    switch (format) {
      case 'iso2':
        return this.countriesDict[id].iso2Code;
      case 'iso3':
        return this.countriesDict[id].iso3Code;
      case 'licensePlate':
        return this.countriesDict[id].licensePlateCode;
      case 'long':
      default:
        return this.getTranslation(id);
    }
  }

  private getTranslation(id: number) {
    return this.localizationService.getTranslation('Countries', id.toString());
  }
}
