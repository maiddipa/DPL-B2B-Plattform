using System;
using System.Collections.Generic;

namespace Dpl.B2b.Dal.Ltms
{
    public partial class AccountDirections
    {
        public AccountDirections()
        {
            PalletChangeDefinitions = new HashSet<PalletChangeDefinitions>();
        }

        public string Id { get; set; }
        public string Description { get; set; }

        public virtual ICollection<PalletChangeDefinitions> PalletChangeDefinitions { get; set; }
    }
}
