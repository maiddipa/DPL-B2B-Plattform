using System;

namespace Dpl.B2b.Dal.Models
{
    public class SortingInterruption : OlmaAuditable
    {
        public int Id { get; set; }

        public SortingInterruptionType Type { get; set; }

        public DateTime FromDateTime { get; set; }
        public DateTime ToDateTime { get; set; }
    }

    public enum SortingInterruptionType
    {
        Break = 0,

        /// <summary>
        ///     Means no load carriers were available for sorting
        /// </summary>
        NoWork = 1,
        Disruption = 3
    }
}