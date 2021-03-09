using System;
using Dpl.B2b.Common.Enumerations;

namespace Dpl.B2b.Contracts.Models
{
    public class ExpressCode
    {
        public int Id { get; set; }
        public string DigitalCode { get; set; }
        public string BookingText { get; set; }
        public DateTime? ValidFrom { get; set; }
        public DateTime? ValidTo { get; set; }
        public int? ValidDischarges { get; set; }
        public bool IsCanceled { get; set; }
        public int? VoucherReasonTypeId { get; set; }
        public VoucherType? VoucherType { get; set; }
        public string IssuingDivisionName { get; set; }
        public VoucherExpressCodeDetails VoucherPresets { get; set; }
        public DestinationAccountPreset DestinationAccountPreset { get; set; }

        public LoadCarrierReceiptPreset LoadCarrierReceiptPreset { get; set; }
    }

    public class LoadCarrierReceiptPreset
    {
        public LoadCarrierReceiptType? Type { get; set; }
        public int LoadCarrierId { get; set; }
        public int LoadCarrierQuantity { get; set; }
        public int? BaseLoadCarrierId { get; set; }
        public int? BaseLoadCarrierQuantity { get; set; }
        public string DeliveryNoteNumber { get; set; }
        public string PickupNoteNumber { get; set; }
        public int? RefLmsBusinessTypeId { get; set; }
        public int PostingAccountId { get; set; }
        public DateTime? PlannedFulfillmentDateTime { get; set; }
        public Guid? RefLtmsTransactionRowGuid { get; set; }
    }

    public class DestinationAccountPreset
    {
        public int PostingAccountId { get; set; }
        public int? LoadCarrierId { get; set; }
        public int? LoadCarrierQuantity { get; set; }
    }

    public class VoucherExpressCodeDetails
    {
        public CustomerPartner Recipient { get; set; }
        public CustomerPartner Shipper { get; set; }
        public CustomerPartner SubShipper { get; set; }
        public CustomerPartner Supplier { get; set; }
    }

    public class ExpressCodesSearchRequest : PaginationRequest
    {
        public int? IssuingPostingAccountId { get; set; }
        public int IssuingCustomerDivisionId { get; set; }
        public PrintType? PrintType { get; set; }
        public string IssuingDivisionName { get; set; }
        public string ExpressCode { get; set; }
    }

    public class ExpressCodeCreateRequest
    {
        public string BookingText { get; set; }
        public int? ValidDischarges { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime? ValidUntil { get; set; }
    }

    public class ExpressCodeUpdateRequest
    {
        public string BookingText { get; set; }

        public DateTime ValidUntil { get; set; }

        public int? ValidDischarges { get; set; }
    }

    public class ExpressCodeCancelRequest
    {
        public bool IsCanceled { get; set; }
    }
}