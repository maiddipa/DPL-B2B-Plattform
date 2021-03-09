using System;
using System.Collections.Generic;

namespace Dpl.B2b.Dal.Ltms
{
    public partial class VBookings
    {
        public int ProcessId { get; set; }
        public string ProcessName { get; set; }
        public string ProcessState { get; set; }
        public string ProcessType { get; set; }
        public string AccountTypeId { get; set; }
        public string Name { get; set; }
        public string PathByName { get; set; }
        public int CustomerAccountId { get; set; }
        public int? CustomerAccountParentId { get; set; }
        public string CustomerAccountDescription { get; set; }
        public int? AddressId { get; set; }
        public int TransactionsId { get; set; }
        public DateTime Valuta { get; set; }
        public string TransactionReferenceNumber { get; set; }
        public string TransactionIntDescription { get; set; }
        public string TransactionExtDescription { get; set; }
        public int BookingId { get; set; }
        public int TransactionId { get; set; }
        public DateTime BookingDate { get; set; }
        public string CarrierName { get; set; }
        public short ArticleId { get; set; }
        public short QualityId { get; set; }
        public int? Quantity { get; set; }
        public string BookingReferenceNumber { get; set; }
        public bool IncludeInBalance { get; set; }
        public bool Matched { get; set; }
        public string BookingExtDescription { get; set; }
        public decimal? Amount { get; set; }
    }
}
