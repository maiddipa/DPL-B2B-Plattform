using System;
using System.Collections.Generic;

namespace Dpl.B2b.Dal.Ltms
{
    public partial class PalletChangeDefinitions
    {
        public PalletChangeDefinitions()
        {
            PalletChangeDefinitionDetails = new HashSet<PalletChangeDefinitionDetails>();
            PalletChangeTriggerPalletChangeDefinitions = new HashSet<PalletChangeTriggerPalletChangeDefinitions>();
        }

        public int Id { get; set; }
        public bool Enabled { get; set; }
        public bool? IsDefault { get; set; }
        public string Name { get; set; }
        public int Factor { get; set; }
        public bool CalculateFee { get; set; }
        public string Description { get; set; }
        public int PalletChangeCopyDefinitionId { get; set; }
        public string AccountDirectionId { get; set; }
        public int AccountId { get; set; }
        public int? OppositeAccoutId { get; set; }
        public int SetId { get; set; }
        public string BookingTypeFromId { get; set; }
        public string BookingTypeToId { get; set; }
        public int Discriminator { get; set; }
        public bool Automatically { get; set; }
        public bool DeleteOrphaned { get; set; }

        public virtual Accounts Account { get; set; }
        public virtual AccountDirections AccountDirection { get; set; }
        public virtual BookingTypes BookingTypeFrom { get; set; }
        public virtual BookingTypes BookingTypeTo { get; set; }
        public virtual Accounts OppositeAccout { get; set; }
        public virtual PalletChangeCopyDefinitions PalletChangeCopyDefinition { get; set; }
        public virtual PalletChangeSets Set { get; set; }
        public virtual ICollection<PalletChangeDefinitionDetails> PalletChangeDefinitionDetails { get; set; }
        public virtual ICollection<PalletChangeTriggerPalletChangeDefinitions> PalletChangeTriggerPalletChangeDefinitions { get; set; }
    }
}
