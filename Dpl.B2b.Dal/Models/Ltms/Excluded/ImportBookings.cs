using System;
using System.Collections.Generic;

namespace Dpl.B2b.Dal.Ltms
{
    public partial class ImportBookings
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public int ImportTransactionId { get; set; }
        public short ArticleId { get; set; }
        public short QualityId { get; set; }
        public bool IsSourceBooking { get; set; }

        public virtual Articles Article { get; set; }
        public virtual ImportTransactions ImportTransaction { get; set; }
        public virtual Qualities Quality { get; set; }
    }
}
