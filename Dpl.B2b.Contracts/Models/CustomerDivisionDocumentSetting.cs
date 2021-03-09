using Dpl.B2b.Common.Enumerations;

namespace Dpl.B2b.Contracts.Models
{
    public class CustomerDivisionDocumentSetting
    {
        public int Id { get; set; }
        public int DivisionId { get; set; }
        public DocumentType DocumentType { get; set; }
        public DocumentNumberSequence DocumentNumberSequence { get; set; }
        public int PrintCountMin { get; set; }
        public int PrintCountMax { get; set; }
        public int DefaultPrintCount { get; set; }

        public bool Override { get; set; }
    }
    
    public class DocumentType
    {
        public int Id { get; set; }

        public DocumentTypeEnum Type { get; set; }

        public string Name { get; set; }

        public string ShortName { get; set; }

        public bool HasReport { get; set; }

        public PrintType PrintType { get; set; }

        public int OriginalAvailableForMinutes { get; set; }
    }
    
    public class CustomerDivisionDocumentSettingSearchRequest
    {
        public int? CustomerDivisionId { get; set; }
    }

    public class CustomerDivisionDocumentSettingCreateRequest
    {
        public int DivisionId { get; set; }
        public int DocumentTypeId { get; set; }
        public int DocumentNumberSequenceId { get; set; }
        public int PrintCountMin { get; set; }
        public int PrintCountMax { get; set; }
        public int DefaultPrintCount { get; set; }
        public bool Override { get; set; }
    }
    
    public class CustomerDivisionDocumentSettingDeleteRequest
    {
        public int Id { get; set; }
    }
}