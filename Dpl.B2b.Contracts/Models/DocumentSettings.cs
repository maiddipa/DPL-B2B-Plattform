using System.Collections.Generic;

namespace Dpl.B2b.Contracts.Models
{
    public class DocumentSettings
    {
        public IEnumerable<CustomerDocumentSettings> Customers { get; set; }
        public IEnumerable<CustomerDivisionDocumentSettings> Divisions { get; set; }
    }

    public class CustomerDocumentSettings
    {
        public int Id { get; set; }
        public int DocumentTypeId { get; set; }
        public int? LoadCarrierTypeId { get; set; }
        public int? ThresholdForWarningQuantity { get; set; }
        public int? MaxQuantity { get; set; }
        public int? CancellationTimeSpan { get; set; }
    }

    public class CustomerDivisionDocumentSettings
    {
        public int Id { get; set; }
        public int DivisionId { get; set; }
        public int DocumentTypeId { get; set; }

        public int DefaultPrintCount { get; set; }
        public int PrintCountMin { get; set; }
        public int PrintCountMax { get; set; }
        public bool Override { get; set; }
    }
    public class CustomerDocumentSettingSearchRequest
    {
        public int? CustomerId { get; set; }
    }

    public class CustomerDocumentSettingCreateRequest
    {
        public int CustomerId { get; set; }
        public int DocumentTypeId { get; set; }
        public int? LoadCarrierTypeId { get; set; }
        public int? ThresholdForWarningQuantity { get; set; }
        public int? MaxQuantity { get; set; }
        public int? CancellationTimeSpan { get; set; }
    }
    
    public class CustomerDocumentSettingDeleteRequest
    {
        public int Id { get; set; }
    }
    

    public class CustomerDocumentSettingsUpdateRequest
    {
        public int ThresholdForWarningQuantity { get; set; }

        public int MaxQuantity { get; set; }
        
        public int CancellationTimeSpan { get; set; }
    }

    public class CustomerDivisionDocumentSettingsUpdateRequest
    {
        public int DefaultPrintCount { get; set; }
        public int PrintCountMin { get; set; }
        public int PrintCountMax { get; set; }
        public bool Override { get; set; }
    }
}
