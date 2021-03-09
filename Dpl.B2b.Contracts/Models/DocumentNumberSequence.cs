using Dpl.B2b.Common.Enumerations;

namespace Dpl.B2b.Contracts.Models
{
    public class DocumentNumberSequence
    {
        public int Id { get; set; }

        //public int CustomerDivisionId { get; set; }

        public DocumentTypeEnum DocumentType { get; set; }

        // [Description]
        public string DisplayName { get; set; }

        //[CurrentNumberLevel]
        public int Counter { get; set; }

        //[Prefix]
        public string Prefix { get; set; }

        // [PrefixSeparator]
        public string SeparatorAfterPrefix { get; set; }

        // [UseDocumentTypeCode]
        public bool CanAddDocumentTypeShortName { get; set; }

        // [LeftHandZerosForCustomerNr]
        public bool CanAddPaddingForCustomerNumber { get; set; }

        //[LengthOfCustomerNr]
        public int PaddingLengthForCustomerNumber { get; set; }

        // [Separator1]
        public string PostfixForCustomerNumber { get; set; }

        // [UseDivisionCode]
        public bool CanAddDivisionShortName { get; set; }

        //[Separator2]
        public string PrefixForCounter { get; set; }

        // [LeftHandZerosForSequentialNumber]
        public bool CanAddPaddingForCounter { get; set; }

        //[LengthOfSequentialNumber]
        public int PaddingLengthForCounter { get; set; }
        public string Separator { get; set; }
        public int CustomerId { get; set; }
    }

    public class DocumentNumberSequenceSearchRequest
    {
        public int? CustomerId { get; set; }
    }
    
    public class DocumentNumberSequenceCreateRequest
    {
        public DocumentTypeEnum DocumentType { get; set; }
        public string DisplayName { get; set; }
        public int Counter { get; set; }
        public string Prefix { get; set; }
        public string SeparatorAfterPrefix { get; set; }
        public bool CanAddDocumentTypeShortName { get; set; }
        public bool CanAddPaddingForCustomerNumber { get; set; }
        public int PaddingLengthForCustomerNumber { get; set; }
        public string PostfixForCustomerNumber { get; set; }
        public bool CanAddDivisionShortName { get; set; }
        public string PrefixForCounter { get; set; }
        public bool CanAddPaddingForCounter { get; set; }
        public int PaddingLengthForCounter { get; set; }
        public string Separator { get; set; }
        public int CustomerId { get; set; }
    }
    
    public class DocumentNumberSequenceDeleteRequest
    {
        public int Id { get; set; }
    }
}