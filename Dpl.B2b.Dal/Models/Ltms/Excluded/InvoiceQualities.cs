using System;
using System.Collections.Generic;

namespace Dpl.B2b.Dal.Ltms
{
    public partial class InvoiceQualities
    {
        public int InvoiceId { get; set; }
        public short QualityId { get; set; }

        public virtual Invoices Invoice { get; set; }
        public virtual Qualities Quality { get; set; }
    }
}
