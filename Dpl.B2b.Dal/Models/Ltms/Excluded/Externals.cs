using System;
using System.Collections.Generic;

namespace Dpl.B2b.Dal.Ltms
{
    public partial class Externals
    {
        public long Id { get; set; }
        public bool NoExpirationDate { get; set; }
        public string SubmitterCustomerNumber { get; set; }
        public int SubmissionId { get; set; }
        public int? DebitId { get; set; }

        public virtual Debits Debit { get; set; }
        public virtual Vouchers IdNavigation { get; set; }
        public virtual Submissions Submission { get; set; }
    }
}
