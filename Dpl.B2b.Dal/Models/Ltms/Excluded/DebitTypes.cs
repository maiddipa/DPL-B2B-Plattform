using System;
using System.Collections.Generic;

namespace Dpl.B2b.Dal.Ltms
{
    public partial class DebitTypes
    {
        public DebitTypes()
        {
            Debits = new HashSet<Debits>();
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public string ProcessName { get; set; }

        public virtual ICollection<Debits> Debits { get; set; }
    }
}
