using System;
using System.Collections.Generic;
using System.Text;

namespace Dpl.B2b.Sync.Tasks
{
    public class Settings
    {
        public string SyncServiceBusConnectionString { get; set; }
        public string OrderSyncRequestQueueName { get; set; }
        public string PostingRequestSyncRequestQueueName { get; set; }
        public int CheckTime { get; set; }

    }
}
