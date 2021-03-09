using System;
using System.Collections.Generic;
using System.Text;
using Dpl.B2b.Common.Enumerations;
using Dpl.B2b.Contracts.Sync;

namespace Dpl.B2b.Dal.Models
{
    public class OrderSyncError :ISyncError
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public string MessageId { get; set; }
        public string SessionId { get; set; }
        public string ContentType { get; set; }
        public int MessageDeliveryCount { get; set; }
        public DateTime EnqueuedDateTime { get; set; }
        public string DeadLetterReason { get; set; }
        public string DeadLetterErrorDescription { get; set; }
        public SyncErrorStatus SyncErrorStatus { get; set; }
        public DateTime CreateDateTime { get; set; }
        public DateTime? UpdateDateTime { get; set; }
        public virtual Order Order { get; set; }
    }
}
