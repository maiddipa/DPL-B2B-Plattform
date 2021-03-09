using System;
using System.Collections.Generic;

namespace Dpl.B2b.Dal.Ltms
{
    public partial class Fees
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public string InvoiceNumber { get; set; }
        public string TermId { get; set; }
        public int? ConditionId { get; set; }
        public string CreateUser { get; set; }
        public DateTime CreateTime { get; set; }
        public string UpdateUser { get; set; }
        public DateTime? UpdateTime { get; set; }
        public string DeleteUser { get; set; }
        public DateTime? DeleteTime { get; set; }
        public int? AccountId { get; set; }
        public int? BookingId { get; set; }
        public byte FeeType { get; set; }

        public virtual Accounts Account { get; set; }
        public virtual Bookings Booking { get; set; }
        public virtual Conditions Condition { get; set; }
        public virtual Terms Term { get; set; }
    }
}
