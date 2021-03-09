using Dpl.B2b.Common.Enumerations;
using System.Collections.Generic;

namespace Dpl.B2b.Contracts.Models
{
    public class LoadCarrier
    {
        public int Id { get; set; }

        public float Order { get; set; }

        public LoadCarrierType Type { get; set; }

        public LoadCarrierQuality Quality { get; set; }
    }

    public class LoadCarrierType
    {
        public int Id { get; set; }
        public float Order { get; set; }
        public int QuantityPerEur { get; set; }

        public BaseLoadCarrierInfo BaseLoadCarrier { get; set; }

        public IEnumerable<LoadCarrier> BaseLoadCarriers { get; set; }

        public short RefLtmsArticleId { get; set; }
    }

    public class LoadCarrierQuality
    {
        public int Id { get; set; }
        public float Order { get; set; }
        public string Name { get; set; }
        public LoadCarrierQualityType Type { get; set; }
    }
}
