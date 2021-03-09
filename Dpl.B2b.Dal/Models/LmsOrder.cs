using Dpl.B2b.Dal.Models;
using System;
using Microsoft.EntityFrameworkCore.Design;
using Dpl.B2b.Common.Enumerations;

namespace Dpl.B2b.Dal.Models
{
    public partial class LmsOrder
    {
        public int Id { get; set; }
        public Guid RowGuid { get; set; }

        public OrderType Type { get; set; }

        public int Quantity { get; set; }
        public int AvailableQuantity { get; set; }

        public int? StackHeightMin { get; set; }
        public int? StackHeightMax { get; set; }

        public DateTime? FromDate { get; set; }
        public DateTime? UntilDate { get; set; }

        public int LoadingLocationId { get; set; }
        public virtual LoadingLocation LoadingLocation { get; set; }

        public int? OlmaOrderId { get; set; }
        public virtual Order OlmaOrder { get; set; }

        public int LoadCarrierId { get; set; }
        public int? BaseLoadCarrierId { get; set; }

        public bool IsPermanent { get; set; }
        public bool IsAvailable { get; set; }
        public bool ShowOnline { get; set; }

        public bool? Geblockt { get; set; }
        public string GeblocktFuer { get; set; }

        public bool SupportsPartialMatching { get; set; }
        public bool SupportsRearLoading { get; set; }
        public bool SupportsSideLoading { get; set; }
        public bool SupportsJumboVehicles { get; set; }
    }
}