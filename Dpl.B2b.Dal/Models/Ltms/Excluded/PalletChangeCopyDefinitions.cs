using System;
using System.Collections.Generic;

namespace Dpl.B2b.Dal.Ltms
{
    public partial class PalletChangeCopyDefinitions
    {
        public PalletChangeCopyDefinitions()
        {
            PalletChangeDefinitions = new HashSet<PalletChangeDefinitions>();
        }

        public int Id { get; set; }
        public bool CopyExtDescription { get; set; }
        public bool CopyIntDescription { get; set; }
        public bool CopyVouchers { get; set; }
        public bool CopyInvoice { get; set; }

        public virtual ICollection<PalletChangeDefinitions> PalletChangeDefinitions { get; set; }
    }
}
