using System;
using System.Collections.Generic;

namespace Dpl.B2b.Dal.Ltms
{
    public partial class PalletChangeTriggerPalletChangeDefinitions
    {
        public int PalletChangeTriggerId { get; set; }
        public int PalletChangeDefinitionId { get; set; }

        public virtual PalletChangeDefinitions PalletChangeDefinition { get; set; }
        public virtual PalletChangeTriggers PalletChangeTrigger { get; set; }
    }
}
