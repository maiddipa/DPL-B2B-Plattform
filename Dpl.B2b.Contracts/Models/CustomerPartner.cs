using Dpl.B2b.Common.Enumerations;
using System;

namespace Dpl.B2b.Contracts.Models
{
    public class CustomerPartner
    {
        public int Id { get; set; }
        public Guid Guid { get; set; }
        public PartnerType Type { get; set; }
        public string CompanyName { get; set; }
        public Address Address { get; set; }
        
        public int? PostingAccountId { get; set; }

        public int[] DirectoryIds { get; set; }
        public string[] DirectoryNames { get; set; }
    }

    public class CustomerPartnersSearchRequest : PaginationRequest
    {
        public int Id { get; set; }
        public string CompanyName { get; set; }
        public string City { get; set; }
        public int? CustomerPartnerDirectoryId { get; set; }
        public string Country { get; set; }
    }

    public class CustomerPartnersCreateRequest
    {
        public string CompanyName { get; set; }
        public PartnerType Type { get; set; }
        public int DirectoryId { get; set; }
        public Address Address { get; set; }
    }

    public class CustomerPartnersUpdateRequest
    {
        public string Value { get; set; }
    }
}