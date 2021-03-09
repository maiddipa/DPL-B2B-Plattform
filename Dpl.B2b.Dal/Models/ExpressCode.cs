using System;
using System.Collections.Generic;
using Dpl.B2b.Common.Enumerations;

namespace Dpl.B2b.Dal.Models
{
    public class ExpressCode : OlmaAuditable
    {
        public int Id { get; set; }
        public string DigitalCode { get; set; }
        public string CustomBookingText { get; set; }
        public DateTime? ValidFrom { get; set; }
        public DateTime? ValidTo { get; set; }
        public int? ValidDischarges { get; set; }
        public bool IsCanceled { get; set; }
        public int? IssuingCustomerId { get; set; }
        public int? VoucherReasonTypeId { get; set; }
        public VoucherType? VoucherType { get; set; }
        public virtual Customer IssuingCustomer { get; set; }
        public int? IssuingDivisionId { get; set; }
        public virtual CustomerDivision IssuingDivision { get; set; }
        public virtual ICollection<PartnerPreset> PartnerPresets { get; set; }
        public virtual PostingAccountPreset PostingAccountPreset { get; set; }
    }
}