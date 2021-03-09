using System;
using System.Collections.Generic;

namespace Dpl.B2b.Dal.Ltms
{
    public partial class StateTypes
    {
        public StateTypes()
        {
            Internals = new HashSet<Internals>();
            StateLogs = new HashSet<StateLogs>();
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Internals> Internals { get; set; }
        public virtual ICollection<StateLogs> StateLogs { get; set; }
    }
}
