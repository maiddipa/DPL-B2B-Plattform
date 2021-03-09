using System;
using System.Collections.Generic;

namespace Dpl.B2b.Dal.Ltms
{
    public partial class ImportPackages
    {
        public ImportPackages()
        {
            ImportAccountMappings = new HashSet<ImportAccountMappings>();
            ImportStacks = new HashSet<ImportStacks>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual ICollection<ImportAccountMappings> ImportAccountMappings { get; set; }
        public virtual ICollection<ImportStacks> ImportStacks { get; set; }
    }
}
