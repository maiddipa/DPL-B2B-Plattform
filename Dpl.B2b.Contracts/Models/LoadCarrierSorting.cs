using System.Collections.Generic;

namespace Dpl.B2b.Contracts.Models
{
    public class LoadCarrierSorting
    {
        public int Id { get; set; }
        public int LoadCarrierReceiptId { get; set; }
        public IEnumerable<LoadCarrierSortingResult> Positions { get; set; }

    }

    public class LoadCarrierSortingCreateRequest
    {
        public int LoadCarrierReceiptId { get; set; }
        public IEnumerable<LoadCarrierSortingResult> Positions { get; set; }

    }

    public class LoadCarrierSortingResult
    {
        public int LoadCarrierId { get; set; }
        public IEnumerable<LoadCarrierSortingResultOutput> Outputs { get; set; }

    }

    public class LoadCarrierSortingResultOutput
    {
        public int LoadCarrierQualityId { get; set; }
        public int Quantity { get; set; }
    }
}