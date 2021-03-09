using System;
using System.Collections.Generic;

namespace Dpl.B2b.Dal.Ltms
{
    public partial class BookingDependent
    {
        public int Id { get; set; }
        public int? Quantity { get; set; }
        public decimal? Amount { get; set; }
        public short ArticleId { get; set; }
        public short? QualityId { get; set; }

        public virtual Conditions IdNavigation { get; set; }
    }
}
