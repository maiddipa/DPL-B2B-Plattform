﻿using Dpl.B2b.Common.Enumerations;

namespace Dpl.B2b.Dal.Models
{
    // TODO Use Table schema from DPLPooling Admin [dbo].[DocumentNumberingScheme]
    public class DocumentNumberSequence : OlmaAuditable
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

    public enum DocumentNumberSequenceType
    {
        Voucher = 0,
        Receipt = 1
        // OLD Abholauftrag = 1,
        // OLD Schuldschein = 2,
    }
}