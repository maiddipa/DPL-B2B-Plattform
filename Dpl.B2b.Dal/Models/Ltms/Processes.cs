using System;
using System.Collections.Generic;

namespace Dpl.B2b.Dal.Ltms
{
    public partial class Processes
    {
        public int Id { get; set; }
        public string ReferenceNumber { get; set; }
        public string Name { get; set; }
        public short ProcessStateId { get; set; }
        public short ProcessTypeId { get; set; }
        public string CreateUser { get; set; }
        public DateTime CreateTime { get; set; }
        public string UpdateUser { get; set; }
        public DateTime? UpdateTime { get; set; }
        public string DeleteUser { get; set; }
        public DateTime? DeleteTime { get; set; }
        public int OptimisticLockField { get; set; }
        public string IntDescription { get; set; }
        public string ExtDescription { get; set; }
        public DateTime? ChangedTime { get; set; }
        
        // HACK Remove when added to LTMS
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]
        public Guid RowGuid { get; set; }

        public virtual ProcessStates ProcessState { get; set; }
        public virtual ProcessTypes ProcessType { get; set; }
        public virtual ICollection<Transactions> Transactions { get; set; }
    }
}
