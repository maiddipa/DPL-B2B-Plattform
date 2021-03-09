using System;
using System.Collections.Generic;

namespace Dpl.B2b.Dal.Ltms
{
    public partial class Terms
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string InvoiceDescription { get; set; }
        public string Unit { get; set; }
        public int GuiOrder { get; set; }
        public int InvoiceOrder { get; set; }
        public bool InvisibleIf0 { get; set; }
        public bool IsDecimal { get; set; }
        public int DecimalPlaces { get; set; }
        public string Description { get; set; }
        public string FeeTitle { get; set; }
        public string ExtFeeTitle { get; set; }
        public string BookingTypeId { get; set; }
        public short? KeySignature { get; set; }
        public bool? IsQuantityDependent { get; set; }
        public bool? CanBeManuallyAdded { get; set; }
        public bool? MatchAllBookingTypes { get; set; }
        public int? LowerLimit { get; set; }
        public int? UpperLimit { get; set; }
        public int? Interval { get; set; }
        public string IntervalOption { get; set; }
        public string ChargeOption { get; set; }
        public string Discriminator { get; set; }
        public string ChargeWithId { get; set; }
        public int? SwapAccountId { get; set; }
        public int? SwapRoundingType { get; set; }
        public string SwapInBookingTypeId { get; set; }
        public short? SwapInPalletId { get; set; }
        public string SwapOutBookingTypeId { get; set; }
        public short? SwapOutPalletId { get; set; }

        public virtual BookingTypes BookingType { get; set; }
        public virtual Terms ChargeWith { get; set; }
        public virtual Accounts SwapAccount { get; set; }
        public virtual BookingTypes SwapInBookingType { get; set; }
        public virtual Pallet SwapInPallet { get; set; }
        public virtual BookingTypes SwapOutBookingType { get; set; }
        public virtual Pallet SwapOutPallet { get; set; }
        public virtual ICollection<Conditions> Conditions { get; set; }
        public virtual ICollection<Fees> Fees { get; set; }
        public virtual ICollection<Terms> InverseChargeWith { get; set; }
    }
}
