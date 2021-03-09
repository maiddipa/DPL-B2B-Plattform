using Dpl.B2b.Common.Enumerations;
using System;
using System.Collections.Generic;

namespace Dpl.B2b.Contracts.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LogoUrl { get; set; }
        public string DefaultLanguage { get; set; }
        public bool IsPoolingPartner { get; set; }
        
        public Newtonsoft.Json.Linq.JObject Settings { get; set; }

        public IEnumerable<CustomerDocumentSettings> DocumentSettings { get; set; }
        public IEnumerable<CustomerDivision> Divisions { get; set; }
        public IEnumerable<CustomDocumentLabel> CustomDocumentLabels { get; set; }

         public IEnumerable<LoadCarrierReceiptDepotPreset> LoadCarrierReceiptDepotPresets { get; set; }

        public int? ParentCustomerId { get; set; }
        public int RefErpCustomerNumber { get; set; }
        public int AddressId { get; set; }
        public int PartnerId { get; set; }
        public int? ParentId { get; set; }
        public bool IsActive { get; set; }
        public int OrganizationId { get; set; }
    }

    public class CustomerCreateRequest
    {
        public int RefErpCustomerNumber { get; set; }
        public string Name { get; set; }
        public string DefaultLanguage { get; set; }
        public bool IsPoolingPartner { get; set; }
        public int AddressId { get; set; }
        public int OrganizationId { get; set; }
        public int PartnerId { get; set; }
        public int? ParentId { get; set; }
        public bool IsActive { get; set; }
        public Newtonsoft.Json.Linq.JObject Settings { get; set; }
    }

    public class CustomerUpdateRequest
    {
        public int Id { get; set; }
        public int RefErpCustomerNumber { get; set; }
        public string Name { get; set; }
        public string DefaultLanguage { get; set; }
        public bool IsPoolingPartner { get; set; }
        public int AddressId { get; set; }
        public int PartnerId { get; set; }
        public int? ParentId { get; set; }
        public bool IsActive { get; set; }
        public Newtonsoft.Json.Linq.JObject Settings { get; set; }
    }
    
    public class CustomerDeleteRequest
    {
        public int Id { get; set; }
    }

    public class LoadCarrierReceiptDepotPreset
    {
        public int Id { get; set; }
        public LoadCarrierReceiptDepotPresetCategory Category { get; set; }

        public string Name { get; set; }

        public bool IsSortingRequired { get; set; }

        public IEnumerable<int> LoadCarriersIds { get; set; }
    }
}
