using System;
using System.Collections.Generic;

namespace Dpl.B2b.Dal.Ltms
{
    public partial class PalletChangeTriggers
    {
        public PalletChangeTriggers()
        {
            PalletChangeTriggerPalletChangeDefinitions = new HashSet<PalletChangeTriggerPalletChangeDefinitions>();
        }

        public int Id { get; set; }
        public string BookingTypeId { get; set; }
        public string BookingTypeOppositeSideId { get; set; }
        public int? AccountOppositeSideId { get; set; }

        public virtual Accounts AccountOppositeSide { get; set; }
        public virtual BookingTypes BookingType { get; set; }
        public virtual BookingTypes BookingTypeOppositeSide { get; set; }
        public virtual ICollection<PalletChangeTriggerPalletChangeDefinitions> PalletChangeTriggerPalletChangeDefinitions { get; set; }
    }
}
