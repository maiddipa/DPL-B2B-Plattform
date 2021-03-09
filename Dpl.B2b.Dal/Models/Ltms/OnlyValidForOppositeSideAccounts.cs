using System;
using System.Collections.Generic;

namespace Dpl.B2b.Dal.Ltms
{
    public partial class OnlyValidForOppositeSideAccounts
    {
        public int ConditionId { get; set; }
        public int AccountId { get; set; }

        public virtual Accounts Account { get; set; }
        public virtual Conditions Condition { get; set; }
    }
}
