using NetTopologySuite.Geometries;

namespace Dpl.B2b.Contracts.Models
{
    public class Address
    {
        public int Id { get; set; }
        public string Street1 { get; set; }
        public string Street2 { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
        public string StateName { get; set; }
        public string CountryName { get; set; }
        //ToDo: rename to StateId
        public int? State { get; set; }
        //ToDo: rename to CountryId
        public int? Country { get; set; }
    }

    public class AddressAdministration : Address
    {
        public Point GeoLocation { get; set; }
    }

    public class AddressesSearchRequest
    {
        public int? Id { get; set; }
        public string Street1 { get; set; }
        public string Street2 { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
        public string StateName { get; set; }
        public string CountryName { get; set; }
    }
    
    public class AddressesCreateRequest
    {
        public string Street1 { get; set; }
        public string Street2 { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
        public int StateId { get; set; }
        public int CountryId { get; set; }
        public Point GeoLocation { get; set; }
    }
    
    public class AddressesUpdateRequest
    {
        public int Id { get; set; }
        public string Street1 { get; set; }
        public string Street2 { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
        public int StateId { get; set; }
        public int CountryId { get; set; }
        public Point GeoLocation { get; set; }
    }
    
    public class AddressesDeleteRequest
    {
        public int Id { get; set; }
    }
}
