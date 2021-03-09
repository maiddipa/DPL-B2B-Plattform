using System;
using System.Collections.Generic;

namespace Dpl.B2b.Dal.Ltms
{
    public partial class Repair
    {
        public int TransactionId { get; set; }
        public string Bsn { get; set; }
        public bool? Map { get; set; }
        public int? Minid { get; set; }
        public bool? Del { get; set; }
    }
}
