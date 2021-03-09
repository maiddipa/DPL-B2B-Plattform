using System;
using System.Collections.Generic;
using System.Text;

namespace Dpl.B2b.Contracts.Models
{
    public class OrderSummary
    {
        public int LoadCarrierId { get; set; }
        public OrderAggregate Sum { get; set; }
        public OrderAggregate Count { get; set; }
    }

    public class OrderAggregate
    {
        public int Open { get; set; }
        public int Confirmed { get; set; }
        public int PartiallyMatched { get; set; }
        public int Matched { get; set; }
        public int Planned { get; set; }
        public int Fulfilled { get; set; }
        public int Cancelled { get; set; }
        public int Expired { get; set; }
    }
}
