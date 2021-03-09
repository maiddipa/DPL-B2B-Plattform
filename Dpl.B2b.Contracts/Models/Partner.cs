using Dpl.B2b.Common.Enumerations;
using System;
using System.Collections.Generic;

namespace Dpl.B2b.Contracts.Models
{
    public class Partner
    {
        public int Id { get; set; }
        public Guid RowGuid { get; set; }
        public PartnerType Type { get; set; }
        public string CompanyName { get; set; }
        public bool IsPoolingPartner { get; set; }
        public virtual Address DefaultAddress { get; set; }
        public virtual IEnumerable<Address> Addresses { get; set; }
        public virtual IEnumerable<LoadingLocation> LoadingLocations { get; set; }
        public virtual PostingAccount DefaultPostingAccount { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual IEnumerable<PostingAccount> PostingAccounts { get; set; }
        public virtual IEnumerable<CustomerPartner> CustomerPartners { get; set; }
    }

    public class PartnersSearchRequest
    {
        public int? Id { get; set; }
        public string CompanyName { get; set; }
    }

    public class PartnersCreateRequest
    {
        public string CompanyName { get; set; }
        public PartnerType Type { get; set; }
        public Address Address { get; set; }
    }
    public class PartnersUpdateRequest
    {
        public string Value { get; set; }
    }

    public class PartnersDeleteRequest
    {
        public int Id { get; set; }
    }
}