import { normalize, schema } from 'normalizr';

const partner = new schema.Entity('partners');
const partnerDirectory = new schema.Entity('partnerDirectories', {
  partners: [partner],
});

export const normalizePartnerDirectories = (serverResponse) =>
  (normalize(serverResponse, [partnerDirectory]) as any) as {
    entities: {
      partnerDirectories: any;
      partners: any;
    };
    result: any;
  };
