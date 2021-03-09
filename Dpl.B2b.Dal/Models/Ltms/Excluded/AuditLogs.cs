using System;
using System.Collections.Generic;

namespace Dpl.B2b.Dal.Ltms
{
    public partial class AuditLogs
    {
        public long Id { get; set; }
        public string UserId { get; set; }
        public DateTime EventTime { get; set; }
        public string EventType { get; set; }
        public string TableName { get; set; }
        public string RecordId { get; set; }
        public string ColumnName { get; set; }
        public string OriginalValue { get; set; }
        public string NewValue { get; set; }
    }
}
