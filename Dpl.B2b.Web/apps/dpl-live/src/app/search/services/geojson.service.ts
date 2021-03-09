import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { from } from 'rxjs';
import { map, mergeMap, tap } from 'rxjs/operators';
import * as topojson from 'topojson';

import { IZipFeatureCollection } from './search.service.types';
import { localizeUrl } from 'apps/dpl-live/src/utils';

@Injectable({
  providedIn: 'root',
})
export class GeojsonService {
  private _zipLookup: {
    [plz: string]: IZipFeatureCollection['features'][0];
  } = {};
  constructor(httpClient: HttpClient) {
    from([1, 2, 3, 5])
      .pipe(
        mergeMap((i) =>
          httpClient.get(localizeUrl(`/assets/zip/plz-${i}stellig.topojson`), {
            responseType: 'json',
          })
        ),
        map((data: any) => {
          return (topojson.feature(
            data,
            data.objects.plzs
          ) as any) as IZipFeatureCollection;
        }),
        tap((data) => {
          data.features.reduce((prev, current) => {
            prev[current.properties.plz] = current;
            return prev;
          }, this._zipLookup);
        })
      )
      .subscribe();
  }

  getGeoJsonForZip(zip: string) {
    const feature = this._zipLookup[zip];
    return feature;
  }
}
