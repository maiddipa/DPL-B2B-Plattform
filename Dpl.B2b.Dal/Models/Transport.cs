using System;
using System.Collections.Generic;
using System.Text;

namespace Dpl.B2b.Dal.Models
{
    public class Transport : OlmaAuditable
    {
        public int Id { get; set; }

        public string ReferenceNumber { get; set; }

        public TransportType Type { get; set; }

        // TODO dicuss how to identify DPL person responsible for given transport
        // after discussion with fabian responsible is usualy one that is responsible for plz of demand side
        public int? OrderMatchId { get; set; }
        public virtual OrderMatch OrderMatch { get; set; }

        // TODO should be GUID
        public Guid? RefLmsAvail2DeliRowId { get; set; }

        public TransportStatus Status { get; set; }

        public DateTime PlannedLatestBy { get; set; } // bid deadline
        public DateTime ConfirmedLatestBy { get; set; } // when does customers (demand/supply customres) need to be informed latest

        public int? WinningBidId { get; set; }
        public virtual TransportBid WinningBid { get; set; }
        public virtual ICollection<TransportBid> Bids { get; set; }

        public long? RoutedDistance { get; set; }
    }

    public enum TransportStatus
    {
        Requested = 0,
        Allocated = 1, // bid has ben accepted
        Planned = 2, // shipper has accepted
        Confirmed = 3, // customer has been notified
        Canceled = 4
    }

    public enum TransportType
    {
        OrderMatch,
        Avail2Deli,
        Other
    }
}
