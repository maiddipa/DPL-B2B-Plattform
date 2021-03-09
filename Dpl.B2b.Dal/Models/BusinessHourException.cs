using System;
using Dpl.B2b.Common.Enumerations;

namespace Dpl.B2b.Dal.Models
{
    public class BusinessHourException : OlmaAuditable
    {
        public int Id { get; set; }

        public BusinessHourExceptionType Type { get; set; }

        public DateTime FromDateTime { get; set; }

        public DateTime ToDateTime { get; set; }
    }
}