using System;
using System.Collections.Generic;

namespace Dpl.B2b.Dal.Ltms
{
    public partial class ReportStates
    {
        public ReportStates()
        {
            Reports = new HashSet<Reports>();
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Reports> Reports { get; set; }
    }
}
