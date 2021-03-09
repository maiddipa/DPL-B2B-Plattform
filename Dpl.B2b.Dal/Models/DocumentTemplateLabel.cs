using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dpl.B2b.Dal.Models
{
    public class DocumentTemplateLabel
    {
        public int Id { get; set; }

        public int DocumentTemplateId { get; set; }

        public int LanguageId { get; set; }
        public virtual LocalizationLanguage Language { get; set; }

        public string Label { get; set; }

        public string Text { get; set; }

        public int Version { get; set; }
    }
}
