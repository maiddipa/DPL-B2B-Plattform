using System;
using System.Collections.Generic;
using System.Text;

namespace Dpl.B2b.Dal.Models
{
    public class OrganizationPartnerDirectory
    {
        public int Id { get; set; }
        public int OrganizationId { get; set; }
        public virtual  Organization Organization{ get; set; }

        public int PartnerDirectoryId { get; set; }
        public virtual PartnerDirectory PartnerDirectory { get; set; }
    }
}
