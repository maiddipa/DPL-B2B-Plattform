using System;
using System.Collections.Generic;

namespace Dpl.B2b.Dal.Ltms
{
    public partial class Pallet
    {
        public short Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public short ArticleId { get; set; }
        public short QualityId { get; set; }
        public int Order { get; set; }

        public virtual Articles Article { get; set; }
        public virtual Qualities Quality { get; set; }
        public virtual ICollection<Terms> TermsSwapInPallet { get; set; }
        public virtual ICollection<Terms> TermsSwapOutPallet { get; set; }
    }
}
