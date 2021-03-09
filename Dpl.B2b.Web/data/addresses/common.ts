export const geocodedLMSCustomersFilePath =
  'data/addresses/lms-customer-geocoded.csv';

export type GeocodedAddress = {
  adressNr: string;
  street: string;
  postalCode: string;
  city: string;
  country: string;
  search: string;
  latitude?: string;
  longitude?: string;
  errors?: string;
};
