using Dpl.B2b.Common.Enumerations;
using System.Collections.Generic;

namespace Dpl.B2b.Dal.Models
   {
   public class LoadCarrierType
      {
      public int Id { get; set; }

      public short RefLtmsArticleId { get; set; }
      public int RefLmsLoadCarrierTypeId { get; set; }

      public string Name { get; set; }

      public string Description { get; set; }

      public float Order { get; set; }

      //ToDo: ?
      public int QuantityPerEur { get; set; }

      public int MaxStackHeight { get; set; }
      public int MaxStackHeightJumbo { get; set; }

      public virtual BaseLoadCarrierInfo BaseLoadCarrier { get; set; }
      public virtual ICollection<BaseLoadCarrierMapping> BaseLoadCarrierMappings { get; set; }
      }

   public class BaseLoadCarrierMapping
      {
      public int LoadCarrierId { get; set; }
      public virtual LoadCarrier LoadCarrier { get; set; }

      public int LoadCarrierTypeId { get; set; }
      public virtual LoadCarrierType LoadCarrierType { get; set; }
      }
   }