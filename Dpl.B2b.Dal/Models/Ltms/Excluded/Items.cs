using System;
using System.Collections.Generic;

namespace Dpl.B2b.Dal.Ltms
{
    public partial class Items
    {
        public long VoucherId { get; set; }
        public short ArticleId { get; set; }
        public short QualityId { get; set; }
        public int Quantity { get; set; }

        public virtual Articles Article { get; set; }
        public virtual Qualities Quality { get; set; }
        public virtual Vouchers Voucher { get; set; }
    }
}
