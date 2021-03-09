using System;
using Dpl.B2b.Common.Enumerations;

namespace Dpl.B2b.Contracts.Sync
{
    public interface ISyncError
    {
        public int Id { get; set; }
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
    }
}
