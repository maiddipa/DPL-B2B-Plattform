using System;
using System.Collections.Generic;

namespace Dpl.B2b.Dal.Ltms
{
    public partial class SyncMap
    {
        public int Pid { get; set; }
        public int Tid { get; set; }
        public int Bid { get; set; }
        public bool IncludeInBalance { get; set; }
    }
}
