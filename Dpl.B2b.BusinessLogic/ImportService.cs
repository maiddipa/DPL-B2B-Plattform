using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Dpl.B2b.BusinessLogic.ErrorHandlers;
using Dpl.B2b.BusinessLogic.Extensions;
using Dpl.B2b.BusinessLogic.ThirdParty;
using Dpl.B2b.Common.Enumerations;
using Dpl.B2b.Contracts;
using Dpl.B2b.Contracts.Models;
using Dpl.B2b.Dal;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using Olma = Dpl.B2b.Dal.Models;

namespace Dpl.B2b.BusinessLogic
{

    //[HandleServiceExceptions]
    public class ImportService : BaseService, IImportService
    {
        private readonly OlmaDbContext _olmaDbContext;
        private readonly IMapsService _mapsService;

        public ImportService(
            IAuthorizationDataService authData,
            IMapper mapper,
            OlmaDbContext olmaDbContext,
            IMapsService mapsService
            ) : base(authData, mapper)
        {
            _olmaDbContext = olmaDbContext;
            _mapsService = mapsService;
        }

        public async Task<IWrappedResponse> ImportCustomerPartners(int directoryId, IEnumerable<CustomerPartnerImportRecord> records)
        {
            var countryIso2Dict = _olmaDbContext.Countries
                .Include(i => i.States)
                .Where(i => i.Iso2Code != null)
                .ToDictionary(i => i.Iso2Code, i => new CountryDicEntry()
                {
                    Id = i.Id,
                    StatesDict = i.States
                        .Where(i => i.Iso2Code != null)
                        .ToDictionary(i => i.Iso2Code, i => i.Id)
                });

            var countryIso3Dict = _olmaDbContext.Countries
                .Where(i => i.Iso3Code != null)
                .ToDictionary(i => i.Iso3Code, i => i.Id);

            if (!Validate(records, countryIso3Dict))
            {
                Console.WriteLine("Imported aborted");
                return Failed<object>("Data validation failed");
            }

            var existingAddresses = _olmaDbContext.Addresses
                .Include(i => i.Country)
                .Include(i => i.State)
                .ToList();

            var existingCustomerPartnerReferenceDict = _olmaDbContext.CustomerPartners
                .Where(i => i.CustomerReference != null)
                .ToDictionary(i => i.CustomerReference);

            var referenceAddressDict = new Dictionary<string, Olma.Address>();
            var addressesToGeocode = new List<Olma.Address>();
            foreach (var row in records)
            {
                var address = existingAddresses.FirstOrDefault(i => i.Street1 == row.Street1
                    && i.Street2 == row.Street2
                    && i.PostalCode == row.PostalCode
                    && i.City == row.City
                    && i.Country.Iso3Code == row.CountryIso3Code
                );

                // we have decided that any change results in a new address (no updates on addresses)
                if (address == null)
                {


                    if (!countryIso3Dict.ContainsKey(row.CountryIso3Code))
                    {
                        throw new ArgumentException($"Value is not a supported ISO 3 CountryCode: {row.CountryIso3Code}");
                    }

                    var countryId = countryIso3Dict[row.CountryIso3Code];

                    address = new Olma.Address()
                    {
                        Street1 = row.Street1,
                        Street2 = row.Street2,
                        PostalCode = row.PostalCode,
                        City = row.City,
                        CountryId = countryId
                    };

                    // this is neccesssary to ensure duplicate addresses within rows are also detected
                    existingAddresses.Add(address);

                    // this is required to be able to execute geo coding for all new addreses
                    addressesToGeocode.Add(address);
                    _olmaDbContext.Addresses.Add(address);
                }

                if(address.GeoLocation == null)
                {
                    addressesToGeocode.Add(address);
                }

                // this is used to easily find partner records based on reference lateron
                referenceAddressDict.Add(row.Reference, address);
            }

            Parallel.ForEach(addressesToGeocode, new ParallelOptions { MaxDegreeOfParallelism = 2 }, async address =>
            {
                var location = await _mapsService.Geocode(Mapper.Map<Address>(address));

                if (location == null)
                {
                    Console.WriteLine($"Provided address of partner coud not be geocoded: {address.Street1}, {address.PostalCode} {address.City}");
                    return;
                }

                address.GeoLocation = new Point(location.Longitude, location.Latitude) { SRID = 4326 };

                if (address.StateId != null || address.State != null)
                {
                    return;
                }

                if (location.CountryIso2Code == null)
                {
                    Console.WriteLine($"Country ISO2 code could not be determined: {address.Street1}, {address.PostalCode} {address.City}");
                    return;
                }

                if (location.StateIso2Code == null)
                {
                    Console.WriteLine($"State ISO2 code could not be determined: {address.Street1}, {address.PostalCode} {address.City}");
                    return;
                }

                var countryEntry = countryIso2Dict.ContainsKey(location.CountryIso2Code) ? countryIso2Dict[location.CountryIso2Code] : null;
                if (countryEntry == null)
                {
                    Console.WriteLine($"Country with ISO2 code:'{location.CountryIso2Code}' is not supported: {address.Street1}, {address.PostalCode} {address.City}");
                    return;
                }

                var stateId = countryEntry.StatesDict.ContainsKey(location.StateIso2Code) ? countryEntry.StatesDict[location.StateIso2Code] as int? : null;
                if (stateId == null)
                {
                    Console.WriteLine($"State with ISO2 code:'{location.StateIso2Code}' is not supported: {address.Street1}, {address.PostalCode} {address.City}");
                    return;
                }

                address.StateId = countryIso2Dict[location.CountryIso2Code].StatesDict[location.StateIso2Code];
            });

            foreach (var row in records)
            {
                var address = referenceAddressDict[row.Reference];
                if (address == null)
                {
                    throw new ArgumentOutOfRangeException("There should be a address for each reference at this point");
                }

                var uniqueCustomerReferenceOld = $"{directoryId}||{row.ReferenceOld ?? row.Reference}";
                var uniqueCustomerReferenceCurrent = $"{directoryId}||{row.Reference}";

                var referenceExists = existingCustomerPartnerReferenceDict.ContainsKey(uniqueCustomerReferenceOld);

                // if reference old was supplied but does not exist
                if(row.ReferenceOld != null && !referenceExists)
                {
                    Console.WriteLine($"Old customer reference does not exist: {row.ReferenceOld}");
                    continue;
                }

                // this represents an update
                if (referenceExists)
                {
                    var customerPartner = existingCustomerPartnerReferenceDict[uniqueCustomerReferenceOld];
                    customerPartner.CompanyName = row.CompanyName;
                    customerPartner.Type = row.Type;
                    customerPartner.CustomerReference = uniqueCustomerReferenceCurrent;

                    if (address.Id == 0)
                    {
                        customerPartner.Address = address;
                    };

                    // there is no need to call update on the context as change tracking is active
                    // the context identifies updates on SaveChanges automatically
                }
                // this represents an insert
                else
                {
                    _olmaDbContext.CustomerPartners.Add(new Olma.CustomerPartner()
                    {
                        RowGuid = Guid.NewGuid(),
                        CustomerReference = uniqueCustomerReferenceCurrent,
                        CompanyName = row.CompanyName,
                        Type = row.Type,
                        Address = address,
                        DirectoryAccesses = new List<Olma.CustomerPartnerDirectoryAccess>()
                        {
                            new Olma.CustomerPartnerDirectoryAccess()
                            {
                                DirectoryId = directoryId
                            }
                        }
                    });
                }
            }

            var affectedRows = _olmaDbContext.SaveChanges();

            Console.WriteLine($"Number of partners created or updated: {affectedRows}");

            return Ok<object>(null);
        }

        private bool Validate(IEnumerable<CustomerPartnerImportRecord> records, IDictionary<string, int> countryIso3Dict)
        {
            var validationErrors = records.Select(i =>
            {
                var countryMatched = i.CountryIso3Code != null && countryIso3Dict.ContainsKey(i.CountryIso3Code);
                var valid = countryMatched;

                return new
                {
                    data = i,
                    countryMatched,
                    valid
                };
            })
            .Where(i => !i.valid)
            .ToList();

            foreach (var error in validationErrors)
            {
                if (!error.countryMatched)
                {
                    Console.WriteLine($"Entry with reference: {error.data.Reference}, has unsupported country code: {error.data.CountryIso3Code}");
                }
            }

            return validationErrors.Count == 0;
        }
    }

    public class CountryDicEntry
    {
        public int Id { get; set; }

        public IDictionary<string, int> StatesDict { get; set; }
    }
}
