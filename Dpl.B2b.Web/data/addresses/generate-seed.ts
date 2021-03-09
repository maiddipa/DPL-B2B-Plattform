//import sql from 'mssql'
import csvWriter from 'csv-write-stream';
import csvtojson from 'csvtojson';
import { CSVParseParam } from 'csvtojson/v2/Parameters';
import fs from 'fs';

import { GeocodedAddress, geocodedLMSCustomersFilePath } from './common';

type Address = {
  Id: number;
  RefLmsAddressNumber: string;
  Street1: string;
  PostalCode: string;
  City: string;
  CountryId: string;
  Lng: string;
  Lat: string;
};

type LoadingLocation = {
  Id: number;
  AddressId: number;
  StackHeightMin: number;
  StackHeightMax: number;
  SupportsPartialMatching: number;
  SupportsRearLoading: number;
  SupportsSideLoading: number;
  SupportsJumboVehicles: number;
};

const csvReadOptions: Partial<CSVParseParam> = {
  delimiter: ';',
};

const csvWriteOptions = {
  delimiter: ',',
  newline: '\r\n',
};

const countries = {
  Deutschland: 1,
  Frankreich: 2,
  Österreich: 3,
  Schweiz: 4,
  Polen: 5,
  'Vereinigtes Königreich': 6,
  'Tschechische Republik': 7,
  Niederlande: 8,
  Italien: 9,
  Belgien: 10,
  Griechenland: 11,
  Dänemark: 12,
  Luxemburg: 13,
  Bulgarien: 14,
  Slowakei: 15,
  Slowenien: 16,
  Ungarn: 17,
  Schweden: 18,
  Finnland: 19,
  Spanien: 20,
  Litauen: 21,
  Serbien: 22,
  Rumänien: 23,
  Ukraine: 24,
  Liechtenstein: 25,
  Kroatien: 26,
  Lettland: 27,
  Zypern: 28,
  Mazedonien: 29,
  Portugal: 30,
  Estland: 31,
  'Bosnien und Herzegowina': 32,
  Norwegen: 33,
  'Nicht ermittelte Länder u. Ge.': 34,
  Montenegro: 35,
};

async function run() {
  const outputExists = fs.existsSync(geocodedLMSCustomersFilePath);

  if (!outputExists) {
    throw new Error(`${geocodedLMSCustomersFilePath} does not exist`);
  }

  const contents: GeocodedAddress[] = await csvtojson(csvReadOptions).fromFile(
    geocodedLMSCustomersFilePath
  );

  const filteredContents = contents.filter((address) => !address.errors);

  const addressWriter = createCsvWriter('data/addresses/addresses.csv');
  const loadingLocationWriter = createCsvWriter(
    'data/addresses/loading-locations.csv'
  );

  filteredContents.forEach((data, index) => {

    const Id = index + 1;
    const address: Address = {
      Id,
      RefLmsAddressNumber: data.adressNr,
      Lat: data.latitude,
      Lng: data.longitude,
      Street1: data.street,
      PostalCode: data.postalCode,
      City: data.city,
      CountryId: countries[data.country] || null,
    };

    const loadingLocation: LoadingLocation = {
      Id,
      AddressId: Id,
      StackHeightMin: 0,
      StackHeightMax: 20,
      SupportsPartialMatching: 0,
      SupportsRearLoading: 0,
      SupportsSideLoading: 0,
      SupportsJumboVehicles: 0,
    };

    addressWriter.write(address);
    loadingLocationWriter.write(loadingLocation);
  });

  addressWriter.end();
  loadingLocationWriter.end();

  console.log('Done');
}

run();

function createCsvWriter(filePath: string, flags: 'w' | 'a' = 'w') {
  const writer = csvWriter({
    separator: csvWriteOptions.delimiter,
    newline: csvWriteOptions.newline,
  });

  writer.pipe(fs.createWriteStream(filePath, { flags }));

  return writer;
}
