import * as d3 from 'd3';
import * as jf from 'jsonfile';
import * as topojson from 'topojson';
import { execSync } from 'child_process';
import * as fs from 'fs';

const array = [1, 2, 3, 5];
for (let i = 0; i < array.length; i++) {
  const index = array[i];
  const fileName = `plz-${index}stellig`;
  const sourceFileName = `./data/plzs/${fileName}.topojson`;
  const geoJsonFileName = `./data/plzs/${fileName}.geojson`;
  const assetFileName = `./src/assets/zip/${fileName}.topojson`;

  const data = jf.readFileSync(sourceFileName);

  const geoJson = (topojson.feature(
    data,
    data.objects[fileName]
  ) as any) as GeoJSON.FeatureCollection<GeoJSON.GeometryObject, any>;

  // Compute the projected centroid, area and length of the side
  // of the squares.
  geoJson.features.forEach(function(d) {
    const centroid = (d3.geoCentroid(d) as any) as [number, number];
    d.properties = {
      ...{
        plz: d.properties.plz,
        center: [centroid[1], centroid[0]]
      },
      ...(d.properties.note ? { note: d.properties.note } : {})
    };
  });

  jf.writeFileSync(geoJsonFileName, geoJson);
  execSync(
    `npx mapshaper ${geoJsonFileName} -o ${assetFileName} format=topojson`
  );

  fs.unlinkSync(geoJsonFileName);

  const topoJson = jf.readFileSync(assetFileName);
  topoJson.objects['plzs'] = topoJson.objects[fileName];
  delete topoJson.objects[fileName];
  jf.writeFileSync(assetFileName, topoJson);
}
