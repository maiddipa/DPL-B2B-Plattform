using System;
using System.Collections.Generic;

namespace Dpl.B2b.Dal.Models
{
    public class SortingShiftLog : OlmaAuditable
    {
        public int Id { get; set; }

        public int CustomerId { get; set; }
        public virtual Customer Customer { get; set; }

        public DateTime FromDateTime { get; set; }
        public DateTime ToDateTime { get; set; }

        public string Note { get; set; }

        public int ResponsibleUserId { get; set; }
        public virtual User ResponsibleUser { get; set; }

        public int SortingWorkerId { get; set; }
        public virtual SortingWorker SortingWorker { get; set; }

        public virtual ICollection<SortingShiftLogPosition> Positions { get; set; }
    }
}