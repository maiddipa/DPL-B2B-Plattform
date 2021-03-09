using System;
using System.Collections.Generic;

namespace Dpl.B2b.Dal.Ltms
{
    public partial class AccountContacts
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public long ContactId { get; set; }
        public string Note { get; set; }
        public string CreateUser { get; set; }
        public DateTime CreateTime { get; set; }
        public string UpdateUser { get; set; }
        public DateTime? UpdateTime { get; set; }
        public string TransmissionMethod { get; set; }
        public bool? ProcessAutomatically { get; set; }
        public string Discriminator { get; set; }

        public virtual Accounts Account { get; set; }
    }
}
