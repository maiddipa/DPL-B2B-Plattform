using System.Collections.Generic;
using Dpl.B2b.Contracts.Models;
using Dpl.B2b.Dal.Ltms;

namespace Dpl.B2b.Dal.Models
{
    public class PostingAccount : OlmaAuditable
    {
        public int Id { get; set; }
        public PostingAccountType Type { get; set; }
        public string DisplayName { get; set; }
        public int RefLtmsAccountId { get; set; }
        public string RefLtmsAccountNumber { get; set; }
        // this is a computed field and is used for searching
        public string RefLtmsAccountIdString { get; set; }
        public int? RefErpCustomerNumber { get; set; }

        // TODO Do we still need to include address if sub accounts is handled differently
        public int? AddressId { get; set; }
        public virtual Address Address { get; set; }

        public int? CustomerId { get; set; }
        public virtual Customer Customer { get; set; }
        public int? PartnerId { get; set; }
        public virtual Partner Partner { get; set; }
        public virtual ICollection<CustomerDivision> CustomerDivisions { get; set; }
        public virtual ICollection<OrderCondition> OrderConditions { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<PostingRequest> PostingRequests { get; set; }
        public virtual ICollection<ExpressCode> ExpressCodes { get; set; }

        public int? CalculatedBalanceId { get; set; }
        public virtual CalculatedBalance CalculatedBalance { get; set; }

        // There will be only one level of hierarchy, so we don''t need a root account field
        public int? ParentId { get; set; }
        public virtual PostingAccount Parent { get; set; }
        public virtual ICollection<PostingAccount> Children { get; set; }

        public virtual ExpressCodeUsageCondition ExpressCodeCondition { get; set; }
    }

    public class ExpressCodeUsageCondition
    {
        public int PostingAccountId { get; set; }
        public PostingAccount PostingAccount { get; set; }
        public bool AllowDropOff { get; set; }

        public bool AllowReceivingVoucher { get; set; }

        public bool AcceptForDropOff { get; set; }
    }

    public enum PostingAccountType
    {
        Main = 0,
        SubAccount = 1
    }
}