using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Text;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using Dpl.B2b.Common.Enumerations;
using Dpl.B2b.Contracts;
using Dpl.B2b.Dal.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NetTopologySuite.Geometries;

namespace Dpl.B2b.Dal
{
    public static class OlmaDbContextDevSeed
    {
        public static void Seed(DbContextOptions<OlmaDbContext> options, IServiceProvider serviceProvider)
        {
            var maps = serviceProvider.GetService<IMapsService>();
            using (var context = new OlmaDbContext(options, serviceProvider))
            {

                using (context.Database.BeginTransaction())
                {
                    context.Database.SetCommandTimeout(300);

                    #region Dummy LoadingLocations based on LMS_Customers

                    GenerateLmsCustomerLoadingLocations(context);
                    // this is done once in source lms
                    //UpdateLmsAvailabilityAndDeliveryLoadingLocatrionIds(connectionString);

                    #endregion

                    var loadCarriers = context.LoadCarriers
                         .IgnoreQueryFilters()
                         .Include(i => i.Quality)
                         .Include(i => i.Type)
                         .ToArray();

                    var loadCarrierIds = loadCarriers.Select(i => i.Id).ToArray();

                    var loadCarrierTypes = loadCarriers.Select(i => i.Type).Distinct()
                        .ToArray();

                    var documentTypes = context.DocumentTypes
                        .IgnoreQueryFilters()
                        .ToList();

                    var countries = context.Countries
                         .IgnoreQueryFilters()
                         .Include(i => i.States)
                         .OrderBy(i => i.Id)
                         .ToArray();

                    #region Addresses

                    var dummySupermarktRefNumber = 999999;

                    var addresses = new List<Address>()
                {
                    new Address()
                    {
                        //RefLmsAddressNumber = "Cust Address Ref 1",
                        Street1 = "Cust street 1",
                        City = "Cust city 1",
                        PostalCode = "Cust 1",
                        CountryId = countries.First().Id,
                        StateId = countries.First().States.Skip(3).First().Id
                    },
                    new Address()
                    {
                        //RefLmsAddressNumber = "Cust Address Ref 2",
                        Street1 = "Cust street 2",
                        City = "Cust city 2",
                        PostalCode = "Cust 2",
                        CountryId = countries.First().Id,
                        StateId = countries.First().States.Skip(3).First().Id
                    },
                    new Address()
                    {
                        //RefLmsAddressNumber = "Lief Address Ref 3",
                        Street1 = "Lief street 3",
                        City = "Lief city 3",
                        PostalCode = "Lief 3",
                        CountryId = countries.First().Id,
                        StateId = countries.First().States.Skip(3).First().Id
                    },
                    new Address()
                    {
                        //RefLmsAddressNumber = "Address Ref 4",
                        Street1 = "Street 4",
                        City = "City 4",
                        PostalCode = "4",
                        CountryId = countries.First().Id,
                        StateId = countries.First().States.Skip(3).First().Id
                    },
                    new Address()
                    {
                        //RefLmsAddressNumber = "Sped Address Ref 5",
                        Street1 = "Sped street 5",
                        City = "Sped city 5",
                        PostalCode = "Sped 5",
                        CountryId = countries.First().Id,
                        StateId = countries.First().States.Skip(3).First().Id
                    }
                };

                    context.Addresses.AddRange(addresses);
                    context.SaveChanges();

                    #endregion

                    #region Organisation / Customer / Division / Posting Account / Loading Location

                    #region DocumentSettings helper

                    Func<List<CustomerDocumentSetting>> generateCustomerDocumentSettings = () =>
                    {

                        return new List<CustomerDocumentSetting>(); // HACK remove customer document settinsg as it caused laoing issues
                        Func<int, int, CustomerDocumentSetting> generateCustomerSetting = (int documentTypeId, int loadCarrierTypeId) => new CustomerDocumentSetting()
                        {
                            DocumentTypeId = documentTypeId,
                            LoadCarrierTypeId = loadCarrierTypeId,
                            MaxQuantity = 2000,
                            ThresholdForWarningQuantity = 1000,
                        };

                        return documentTypes
                            .Where(i => i.HasReport)
                            .SelectMany(documentType => loadCarrierTypes.Select(loadCarrrierType => generateCustomerSetting(documentType.Id, loadCarrrierType.Id)))
                            .Take(10)
                            .ToList();
                    };

                    Func<int, List<CustomerDivisionDocumentSetting>> generateCustomerDivisionDocumentSettings = (int customerId) =>
                    {
                        Func<int, CustomerDivisionDocumentSetting> generateSetting = (int documentTypeId) => new CustomerDivisionDocumentSetting()
                        {
                            DocumentTypeId = documentTypeId,
                            DefaultPrintCount = 2,
                            PrintCountMin = 1,
                            PrintCountMax = 5,
                            Override = true,
                            DocumentNumberSequence = new DocumentNumberSequence
                            {
                                Counter = 1,
                                CanAddDivisionShortName = true,
                                CanAddDocumentTypeShortName = true,
                                CanAddPaddingForCounter = true,
                                CanAddPaddingForCustomerNumber = true,
                                PaddingLengthForCounter = 6,
                                DisplayName = "Schema",
                                PaddingLengthForCustomerNumber = 8,
                                PostfixForCustomerNumber = "/",
                                PrefixForCounter = "-",
                                Prefix = "Doc",
                                SeparatorAfterPrefix = "-",
                                Separator = "-",
                                //Type = documentTypeId == 1 ? PrintType.VoucherDirectReceipt : documentTypeId == 6 ? PrintType.PickupOrder : documentTypeId == 7 ? PrintType.VoucherDigital : documentTypeId == 10 ? PrintType.VoucherOriginal : documentTypeId == 13 ? PrintType.LoadCarrierReceipt : PrintType.VoucherOriginal  //(PrintType)Enum.Parse(typeof(PrintType), documentTypeId.ToString())
                                DocumentType = documentTypes.Single(i => i.Id == documentTypeId).Type,
                                CustomerId =  customerId
                            }
                        };

                        return documentTypes.Select(documentType => generateSetting(documentType.Id)).ToList();
                    };

                    #endregion

                    var organizations = new List<Organization>()
                {
                    new Organization()
                    {
                        Name = "Organisation Handel Kld",
                        Address = addresses.First(),
                        Customers = new Customer[] { 
                            new Customer() {
                                Name = "Handel Headquarter",
                                RefErpCustomerNumber = 39790,
                                DocumentSettings = generateCustomerDocumentSettings(),
                                Address = addresses.First(),
                                Partner = new Partner(){RowGuid = new Guid("AC7AD658-B0D5-4C01-B1B7-A2ECF7C0678C"),CompanyName = "Part Handel Headquater - Cust city 1",DefaultAddress = addresses.First()},
                                Divisions = new CustomerDivision[] {
                                    new CustomerDivision() {
                                        Name="Abt. 1 Handel" ,
                                        LoadingLocations = new LoadingLocation[] {
                                            new LoadingLocation(){
                                                Address = new Address()
                                                {
                                                    //RefLmsAddressNumber = "Ladestelle 1",
                                                    Street1 = "Raiffeisenstraße 5",
                                                    City = "Iserlohn",
                                                    PostalCode = "58638",
                                                    CountryId = countries.First().Id,
                                                    StateId = countries.First().States.Skip(3).First().Id,
                                                    GeoLocation = new Point(7.6743767, 51.3837357)  { SRID = 4326 }
                                                },
                                                BusinessHours = GenerateBusinessHours(start: 7, end :17, half: 12),
                                                SupportsRearLoading = true,
                                                SupportsSideLoading = true,
                                            },
                                            new LoadingLocation(){
                                                Address = new Address()
                                                {
                                                    //RefLmsAddressNumber = "Ladestelle 2",
                                                    Street1 = "Bornstraße 160 a",
                                                    City = "Dortmund",
                                                    PostalCode = "44145",
                                                    CountryId = countries.First().Id,
                                                    StateId = countries.First().States.Skip(3).First().Id,
                                                    GeoLocation = new Point(7.4705885, 51.5289347)  { SRID = 4326 }
                                                },
                                                BusinessHours = GenerateBusinessHours(start: 7, end :17, half: 12),
                                                SupportsSideLoading = true,
                                            },
                                            new LoadingLocation(){
                                                Address = new Address()
                                                {
                                                    //RefLmsAddressNumber = "Ladestelle 3",
                                                    Street1 = "Kurze-Geismar-Straße 26",
                                                    City = "Göttingen",
                                                    PostalCode = "37073",
                                                    CountryId = countries.First().Id,
                                                    StateId = countries.First().States.Skip(3).First().Id,
                                                    GeoLocation = new Point(9.938267, 51.5310906)  { SRID = 4326 }
                                                },
                                                BusinessHours = GenerateBusinessHours(start: 7, end :17, half: 12),
                                                SupportsRearLoading = true,
                                                SupportsSideLoading = true,
                                                SupportsJumboVehicles = true
                                            },
                                            new LoadingLocation(){
                                                Address = new Address()
                                                {
                                                    //RefLmsAddressNumber = "Ladestelle 5",
                                                    Street1 = "Am Schreiberteich 3",
                                                    City = "Wernigerode",
                                                    PostalCode = "38855",
                                                    CountryId = countries.First().Id,
                                                    StateId = countries.First().States.Skip(3).First().Id,
                                                    GeoLocation = new Point(10.782101, 51.8430802)  { SRID = 4326 }
                                                },
                                                BusinessHours = GenerateBusinessHours(start: 7, end :17, half: 12),
                                                SupportsRearLoading = true,
                                            },
                                        },
                                        PostingAccount = new PostingAccount()
                                        {
                                            DisplayName = "Account 39790-HZL",
                                            RefLtmsAccountId = 6402,
                                            RefLtmsAccountNumber = "39790-HZL",
                                            RefErpCustomerNumber = 39790,
                                            Address = new Address()
                                            {
                                                //RefLmsAddressNumber = "Booking Address Ref 2",
                                                Street1 = "Booking street 1",
                                                City = "Booking city 1",
                                                PostalCode = "Booking Postal 1",
                                                CountryId = countries.First().Id,
                                                StateId = countries.First().States.Skip(3).First().Id,
                                            },
                                            CalculatedBalance = GenerateCalculatedBalance(loadCarriers),
                                            OrderConditions = GenerateOrderConditions()
                                        },
                                        ShortName = "D1"
                                    }
                                },
                            }
                        }
                    },
                    new Organization()
                    {
                        Name = "Organisation Dpl",
                        Address = addresses.Skip(1).First(),
                        Customers = new Customer[]{
                            new Customer() {
                                Name = "Dpl Zentrale",
                                RefErpCustomerNumber = 40429,
                                DocumentSettings = generateCustomerDocumentSettings(),
                                Address = addresses.Skip(1).First(),
                                Partner = new Partner(){RowGuid = new Guid("EF2C9382-DD23-46CC-A651-16D828B21787"),CompanyName = "Part Lieferant Zentrale - Cust city 2",DefaultAddress = addresses.Skip(1).First()},
                                Divisions = new CustomerDivision[] {
                                    new CustomerDivision() {
                                        Name="Abt. 1 Dpl" ,
                                        LoadingLocations = new LoadingLocation[] {
                                            new LoadingLocation(){
                                                Address = new Address()
                                                {
                                                    //RefLmsAddressNumber = "DPL Deutsche Paletten Logistik GmbH",
                                                    Street1 = "Overweg 12",
                                                    City = "Soest",
                                                    PostalCode = "59494",
                                                    CountryId = countries.First().Id,
                                                    StateId = countries.First().States.Skip(3).First().Id,
                                                    GeoLocation = new Point( 8.1412768, 51.5563682)  { SRID = 4326 }
                                                },
                                                BusinessHours = GenerateBusinessHours(start: 6, end: 18, half: 12),
                                                SupportsRearLoading = true,
                                                SupportsJumboVehicles = true,
                                            },
                                            new LoadingLocation(){
                                                Address = new Address()
                                                {
                                                    //RefLmsAddressNumber = "Depot 1",
                                                    Street1 = "Am Großmarkt 37",
                                                    City = "Herne",
                                                    PostalCode = "44653",
                                                    CountryId = countries.First().Id,
                                                    StateId = countries.First().States.Skip(3).First().Id,
                                                    GeoLocation = new Point( 7.1990149, 51.5414016)  { SRID = 4326 }
                                                },
                                                BusinessHours = GenerateBusinessHours(start: 6, end: 18, half: 12),
                                                SupportsSideLoading = true,
                                                SupportsRearLoading = true,
                                                SupportsJumboVehicles = true,
                                            },
                                            new LoadingLocation(){
                                                Address = new Address()
                                                {
                                                    //RefLmsAddressNumber = "Depot 2",
                                                    Street1 = "Europaallee 17 - 19",
                                                    City = "Föhren",
                                                    PostalCode = "54343",
                                                    CountryId = countries.First().Id,
                                                    StateId = countries.First().States.Skip(3).First().Id,
                                                    GeoLocation = new Point( 6.7902701, 49.8622498)  { SRID = 4326 }
                                                },
                                                BusinessHours = GenerateBusinessHours(start: 6, end: 18, half: 12),
                                                SupportsSideLoading = true,
                                                SupportsRearLoading = true,
                                            },
                                            new LoadingLocation(){
                                                Address = new Address()
                                                {
                                                    //RefLmsAddressNumber = "Depot 3",
                                                    Street1 = "Glockenstraße 87",
                                                    City = "Troisdorf",
                                                    PostalCode = "53844",
                                                    CountryId = countries.First().Id,
                                                    StateId = countries.First().States.Skip(3).First().Id,
                                                    GeoLocation = new Point( 7.0842999, 50.78012)  { SRID = 4326 }
                                                },
                                                BusinessHours = GenerateBusinessHours(start: 6, end: 18, half: 12),
                                                SupportsRearLoading = true,
                                                SupportsJumboVehicles = true,
                                            },
                                            new LoadingLocation(){
                                                Address = new Address()
                                                {
                                                    //RefLmsAddressNumber = "Depot 4",
                                                    Street1 = "Vinner Str. 157",
                                                    City = "Bielefeld",
                                                    PostalCode = "33729",
                                                    CountryId = countries.First().Id,
                                                    StateId = countries.First().States.Skip(3).First().Id,
                                                    GeoLocation = new Point( 8.6593639, 52.0483985)  { SRID = 4326 }
                                                },
                                                BusinessHours = GenerateBusinessHours(start: 6, end: 18, half: 12),
                                                SupportsSideLoading = true,
                                            },
                                            new LoadingLocation(){
                                                Address = new Address()
                                                {
                                                    //RefLmsAddressNumber = "Lager 1",
                                                    Street1 = "Feineisenstraße",
                                                    City = "Dortmund",
                                                    PostalCode = "44339",
                                                    CountryId = countries.First().Id,
                                                    StateId = countries.First().States.Skip(3).First().Id,
                                                    GeoLocation = new Point( 7.4942421, 51.5474149)  { SRID = 4326 }
                                                },
                                                BusinessHours = GenerateBusinessHours(start: 6, end: 18, half: 12),
                                                SupportsSideLoading = true,
                                                SupportsRearLoading = true,
                                            },
                                            new LoadingLocation(){
                                                Address = new Address()
                                                {
                                                    //RefLmsAddressNumber = "Lager 2",
                                                    Street1 = "Rüdigerstraße 11",
                                                    City = "Dortmund",
                                                    PostalCode = "44319",
                                                    CountryId = countries.First().Id,
                                                    StateId = countries.First().States.Skip(3).First().Id,
                                                    GeoLocation = new Point(7.623348, 51.531729)  { SRID = 4326 }
                                                },
                                                BusinessHours = GenerateBusinessHours(start: 8, end: 16, half: 11),
                                                BusinessHourExceptions = new BusinessHourException[]
                                                {
                                                    new BusinessHourException() {FromDateTime = new DateTime(2019,2,1),ToDateTime = new DateTime(2019,2,5),Type = BusinessHourExceptionType.Closed}
                                                },
                                                SupportsSideLoading = true,
                                                SupportsRearLoading = true,
                                                SupportsJumboVehicles = true
                                            },



                                            new LoadingLocation(){
                                                Address = new Address()
                                                {
                                                    //RefLmsAddressNumber = "Supermarkt 1",
                                                    RefLmsAddressNumber = dummySupermarktRefNumber,
                                                    Street1 = "Senator-Schwartz-Ring 24",
                                                    City = "Soest",
                                                    PostalCode = "59494",
                                                    CountryId = countries.First().Id,
                                                    StateId = countries.First().States.Skip(3).First().Id,
                                                    GeoLocation = new Point(8.0808048, 51.5605733)  { SRID = 4326 }
                                                },
                                                BusinessHours = GenerateBusinessHours(start: 8, end: 18, half: 12),
                                                BusinessHourExceptions = new BusinessHourException[]
                                                {
                                                    new BusinessHourException() {FromDateTime = new DateTime(2019,2,1),ToDateTime = new DateTime(2019,2,5),Type = BusinessHourExceptionType.Closed}
                                                },
                                                SupportsSideLoading = true,
                                                SupportsRearLoading = true,
                                            },
                                            new LoadingLocation(){
                                                Address = new Address()
                                                {
                                                    //RefLmsAddressNumber = "Supermarkt 2",
                                                    RefLmsAddressNumber = dummySupermarktRefNumber,
                                                    Street1 = "Soester Str. 26",
                                                    City = "Werl",
                                                    PostalCode = "59457",
                                                    CountryId = countries.First().Id,
                                                    StateId = countries.First().States.Skip(3).First().Id,
                                                    GeoLocation = new Point(7.9167842, 51.5492104)  { SRID = 4326 }
                                                },
                                                BusinessHours = GenerateBusinessHours(start: 8, end: 18, half: 12),
                                                BusinessHourExceptions = new BusinessHourException[]
                                                {
                                                    new BusinessHourException() {FromDateTime = new DateTime(2019,2,1),ToDateTime = new DateTime(2019,2,5),Type = BusinessHourExceptionType.Closed}
                                                },
                                                SupportsSideLoading = true,
                                                SupportsRearLoading = true,
                                            },
                                            new LoadingLocation(){
                                                Address = new Address()
                                                {
                                                    //RefLmsAddressNumber = "Supermarkt 3",
                                                    RefLmsAddressNumber = dummySupermarktRefNumber,
                                                    Street1 = "Zollpost 15",
                                                    City = "Kamen",
                                                    PostalCode = "59174",
                                                    CountryId = countries.First().Id,
                                                    StateId = countries.First().States.Skip(3).First().Id,
                                                    GeoLocation = new Point(7.6702908, 51.5789923)  { SRID = 4326 }
                                                },
                                                BusinessHours = GenerateBusinessHours(start: 8, end: 18, half: 12),
                                                BusinessHourExceptions = new BusinessHourException[]
                                                {
                                                    new BusinessHourException() {FromDateTime = new DateTime(2019,2,1),ToDateTime = new DateTime(2019,2,5),Type = BusinessHourExceptionType.Closed}
                                                },
                                                SupportsSideLoading = true,
                                            },
                                            new LoadingLocation(){
                                                Address = new Address()
                                                {
                                                    //RefLmsAddressNumber = "Supermarkt 4",
                                                    Street1 = "Merschstraße 19",
                                                    City = "Lünen",
                                                    PostalCode = "44534",
                                                    CountryId = countries.First().Id,
                                                    StateId = countries.First().States.Skip(3).First().Id,
                                                    GeoLocation = new Point(7.5292773, 51.6154856)  { SRID = 4326 }
                                                },
                                                BusinessHours = GenerateBusinessHours(start: 8, end: 18, half: 12),
                                                BusinessHourExceptions = new BusinessHourException[]
                                                {
                                                    new BusinessHourException() {FromDateTime = new DateTime(2019,2,1),ToDateTime = new DateTime(2019,2,5),Type = BusinessHourExceptionType.Closed}
                                                },
                                                SupportsRearLoading = true,
                                            },
                                            new LoadingLocation(){
                                                Address = new Address()
                                                {
                                                    //RefLmsAddressNumber = "Supermarkt 5",
                                                    RefLmsAddressNumber = dummySupermarktRefNumber,
                                                    Street1 = "Töddinghauser Str. 150",
                                                    City = "Bergkamen",
                                                    PostalCode = "59192",
                                                    CountryId = countries.First().Id,
                                                    StateId = countries.First().States.Skip(3).First().Id,
                                                    GeoLocation = new Point(7.6310032, 51.614191)  { SRID = 4326 }
                                                },
                                                BusinessHours = GenerateBusinessHours(start: 8, end: 18, half: 12),
                                                BusinessHourExceptions = new BusinessHourException[]
                                                {
                                                    new BusinessHourException() {FromDateTime = new DateTime(2019,2,1),ToDateTime = new DateTime(2019,2,5),Type = BusinessHourExceptionType.Closed}
                                                },
                                                SupportsSideLoading = true,
                                                SupportsRearLoading = true,
                                            },
                                        },
                                        PostingAccount = new PostingAccount()
                                        {
                                            DisplayName = "Account 40429-LAG",
                                            RefLtmsAccountId = 6617,
                                            RefLtmsAccountNumber = "40429-LAG",
                                            RefErpCustomerNumber = 40429,
                                            Address = new Address()
                                            {
                                                //RefLmsAddressNumber = "Booking Address Ref 4",
                                                Street1 = "Booking street 4",
                                                City = "Booking city 4",
                                                PostalCode = "Booking Postal 4",
                                                CountryId = countries.First().Id,
                                                StateId = countries.First().States.Skip(3).First().Id,
                                            },
                                            CalculatedBalance = GenerateCalculatedBalance(loadCarriers),
                                            OrderConditions = GenerateOrderConditions()
                                        },
                                        ShortName = "DFS"
                                    }
                                }
                            }
                         }
                    },
                    new Organization()
                    {
                        Name = "Organisation Lieferant EGr",
                        Address = addresses.Skip(3).First(),
                        Customers = new Customer[]{
                            new Customer() {
                                Name = "Lieferant 1",
                                RefErpCustomerNumber = 11459,
                                DocumentSettings = generateCustomerDocumentSettings(),
                                Address = addresses.Skip(3).First(),
                                Partner = new Partner(){RowGuid = new Guid("388B63F3-08FB-464F-839F-37E6F90E8340"),CompanyName = "Part TUNIX Ltd. - City 4",DefaultAddress = addresses.Skip(3).First()},
                                Divisions = new CustomerDivision[] {
                                    new CustomerDivision() {
                                        Name="Abt. 1 Lieferant" ,
                                        LoadingLocations = new LoadingLocation[] {
                                            new LoadingLocation(){
                                                Address = new Address()
                                                {
                                                    //RefLmsAddressNumber = "Loading Address Ref 3",
                                                    Street1 = "Loading street 3",
                                                    City = "Loading city 3",
                                                    PostalCode = "LoadingPostal 3",
                                                    CountryId = countries.First().Id,
                                                    StateId = countries.First().States.Skip(3).First().Id,
                                                },
                                                BusinessHours = GenerateBusinessHours(start: 6, end: 18, half: 12),

                                            }
                                        },
                                        PostingAccount = new PostingAccount()
                                        {
                                            DisplayName = "Account 11459-TK",
                                            RefLtmsAccountId = 124,
                                            RefLtmsAccountNumber = "11459-TK",
                                            RefErpCustomerNumber = 11459,
                                            Address = new Address()
                                            {
                                                //RefLmsAddressNumber = "Booking Address Ref 4",
                                                Street1 = "Booking street 4",
                                                City = "Booking city 4",
                                                PostalCode = "Booking Postal 4",
                                                CountryId = countries.First().Id,
                                                StateId = countries.First().States.Skip(10).First().Id,
                                            },
                                            CalculatedBalance = GenerateCalculatedBalance(loadCarriers),
                                            OrderConditions = GenerateOrderConditions()
                                        },
                                        ShortName = "DTX"
                                    }
                                }
                            }
                         }
                    },
                    new Organization()
                    {
                        Name = "Organisation Depot Fzl",
                        Address = addresses.Skip(2).First(),
                        Customers = new Customer[]{
                            new Customer() {
                                Name = "Depot 1",
                                RefErpCustomerNumber = 41916,
                                DocumentSettings = generateCustomerDocumentSettings(),
                                Address = addresses.Skip(2).First(),
                                Partner = new Partner(){RowGuid = new Guid("564EDD18-395C-4721-B17E-51500BB6EA10"),CompanyName = "Part Depot 1 - City 3",DefaultAddress = addresses.Skip(2).First()},
                                Divisions = new CustomerDivision[] {
                                    new CustomerDivision() {
                                        Name="Abt. 1 Depot" ,
                                        LoadingLocations = new LoadingLocation[] {
                                            new LoadingLocation(){
                                                Address = new Address()
                                                {
                                                    //RefLmsAddressNumber = "Loading Address Ref 3",
                                                    Street1 = "Zur Hubertushalle 4",
                                                    City = "Sundern",
                                                    PostalCode = "59846",
                                                    CountryId = countries.First().Id,
                                                    StateId = countries.First().States.Skip(10).First().Id,
                                                    GeoLocation = new Point(8.0046676, 51.3287044)  { SRID = 4326 }
                                                },
                                                BusinessHours = GenerateBusinessHours(start: 8, end: 18, half: 11),

                                            }
                                        },
                                        PostingAccount = new PostingAccount()
                                        {
                                            DisplayName = "Account 41916-DEP",
                                            RefLtmsAccountId = 6550,
                                            RefLtmsAccountNumber = "41916-DEP",
                                            RefErpCustomerNumber = 41916,
                                            Address = new Address()
                                            {
                                                //RefLmsAddressNumber = "Booking Address Ref 4",
                                                Street1 = "Zur Hubertushalle 4",
                                                City = "Sundern",
                                                PostalCode = "59846",
                                                CountryId = countries.First().Id,
                                                StateId = countries.First().States.Skip(10).First().Id,
                                                GeoLocation = new Point(8.0046676, 51.3287044)  { SRID = 4326 }
                                            },
                                            CalculatedBalance = GenerateCalculatedBalance(loadCarriers),
                                            OrderConditions = GenerateOrderConditions()
                                        },
                                        ShortName = "DL1"
                                    }
                                }
                            }
                         }
                    },
                    new Organization()
                    {
                        Name = "Organisation Spedition",
                        Address = addresses.Skip(4).First(),
                        Customers = new Customer[]{
                                new Customer() {
                                    Name = "Spedition 1 Dxa",
                                    RefErpCustomerNumber = 33006,
                                    DocumentSettings = generateCustomerDocumentSettings(),
                                    Address = addresses.Skip(4).First(),
                                    //Partner = new Partner(){RowGuid = new Guid("0C3DF148-9F68-495C-91DE-0DB75F314260"),CompanyName = "Part Spedition 1 - City 5",DefaultAddress = addresses.Skip(4).First()},
                                    Partner = new Partner()
                                    {
                                        RowGuid = new Guid("21DCD904-E599-4D09-AFDC-1FD9739284A8"),
                                        CompanyName = "Spedition #1",
                                        CustomerPartners = new List<CustomerPartner>()
                                        {
                                            new CustomerPartner()
                                            {
                                                RowGuid = new Guid("B2649444-5A1D-45BE-B08E-6183EEB00327"),
                                                CompanyName = "Spedition #1",
                                                Type = PartnerType.Shipper,
                                                Address = new Address(){
                                                    City = "Gevelsberg",
                                                    Street1 = "Augsburger Straße 11",
                                                    PostalCode = "58285",
                                                    CountryId = countries.First().Id,
                                                    StateId = countries.First().States.Last().Id
                                                }
                                            }
                                        }
                                        ,DefaultAddress = new Address()
                                        {
                                            City = "Gevelsberg",
                                            Street1 = "Augsburger Straße 11",
                                            PostalCode = "58285",
                                            CountryId = countries.First().Id,
                                            StateId = countries.First().States.Skip(4).First().Id,
                                        },
                                        IsPoolingPartner = true
                                    },
                                    Divisions = new CustomerDivision[] {
                                        new CustomerDivision() {
                                            Name="Abt. 1 Spedition" ,
                                            LoadingLocations = new LoadingLocation[] {
                                                new LoadingLocation(){
                                                    Address = new Address()
                                                    {
                                                        //RefLmsAddressNumber = "Loading Address Ref 3",
                                                        Street1 = "Romstraße 1",
                                                        City = "Schweinfurt",
                                                        PostalCode = "97424",
                                                        CountryId = countries.First().Id,
                                                        StateId = countries.First().States.Skip(2).First().Id,
                                                        GeoLocation = new Point(10.2124413, 50.0196134)  { SRID = 4326 }
                                                    },
                                                    BusinessHours = GenerateBusinessHours(start: 8, end: 16, half: 11),
                                                }
                                            },
                                            PostingAccount = new PostingAccount()
                                            {
                                                DisplayName = "Account 33006-TK",
                                                RefLtmsAccountId = 6862,
                                                RefLtmsAccountNumber = "33006-TK",
                                                RefErpCustomerNumber = 33006,
                                                Address = new Address()
                                                {
                                                    //RefLmsAddressNumber = "Booking Address Ref 4",
                                                    Street1 = "Romstraße 1",
                                                    City = "Schweinfurt",
                                                    PostalCode = "97424",
                                                    CountryId = countries.First().Id,
                                                    StateId = countries.First().States.Skip(10).First().Id,
                                                    GeoLocation = new Point(10.2124413, 50.0196134)  { SRID = 4326 }
                                                },
                                                CalculatedBalance = GenerateCalculatedBalance(loadCarriers),
                                                OrderConditions = GenerateOrderConditions()
                                            },
                                            ShortName = "DS1"
                                        }
                                    }
                             }
                        }
                    }
                };
                    context.Organizations.AddRange(organizations);
                    //Set Customer Reference for PostingAccounts
                    foreach (var division in organizations.SelectMany(o => o.Customers).SelectMany(c => c.Divisions))
                    {
                        if (division.PostingAccount != null) 
                            division.PostingAccount.Customer = division.Customer;
                    }
                    context.SaveChanges();
                    #endregion

                    #region CustomerDivisionDocumentSettings

                    var divisions = context.CustomerDivisions.ToList();
                    foreach (var division in divisions)
                    {
                        division.DocumentSettings = generateCustomerDivisionDocumentSettings(division.CustomerId);
                    }
                    context.SaveChanges();

                    #endregion

                    #region Users

                    var customerDpl = organizations.First(o => o.Name.Contains("Dpl")).Customers.First();
                    var customerHandel = organizations.First(o => o.Name.Contains("Handel")).Customers.First();
                    var customerSpedition = organizations.First(o => o.Name.Contains("Spedition")).Customers.First();
                    var customerLieferant = organizations.First(o => o.Name.Contains("Lieferant")).Customers.First();
                    var customerDepot = organizations.First(o => o.Name.Contains("Depot")).Customers.First();
                    var users = new List<User>()
                    {
                        new User()
                        {
                            Customers = new List<UserCustomerRelation>(){new UserCustomerRelation(){CustomerId = customerDpl.Id}}  ,
                            OrganizationId = customerDpl.OrganizationId,
                            Upn = "demo.user@dpl-ltms.com",
                            Role = UserRole.Retailer,
                            Person = new Person()
                            {
                                Salutation = "Herr",
                                FirstName = "Demo",
                                LastName = "User",
                                Email = "demo.user@dpl-ltms.com",
                                PhoneNumber = "+49 7636 62143",
                                MobileNumber = "+49 171 862373",
                                Gender = PersonGender.Male,
                            }
                        },
                        new User()
                        {
                            Customers = new List<UserCustomerRelation>(){new UserCustomerRelation(){CustomerId = customerHandel.Id}},
                            OrganizationId =  customerHandel.OrganizationId,
                            Upn = "handel.testuser@dpl-ltms.com",
                            Role = UserRole.Retailer,
                            Person = new Person()
                            {
                                Salutation = "Herr",
                                FirstName = "Test",
                                LastName = "User",
                                Email = "handel.testuser@dpl-ltms.com",
                                PhoneNumber = "+49 7636 62143",
                                MobileNumber = "+49 171 862373",
                                Gender = PersonGender.Male,
                            }
                        },
                        new User()
                        {
                            Customers = new List<UserCustomerRelation>(){new UserCustomerRelation(){CustomerId = customerDepot.Id}},
                            OrganizationId = customerDepot.OrganizationId,
                            Upn = "depot.testuser@dpl-ltms.com",
                            Role = UserRole.Warehouse,
                            Person = new Person()
                            {
                                Salutation = "Herr",
                                FirstName = "Test",
                                LastName = "User",
                                Email = "depot.testuser@dpl-ltms.com",
                                PhoneNumber = "+49 7636 62143",
                                MobileNumber = "+49 171 862373",
                                Gender = PersonGender.Male,
                            }
                        },
                        new User()
                        {
                            Customers = new List<UserCustomerRelation>(){new UserCustomerRelation(){CustomerId = customerLieferant.Id}},
                            OrganizationId = customerLieferant.OrganizationId,
                            Upn = "lieferant.testuser@dpl-ltms.com",
                            //ToDo: add new UserRole for "Lieferant"?
                            Role = UserRole.Retailer,
                            Person = new Person()
                            {
                                Salutation = "Herr",
                                FirstName = "Test",
                                LastName = "User",
                                Email = "lieferant.testuser@dpl-ltms.com",
                                PhoneNumber = "+49 7636 62143",
                                MobileNumber = "+49 171 862373",
                                Gender = PersonGender.Male,
                            }
                        },
                        new User()
                        {
                            Customers = new List<UserCustomerRelation>(){new UserCustomerRelation(){CustomerId = customerSpedition.Id}},
                            OrganizationId = customerSpedition.OrganizationId,
                            Upn = "spediteur.testuser@dpl-ltms.com",
                            Role = UserRole.Shipper,
                            Person = new Person()
                            {
                                Salutation = "Herr",
                                FirstName = "Test",
                                LastName = "User",
                                Email = "lieferant.testuser@dpl-ltms.com",
                                PhoneNumber = "+49 7636 62143",
                                MobileNumber = "+49 171 862373",
                                Gender = PersonGender.Male,
                            }
                        },
                        new User()
                        {
                            Customers = new List<UserCustomerRelation>(){new UserCustomerRelation(){CustomerId = customerHandel.Id}},
                            OrganizationId =  customerHandel.OrganizationId,
                            Upn = "dominik.schulte@dpl-ltms.com",
                            Role = UserRole.Retailer,
                            Person = new Person()
                            {
                                Salutation = "Mr",
                                FirstName = "Domme",
                                LastName = "Nic",
                                Email = "d.schulte@dpl-pooling.com",
                                PhoneNumber = "+49 2921 7899 0",
                                Gender = PersonGender.Male
                            }
                        },
                        new User()
                        {
                            Customers = new List<UserCustomerRelation>(){new UserCustomerRelation(){CustomerId = customerHandel.Id}},
                            OrganizationId =  customerHandel.OrganizationId,
                            Upn = "kt.it@dpl-ltms.com",
                            Role = UserRole.Retailer,
                            Person = new Person()
                            {
                                Salutation = "Mr",
                                FirstName = "KT",
                                LastName = "IT",
                                Email = "info@kt-it-solutions.de",
                                PhoneNumber = "+49 5251 8775174",
                                Gender = PersonGender.Male
                            }
                        },
                        new User()
                        {
                            Customers = new List<UserCustomerRelation>(){new UserCustomerRelation(){CustomerId = customerHandel.Id}},
                            OrganizationId =  customerHandel.OrganizationId,
                            Upn = "mirko.rodloff@dpl-ltms.com",
                            Role = UserRole.Retailer,
                            Person = new Person()
                            {
                                Salutation = "Mr",
                                FirstName = "Mirko",
                                LastName = "Rodloff",
                                Email = "mirko.rodloff@dpl-ltms.com",
                                PhoneNumber = "+49 5251 8775174",
                                Gender = PersonGender.Male
                            }
                        },
                        new User()
                        {
                            Customers = new List<UserCustomerRelation>(){new UserCustomerRelation(){CustomerId = customerHandel.Id}},
                            OrganizationId =  customerHandel.OrganizationId,
                            Upn = "dirk.schmidt@dpl-ltms.com",
                            Role = UserRole.Retailer,
                            Person = new Person()
                            {
                                Salutation = "Mr",
                                FirstName = "Dirk",
                                LastName = "Schmidt",
                                Email = "d.schmidt@dpl-pooling.com",
                                PhoneNumber = "+49 2921 7899 0",
                                Gender = PersonGender.Male
                            }
                        },
                        new User()
                        {
                            Customers = new List<UserCustomerRelation>(){new UserCustomerRelation(){CustomerId = customerHandel.Id}},
                            OrganizationId =  customerHandel.OrganizationId,
                            Upn = "alexander.schlicht@dpl-ltms.com",
                            Role = UserRole.Retailer,
                            Person = new Person()
                            {
                                Salutation = "Mr",
                                FirstName = "Alexander",
                                LastName = "Schlicht    ",
                                Email = "a.schlicht@dpl-pooling.com",
                                PhoneNumber = "+49 2921 7899 0",
                                Gender = PersonGender.Male
                            }
                        },
                        new User()
                        {
                            Customers = new List<UserCustomerRelation>(){new UserCustomerRelation(){CustomerId = customerHandel.Id}},
                            OrganizationId =  customerHandel.OrganizationId,
                            Upn = "dpl.kundenbetreuung@dpl-ltms.com",
                            Role = UserRole.Retailer,
                            Person = new Person()
                            {
                                Salutation = "",
                                FirstName = "Kunden",
                                LastName = "Betreuung",
                                Email = "dpl.kundenbetreuung@dpl-ltms.com",
                                PhoneNumber = "+49 2921 7899 0",
                                Gender = PersonGender.Male
                            }
                        },
                        new User()
                        {
                            Customers = new List<UserCustomerRelation>(){new UserCustomerRelation(){CustomerId = customerHandel.Id}},
                            OrganizationId =  customerHandel.OrganizationId,
                            Upn = "handel1.testuser@dpl-ltms.com",
                            Role = UserRole.Retailer,
                            Person = new Person()
                            {
                                Salutation = "Herr",
                                FirstName = "Test",
                                LastName = "User1",
                                Email = "handel1.testuser@dpl-ltms.com",
                                PhoneNumber = "+49 7636 62143",
                                MobileNumber = "+49 171 862373",
                                Gender = PersonGender.Male,
                            }
                        },
                        new User()
                        {
                            Customers = new List<UserCustomerRelation>(){new UserCustomerRelation(){CustomerId = customerHandel.Id}},
                            OrganizationId =  customerHandel.OrganizationId,
                            Upn = "handel2.testuser@dpl-ltms.com",
                            Role = UserRole.Retailer,
                            Person = new Person()
                            {
                                Salutation = "Herr",
                                FirstName = "Test",
                                LastName = "User2",
                                Email = "handel2.testuser@dpl-ltms.com",
                                PhoneNumber = "+49 7636 62143",
                                MobileNumber = "+49 171 862373",
                                Gender = PersonGender.Male,
                            }
                        },
                        new User()
                        {
                            Customers = new List<UserCustomerRelation>(){new UserCustomerRelation(){CustomerId = customerHandel.Id}},
                            OrganizationId =  customerHandel.OrganizationId,
                            Upn = "handel3.testuser@dpl-ltms.com",
                            Role = UserRole.Retailer,
                            Person = new Person()
                            {
                                Salutation = "Herr",
                                FirstName = "Test",
                                LastName = "User3",
                                Email = "handel3.testuser@dpl-ltms.com",
                                PhoneNumber = "+49 7636 62143",
                                MobileNumber = "+49 171 862373",
                                Gender = PersonGender.Male,
                            }
                        },
                        new User()
                        {
                            Customers = new List<UserCustomerRelation>(){new UserCustomerRelation(){CustomerId = customerHandel.Id}},
                            OrganizationId =  customerHandel.OrganizationId,
                            Upn = "handel4.testuser@dpl-ltms.com",
                            Role = UserRole.Retailer,
                            Person = new Person()
                            {
                                Salutation = "Herr",
                                FirstName = "Test",
                                LastName = "User4",
                                Email = "handel4.testuser@dpl-ltms.com",
                                PhoneNumber = "+49 7636 62143",
                                MobileNumber = "+49 171 862373",
                                Gender = PersonGender.Male,
                            }
                        },
                    };

                    context.Users.AddRange(users);

                    context.SaveChanges();

                    #endregion

                    #region Partners

                    var partners = new List<Partner>()
                {
                    new Partner()
                    {
                        RowGuid = new Guid("DF3A2864-8017-4371-9C35-A905461FD219"),
                        CompanyName = "Lieferant #1",
                        CustomerPartners = new List<CustomerPartner>()
                        {
                            new CustomerPartner()
                            {
                                RowGuid = new Guid("9B62D0FC-7CC8-497E-ADDB-5E2841A5AB7B"),
                                CompanyName = "Lieferant #1",
                                Type = PartnerType.Supplier,
                                Address = new Address(){
                                    Street1 = "Mohrenstrasse 71",
                                    PostalCode = "88521",
                                    City = "Ertingen",
                                    CountryId = countries.First().Id,
                                    StateId = countries.First().States.Skip(9).First().Id,
                                }
                            }
                        }
                        ,DefaultAddress = new Address()
                        {
                            Street1 = "Mohrenstrasse 71",
                            PostalCode = "88521",
                            City = "Ertingen",
                            CountryId = countries.First().Id,
                            StateId = countries.First().States.First().Id,
                        },
                        DefaultPostingAccount = new PostingAccount()
                        {
                            DisplayName = "Lieferant #1",
                            RefLtmsAccountId = 10955,
                            RefLtmsAccountNumber = "10955-TK",
                            RefErpCustomerNumber = 48880,
                            Address =  new Address()
                            {
                                Street1 = "Mohrenstrasse 71",
                                PostalCode = "88521",
                                City = "Ertingen",
                                CountryId = countries.First().Id,
                                StateId = countries.First().States.First().Id,
                            }
                        }
                    },
                    new Partner()
                    {
                        RowGuid = new Guid("DF3A2864-9017-4371-9C35-A905461FD219"),
                        CompanyName = "Lieferant #2",
                        CustomerPartners = new List<CustomerPartner>()
                        {
                            new CustomerPartner()
                            {
                                RowGuid = new Guid("9B62D0FC-8CC8-497E-ADDB-5E2841A5AB7B"),
                                CompanyName = "Lieferant #2",
                                Type = PartnerType.Supplier,
                                Address = new Address(){
                                    City = "Waldrohrbach",
                                    Street1 = "Brandenburgische Strasse 89",
                                    PostalCode = "76857",
                                    CountryId = countries.First().Id,
                                    StateId = countries.First().States.Skip(9).First().Id,
                                }
                            }
                        }
                        ,DefaultAddress = new Address()
                        {
                            City = "Waldrohrbach",
                            Street1 = "Brandenburgische Strasse 89",
                            PostalCode = "76857",
                            CountryId = countries.First().Id,
                            StateId = countries.First().States.First().Id,
                        },
                        DefaultPostingAccount = new PostingAccount()
                        {
                            DisplayName = "Lieferant #2",
                            RefLtmsAccountId = 10956,
                            RefLtmsAccountNumber = "10956-TK",
                            RefErpCustomerNumber = 48883,
                            Address =  new Address()
                            {
                                City = "Waldrohrbach",
                                Street1 = "Brandenburgische Strasse 89",
                                PostalCode = "76857",
                                CountryId = countries.First().Id,
                                StateId = countries.First().States.First().Id,
                            }
                        }
                    },
                    new Partner()
                    {
                        RowGuid = new Guid("DF3B2864-9017-4371-9C35-A905461FD219"),
                        CompanyName = "Lieferant #3",
                        CustomerPartners = new List<CustomerPartner>()
                        {
                            new CustomerPartner()
                            {
                                RowGuid = new Guid("9B61D0FC-8CC8-497E-ADDB-5E2841A5AB7B"),
                                CompanyName = "Lieferant #3",
                                Type = PartnerType.Supplier,
                                Address = new Address(){
                                    Street1 = "Hoheluftchaussee 4",
                                    PostalCode = "04570",
                                    City = "Rötha",
                                    CountryId = countries.First().Id,
                                    StateId = countries.First().States.Skip(9).First().Id,
                                }
                            }
                        }
                        ,DefaultAddress = new Address()
                        {
                            Street1 = "Hoheluftchaussee 4",
                            PostalCode = "04570",
                            City = "Rötha",
                            CountryId = countries.First().Id,
                            StateId = countries.First().States.First().Id,
                        },
                        DefaultPostingAccount = new PostingAccount()
                        {
                            DisplayName = "Lieferant #3",
                            RefLtmsAccountId = 10958,
                            RefLtmsAccountNumber = "10958-TK",
                            RefErpCustomerNumber = 48703,
                            Address =  new Address()
                            {
                                Street1 = "Hoheluftchaussee 4",
                                PostalCode = "04570",
                                City = "Rötha",
                                CountryId = countries.First().Id,
                                StateId = countries.First().States.First().Id,
                            }
                        }
                    },
                    new Partner()
                    {
                        RowGuid = new Guid("DF3B1864-9017-4371-9C35-A905461FD219"),
                        CompanyName = "Lieferant #4",
                        CustomerPartners = new List<CustomerPartner>()
                        {
                            new CustomerPartner()
                            {
                                RowGuid = new Guid("8B61D0FC-8CC8-497E-ADDB-5E2841A5AB7B"),
                                CompanyName = "Lieferant #4",
                                Type = PartnerType.Supplier,
                                Address = new Address(){
                                    Street1 = "Hoheluftchaussee 87",
                                    PostalCode = "06688",
                                    City = "Großkorbetha",
                                    CountryId = countries.First().Id,
                                    StateId = countries.First().States.Skip(9).First().Id,
                                }
                            }
                        }
                        ,DefaultAddress = new Address()
                        {
                            Street1 = "Hoheluftchaussee 87",
                            PostalCode = "06688",
                            City = "Großkorbetha",
                            CountryId = countries.First().Id,
                            StateId = countries.First().States.First().Id,
                        },
                        DefaultPostingAccount = new PostingAccount()
                        {
                            DisplayName = "Lieferant #4",
                            RefLtmsAccountId = 10960,
                            RefLtmsAccountNumber = "10960-TK",
                            RefErpCustomerNumber = 47259,
                            Address =  new Address()
                            {
                                Street1 = "Hoheluftchaussee 87",
                                PostalCode = "06688",
                                City = "Großkorbetha",
                                CountryId = countries.First().Id,
                                StateId = countries.First().States.First().Id,
                            }
                        }
                    },
                    new Partner()
                    {
                        RowGuid = new Guid("DF3B1864-9017-4371-9C35-A915461FD219"),
                        CompanyName = "Lieferant #5",
                        CustomerPartners = new List<CustomerPartner>()
                        {
                            new CustomerPartner()
                            {
                                RowGuid = new Guid("8B61D0FC-8CC8-497E-ADDB-5E1841A5AB7B"),
                                CompanyName = "Lieferant #5",
                                Type = PartnerType.Supplier,
                                Address = new Address(){
                                    Street1 = "Schönhauser Allee 9",
                                    PostalCode = "79695",
                                    City = "Wieden",
                                    CountryId = countries.First().Id,
                                    StateId = countries.First().States.Skip(9).First().Id,
                                }
                            }
                        }
                        ,DefaultAddress = new Address()
                        {
                            Street1 = "Schönhauser Allee 9",
                            PostalCode = "79695",
                            City = "Wieden",
                            CountryId = countries.First().Id,
                            StateId = countries.First().States.First().Id,
                        },
                        DefaultPostingAccount = new PostingAccount()
                        {
                            DisplayName = "Lieferant #5",
                            RefLtmsAccountId = 10961,
                            RefLtmsAccountNumber = "10961",
                            RefErpCustomerNumber = 48885,
                            Address =  new Address()
                            {
                                Street1 = "Schönhauser Allee 9",
                                PostalCode = "79695",
                                City = "Wieden",
                                CountryId = countries.First().Id,
                                StateId = countries.First().States.First().Id,
                            }
                        }
                    },

            //         new Partner()
            //         {
            //             RowGuid = new Guid("21DCD904-E599-4D09-AFDC-1FD9739284A8"),
            //             CompanyName = "Spedition #1",
            //             CustomerPartners = new List<CustomerPartner>()
            //             {
            //                 new CustomerPartner()
            //                 {
            //                     RowGuid = new Guid("B2649444-5A1D-45BE-B08E-6183EEB00327"),
            //                     CompanyName = "Spedition #1",
            //                     Type = PartnerType.Shipper,
            //                     Address = new Address(){
            //                         City = "Gevelsberg",
            //                         Street1 = "Augsburger Straße 11",
            //                         PostalCode = "58285",
            //                         CountryId = countries.First().Id,
            //                         StateId = countries.First().States.Last().Id
            //                     }
            //                 }
            //
            //             }
            //             ,DefaultAddress = new Address()
            //             {
            //                 City = "Gevelsberg",
            //                 Street1 = "Augsburger Straße 11",
            //                 PostalCode = "58285",
            //                 CountryId = countries.First().Id,
            //                 StateId = countries.First().States.Skip(4).First().Id,
            //             },
            //             DefaultPostingAccount = organizations.Find(i => i.Name.Contains("Spedition")).Customers.First().Divisions.First().PostingAccount
            // },
                    new Partner()
                    {
                        RowGuid = new Guid("21DCD903-E599-4D09-AFDC-1FD9739284A8"),
                        CompanyName = "Spedition #2",
                        CustomerPartners = new List<CustomerPartner>()
                        {
                            new CustomerPartner()
                            {
                                RowGuid = new Guid("B2649443-5A1D-45BE-B08E-6183EEB00327"),
                                CompanyName = "Spedition #2",
                                Type = PartnerType.Shipper,
                                Address = new Address(){
                                    Street1 = "Schillerstrasse 88",
                                    PostalCode = "86568",
                                    City = "Hollenbach",
                                    CountryId = countries.First().Id,StateId = countries.First().States.Last().Id }
                            }

                        }
                        ,DefaultAddress = new Address()
                        {
                            Street1 = "Schillerstrasse 88",
                            PostalCode = "86568",
                            City = "Hollenbach",
                            CountryId = countries.First().Id,
                            StateId = countries.First().States.Skip(4).First().Id,
                        },
                        DefaultPostingAccount = new PostingAccount()
                        {
                            DisplayName = "Spedition #2",
                            RefLtmsAccountId = 11020 ,
                            RefErpCustomerNumber = 50054,
                            Address = new Address()
                            {
                                Street1 = "Schillerstrasse 88",
                                PostalCode = "86568",
                                City = "Hollenbach",
                                CountryId = countries.First().Id,
                                StateId = countries.First().States.Skip(4).First().Id,
                            }
                        }
                    },
                    new Partner()
                    {
                        RowGuid = new Guid("21DBD903-E599-4D09-AFDC-1FD9739284A8"),
                        CompanyName = "Spedition #3",
                        CustomerPartners = new List<CustomerPartner>()
                        {
                            new CustomerPartner()
                            {
                                RowGuid = new Guid("B2639443-5A1D-45BE-B08E-6183EEB00327"),
                                CompanyName = "Spedition #3",
                                Type = PartnerType.Shipper,
                                Address = new Address(){
                                    Street1 = "Billwerder Neuer Deich 83",
                                    PostalCode = "96334",
                                    City = "Ludwigsstadt",
                                    CountryId = countries.First().Id,StateId = countries.First().States.Last().Id }
                            }

                        }
                        ,DefaultAddress = new Address()
                        {
                            Street1 = "Billwerder Neuer Deich 83",
                            PostalCode = "96334",
                            City = "Ludwigsst",
                            CountryId = countries.First().Id,
                            StateId = countries.First().States.Skip(4).First().Id,
                        },
                        DefaultPostingAccount = new PostingAccount()
                        {
                            DisplayName = "Spedition #3",
                            RefLtmsAccountId = 11026 ,
                            RefErpCustomerNumber = 48828,
                            Address = new Address()
                            {
                                Street1 = "Billwerder Neuer Deich 83",
                                PostalCode = "96334",
                                City = "Ludwigsst",
                                CountryId = countries.First().Id,
                                StateId = countries.First().States.Skip(4).First().Id,
                            }
                        }
                    },
                    new Partner()
                    {
                        RowGuid = new Guid("21CBD903-E599-4D09-AFDC-1FD9739284A8"),
                        CompanyName = "Spedition #4",
                        CustomerPartners = new List<CustomerPartner>()
                        {
                            new CustomerPartner()
                            {
                                RowGuid = new Guid("B2539443-5A1D-45BE-B08E-6183EEB00327"),
                                CompanyName = "Spedition #4",
                                Type = PartnerType.Shipper,
                                Address = new Address(){
                                    Street1 = "Landhausstraße 19",
                                    PostalCode = "14632",
                                    City = "Nauen",
                                    CountryId = countries.First().Id,StateId = countries.First().States.Last().Id }
                            }

                        }
                        ,DefaultAddress = new Address()
                        {
                            Street1 = "Landhausstraße 19",
                            PostalCode = "14632",
                            City = "Nauen",
                            CountryId = countries.First().Id,
                            StateId = countries.First().States.Skip(4).First().Id,
                        },
                        DefaultPostingAccount = new PostingAccount()
                        {
                            DisplayName = "Spedition #4",
                            RefLtmsAccountId = 11031 ,
                            RefErpCustomerNumber = 48950,
                            Address = new Address()
                            {
                                Street1 = "Landhausstraße 19",
                                PostalCode = "14632",
                                City = "Nauen",
                                CountryId = countries.First().Id,
                                StateId = countries.First().States.Skip(4).First().Id,
                            }
                        }
                    },
                    new Partner()
                    {
                        RowGuid = new Guid("21CBD903-E599-4D09-AFDC-1FD9739284A7"),
                        CompanyName = "Spedition #5",
                        CustomerPartners = new List<CustomerPartner>()
                        {
                            new CustomerPartner()
                            {
                                RowGuid = new Guid("B2539443-5A1D-45BE-B08E-6183EEB00326"),
                                CompanyName = "Spedition #5",
                                Type = PartnerType.Shipper,
                                Address = new Address(){
                                    Street1 = "Pasewalker Straße 52",
                                    PostalCode = "75175",
                                    City = "Pforzheim Innenstadt",
                                    CountryId = countries.First().Id,StateId = countries.First().States.Last().Id }
                            }

                        }
                        ,DefaultAddress = new Address()
                        {
                            Street1 = "Pasewalker Straße 52",
                            PostalCode = "75175",
                            City = "Pforzheim Innenstadt",
                            CountryId = countries.First().Id,
                            StateId = countries.First().States.Skip(4).First().Id,
                        },
                        DefaultPostingAccount = new PostingAccount()
                        {
                            DisplayName = "Spedition #5",
                            RefLtmsAccountId = 11033 ,
                            RefErpCustomerNumber = 48872,
                            Address = new Address()
                            {
                                Street1 = "Pasewalker Straße 52",
                                PostalCode = "75175",
                                City = "Pforzheim Innenstadt",
                                CountryId = countries.First().Id,
                                StateId = countries.First().States.Skip(4).First().Id,
                            }
                        }
                    },

                    new Partner()
                    {
                        RowGuid = new Guid("B380410A-0800-4573-A411-28EEA5A5743B"),
                        CompanyName = "ACME",
                        CustomerPartners = new List<CustomerPartner>()
                        {
                            new CustomerPartner()
                            {
                                RowGuid = new Guid("8BB7FAA9-E95E-443B-BAA3-81A3B25798FD"),
                                CompanyName = "ACME Ldt.",
                                Type = PartnerType.Supplier
                            }

                        },
                        DefaultAddress = new Address()
                        {
                            City = "Bristol",
                            Street1 = "B1",
                            PostalCode = "BS",
                            CountryId = countries.First().Id,
                            StateId = countries.First().States.Skip(8).First().Id,
                        }
                    },
                    new Partner()
                    {
                        RowGuid = new Guid("B9A62ABC-38DD-443C-89F5-446B2B3F9274"),
                        CompanyName = "Supplier 1",
                        CustomerPartners = new List<CustomerPartner>()
                        {
                            new CustomerPartner()
                            {
                                RowGuid = new Guid("57999A1C-BA28-41ED-9979-D2F115188793"),
                                CompanyName = "Supplier 1 Ldt.",
                                Type = PartnerType.Supplier
                            }

                        },
                        DefaultAddress = new Address()
                        {
                            City = "Bristol",
                            Street1 = "Sup. Street 4",
                            PostalCode = "BS",
                            CountryId = countries.First().Id,
                            StateId = countries.First().States.Skip(7).First().Id,
                        }
                    },
                    new Partner()
                    {
                        RowGuid = new Guid("984D8DA3-51A2-44BA-A3F6-4D34B368816E"),
                        CompanyName = "Shipper 1",
                        CustomerPartners = new List<CustomerPartner>()
                        {
                            new CustomerPartner()
                            {
                                RowGuid = new Guid("674E1759-5F91-4ADA-9904-4F327923EBDB"),
                                CompanyName = "Spedition 1 GmbH",
                                Type = PartnerType.Shipper
                            }
                        },
                        DefaultAddress = new Address()
                        {
                            City = "München",
                            Street1 = "B1",
                            PostalCode = "BS",
                            CountryId = countries.First().Id,
                            StateId = countries.First().States.First().Id,
                        }
                    },
                    new Partner()
                    {
                        RowGuid = new Guid("CB376499-7764-4151-AB37-3473813159B2"),
                        CompanyName = "Sub Shipper 1",
                        CustomerPartners = new List<CustomerPartner>()
                        {
                            new CustomerPartner()
                            {
                                RowGuid = new Guid("523025F9-242C-4B80-AD7F-AFCA7D00DE05"),
                                CompanyName = "Sub Spedition 1 GmbH",
                                Type = PartnerType.SubShipper
                            }

                        },
                        DefaultAddress = new Address()
                        {
                            City = "Soest",
                            Street1 = "B1",
                            PostalCode = "59494",
                            CountryId = countries.First().Id,
                            StateId = countries.First().States.Skip(1).First().Id,
                        }
                    },
                    new Partner()
                    {
                        RowGuid = new Guid("4A31A3B0-22A2-4E9C-997B-625B18B76187"),
                        CompanyName = "Partner #5 GmbH",
                        CustomerPartners = new List<CustomerPartner>()
                        {
                            new CustomerPartner()
                            {
                                RowGuid = new Guid("DC79333F-D68C-4568-A1F4-E2AA1F449B0E"),
                                CompanyName = "Partner #5 GmbH",
                                Type = PartnerType.Supplier
                            }

                        },
                        DefaultAddress = new Address()
                        {
                            City = "Soest",
                            Street1 = "Rigaring",
                            PostalCode = "59494",
                            CountryId = countries.First().Id,
                            StateId = countries.First().States.Skip(1).First().Id,
                        }
                    }
                };

                    context.Partners.AddRange(partners);
                    var partnerShipper = context.Partners.FirstOrDefault(p => p.Customer.Name.Contains("Spedition 1"));
                    partnerShipper.DefaultPostingAccount = organizations.Find(i => i.Name.Contains("Spedition"))
                        .Customers.First().Divisions.First().PostingAccount;
                    context.SaveChanges();

                    #endregion

                    #region PartnerDirectories

                    var partnerDirectories = new List<PartnerDirectory>();
                    foreach (var organization in organizations)
                    {
                        partnerDirectories.Add(
                            new PartnerDirectory
                            {
                                Name = "Standard",
                                Type = PartnerDirectoryType.CustomerPartner,
                                OrganizationPartnerDirectories = new List<OrganizationPartnerDirectory>
                                {
                                new OrganizationPartnerDirectory
                                {
                                    OrganizationId = organization.Id
                                }
                                }
                            }
                            );

                    }

                    context.PartnerDirectories.AddRange(partnerDirectories);
                    context.SaveChanges();

                    #endregion

                    #region Permissions

                    var organizationUserGroups = organizations.SelectMany(organization => GenerateSystemUserGroupsForOrganization(organization.Id))
                        .ToList();

                    var customerUserGroups = organizations.SelectMany(organization =>
                        organization.Customers.SelectMany(customer => GenerateSystemUserGroupsForCustomer(customer.Id)))
                        .ToList();

                    var divisionUserGroups = organizations.SelectMany(organization =>
                        organization.Customers.SelectMany(customer =>
                            customer.Divisions.SelectMany(division => GenerateSystemUserGroupsForDivision(division.Id))))
                        .ToList();

                    var allUserGroups = organizationUserGroups
                        .Concat(customerUserGroups)
                        .Concat(divisionUserGroups).ToList();

                    context.UserGroups.AddRange(allUserGroups);
                    context.SaveChanges();

                    Func<User, IEnumerable<UserUserGroupRelation>> generateUserGroupRelationsForUser = (user) =>
                    {
                        var organization = organizations.Single(i => i.Id == user.Customers.First().Customer.OrganizationId);
                        var orgUserGroups = organizationUserGroups
                            .Cast<OrganizationUserGroup>()
                            .Where(i => i.OrganizationId == organization.Id)
                            .Cast<UserGroup>();

                        var custUserGroups = customerUserGroups
                            .Cast<CustomerUserGroup>()
                            .Where(i => organization.Customers.Select(i => i.Id).Contains(i.CustomerId))
                            .Cast<UserGroup>();

                        var divUserGroups = divisionUserGroups
                            .Cast<CustomerDivisionUserGroup>()
                            .Where(i => organization.Customers.SelectMany(i => i.Divisions).Select(i => i.Id).Contains(i.CustomerDivisionId))
                            .Cast<UserGroup>();

                        var allUserGroups = orgUserGroups
                            .Concat(custUserGroups)
                            .Concat(divUserGroups);

                        return allUserGroups.Select(group => new UserUserGroupRelation()
                        {
                            UserGroupId = group.Id,
                            UserId = user.Id
                        });
                    };

                    var allUserGroupRelations = users.SelectMany(user =>
                    {
                        return generateUserGroupRelationsForUser(user);
                    });

                    context.AddRange(allUserGroupRelations);
                    context.SaveChanges();

                    #endregion

                    #region CustomerPartnerDirectoryAccesses

                    context.CustomerPartnerDirectoryAccesses.AddRange(new List<CustomerPartnerDirectoryAccess>
                {
                    new CustomerPartnerDirectoryAccess
                    {
                        DirectoryId = partnerDirectories.First().Id,
                        CustomerPartnerId = partnerShipper.CustomerPartners.First().Id
                    },
                    new CustomerPartnerDirectoryAccess
                    {
                        DirectoryId = partnerDirectories.First().Id,
                        CustomerPartnerId = partners.First().CustomerPartners.First().Id,
                    },
                    new CustomerPartnerDirectoryAccess
                    {
                        DirectoryId = partnerDirectories.First().Id,
                        CustomerPartnerId = partners.Skip(1).First().CustomerPartners.First().Id,
                    },
                    new CustomerPartnerDirectoryAccess
                    {
                        DirectoryId = partnerDirectories.First().Id,
                        CustomerPartnerId = partners.Skip(2).First().CustomerPartners.First().Id,
                    },
                    new CustomerPartnerDirectoryAccess
                    {
                        DirectoryId = partnerDirectories.First().Id,
                        CustomerPartnerId = partners.Skip(3).First().CustomerPartners.First().Id,
                    },
                    new CustomerPartnerDirectoryAccess
                    {
                        DirectoryId = partnerDirectories.First().Id,
                        CustomerPartnerId = partners.Skip(4).First().CustomerPartners.First().Id,
                    },
                    new CustomerPartnerDirectoryAccess
                    {
                        DirectoryId = partnerDirectories.First().Id,
                        CustomerPartnerId = partners.Skip(5).First().CustomerPartners.First().Id,
                    },
                    new CustomerPartnerDirectoryAccess
                    {
                        DirectoryId = partnerDirectories.First().Id,
                        CustomerPartnerId = partners.Skip(6).First().CustomerPartners.First().Id,
                    },
                    new CustomerPartnerDirectoryAccess
                    {
                        DirectoryId = partnerDirectories.First().Id,
                        CustomerPartnerId = partners.Skip(7).First().CustomerPartners.First().Id,
                    },
                    new CustomerPartnerDirectoryAccess
                    {
                        DirectoryId = partnerDirectories.First().Id,
                        CustomerPartnerId = partners.Skip(8).First().CustomerPartners.First().Id,
                    },
                    new CustomerPartnerDirectoryAccess
                    {
                        DirectoryId = partnerDirectories.First().Id,
                        CustomerPartnerId = partners.Skip(9).First().CustomerPartners.First().Id,
                    }
                });
                    context.SaveChanges();

                    #endregion

                    #region ExpressCode

                    var expressCodes = new List<ExpressCode>() {
                        new ExpressCode()
                        {
                            DigitalCode = "CODEAA",
                            CustomBookingText = "Anlieferung/Abholungen KT-IT",
                            ValidFrom = DateTime.Today.AddDays(-1),
                            IssuingCustomerId = organizations.First().Customers.First().Id,
                            IssuingDivisionId = organizations.First().Customers.First().Divisions.First().Id,
                            VoucherReasonTypeId = 1
                            //Partner is set later in Partner Region
                        },
                        new ExpressCode()
                        {
                            DigitalCode = "CODEBB",
                            CustomBookingText = "I am canceled",
                            ValidFrom = DateTime.Today.AddDays(-1),
                            ValidTo = DateTime.Today.AddMonths(1),
                            IsCanceled = true,
                            IssuingCustomerId = organizations.First().Customers.First().Id,
                            IssuingDivisionId = organizations.First().Customers.First().Divisions.First().Id,
                            VoucherReasonTypeId = 1
                            //Partner is set later in Partner Region
                        },
                        new ExpressCode()
                        {
                            DigitalCode = "CODECC",
                            CustomBookingText = "Preset Test",
                            ValidFrom = DateTime.Today.AddDays(-1),
                            ValidTo = DateTime.Today.AddMonths(1),
                            IsCanceled = false,
                            IssuingCustomerId = organizations.First().Customers.First().Id,
                            IssuingDivisionId = organizations.First().Customers.First().Divisions.First().Id,
                            VoucherReasonTypeId = 1,
                            VoucherType = VoucherType.Direct,
                            PartnerPresets = new List<PartnerPreset>()
                            {
                                new PartnerPreset()
                                {
                                    Type = ExpressCodePresetType.Recipient,
                                    PartnerId = partners.First().Id,
                                },
                                new PartnerPreset()
                                {
                                    Type = ExpressCodePresetType.Supplier,
                                    PartnerId = partners.Skip(1).First().Id,
                                }
                            }
                        },
                        new ExpressCode()
                        {
                            DigitalCode = "CODEDD",
                            CustomBookingText = "PostingAccount Preset Test",
                            ValidFrom = DateTime.Today.AddDays(-1),
                            ValidTo = DateTime.Today.AddMonths(1),
                            IsCanceled = false,
                            IssuingCustomerId = organizations.First().Customers.First().Id,
                            IssuingDivisionId = organizations.First().Customers.First().Divisions.First().Id,
                            VoucherReasonTypeId = 1,
                            PostingAccountPreset = new PostingAccountPreset()
                            {
                                DestinationAccountId = 6,
                                LoadCarrierQuantity = 495,
                                LoadCarrierId = 203
                            }
                        },
                        new ExpressCode()
                        {
                            DigitalCode = "CODEEE",
                            CustomBookingText = "PostingAccount Preset Test without DestinationAccount",
                            ValidFrom = DateTime.Today.AddDays(-1),
                            ValidTo = DateTime.Today.AddMonths(1),
                            IsCanceled = false,
                            IssuingCustomerId = organizations.First().Customers.First().Id,
                            IssuingDivisionId = organizations.First().Customers.First().Divisions.First().Id,
                            VoucherReasonTypeId = 1,
                            PostingAccountPreset = new PostingAccountPreset()
                            {
                                LoadCarrierQuantity = 495,
                                LoadCarrierId = 203
                            }
                        }
                    };
                    context.ExpressCodes.AddRange(expressCodes);
                    context.SaveChanges();
                    #endregion

                    context.Database.CommitTransaction();
                }
            }
        }

