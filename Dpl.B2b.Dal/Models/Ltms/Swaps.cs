using System;
using System.Collections.Generic;

namespace Dpl.B2b.Dal.Ltms
{
    public partial class Swaps
    {
        public int Id { get; set; }
        public int InFactor { get; set; }
        public int OutFactor { get; set; }

        public virtual Conditions IdNavigation { get; set; }
    }
}
