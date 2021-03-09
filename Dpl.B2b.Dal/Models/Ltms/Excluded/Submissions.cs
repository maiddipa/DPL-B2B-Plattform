using System;
using System.Collections.Generic;

namespace Dpl.B2b.Dal.Ltms
{
    public partial class Submissions
    {
        public Submissions()
        {
            Externals = new HashSet<Externals>();
            Internals = new HashSet<Internals>();
        }

        public int Id { get; set; }
        public DateTime DateOfReceipt { get; set; }
        public string SubmitterCustomerNumber { get; set; }
        public int CreditorId { get; set; }
        public DateTime CreditorBookingDate { get; set; }
        public string Number { get; set; }
        public string IntDescription { get; set; }
        public string ExtDescription { get; set; }
        public string ExtNumber { get; set; }
        public int? ProcessId { get; set; }
        public int? TransactionId { get; set; }
        public string CreateUser { get; set; }
        public DateTime CreateTime { get; set; }
        public string UpdateUser { get; set; }
        public DateTime? UpdateTime { get; set; }
        public string DeleteUser { get; set; }
        public DateTime? DeleteTime { get; set; }
        public int OptimisticLockField { get; set; }
        public Guid RowGuid { get; set; }

        public virtual Accounts Creditor { get; set; }
        public virtual Processes Process { get; set; }
        public virtual Transactions Transaction { get; set; }
        public virtual ICollection<Externals> Externals { get; set; }
        public virtual ICollection<Internals> Internals { get; set; }
    }
}