        public static BusinessHour[] GenerateBusinessHours(int start = 8, int end = 16, int half = 11)
        {
            return new BusinessHour[] {
                new BusinessHour()
                {
                    DayOfWeek = DayOfWeek.Monday,
                    FromTime = new DateTime(1,1,1,start,0,0),
                    ToTime = new DateTime(1,1,1,end,0,0)
                },
                new BusinessHour()
                {
                    DayOfWeek = DayOfWeek.Tuesday,
                    FromTime = new DateTime(1,1,1,start,0,0),
                    ToTime = new DateTime(1,1,1,end,0,0)
                },
                new BusinessHour()
                {
                    DayOfWeek = DayOfWeek.Wednesday,
                    FromTime = new DateTime(1,1,1,start,0,0),
                    ToTime = new DateTime(1,1,1,half,0,0)
                },
                new BusinessHour()
                {
                    DayOfWeek = DayOfWeek.Wednesday,
                    FromTime = new DateTime(1,1,1,half +1,0,0),
                    ToTime = new DateTime(1,1,1,end,0,0)
                },
                new BusinessHour()
                {
                    DayOfWeek = DayOfWeek.Thursday,
                    FromTime = new DateTime(1,1,1,start,0,0),
                    ToTime = new DateTime(1,1,1,end,0,0)
                },
                new BusinessHour()
                {
                    DayOfWeek = DayOfWeek.Friday,
                    FromTime = new DateTime(1,1,1,start,0,0),
                    ToTime = new DateTime(1,1,1,end,0,0)
                },
                new BusinessHour()
                {
                    DayOfWeek = DayOfWeek.Saturday,
                    FromTime = new DateTime(1,1,1,start,0,0),
                    ToTime = new DateTime(1,1,1,half,0,0)
                }
            };
        }

