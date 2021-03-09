using System;
using System.Collections.Generic;

namespace Dpl.B2b.Dal.Ltms
{
    public partial class VBookings1
    {
        public int Pid { get; set; }
        public int Tid { get; set; }
        public int Bid { get; set; }
        public string UserTask { get; set; }
        public string BookingTypeId { get; set; }
        public string ReferenceNumber { get; set; }
        public string Bsn { get; set; }
        public string AccountNumber { get; set; }
        public int AccountId { get; set; }
        public DateTime Valuta { get; set; }
        public DateTime BookingDate { get; set; }
        public bool IncludeInBalance { get; set; }
        public string Pallet { get; set; }
        public int? Quantity { get; set; }
        public string Ls { get; set; }
        public string We { get; set; }
        public string Extau { get; set; }
        public string Ab { get; set; }
        public bool? Storniert { get; set; }
        public DateTime CreateTime { get; set; }
        public string CreateUser { get; set; }
        public DateTime? UpdateTime { get; set; }
        public string UpdateUser { get; set; }
        public string Memo { get; set; }
    }
}
