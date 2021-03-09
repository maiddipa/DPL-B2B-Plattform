using System;
using System.Collections.Generic;

namespace Dpl.B2b.Dal.Ltms
{
    public partial class Swaps1
    {
        public int Id { get; set; }
        public string TermId { get; set; }
        public int? ConditionId { get; set; }
        public int SwapAccountId { get; set; }
        public int InBookingId { get; set; }
        public int OutBookingId { get; set; }
        public int InFactor { get; set; }
        public int OutFactor { get; set; }
        public double Remainder { get; set; }

        public virtual Swaps Condition { get; set; }
        public virtual Transactions IdNavigation { get; set; }
        public virtual Bookings InBooking { get; set; }
        public virtual Bookings OutBooking { get; set; }
        public virtual Accounts SwapAccount { get; set; }
        public virtual Terms Term { get; set; }
    }
}
