using System;
using System.Collections.Generic;

namespace Dpl.B2b.Dal.Ltms
{
    public partial class ReasonTypes
    {
        public ReasonTypes()
        {
            Internals = new HashSet<Internals>();
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Internals> Internals { get; set; }
    }
}
