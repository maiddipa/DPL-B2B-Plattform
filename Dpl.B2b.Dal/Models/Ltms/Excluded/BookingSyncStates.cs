using System;
using System.Collections.Generic;

namespace Dpl.B2b.Dal.Ltms
{
    public partial class BookingSyncStates
    {
        public int Id { get; set; }
        public int? ErrorCode { get; set; }
        public DateTime LastSync { get; set; }
        public int SyncOperation { get; set; }
        public int ChangeOperation { get; set; }
        public DateTime RequestEnqueued { get; set; }
        public DateTime? RequestDeQueued { get; set; }
        public DateTime? RequestDeferred { get; set; }
        public DateTime? ResponseEnqueued { get; set; }
        public DateTime? ResponseDeQueued { get; set; }
        public DateTime? ResponseDeferred { get; set; }
        public DateTime? Acknowledged { get; set; }
        public string SessionId { get; set; }
        public int AccountId { get; set; }

        public virtual Bookings IdNavigation { get; set; }
    }
}
