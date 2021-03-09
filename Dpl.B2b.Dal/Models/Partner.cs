using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Dpl.B2b.Common.Enumerations;
using Microsoft.Extensions.Options;

namespace Dpl.B2b.Dal.Models
{
    public class Partner : OlmaAuditable
    {
        public int Id { get; set; }
        public Guid RowGuid { get; set; } //TODO DB Doku: Added new GUID Field 

        // TODO added partner type field as we wanne allow filtering and displayal of difefernt partner types in UI
        public PartnerType Type { get; set; }

        //[Index]
        public string CompanyName { get; set; }

        public bool IsPoolingPartner { get; set; }

        public int? DefaultAddressId { get; set; }
        public virtual Address DefaultAddress { get; set; }

        //ToDo: brauchen wir keine AddressId?
        public int AddressId { get; set; }

        public virtual ICollection<Address> Addresses { get; set; }
        public virtual ICollection<LoadingLocation> LoadingLocations { get; set; }

        // TODO Discuss with Dominik if we should require partners in central partner list to always have a booking account
        public virtual int? DefaultPostingAccountId { get; set; }
        public virtual PostingAccount DefaultPostingAccount { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual ICollection<PostingAccount> PostingAccounts { get; set; }
        public virtual ICollection<CustomerPartner> CustomerPartners { get; set; }

        public virtual ICollection<PartnerDirectoryAccess> DirectoryAccesses { get; set; }
        
        public virtual ICollection<PartnerPreset> ExpressCodePresets { get; set; }
    }
}