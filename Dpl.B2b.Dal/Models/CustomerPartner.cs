using Dpl.B2b.Common.Enumerations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dpl.B2b.Dal.Models
{
    public class CustomerPartner : OlmaAuditable
    {
        public int Id { get; set; }
        public Guid RowGuid { get; set; } //TODO DB Doku: Added new GUID Field 

        public string CustomerReference { get; set; }

        // TODO added partner type field as we wanne allow filtering and displayal of difefernt partner types in UI
        public virtual PartnerType Type { get; set; }

        public string CompanyName { get; set; }

        public virtual Address Address { get; set; }

        public virtual ICollection<LoadingLocation> LoadingLocations { get; set; }  

        public int? PartnerId { get; set; }
        public virtual Partner Partner { get; set; }

        public virtual ICollection<CustomerPartnerDirectoryAccess> DirectoryAccesses { get; set; }
    }
}