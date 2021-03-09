using System;
using System.Collections.Generic;

namespace Dpl.B2b.Dal.Ltms
{
    public partial class ImportAccountMappings
    {
        public int Id { get; set; }
        public int ImportPackageId { get; set; }
        public string Name1 { get; set; }
        public string Name2 { get; set; }
        public string AccountNumber { get; set; }
        public string CreateUser { get; set; }
        public DateTime CreateTime { get; set; }
        public string UpdateUser { get; set; }
        public DateTime? UpdateTime { get; set; }

        public virtual ImportPackages ImportPackage { get; set; }
    }
}
