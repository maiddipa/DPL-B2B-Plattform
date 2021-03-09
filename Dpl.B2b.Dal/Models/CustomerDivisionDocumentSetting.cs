namespace Dpl.B2b.Dal.Models
{
    public class CustomerDivisionDocumentSetting : OlmaAuditable
    {
        public int Id { get; set; }

        public int DivisionId { get; set; }
        public virtual CustomerDivision Division { get; set; }

        public int DocumentTypeId { get; set; }
        public virtual DocumentType DocumentType { get; set; }

        public int DocumentNumberSequenceId { get; set; }
        public virtual DocumentNumberSequence DocumentNumberSequence { get; set; }

        public int PrintCountMin { get; set; }
        public int PrintCountMax { get; set; }
        public int DefaultPrintCount { get; set; }

        public bool Override { get; set; }
    }
}