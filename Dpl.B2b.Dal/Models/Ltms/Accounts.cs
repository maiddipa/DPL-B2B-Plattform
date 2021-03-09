using System;
using System.Collections.Generic;
using Dpl.B2b.Dal.Models;

namespace Dpl.B2b.Dal.Ltms
{
    public partial class Accounts
    {
        public int Id { get; set; }
        public Guid RowGuid { get; set; }
        public string Name { get; set; }
        public int? ParentId { get; set; }
        public string AccountTypeId { get; set; }
        public string AccountNumber { get; set; }
        public string CustomerNumber { get; set; }
        public int? AddressId { get; set; }
        public string Description { get; set; }
        public string PathByName { get; set; }
        public int OptimisticLockField { get; set; }
        public string CreateUser { get; set; }
        public DateTime CreateTime { get; set; }
        public string UpdateUser { get; set; }
        public DateTime? UpdateTime { get; set; }
        public string FullName { get; set; }
        public string PathName { get; set; }
        public bool Inactive { get; set; }
        public bool Locked { get; set; }
        public string ResponsiblePersonId { get; set; }
        public int? InvoiceAccountId { get; set; }
        public string KeyAccountManagerId { get; set; }
        public string SalespersonId { get; set; }

        public virtual AccountTypes AccountType { get; set; }
        //public virtual Accounts InvoiceAccount { get; set; }
        //public virtual Accounts Parent { get; set; }
        public virtual ICollection<Bookings> Bookings { get; set; }
        public virtual ICollection<Conditions> Conditions { get; set; }
        public virtual ICollection<ExceptionalOppositeSideAccounts> ExceptionalOppositeSideAccounts { get; set; }
        public virtual ICollection<Fees> Fees { get; set; }
        //public virtual ICollection<Accounts> InverseInvoiceAccount { get; set; }
        //public virtual ICollection<Accounts> InverseParent { get; set; }
        public virtual ICollection<OnlyValidForOppositeSideAccounts> OnlyValidForOppositeSideAccounts { get; set; }
        public virtual ICollection<Reports> Reports { get; set; }
        public virtual ICollection<Terms> Terms { get; set; }
    }
}
