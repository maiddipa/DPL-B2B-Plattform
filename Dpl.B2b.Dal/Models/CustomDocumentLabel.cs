﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dpl.B2b.Dal.Models
{
    public class CustomDocumentLabel
    {
        public int Id { get; set; }
        public int? CustomerId { get; set; }
        public int DocumentTemplateId { get; set; }
        public int LanguageId { get; set; }
        public string ReportLabel { get; set; }
        public string UiLabel { get; set; }
        public string Text { get; set; }
    }
}