        private static List<LoadCarrierCondition> generateLoadCarrierConditions()
        {
            return new List<LoadCarrierCondition> {
                // EUR B
                new LoadCarrierCondition
                {
                    LoadCarrierId = 2,
                    MinQuantity = 100,
                    MaxQuantity = 99999
                },
                // DD 2B
                new LoadCarrierCondition{
                    LoadCarrierId = 103,
                    MinQuantity = 100,
                    MaxQuantity = 99999
                },
                // EUR 2B
                new LoadCarrierCondition{
                    LoadCarrierId = 203,
                    MinQuantity = 100,
                    MaxQuantity = 99999
                },
                // EPAL
                new LoadCarrierCondition{
                    LoadCarrierId = 151,
                    MinQuantity = 100,
                    MaxQuantity = 99999
                },
            };
        }

        private static IEnumerable<UserGroup> GenerateSystemUserGroupsForOrganization(int organizationId)
        {
            var resourceType = PermissionResourceType.Organization;
            return new List<UserGroup>()
            {
                new OrganizationUserGroup() {
                    Name = "Admins",
                    IsSystemGroup = true,
                    OrganizationId = organizationId,
                    Permissions = GeneratePermissions(resourceType, organizationId, Common.Enumerations.ResourceActions.Organization.Admin)
                },
                new OrganizationUserGroup() {
                    Name = "Standard Users",
                    IsSystemGroup = true,
                    OrganizationId = organizationId,
                    Permissions = GeneratePermissions(resourceType, organizationId, Common.Enumerations.ResourceActions.Organization.Standard)
                },
            };
        }

