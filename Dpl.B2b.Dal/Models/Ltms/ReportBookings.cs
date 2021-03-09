using System;
using System.Collections.Generic;

namespace Dpl.B2b.Dal.Ltms
{
    public partial class ReportBookings
    {
        public int ReportId { get; set; }
        public int BookingId { get; set; }

        public virtual Bookings Booking { get; set; }
        public virtual Reports Report { get; set; }
    }
}
