using System;
using System.Collections.Generic;

namespace Dpl.B2b.Dal.Ltms
{
    public partial class PalletChanges
    {
        public int Id { get; set; }
        public short ChangeFromId { get; set; }
        public short ChangeToId { get; set; }
        public int PalletChangeSetId { get; set; }

        public virtual Pallet ChangeFrom { get; set; }
        public virtual Pallet ChangeTo { get; set; }
        public virtual PalletChangeSets PalletChangeSet { get; set; }
    }
}
