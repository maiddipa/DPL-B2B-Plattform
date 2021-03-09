using System;
using System.Collections.Generic;

namespace Dpl.B2b.Dal.Ltms
{
    public partial class Reports1
    {
        public long VoucherId { get; set; }
        public int ReportId { get; set; }

        public virtual Reports Report { get; set; }
        public virtual Vouchers Voucher { get; set; }
    }
}
