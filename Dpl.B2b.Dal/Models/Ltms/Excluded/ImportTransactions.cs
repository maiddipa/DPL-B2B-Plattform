using System;
using System.Collections.Generic;

namespace Dpl.B2b.Dal.Ltms
{
    public partial class ImportTransactions
    {
        public ImportTransactions()
        {
            ImportBookings = new HashSet<ImportBookings>();
        }

        public int Id { get; set; }
        public int ImportStackId { get; set; }
        public string ProcessName { get; set; }
        public DateTime BookingDate { get; set; }
        public string SourceAccountNumber { get; set; }
        public string SourceName1 { get; set; }
        public string SourceName2 { get; set; }
        public string SourceExtDescription { get; set; }
        public string DestinationAccountNumber { get; set; }
        public string DestinationName1 { get; set; }
        public string DestinationName2 { get; set; }
        public string DestinationExtDescription { get; set; }
        public string ReferenceNumber { get; set; }
        public string IntDescription { get; set; }
        public string UpdateUser { get; set; }
        public DateTime? UpdateTime { get; set; }
        public string DeleteUser { get; set; }
        public DateTime? DeleteTime { get; set; }
        public int OptimisticLockField { get; set; }
        public int? TransactionId { get; set; }

        public virtual ImportStacks ImportStack { get; set; }
        public virtual Transactions Transaction { get; set; }
        public virtual ICollection<ImportBookings> ImportBookings { get; set; }
    }
}
