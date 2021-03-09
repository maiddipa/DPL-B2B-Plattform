using System;
using System.Collections.Generic;
using Dpl.B2b.Common.Enumerations;
using Microsoft.EntityFrameworkCore;

namespace Dpl.B2b.Dal.Models
{
    public class OrderCondition : OlmaAuditable
    {
        public int Id { get; set; }

        public int PostingAccountId { get; set; }

        public OrderType OrderType { get; set; }

        public DayOfWeek LatestDeliveryDayOfWeek { get; set; }

        public virtual ICollection<LoadCarrierCondition> LoadCarrierConditions { get; set; }
    }

    public class LoadCarrierCondition
    {
        public int Id { get; set; }

        public int LoadCarrierId { get; set; }
        public virtual LoadCarrier LoadCarrier { get; set; }

        public int MinQuantity { get; set; }

        public int MaxQuantity { get; set; }
    }
}