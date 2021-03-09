using System;
using System.Collections.Generic;

namespace Dpl.B2b.Dal.Ltms
{
    public partial class PalletChangeSets
    {
        public PalletChangeSets()
        {
            PalletChangeDefinitions = new HashSet<PalletChangeDefinitions>();
            PalletChanges = new HashSet<PalletChanges>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual ICollection<PalletChangeDefinitions> PalletChangeDefinitions { get; set; }
        public virtual ICollection<PalletChanges> PalletChanges { get; set; }
    }
}
