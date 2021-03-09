using System;
using System.Collections.Generic;

namespace Dpl.B2b.Dal.Ltms
{
    public partial class ReceiptStateHistories
    {
        public long VoucherId { get; set; }
        public int Type { get; set; }
        public DateTime Occurence { get; set; }

        public virtual Internals Voucher { get; set; }
    }
}
