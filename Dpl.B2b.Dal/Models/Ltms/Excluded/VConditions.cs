using System;
using System.Collections.Generic;

namespace Dpl.B2b.Dal.Ltms
{
    public partial class VConditions
    {
        public int Id { get; set; }
        public DateTime ValidFrom { get; set; }
        public string CreateUser { get; set; }
        public DateTime CreateTime { get; set; }
        public string UpdateUser { get; set; }
        public DateTime? UpdateTime { get; set; }
        public int AccountId { get; set; }
        public string TermId { get; set; }
        public string Note { get; set; }
        public DateTime? ValidUntil { get; set; }
        public short? MatchingOrder { get; set; }
        public int? InvoicingDay { get; set; }
        public string Discriminator { get; set; }
        public int? Quantity { get; set; }
        public decimal? Amount { get; set; }
        public short ArticleId { get; set; }
        public short? QualityId { get; set; }
    }
}
