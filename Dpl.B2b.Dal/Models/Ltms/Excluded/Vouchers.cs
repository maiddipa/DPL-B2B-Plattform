using System;
using System.Collections.Generic;

namespace Dpl.B2b.Dal.Ltms
{
    public partial class Vouchers
    {
        public Vouchers()
        {
            Items = new HashSet<Items>();
            Reports1 = new HashSet<Reports1>();
        }

        public long Id { get; set; }
        public string Number { get; set; }
        public int IssuerId { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime DateOfExpiry { get; set; }
        public string CreateUser { get; set; }
        public DateTime CreateTime { get; set; }
        public string UpdateUser { get; set; }
        public DateTime? UpdateTime { get; set; }
        public string DeleteUser { get; set; }
        public DateTime? DeleteTime { get; set; }
        public int OptimisticLockField { get; set; }
        public string ReverseNumber { get; set; }
        public Guid RowGuid { get; set; }

        public virtual Accounts Issuer { get; set; }
        public virtual Externals Externals { get; set; }
        public virtual Internals Internals { get; set; }
        public virtual ICollection<Items> Items { get; set; }
        public virtual ICollection<Reports1> Reports1 { get; set; }
    }
}
