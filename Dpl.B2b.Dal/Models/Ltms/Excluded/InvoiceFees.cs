using System;
using System.Collections.Generic;

namespace Dpl.B2b.Dal.Ltms
{
    public partial class InvoiceFees
    {
        public int InvoiceId { get; set; }
        public int FeeId { get; set; }

        public virtual Fees Fee { get; set; }
        public virtual Invoices Invoice { get; set; }
    }
}
