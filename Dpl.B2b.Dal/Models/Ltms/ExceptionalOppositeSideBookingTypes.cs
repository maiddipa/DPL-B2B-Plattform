using System;
using System.Collections.Generic;

namespace Dpl.B2b.Dal.Ltms
{
    public partial class ExceptionalOppositeSideBookingTypes
    {
        public int ConditionId { get; set; }
        public string BookingTypeId { get; set; }

        public virtual BookingTypes BookingType { get; set; }
        public virtual Conditions Condition { get; set; }
    }
}
