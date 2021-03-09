using System;
using System.Collections.Generic;

namespace Dpl.B2b.Dal.Ltms
{
    public partial class AccountAssociations
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public int AssociatedId { get; set; }
        public string CreateUser { get; set; }
        public DateTime CreateTime { get; set; }
        public string Owner { get; set; }
        public string Discriminator { get; set; }

        public virtual Accounts Account { get; set; }
        public virtual Accounts Associated { get; set; }
    }
}