        private static IEnumerable<UserGroup> GenerateSystemUserGroupsForCustomer(int customerId)
        {
            var resourceType = PermissionResourceType.Customer;
            return new List<UserGroup>()
            {
                new CustomerUserGroup() {
                    Name = "Admins",
                    IsSystemGroup = true,
                    CustomerId = customerId,
                    Permissions = GeneratePermissions(resourceType, customerId, Common.Enumerations.ResourceActions.Customer.Admin)
                },
                new CustomerUserGroup() {
                    Name = "Standard Users",
                    IsSystemGroup = true,
                    CustomerId = customerId,
                    Permissions = GeneratePermissions(resourceType, customerId, Common.Enumerations.ResourceActions.Customer.Standard)
                },
            };
        }

        private static IEnumerable<UserGroup> GenerateSystemUserGroupsForDivision(int divisionId)
        {
            var resourceType = PermissionResourceType.Division;
            return new List<UserGroup>()
            {
                new CustomerDivisionUserGroup() {
                    Name = "Admins",
                    IsSystemGroup = true,
                    CustomerDivisionId = divisionId,
                    Permissions = GeneratePermissions(resourceType, divisionId, Common.Enumerations.ResourceActions.Division.Admin)
                },
                new CustomerDivisionUserGroup() {
                    Name = "Standard Users",
                    IsSystemGroup = true,
                    CustomerDivisionId = divisionId,
                    Permissions = GeneratePermissions(resourceType, divisionId, Common.Enumerations.ResourceActions.Division.Standard)
                },
            };
        }

