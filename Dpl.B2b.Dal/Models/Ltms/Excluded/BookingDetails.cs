using System;
using System.Collections.Generic;

namespace Dpl.B2b.Dal.Ltms
{
    public partial class BookingDetails
    {
        public long Id { get; set; }
        public string FieldName { get; set; }
        public string FieldValue { get; set; }
        public int OptimisticLockField { get; set; }
        public int BookingId { get; set; }
        public DateTime? FieldDate { get; set; }
        public int? FieldInt { get; set; }
        public decimal? FieldDecimal { get; set; }
        public bool? FieldBool { get; set; }
        public string FieldText { get; set; }
        public Guid? FieldGuid { get; set; }
        public string FieldType { get; set; }

        public virtual Bookings Booking { get; set; }
    }
}
