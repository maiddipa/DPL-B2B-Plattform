using System;
using System.Collections.Generic;

namespace Dpl.B2b.Dal.Ltms
{
    public partial class Transactions
    {
        public int Id { get; set; }
        public Guid RowGuid { get; set; }
        public string TransactionTypeId { get; set; }
        public string ReferenceNumber { get; set; }
        public DateTime Valuta { get; set; }
        public string IntDescription { get; set; }
        public string ExtDescription { get; set; }
        public int ProcessId { get; set; }
        public string CreateUser { get; set; }
        public DateTime CreateTime { get; set; }
        public string UpdateUser { get; set; }
        public DateTime? UpdateTime { get; set; }
        public short TransactionStateId { get; set; }
        public int? CancellationId { get; set; }
        public string DeleteUser { get; set; }
        public DateTime? DeleteTime { get; set; }
        public int OptimisticLockField { get; set; }
        public DateTime? ChangedTime { get; set; }

        public virtual Transactions Cancellation { get; set; }
        public virtual Processes Process { get; set; }
        public virtual TransactionStates TransactionState { get; set; }
        public virtual TransactionTypes TransactionType { get; set; }
        public virtual ICollection<Bookings> Bookings { get; set; }
        public virtual ICollection<Transactions> InverseCancellation { get; set; }
    }
}
