using Dpl.B2b.Common.Enumerations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dpl.B2b.Dal.Models
{
    public class DocumentTemplate : OlmaAuditable
    {
        public int Id { get; set; }

        public int? CustomerId { get; set; }
        public virtual Customer Customer { get; set; }

        public Boolean IsDefault { get; set; }

        public PrintType PrintType { get; set; }

        public int Version { get; set; }

        public byte[] Data { get; set; }

        public virtual ICollection<DocumentTemplateLabel> Labels { get; set; }
    }
}
