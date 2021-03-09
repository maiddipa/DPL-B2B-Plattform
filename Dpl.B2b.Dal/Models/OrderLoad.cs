using System;
using System.Collections.Generic;
using Dpl.B2b.Common.Enumerations;

namespace Dpl.B2b.Dal.Models
{
    public class OrderLoad : OlmaAuditable
    {
        public int Id { get; set; }

        public int OrderId { get; set; }
        public virtual Order Order { get; set; }

        public int? DemandOrderMatchId { get; set; }
        public virtual OrderMatch DemandOrderMatch { get; set; }

        public int? SupplyOrderMatchId { get; set; }
        public virtual OrderMatch SupplyOrderMatch { get; set; }

        public int DetailId { get; set; }
        public virtual OrderLoadDetail Detail { get; set; }

        public virtual ICollection<EmployeeNote> DplNotes { get; set; }
    }

    public class OrderLoadDetail
    {
        public int Id { get; set; }
        public OrderLoadStatus Status { get; set; }
        public DateTime PlannedFulfillmentDateTime { get; set; }
        public DateTime? ActualFulfillmentDateTime { get; set; }

        public int? LoadCarrierReceiptId { get; set; }
        public virtual LoadCarrierReceipt LoadCarrierReceipt { get; set; }

        public virtual ICollection<PostingRequest> PostingRequests { get; set; }

        public int AddressId { get; set; }
        public virtual Address Address { get; set; }
    }    
}