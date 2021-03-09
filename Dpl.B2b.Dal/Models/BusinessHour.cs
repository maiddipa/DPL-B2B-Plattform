using System;

namespace Dpl.B2b.Dal.Models
{
    public class BusinessHour : OlmaAuditable
    {
        public int Id { get; set; }
        public int LoadingLocationId { get; set; }

        public DayOfWeek DayOfWeek { get; set; }
        public DateTime FromTime { get; set; }
        public DateTime ToTime { get; set; }
    }
}