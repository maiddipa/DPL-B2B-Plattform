using Dpl.B2b.Common.Enumerations;
using System;
using System.Collections.Generic;

namespace Dpl.B2b.Dal.Models
{
    public class Customer : OlmaAuditable
    {
        public int Id { get; set; }

        public int RefErpCustomerNumber { get; set; }
        public string RefErpCustomerNumberString { get; set; }

        public string Name { get; set; }

        // TODO check if not optional
        public string LogoUrl { get; set; }

        // TODO check if we should link to a language entity instead of language as string
        public string DefaultLanguage { get; set; }

        public bool IsPoolingPartner { get; set; }

        public int AddressId { get; set; }

        public virtual Address Address { get; set; }

        public virtual ICollection<CustomerDocumentSetting> DocumentSettings { get; set; }

        public virtual ICollection<DocumentNumberSequence> DocumentNumberSequences { get; set; }

        public virtual ICollection<CustomDocumentLabel> CustomDocumentLabels { get; set; }

        public int OrganizationId { get; set; }

        public virtual Organization Organization { get; set; }

        public int PartnerId { get; set; }
        public virtual Partner Partner { get; set; } //TODO DB Added after discussion with Amine (21.11.2019)

        public virtual ICollection<CustomerDivision> Divisions { get; set; }

        public virtual ICollection<UserCustomerRelation> Users { get; set; }

        public int? ParentId { get; set; }
        public Newtonsoft.Json.Linq.JObject Settings { get; set; }

        public virtual Customer Parent { get; set; }

        public virtual ICollection<Customer> Children { get; set; }

        // TODO we should move this into an interface so we can apply query filters on it
        public bool IsActive { get; set; }

        public virtual ICollection<CustomerSortingWorker> SortingWorkerMapping { get; set; }
        public virtual ICollection<PostingAccount> PostingAccounts { get; set; }

        public virtual ICollection<CustomerLoadCarrierReceiptDepotPreset> LoadCarrierReceiptDepotPresets { get; set; }
    }

    public class LoadCarrierReceiptDepotPreset : OlmaAuditable
    {
        public int Id { get; set; }

        public LoadCarrierReceiptDepotPresetCategory Category { get; set; }

        public string Name { get; set; }

        public bool IsSortingRequired { get; set; }

        public ICollection<CustomerLoadCarrierReceiptDepotPreset> Customers { get; set; }
        public ICollection<LoadCarrierReceiptDepotPresetLoadCarrier> LoadCarriers { get; set; }
    }

    //ToDo: EF5 Remove after upgrade
    public class CustomerLoadCarrierReceiptDepotPreset
    {
        public int CustomerId { get; set; }
        public virtual Customer Customer { get; set; }

        public int LoadCarrierReceiptDepotPresetId { get; set; }
        public virtual LoadCarrierReceiptDepotPreset LoadCarrierReceiptDepotPreset { get; set; }
    }

    //ToDo: EF5 Remove after upgrade
    public class LoadCarrierReceiptDepotPresetLoadCarrier
    {
        public int LoadCarrierId { get; set; }
        public virtual LoadCarrier LoadCarrier { get; set; }

        public int LoadCarrierReceiptDepotPresetId { get; set; }
        public virtual LoadCarrierReceiptDepotPreset LoadCarrierReceiptDepotPreset { get; set; }
    }
}