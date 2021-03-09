using Dpl.B2b.Common.Enumerations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dpl.B2b.Dal.Models
{
    public class EmployeeNote : OlmaAuditable
    {
        public int Id { get; set; }

        public EmployeeNoteReason Reason { get; set; }
        public EmployeeNoteType Type { get; set; }

        public int UserId { get; set; }
        public virtual User User { get; set; }
        
        public string Contact { get; set; }
        public DateTime ContactedAt { get; set; }

        public string Text { get; set; }
    }

}
