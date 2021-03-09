using System;
using Dpl.B2b.Common.Enumerations;

namespace Dpl.B2b.Contracts.Models
{
    public class BusinessHours
    {
        public int Id { get; set; }
        public DayOfWeek DayOfWeek { get; set; }
        public DateTime FromTime { get; set; }
        public DateTime ToTime { get; set; }
    }

    
    public class BusinessHourException
    {
        public int Id { get; set; }

        public BusinessHourExceptionType Type { get; set; }

        public DateTime FromDateTime { get; set; }

        public DateTime ToDateTime { get; set; }
    }

    public class PublicHoliday
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime Date { get; set; }
    }
}
