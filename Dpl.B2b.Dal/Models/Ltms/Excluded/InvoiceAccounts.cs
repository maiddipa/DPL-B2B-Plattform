using System;
using System.Collections.Generic;

namespace Dpl.B2b.Dal.Ltms
{
    public partial class InvoiceAccounts
    {
        public int InvoiceId { get; set; }
        public int AccountId { get; set; }

        public virtual Accounts Account { get; set; }
        public virtual Invoices Invoice { get; set; }
    }
}
