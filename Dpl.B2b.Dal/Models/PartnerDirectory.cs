using System.Collections.Generic;

namespace Dpl.B2b.Dal.Models
{
    public class PartnerDirectory : OlmaAuditable
    {
        public int Id { get; set; }
        public string Name { get; set; }        
        public PartnerDirectoryType Type { get; set; }
        public virtual ICollection<PartnerDirectoryAccess> PartnerDirectoryAccesses { get; set; }
        public virtual ICollection<CustomerPartnerDirectoryAccess> CustomerPartnerDirectoryAccesses { get; set; }
        public virtual ICollection<OrganizationPartnerDirectory> OrganizationPartnerDirectories { get; set; }
    }

    public enum PartnerDirectoryType  //TODO Localization for this enum necessary?
    {
        Partner = 0,
        CustomerPartner = 1,
        ExpressCodeImportCustom = 2
    }
}