        public static ICollection<Permission> GeneratePermissions<TActionEnum>(PermissionResourceType resource, int referenceId, IEnumerable<TActionEnum> actionEnumValues)
            where TActionEnum : Enum
        {
            Func<Enum, string> GetName = (Enum value) => Enum.GetName(value.GetType(), value);
            var enumValueNames = actionEnumValues.Select(enumValue => GetName(enumValue));
            return enumValueNames.Select(action => new Permission()
            {
                Type = PermissionType.Allow,
                Scope = PermissionScope.Group,
                Action = action,
                Resource = resource,
                ReferenceId = referenceId
            }).ToList();
        }

        public static CalculatedBalance GenerateCalculatedBalance(IList<LoadCarrier> loadCarriers, int[] loadCarrierIds = null)
        {
            var random = new Random();

            // default load carriers EUR B, DD 2B, EUR 2B, EPAL, E2KR I
            loadCarrierIds = loadCarrierIds != null ? loadCarrierIds : new[] { 2, 103, 203, 151, 702 };

            return new CalculatedBalance()
            {
                // TODO Add back once LoadCarrier Type / Quality to LTMS mapping is done
                LastBookingDateTime = DateTime.UtcNow.AddYears(-10),
                //LastBookingDateTime = DateTime.UtcNow,
                //LastPostingRequestDateTime = DateTime.UtcNow,

                Positions = loadCarriers.Where(i => loadCarrierIds.Contains(i.Id))
                    .Select(loadCarrier =>
                    {
                        return new CalculatedBalancePosition()
                        {
                            LoadCarrierId = loadCarrier.Id,
                            //PostingRequestBalanceCharge = random.Next(randomMin, 1000),
                            //PostingRequestBalanceCredit = random.Next(randomMin, 1000),
                            //CoordinatedBalance = random.Next(randomMin, 1000),
                            //ProvisionalBalance = 10000,
                            //ProvisionalCharge = random.Next(randomMin, 1000),
                            //ProvisionalCredit = 10000,
                            CoordinatedBalance = 10000,
                            //InCoordinationCharge = random.Next(randomMin, 1000),
                            //InCoordinationCredit = random.Next(randomMin, 1000),
                            //UncoordinatedCharge = random.Next(randomMin, 1000),
                            //UncoordinatedCredit = random.Next(randomMin, 1000),

                        };
                    }).ToArray()
            };
        }

