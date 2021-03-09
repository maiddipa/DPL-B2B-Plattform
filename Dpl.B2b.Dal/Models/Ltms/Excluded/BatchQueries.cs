using System;
using System.Collections.Generic;

namespace Dpl.B2b.Dal.Ltms
{
    public partial class BatchQueries
    {
        public Guid Id { get; set; }
        public Guid BatchId { get; set; }
        public int? RowId { get; set; }
        public string RowValue { get; set; }
    }
}
