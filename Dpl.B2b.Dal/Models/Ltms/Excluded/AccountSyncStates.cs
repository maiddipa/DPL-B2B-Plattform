using System;
using System.Collections.Generic;

namespace Dpl.B2b.Dal.Ltms
{
    public partial class AccountSyncStates
    {
        public int Id { get; set; }
        public int Requests { get; set; }
        public int RequestMessages { get; set; }
        public long RequestTotalSize { get; set; }
        public int Responses { get; set; }
        public int ResponseMessages { get; set; }
        public long ResponseTotalSize { get; set; }
        public long StorageTotalSize { get; set; }
        public int Errors { get; set; }
        public int LastErrorCode { get; set; }
        public string LastError { get; set; }
        public string SyncMessage { get; set; }
        public int Inserts { get; set; }
        public int Updates { get; set; }
        public int Deletes { get; set; }
        public int OptimisticLockField { get; set; }
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

        public virtual Accounts IdNavigation { get; set; }
    }
}
