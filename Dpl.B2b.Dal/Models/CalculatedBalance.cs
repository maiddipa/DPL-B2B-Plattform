using System;
using System.Collections.Generic;

namespace Dpl.B2b.Dal.Models
{
    public class CalculatedBalance
    {
        public int Id { get; set; }

        //TODO Add [Index(IsClustered = true)]
        public DateTime LastBookingDateTime { get; set; }

        public int PostingAccountId { get; set; }
        public virtual PostingAccount PostingAccount { get; set; }

        public virtual ICollection<CalculatedBalancePosition> Positions { get; set; }
    }
}