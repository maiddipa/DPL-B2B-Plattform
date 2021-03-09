using System;
using System.Collections.Generic;

namespace Dpl.B2b.Contracts.Models
{
    public class LoadingLocation
    {
        public int Id { get; set; }
        public Address Address { get; set; }
        public int? CustomerDivisionId { get; set; }
        public int? CustomerPartnerId { get; set; }
        public int? PartnerId { get; set; }
        public IEnumerable<BusinessHours> BusinessHours { get; set; }
        public IEnumerable<BusinessHourException> BusinessHourExceptions { get; set; }
        public IEnumerable<PublicHoliday> PublicHolidays { get; set; }
        public LoadingLocationDetail Detail { get; set; }
    }
    public class LoadingLocationAdministration
    {
        public int Id { get; set; }
        public int AddressId { get; set; }
        public int? CustomerDivisionId { get; set; }
        public int? CustomerPartnerId { get; set; }
        public int? PartnerId { get; set; }
        public IEnumerable<BusinessHours> BusinessHours { get; set; }
        public IEnumerable<BusinessHourException> BusinessHourExceptions { get; set; }
        public int StackHeightMin { get; set; }
        public int StackHeightMax { get; set; }
        public bool SupportsPartialMatching { get; set; }
        public bool SupportsRearLoading { get; set; }
        public bool SupportsSideLoading { get; set; }        
        public bool SupportsJumboVehicles { get; set; }
    }
    public class LoadingLocationDetail
    {
        public int StackHeightMin { get; set; }
        public int StackHeightMax { get; set; }
        public bool SupportsPartialMatching { get; set; }
        public bool SupportsRearLoading { get; set; }
        public bool SupportsSideLoading { get; set; }        
        public bool SupportsJumboVehicles { get; set; }
    }
    [Obsolete]
    public class LoadingLocationSearchRequest : PaginationRequest
    {
        public List<int> Divisions { get; set; }
        public List<int> Partners { get; set; }
    }
    [Obsolete]
    public class UpdateLoadingLocationRequest
    {
    }

    public class LoadingLocationsSearchRequest
    {
        public int? Id { get; set; }
        public int? CustomerDivisionId { get; set; }
    }
    
    public class LoadingLocationsCreateRequest
    {
        public int AddressId { get; set; }
        public IEnumerable<BusinessHours> BusinessHours { get; set; }
        public IEnumerable<BusinessHourException> BusinessHourExceptions { get; set; }
        public int? CustomerDivisionId { get; set; }
        public int? CustomerPartnerId { get; set; }
        public int? PartnerId { get; set; }
        public int StackHeightMin { get; set; }
        public int StackHeightMax { get; set; }
        public bool SupportsPartialMatching { get; set; }
        public bool SupportsRearLoading { get; set; }
        public bool SupportsSideLoading { get; set; }
        public bool SupportsJumboVehicles { get; set; }
    }
    
    public class LoadingLocationsUpdateRequest
    {
        public int Id { get; set; }
        public int AddressId { get; set; }
        public int? CustomerDivisionId { get; set; }
        public int? CustomerPartnerId { get; set; }
        public int? PartnerId { get; set; }
        public int StackHeightMin { get; set; }
        public int StackHeightMax { get; set; }
        public bool SupportsPartialMatching { get; set; }
        public bool SupportsRearLoading { get; set; }
        public bool SupportsSideLoading { get; set; }
        public bool SupportsJumboVehicles { get; set; }
        public ICollection<BusinessHours> BusinessHours { get; set; }
        public ICollection<BusinessHourException> BusinessHourExceptions { get; set; }
    }
    
    public class LoadingLocationsDeleteRequest
    {
        public int Id { get; set; }
    }
}
