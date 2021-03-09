using System;
using System.Collections.Generic;

namespace Dpl.B2b.Dal.Ltms
{
    public partial class VDpg
    {
        public Guid? LtmsDpgId { get; set; }
        public int TransactionId { get; set; }
        public string SubmissionNumber { get; set; }
        public string ReferenceNumber { get; set; }
        public string BookingTypeId { get; set; }
        public string AccountNumber { get; set; }
        public DateTime Valuta { get; set; }
        public DateTime BookingDate { get; set; }
        public string Pallet { get; set; }
        public int? Quantity { get; set; }
    }
}
