namespace Dpl.B2b.Dal.Models
{
    public class CustomerDocumentSetting : OlmaAuditable
    {
        public int Id { get; set; }

        public int? CustomerId { get; set; }
        public virtual Customer Customer { get; set; }

        public int DocumentTypeId { get; set; }
        public virtual DocumentType DocumentType { get; set; }

        public int? LoadCarrierTypeId { get; set; }
        public virtual LoadCarrierType LoadCarrierType { get; set; }

        public int? ThresholdForWarningQuantity { get; set; }

        public int? MaxQuantity { get; set; }
        
        // in minutes;
        public int? CancellationTimeSpan { get; set; }
        public int? ValidForMonths{ get; set; }
    }
}