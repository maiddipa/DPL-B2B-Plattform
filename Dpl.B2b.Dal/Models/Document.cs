using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Dpl.B2b.Common.Enumerations;

namespace Dpl.B2b.Dal.Models
{
    // TODO do we need a link to a file?
    public class Document : OlmaAuditable
    {
        public int Id { get; set; }

        public string Number { get; set; } //generated based on number sequence

        public int TypeId { get; set; }
        public virtual DocumentType Type { get; set; }

        public int StateId { get; set; }
        public virtual DocumentState State { get; set; }

        public int LanguageId { get; set; }
        public virtual LocalizationLanguage Language { get; set; }

        public DateTime IssuedDateTime { get; set; }
        public string CancellationReason { get; set; }
        public DateTime? LtmsImportDateTime { get; set; } //TODO: Discuss if Property can be moved to Voucher

        public int CustomerDivisionId { get; set; } // TODO remove division ids from document as they cannot always be set (order match)
        public virtual CustomerDivision CustomerDivision { get; set; }

        public virtual Voucher Voucher { get; set; }
        
        public virtual LoadCarrierReceipt LoadCarrierReceipt { get; set; }

        public virtual OrderMatch OrderMatch { get; set; }

        public virtual ICollection<DocumentFile> Files { get; set; }
    }

    public class DocumentFile
    {
        public int Id { get; set; }

        public DocumentFileType FileType { get; set; }
       
        public int FileId { get; set; }
        public virtual File File { get; set; }
    }
}