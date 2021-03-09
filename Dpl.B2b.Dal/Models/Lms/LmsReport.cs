using System;
using System.Collections.Generic;

namespace Dpl.B2b.Dal.Models.Lms
{
    public partial class LmsReport
    {
        public int ReportId { get; set; }
        public string Name { get; set; }
        public string Statement { get; set; }
        public string Parameter { get; set; }
    }
}