        private static OrderGroup GenerateOrderGroup(
            OrderType type,
            CustomerDivision division,
            LoadCarrier loadCarrier,
            int quantity,
            int daysFromToday,
            OrderStatus status = OrderStatus.Confirmed,
            int numberOfStacks = 33,
            int stackHeightMin = 15,
            int stackHeightMax = 17,
            int loadingLocationNumber = 1,
            int daysAfterEarliest = 5,
            OrderTransportType transportType = OrderTransportType.ProvidedByOthers,
            OrderQuantityType quantityType = OrderQuantityType.Load,
            LoadCarrier baseLoadCarrier = null
        )
        {
            var quantityPerEur = loadCarrier.Type.QuantityPerEur;

            Func<int> getCurrentLoadCarrierQuantity = () =>
            {
                switch (quantityType)
                {
                    case OrderQuantityType.Load:
                        return numberOfStacks * stackHeightMax * quantityPerEur * quantity;
                    case OrderQuantityType.Stacks:
                        return numberOfStacks * stackHeightMax * quantityPerEur * quantity;
                    case OrderQuantityType.LoadCarrierQuantity:
                        return quantity;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            };

            var orderGroup = new OrderGroup()
            {
                Orders = new List<Order>()
            };

            for (int i = 0; i < quantity; i++)
            {
                var order = new Order()
                {
                    RefLmsOrderRowGuid = Guid.NewGuid(),
                    Type = type,
                    LoadCarrierId = loadCarrier.Id,
                    QuantityType = quantityType,
                    LoadCarrierQuantity = quantityType == OrderQuantityType.LoadCarrierQuantity ? quantity : 0,
                    DivisionId = division.Id,
                    LoadingLocationId = division.LoadingLocations.Skip(loadingLocationNumber - 1).First().Id,
                    PostingAccountId = division.PostingAccount.Id,
                    EarliestFulfillmentDateTime = DateTime.UtcNow.AddDays(daysFromToday),
                    LatestFulfillmentDateTime = DateTime.UtcNow.AddDays(daysFromToday + daysAfterEarliest),
                    StackHeightMax = stackHeightMax,
                    StackHeightMin = stackHeightMin,
                    SupportsJumboVehicles = false,
                    SupportsRearLoading = true,
                    SupportsSideLoading = true,
                    TransportType = transportType,
                    BaseLoadCarrierId = baseLoadCarrier?.Id,
                };
                orderGroup.Orders.Add(order);
            }

            return orderGroup;

        }

        private static List<OrderCondition> GenerateOrderConditions()
        {
            var orderConditions = new List<OrderCondition>();

            orderConditions.Add(new OrderCondition()
            {
                OrderType = OrderType.Demand,
                LoadCarrierConditions = generateLoadCarrierConditions()
            });

            orderConditions.Add(new OrderCondition()
            {
                OrderType = OrderType.Supply,
                LatestDeliveryDayOfWeek = DayOfWeek.Wednesday,
                LoadCarrierConditions = generateLoadCarrierConditions()
            });

            return orderConditions;
        }

        public static DateTime GetNextWeekday(DateTime start, DayOfWeek day)
        {
            return start.AddDays(GetDaysToNextWeekday(start, day));
        }

        public static int GetDaysToNextWeekday(DateTime start, DayOfWeek day)
        {
            // The (... + 7) % 7 ensures we end up with a value in the range [0, 6]
            return ((int)day - (int)start.DayOfWeek + 7) % 7;
        }

        private static void UpdateCreatedAtTime(IEnumerable<IAuditable> items)
        {
            var random = new Random();
            var today = DateTime.UtcNow.Date;
            var startTime = 6 * 60; // represents 6:00
            var endTime = 18 * 60; // represents 18:00

            foreach (var item in items)
            {
                var daysInThePast = random.Next(1, 14) * -1;

                // get a time between 6:00 and 18:00 in minutes
                var minutes = random.Next(startTime, endTime);

                item.CreatedAt = today.AddDays(daysInThePast).AddMinutes(minutes);
            }
        }

        public static void GenerateLmsCustomerLoadingLocations(OlmaDbContext context)
        {
            Console.WriteLine($"Begin - Inserting LmsCustomers");

            var addresses = Helper.ReadCsv<Address, AddressCsvMap>("Seed\\LmsCustomers\\addresses.csv", gzip: false);
            var loadingLocations = Helper.ReadCsv<LoadingLocation, LoadingLocationCsvMap>("Seed\\LmsCustomers\\loading-locations.csv", gzip: false);
            //var businessHours = Helper.ReadCsv<BusinessHour, BusinessHourCsvMap>("Seed\\LmsCustomers\\business-hours.csv", gzip: false);

            var businessHours = loadingLocations.SelectMany(loadingLocation => new[] { 1, 2, 3, 4, 5, 6 }.Select(day => new BusinessHour()
            {
                Id = (loadingLocation.Id - 1) * 6 + day,
                LoadingLocationId = loadingLocation.Id,
                DayOfWeek = (DayOfWeek)day,
                FromTime = new DateTime(1, 1, 1, 8, 0, 0),
                ToTime = new DateTime(1, 1, 1, 18, 0, 0),
            })).ToArray();

            context.Database.ExecuteSqlRaw($"SET IDENTITY_INSERT [dbo].[Addresses] ON ");
            context.Addresses.AddRange(addresses);
            context.SaveChanges();
            context.Database.ExecuteSqlRaw($"SET IDENTITY_INSERT [dbo].[Addresses] OFF ");
            Console.WriteLine($"Addresses - Done");

            context.Database.ExecuteSqlRaw($"SET IDENTITY_INSERT [dbo].[LoadingLocations] ON ");
            context.LoadingLocations.AddRange(loadingLocations);
            context.SaveChanges();
            context.Database.ExecuteSqlRaw($"SET IDENTITY_INSERT [dbo].[LoadingLocations] OFF ");
            Console.WriteLine($"LoadingLocations - Done");

            context.Database.ExecuteSqlRaw($"SET IDENTITY_INSERT [dbo].[BusinessHours] ON ");
            context.BusinessHours.AddRange(businessHours);
            context.SaveChanges();
            context.Database.ExecuteSqlRaw($"SET IDENTITY_INSERT [dbo].[BusinessHours] OFF ");
            Console.WriteLine($"BusinessHours - Done");

            Console.WriteLine($"Completed - Inserting LmsCustomers");
        }

        public static void UpdateLmsAvailabilityAndDeliveryLoadingLocatrionIds(string connectionString)
        {
            Console.WriteLine("Begin - Update LoadingLocationIds in Lms Availability / Delivery");

            var sql = Helper.ReadTextFromFileUtf8("Seed\\lms-update-loading-location-ids.sql");
            Helper.ExecuteSql(connectionString, sql);

            Console.WriteLine("Completed - Updating LoadingLocationIds in Lms Availability / Delivery");
        }

        internal class AddressCsvMap : CsvHelper.Configuration.ClassMap<Address>, ITypeConverter
        {
            //NetTopologySuite.IO.SqlServerBytesReader reader;
            public AddressCsvMap()
            {
                //reader = new NetTopologySuite.IO.SqlServerBytesReader();
                //AutoMap();
                //Id, RefLmsAddressNumber, Street1, PostalCode, City, StateId, CountryId, GeoLocation
                Map(m => m.Id);
                Map(m => m.RefLmsAddressNumber);
                Map(m => m.Street1);
                Map(m => m.PostalCode);
                Map(m => m.City);                
                Map(m => m.CountryId);
                Map(m => m.GeoLocation).Name("Lng").TypeConverter(this);
                Map(m => m.StateId).Ignore();
                Map(m => m.ChangedAt).Ignore();
                Map(m => m.ChangedBy).Ignore();
                Map(m => m.ChangedById).Ignore();
                Map(m => m.Country).Ignore();
                Map(m => m.CreatedAt).Ignore();
                Map(m => m.CreatedBy).Ignore();
                Map(m => m.CreatedById).Ignore();
                Map(m => m.DeletedAt).Ignore();
                Map(m => m.DeletedBy).Ignore();
                Map(m => m.DeletedById).Ignore();
                Map(m => m.IsDeleted).Ignore();
                Map(m => m.State).Ignore();
                Map(m => m.Street2).Ignore();
            }

            public object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
            {
                var lngString = row.GetField("Lng");
                var latString = row.GetField("Lat");
                if(string.IsNullOrEmpty(lngString) || string.IsNullOrEmpty(latString))
                {
                    return null;
                }

                var lng = double.Parse(lngString, CultureInfo.InvariantCulture);
                var lat = double.Parse(latString, CultureInfo.InvariantCulture);
                return new Point(lng, lat) { SRID = 4326 };
                //var parsed = reader.Read(this.ConvertHexStringToByteArray(text.Substring(2)));
                //return new Point(parsed.Coordinate.Y, parsed.Coordinate.X) { SRID = 4326 };
            }

            //private byte[] ConvertHexStringToByteArray(string hexString)
            //{
            //    if (hexString.Length % 2 != 0)
            //    {
            //        throw new ArgumentException(String.Format(System.Globalization.CultureInfo.InvariantCulture, "The binary key cannot have an odd number of digits: {0}", hexString));
            //    }

            //    byte[] data = new byte[hexString.Length / 2];
            //    for (int index = 0; index < data.Length; index++)
            //    {
            //        string byteValue = hexString.Substring(index * 2, 2);
            //        data[index] = byte.Parse(byteValue, System.Globalization.NumberStyles.HexNumber, System.Globalization.CultureInfo.InvariantCulture);
            //    }

            //    return data;
            //}

            public string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData)
            {
                throw new NotImplementedException();
            }
        }

