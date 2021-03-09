using System.Collections.Generic;

namespace Dpl.B2b.Dal.Models
{
    public class LoadCarrier
    {
        public int Id { get; set; }

        public int RefLmsQuality2PalletId { get; set; }

        public short RefLtmsPalletId { get; set; }

        //TODO All the name info is dereived from article + quality
        // so by adding name on pallet we are managing duplicate data
        public string Name { get; set; }

        public float Order { get; set; }

        public int TypeId { get; set; }

        public virtual LoadCarrierType Type { get; set; }

        public int QualityId { get; set; }

        public virtual LoadCarrierQuality Quality { get; set; }

        public virtual ICollection<BaseLoadCarrierMapping> BaseLoadCarrierMappings { get; set; }
      }
}