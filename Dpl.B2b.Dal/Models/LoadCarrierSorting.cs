using System.Collections.Generic;
using Dpl.B2b.Contracts.Models;

namespace Dpl.B2b.Dal.Models
{
    public class LoadCarrierSorting : OlmaAuditable
    {
        public int Id { get; set; }
        
        public int  LoadCarrierReceiptId { get; set; }
        public LoadCarrierReceipt LoadCarrierReceipt { get; set; }

        public ICollection<LoadCarrierSortingResult> Positions { get; set; }
    }

    public class LoadCarrierSortingResult 
    {
        public int Id { get; set; }

        public int LoadCarrierSortingId { get; set; }
        public LoadCarrierSorting LoadCarrierSorting { get; set; }
        public int LoadCarrierId { get; set; }
        public LoadCarrier LoadCarrier { get; set; }

        public int InputQuantity { get; set; }

        public int? RemainingQuantity { get; set; }

        public ICollection<LoadCarrierSortingResultOutput> Outputs { get; set; }
    }

    public class LoadCarrierSortingResultOutput
    {
        public int Id { get; set; }

        public int LoadCarrierSortingResultId { get; set; }
        public LoadCarrierSortingResult LoadCarrierSortingResult { get; set; }

        public int LoadCarrierQuantity { get; set; }

        public int LoadCarrierId  { get; set; }
        public LoadCarrier LoadCarrier { get; set; }

    }
}