        internal class LoadingLocationCsvMap : CsvHelper.Configuration.ClassMap<LoadingLocation>
        {
            public LoadingLocationCsvMap()
            {
                Map(m => m.Id);
                Map(m => m.AddressId);
                Map(m => m.StackHeightMin);
                Map(m => m.StackHeightMax);
                Map(m => m.SupportsPartialMatching);
                Map(m => m.SupportsRearLoading);
                Map(m => m.SupportsSideLoading);
                Map(m => m.SupportsJumboVehicles);
                Map(m => m.ChangedAt).Ignore();
                Map(m => m.ChangedBy).Ignore();
                Map(m => m.ChangedById).Ignore();
                Map(m => m.Address).Ignore();
                Map(m => m.CreatedAt).Ignore();
                Map(m => m.CreatedBy).Ignore();
                Map(m => m.CreatedById).Ignore();
                Map(m => m.DeletedAt).Ignore();
                Map(m => m.DeletedBy).Ignore();
                Map(m => m.DeletedById).Ignore();
                Map(m => m.IsDeleted).Ignore();
                Map(m => m.PartnerId).Ignore();
                Map(m => m.Partner).Ignore();
                Map(m => m.BusinessHourExceptions).Ignore();
                Map(m => m.BusinessHours).Ignore();
                Map(m => m.CustomerDivisionId).Ignore();
                Map(m => m.CustomerDivision).Ignore();
                Map(m => m.CustomerPartnerId).Ignore();
                Map(m => m.CustomerPartner).Ignore();
            }
        }

        internal class BusinessHourCsvMap : CsvHelper.Configuration.ClassMap<BusinessHour>
        {
            public BusinessHourCsvMap()
            {
                Map(m => m.Id);
                Map(m => m.DayOfWeek);
                Map(m => m.FromTime);
                Map(m => m.ToTime);
                Map(m => m.LoadingLocationId);
                Map(m => m.ChangedAt).Ignore();
                Map(m => m.ChangedBy).Ignore();
                Map(m => m.ChangedById).Ignore();
                Map(m => m.CreatedAt).Ignore();
                Map(m => m.CreatedBy).Ignore();
                Map(m => m.CreatedById).Ignore();
                Map(m => m.DeletedAt).Ignore();
                Map(m => m.DeletedBy).Ignore();
                Map(m => m.DeletedById).Ignore();
                Map(m => m.IsDeleted).Ignore();
            }
        }
    }
}
