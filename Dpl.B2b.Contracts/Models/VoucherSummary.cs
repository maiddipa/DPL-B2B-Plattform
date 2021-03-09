using System;
using System.Collections.Generic;
using System.Text;

namespace Dpl.B2b.Contracts.Models
{
    public class VoucherSummary
    {
        public int LoadCarrierTypeId { get; set; }
        public VoucherAggregate Sum { get; set; }

        public VoucherAggregate Count { get; set; }
        public float Order { get; set; }
    }

    public class VoucherAggregate
    {
        public int Issued { get; set; }
        public int Submitted { get; set; }
        public int Accounted { get; set; }
        public int Canceled { get; set; }
        public int Expired { get; set; }

    }
}
