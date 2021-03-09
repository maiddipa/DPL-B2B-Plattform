using Dpl.B2b.Common.Enumerations;
using System.Collections.Generic;

namespace Dpl.B2b.Dal.Models
{
    public class LoadCarrierQuality
    {
        public int Id { get; set; }

        public short RefLtmsQualityId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public LoadCarrierQualityType Type { get; set; }

        public float Order { get; set; }

        public virtual ICollection<LoadCarrier> LoadCarriers { get; set; }

        public virtual ICollection<LoadCarrierQualityMapping> Mappings { get; set; }
    }
}