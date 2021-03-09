using System;
using System.Collections.Generic;

namespace Dpl.B2b.Dal.Ltms
{
    public partial class Debits
    {
        public Debits()
        {
            Externals = new HashSet<Externals>();
        }

        public int Id { get; set; }
        public DateTime ClearingDate { get; set; }
        public DateTime? PickUpDate { get; set; }
        public string ReferenceNumber { get; set; }
        public string PickupAddressCustomerNumber { get; set; }
        public int? TransactionId { get; set; }
        public int OptimisticLockField { get; set; }
        public string CreateUser { get; set; }
        public DateTime CreateTime { get; set; }
        public string UpdateUser { get; set; }
        public DateTime? UpdateTime { get; set; }
        public string DeleteUser { get; set; }
        public DateTime? DeleteTime { get; set; }
        public int DebitorId { get; set; }
        public string DebitTypeId { get; set; }

        public virtual DebitTypes DebitType { get; set; }
        public virtual Accounts Debitor { get; set; }
        public virtual Transactions Transaction { get; set; }
        public virtual ICollection<Externals> Externals { get; set; }
    }
}
