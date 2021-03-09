using Dpl.B2b.Common.Enumerations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dpl.B2b.Dal.Models
{
    public class TransportBid : OlmaAuditable
    {
        public int Id { get; set; }

        public TransportBidStatus Status { get; set; }

        public decimal Price { get; set; }

        public DateTime PickupDate { get; set; }
        public DateTime DeliveryDate { get; set; }

        public string Note { get; set; }

        // TODO discuss if this is supposed to be division
        public int DivisionId { get; set; }
        public virtual CustomerDivision Division { get; set; }

        public int TransportId { get; set; }
        public virtual Transport Transport { get; set; }
    }
}
