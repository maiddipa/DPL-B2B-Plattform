using System;
using System.Collections.Generic;

namespace Dpl.B2b.Dal.Ltms
{
    public partial class StateLogs
    {
        public long Id { get; set; }
        public long VoucherId { get; set; }
        public DateTime EventDate { get; set; }
        public string User { get; set; }
        public string StateId { get; set; }

        public virtual StateTypes State { get; set; }
        public virtual Internals Voucher { get; set; }
    }
}
