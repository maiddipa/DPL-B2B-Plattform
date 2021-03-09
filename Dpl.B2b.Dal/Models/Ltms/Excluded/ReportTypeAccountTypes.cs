using System;
using System.Collections.Generic;

namespace Dpl.B2b.Dal.Ltms
{
    public partial class ReportTypeAccountTypes
    {
        public string ReportTypeId { get; set; }
        public string AccountTypeId { get; set; }

        public virtual AccountTypes AccountType { get; set; }
        public virtual ReportTypes ReportType { get; set; }
    }
}
