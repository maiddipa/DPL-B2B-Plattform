using System;
using System.Collections.Generic;

namespace Dpl.B2b.Dal.Ltms
{
    public partial class TransactionStates
    {
        public short Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Cancelable { get; set; }

        public virtual ICollection<Transactions> Transactions { get; set; }
    }
}
