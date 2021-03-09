//import sql from 'mssql'
import NodeGeocoder from 'node-geocoder';
import fs from 'fs';
import csvWriter from 'csv-write-stream';
import sql from 'mssql/msnodesqlv8';
import { default as csvtojson } from 'csvtojson';
import { CSVParseParam } from 'csvtojson/v2/Parameters';
import { GeocodedAddress, geocodedLMSCustomersFilePath } from './common';

const options = {
  provider: 'google',

  // Optional depending on the providers
  //fetch: customFetchImplementation,
  apiKey: 'AIzaSyDrZgaq6_Wog4ryR_nwM1TQkkCO9aGpp8I', // for Mapquest, OpenCage, Google Premier
  formatter: null, // 'gpx', 'string', ...
};

const csvReadOptions: Partial<CSVParseParam> = {
  delimiter: ';',
  trim: true,
};

const csvWriteOptions = {
  delimiter: ';',
  newline: '\r\n',
};

const geocoder = NodeGeocoder(options);

async function run() {
  const outputExists = fs.existsSync(geocodedLMSCustomersFilePath);

  const contents: GeocodedAddress[] = outputExists
    ? await csvtojson(csvReadOptions).fromFile(geocodedLMSCustomersFilePath)
    : [];

  const alreadyGeocodedAddressIds = new Set(
    contents
      .filter((address) => !address.errors)
      .map((address) => address.adressNr)
  );

  const conn = new sql.ConnectionPool({
    database: 'OlmaDbDvit',
    server: 'bmfch874fj.database.windows.net',
    driver: 'msnodesqlv8',
    user: 'DplLtmsDbDvitUser',
    password: '4uFghCjD8kXD',
    options: {
      encrypt: true,
      trustServerCertificate: true,
    },
  });

  const schemaName = 'LMS';

  // const conn = new sql.ConnectionPool({
  //   database: "LmsDbDvit",
  //   server: "localhost\\SQLEXPRESS",
  //   driver: "msnodesqlv8",
  //   //connectionString: "Driver={SQL Server Native Client 11.0};Server=localhost\\SQLEXPRESS;Database=LmsDbDvit;Trusted_Connection=yes;",
  //   options: {
  //     trustedConnection: true,
  //   },
  // } as any);

  //const schemaName = "dbo";

  await conn.connect();

  const query = `select C.[AdressNr], C.[Strasse], C.[PLZ], C.[Ort], C.[Land] from ${schemaName}.LMS_CUSTOMER AS C
    WHERE C.ClientID = 1 
    AND C.[Strasse] is not NULL AND TRIM(C.[Strasse]) <> ''
    AND C.[PLZ] is not NULL AND TRIM(C.[PLZ]) <> ''
    AND C.[Ort] is not NULL AND TRIM(C.[Ort]) <> ''
    AND C.[Land] is not NULL AND TRIM(C.[Land]) <> ''
    `;

  const sqlQueryResult = await conn.query(query);
  console.dir(sqlQueryResult.recordset.length);

  const resultData = sqlQueryResult.recordset
    .map((i) => ({
      adressNr: i.AdressNr.toString(),
      street: i.Strasse.trim(),
      postalCode: i.PLZ.trim(),
      city: i.Ort.trim(),
      country: i.Land.trim(),
    }))
    .map((data) => {
      return {
        ...data,
        search: `${data.street}, ${data.postalCode} ${data.city}, ${data.country}`,
      };
    })
    .filter((i) => {
      return !alreadyGeocodedAddressIds.has(i.adressNr);
    });

  const chunked = getChunked(resultData, 35) as typeof resultData[];

  const writer = csvWriter({
    separator: csvWriteOptions.delimiter,
    newline: csvWriteOptions.newline,
  });

  writer.pipe(
    fs.createWriteStream(geocodedLMSCustomersFilePath, { flags: 'a' })
  );

  for (const baseData of chunked) {
    const searchStrings = baseData.map((i) => i.search);
    const results = await geocoder.batchGeocode(searchStrings);

    if (searchStrings.length !== results.length) {
      console.log(JSON.stringify(searchStrings));
      console.log(JSON.stringify(results));
      throw new Error('This should not ever happen');
    }

    baseData.forEach((address, index) => {
      const result = results[index];
      const data = !result.error
        ? result.value.length > 0
          ? result.value[0]
          : null
        : null;
      const latitude = data?.latitude;
      const longitude = data?.longitude;
      const csvData = {
        ...address,
        latitude,
        longitude,
        errors: result.error,
      };

      writer.write(csvData);
    });

    await promiseTimeout(200);
  }

  writer.end();
}

run();

function getChunked(array, chunk_size) {
  return Array(Math.ceil(array.length / chunk_size))
    .fill(null)
    .map((_, index) => index * chunk_size)
    .map((begin) => array.slice(begin, begin + chunk_size));
}

function promiseTimeout(time) {
  return new Promise(function (resolve, reject) {
    setTimeout(function () {
      resolve(time);
    }, time);
  });
}
