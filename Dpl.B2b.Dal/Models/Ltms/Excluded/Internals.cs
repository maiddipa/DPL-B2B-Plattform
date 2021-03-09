using System;
using System.Collections.Generic;

namespace Dpl.B2b.Dal.Ltms
{
    public partial class Internals
    {
        public Internals()
        {
            ReceiptStateHistories = new HashSet<ReceiptStateHistories>();
            StateLogs = new HashSet<StateLogs>();
        }

        public long Id { get; set; }
        public string VoucherReasonId { get; set; }
        public string VoucherStateId { get; set; }
        public Guid LegacyId { get; set; }
        public DateTime DateOfExpiryByLaw { get; set; }
        public bool Matched { get; set; }
        public string MatchedUser { get; set; }
        public DateTime? MatchedTime { get; set; }
        public bool Tdl { get; set; }
        public string Recipient { get; set; }
        public string IntDescription { get; set; }
        public int? SubmissionId { get; set; }
        public string SubmitterCustomerNumber { get; set; }
        public int? CreditorId { get; set; }
        public int? TransactionId { get; set; }

        public virtual Accounts Creditor { get; set; }
        public virtual Vouchers IdNavigation { get; set; }
        public virtual Submissions Submission { get; set; }
        public virtual Transactions Transaction { get; set; }
        public virtual ReasonTypes VoucherReason { get; set; }
        public virtual StateTypes VoucherState { get; set; }
        public virtual ICollection<ReceiptStateHistories> ReceiptStateHistories { get; set; }
        public virtual ICollection<StateLogs> StateLogs { get; set; }
    }
}
