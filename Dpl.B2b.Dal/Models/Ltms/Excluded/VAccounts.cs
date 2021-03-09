using System;
using System.Collections.Generic;

namespace Dpl.B2b.Dal.Ltms
{
    public partial class VAccounts
    {
        public int Id { get; set; }
        public Guid RowGuid { get; set; }
        public string Name { get; set; }
        public string ExtName { get; set; }
        public int? ParentId { get; set; }
        public int? AddressId { get; set; }
        public string Description { get; set; }
        public string PathByName { get; set; }
        public string CreateUser { get; set; }
        public DateTime CreateTime { get; set; }
        public string UpdateUser { get; set; }
        public DateTime? UpdateTime { get; set; }
        public int OptimisticLockField { get; set; }
    }
}
