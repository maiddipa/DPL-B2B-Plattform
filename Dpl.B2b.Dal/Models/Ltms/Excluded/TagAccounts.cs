using System;
using System.Collections.Generic;

namespace Dpl.B2b.Dal.Ltms
{
    public partial class TagAccounts
    {
        public int TagId { get; set; }
        public int AccountId { get; set; }

        public virtual Accounts Account { get; set; }
        public virtual Tags Tag { get; set; }
    }
}
