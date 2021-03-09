import { Injectable } from '@angular/core';
import { Location } from './location-search.service.types';
import { Observable, of } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class LocationSearchService {
  locations: Location[] = [
    {
      id: null,
      salutation: null,
      name1: 'Kaufland Logistik VZ GmbH & Co. KG - Möckmühl',
      name2: null,
      name3: null,
      zip: '74219',
      city: 'Möckmühl',
      country: 'Deutschland',
    },
    {
      id: null,
      salutation: null,
      name1: 'Kaufland Logistik VZ GmbH & Co. KG - Barsinghausen',
      name2: null,
      name3: null,
      zip: '30890',
      city: 'Barsinghausen',
      country: 'Deutschland',
    },
    {
      id: null,
      salutation: null,
      name1: 'Kaufland Logistik VZ GmbH & Co. KG - Neckarsulm',
      name2: null,
      name3: null,
      zip: '74172',
      city: 'Neckarsulm',
      country: 'Deutschland',
    },
    {
      id: null,
      salutation: null,
      name1: 'Kaufland Logistik VZ GmbH & Co. KG - Dortmund',
      name2: null,
      name3: null,
      zip: '44339',
      city: 'Dortmund',
      country: 'Deutschland',
    },
    {
      id: null,
      salutation: null,
      name1: 'Kaufland Logistik VZ GmbH & Co.KG - Unterkaka',
      name2: null,
      name3: null,
      zip: '06721',
      city: 'Unterkaka',
      country: 'Deutschland',
    },
    {
      id: null,
      salutation: null,
      name1: 'Kaufland Logistik VZ GmbH & Co. KG - Lübbenau',
      name2: null,
      name3: null,
      zip: '03222',
      city: 'Lübbenau',
      country: 'Deutschland',
    },
    {
      id: null,
      salutation: null,
      name1: 'Kaufland Logistik VZ GmbH & Co. KG - Donnersdorf',
      name2: 'Filiale Donnersdorf',
      name3: null,
      zip: '97499',
      city: 'Donnersdorf',
      country: 'Deutschland',
    },
    {
      id: null,
      salutation: null,
      name1: 'Kaufland Logistik VZ GmbH & Co. KG - Geisenfeld',
      name2: null,
      name3: null,
      zip: '85290',
      city: 'Geisenfeld',
      country: 'Deutschland',
    },
  ];

  constructor() {}

  getLocations(name?: string): Observable<Location[]> {
    return of(
      name
        ? this.locations.filter(
            (x) =>
              x.name1 && x.name1.toLowerCase().indexOf(name.toLowerCase()) > -1
          )
        : this.locations
    );
  }
}
