using System;
using System.Collections.Generic;

namespace Dpl.B2b.Dal.Ltms
{
    public partial class Articles
    {
        public short Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Order { get; set; }

        public virtual ICollection<Bookings> Bookings { get; set; }
        public virtual ICollection<Pallet> Pallet { get; set; }
    }
}
