import * as parser from 'xml2json';
import * as fs from 'fs';
import * as path from 'path';

const options = {
  object: true,
  reversible: true,
  coerce: false,
  sanitize: true,
  trim: false,
  arrayNotation: false,
  alternateTextNode: false,
};

const basePath = './apps/dpl-live/src/locale';

const files = fs.readdirSync(basePath);

function fixFileSection(file) {
  // if there is only one translation trans unit will not be recognized as an array
  const transUnits = Array.isArray(file.body['trans-unit'])
    ? file.body['trans-unit']
    : [file.body['trans-unit']];

  const filteredTransUnits = transUnits.filter(
    (i) => i.source && i.source.$t && (i.source.$t as string).includes('<x')
  );

  for (
    let transUnitIndex = 0;
    transUnitIndex < filteredTransUnits.length;
    transUnitIndex++
  ) {
    const transUnit = filteredTransUnits[transUnitIndex];
    transUnit['xml:space'] = 'html';
  }
}

for (let dirIndex = 0; dirIndex < files.length; dirIndex++) {
  const fileName = files[dirIndex];
  const filePath = path.join(basePath, fileName);
  const xmlString = fs.readFileSync(filePath, 'utf8');
  const xmlJson = parser.toJson(xmlString, options);

  // ignore if file is empty
  if (xmlJson?.xliff?.file) {
    // if there either no code or no template translations there will only be one file
    const fileSections = Array.isArray(xmlJson.xliff.file)
      ? xmlJson.xliff.file
      : [xmlJson.xliff.file];

    for (let fileIndex = 0; fileIndex < fileSections.length; fileIndex++) {
      const file = fileSections[fileIndex];
      fixFileSection(file);
    }

    const fixedXmlString = parser.toXml(JSON.stringify(xmlJson));
    fs.writeFileSync(filePath, fixedXmlString, {
      encoding: 'utf8',
    });
  }
}
