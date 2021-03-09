using System;
using System.Collections.Generic;

namespace Dpl.B2b.Dal.Ltms
{
    public partial class BookingTypes
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Order { get; set; }
        public string Description { get; set; }
        public string LtmsUserTask { get; set; }
        public bool OneDayDelayForRent { get; set; }
        public bool IncludeDaysFreeRentCondition { get; set; }

        public virtual ICollection<Bookings> Bookings { get; set; }
        public virtual ICollection<ExceptionalOppositeSideBookingTypes> ExceptionalOppositeSideBookingTypes { get; set; }
        public virtual ICollection<OnlyValidForOppositeSideBookingTypes> OnlyValidForOppositeSideBookingTypes { get; set; }       
        public virtual ICollection<Terms> TermsBookingType { get; set; }
        public virtual ICollection<Terms> TermsSwapInBookingType { get; set; }
        public virtual ICollection<Terms> TermsSwapOutBookingType { get; set; }
    }
}
