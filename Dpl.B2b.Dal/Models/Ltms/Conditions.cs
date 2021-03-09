using System;
using System.Collections.Generic;

namespace Dpl.B2b.Dal.Ltms
{
    public partial class Conditions
    {
        public int Id { get; set; }
        public DateTime ValidFrom { get; set; }
        public string CreateUser { get; set; }
        public DateTime CreateTime { get; set; }
        public string UpdateUser { get; set; }
        public DateTime? UpdateTime { get; set; }
        public int AccountId { get; set; }
        public string TermId { get; set; }
        public string Note { get; set; }
        public DateTime? ValidUntil { get; set; }
        public short? MatchingOrder { get; set; }
        public int? InvoicingDay { get; set; }
        public string Discriminator { get; set; }

        public virtual Accounts Account { get; set; }
        public virtual Terms Term { get; set; }
        public virtual BookingDependent BookingDependent { get; set; }
        public virtual Swaps Swaps { get; set; }
        public virtual ICollection<ExceptionalOppositeSideAccounts> ExceptionalOppositeSideAccounts { get; set; }
        public virtual ICollection<ExceptionalOppositeSideBookingTypes> ExceptionalOppositeSideBookingTypes { get; set; }
        public virtual ICollection<Fees> Fees { get; set; }
        public virtual ICollection<OnlyValidForOppositeSideAccounts> OnlyValidForOppositeSideAccounts { get; set; }
        public virtual ICollection<OnlyValidForOppositeSideBookingTypes> OnlyValidForOppositeSideBookingTypes { get; set; }
    }
}
