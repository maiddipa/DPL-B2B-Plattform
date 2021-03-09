using System;
using System.Collections.Generic;

namespace Dpl.B2b.Dal.Ltms
{
    public partial class ImportStacks
    {
        public ImportStacks()
        {
            ImportTransactions = new HashSet<ImportTransactions>();
        }

        public int Id { get; set; }
        public int ImportPackageId { get; set; }
        public DateTime Time { get; set; }
        public string User { get; set; }
        public string FileName { get; set; }

        public virtual ImportPackages ImportPackage { get; set; }
        public virtual ICollection<ImportTransactions> ImportTransactions { get; set; }
    }
}
