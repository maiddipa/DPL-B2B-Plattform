using System;
using System.Collections.Generic;

namespace Dpl.B2b.Dal.Models.Lms
{
    public partial class LmsAvailabilityHistory
    {
        public int AvailabilityHistoryId { get; set; }
        public int? AvailabilityId { get; set; }
        public bool? Geblockt { get; set; }
        public string GeblocktVon { get; set; }
        public DateTime? GeblocktDatum { get; set; }
        public string GeblocktAufgehobenVon { get; set; }
        public DateTime? GeblocktAufgehobenDatum { get; set; }
        public string GeblocktFuer { get; set; }
        public DateTime? AvailableFromDate { get; set; }
        public DateTime? AvailableUntilDate { get; set; }
        public string DataCreatedBy { get; set; }
        public DateTime? DataCreationDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
