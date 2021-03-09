using Dpl.B2b.Common.Enumerations;
using System;
using System.Collections.Generic;

namespace Dpl.B2b.Contracts.Models
{
    public class Voucher
    {
        public int Id { get; set; }

        public int DivisionId { get; set; }
        public string DivisionName { get; set; }

        public string IssuerCompanyName { get; set; }

        public int DocumentId { get; set; }
        public string DocumentNumber { get; set; }

        public VoucherType Type { get; set; }
        public VoucherStatus Status { get; set; }

        public DateTime IssuedDate { get; set; }
        public DateTime ValidUntil { get; set; }

        public string LicensePlate { get; set; }
        public string TruckDriverName { get; set; }
        public string TruckDriverCompanyName { get; set; }

        public int RecipientId { get; set; }
        public string Recipient { get; set; }
        public PartnerType RecipientType { get; set; }
        public int? SupplierId { get; set; }
        public string Supplier { get; set; }
        public int? ShipperId { get; set; }
        public string Shipper { get; set; }
        public int? SubShipperId { get; set; }
        public string SubShipper { get; set; }
        public int ReasonTypeId { get; set; }
        public string CancellationReason { get; set; }

        public bool ProcurementLogistics { get; set; }
        public string? CustomerReference { get; set; }

        public IEnumerable<VoucherPosition> Positions { get; set; }
        public IEnumerable<AccountingRecord> AccountingRecords { get; set; }

        public string DownloadLink { get; set; }

        public bool HasDplNote { get; set; }


        public List<EmployeeNote> DplNotes { get; set; }
    }

    public class VoucherPosition
    {
        public int Id { get; set; }
        public int LoadCarrierId { get; set; }
        public int Quantity { get; set; }
    }

    public class VouchersSearchRequest : SortablePaginationRequest<VouchersSearchRequestSortOptions>
    {
        public List<int> CustomerId { get; set; }
        public VoucherType? Type { get; set; }
        public string DocumentNumber { get; set; }
        public string Recipient { get; set; }
        public PartnerType? RecipientType { get; set; }
        public DateTime? ToIssueDate { get; set; }
        public DateTime? FromIssueDate { get; set; }
        public VoucherStatus[] States { get; set; }
        public List<int> ReasonTypes { get; set; }
        public List<int> LoadCarrierTypes { get; set; }
        public string Shipper { get; set; }
        public string SubShipper { get; set; }
        public DateTime? ValidFrom { get; set; }
        public DateTime? ValidTo { get; set; }
        public int? QuantityFrom { get; set; }
        public int? QuantityTo { get; set; }
        //public double[] Options { get; set; }
        public string Supplier { get; set; }
        public bool ProcurementLogistics { get; set; }
        public string? CustomerReference { get; set; }

        public bool? HasDplNote { get; set; }
    }

    public enum VouchersSearchRequestSortOptions
    {
        DocumentNumber,
        IssuedBy,
        IssuanceDate,
        ValidUntil,
        RecipientType,
        Recipient,
        Reason,
        Type,
        Status,
        Supplier,
        SubShipper,
        Shipper,
        Quantity,
        HasDplNote,
        CustomerReference
    }

    public class VouchersCreateRequest
    {        
        public int CustomerDivisionId { get; set; }
        public string ExpressCode { get; set; }

        public int ReasonTypeId { get; set; }
        public VoucherType Type { get; set; }

        public string TruckDriverName { get; set; }
        public string TruckDriverCompanyName { get; set; }

        public string LicensePlate { get; set; }
        public int? LicensePlateCountryId { get; set; }

        public string Note { get; set; }

        public Guid RecipientGuid { get; set; }
        public PartnerType? RecipientType { get; set; } // TODO discuss if we need this field as we do have this information in our DB and can pull it from there

        public Guid SupplierGuid { get; set; }
        public Guid ShipperGuid { get; set; }
        public Guid SubShipperGuid { get; set; }

        public ICollection<VouchersCreateRequestPosition> Positions { get; set; }

        public bool ProcurementLogistics { get; set; }
        public string? CustomerReference { get; set; }

        public int PrintLanguageId { get; set; }
        public int PrintCount { get; set; }
        public int PrintDateTimeOffset { get; set; }
        public EmployeeNoteCreateRequest DplNote { get; set; }
    }

    public class VouchersCreateRequestPosition{
        public int LoadCarrierId { get; set; }
        public int Quantity { get; set; }
    }

    public class VouchersUpdateRequest
    {
        public string Value { get; set; }
    }

    public class VouchersAddToSubmissionRequest
    {
        public int SubmissionId { get; set; }
    }
    public class VouchersRemoveFromSubmissionRequest
    {
        public int SubmissionId { get; set; }
    }

    public class VouchersCancelRequest
    {
        public string Reason { get; set; }
        public EmployeeNoteCreateRequest DplNote { get; set; }
    }

    public class VoucherReasonType
    {
        public int Id { get; set; }

        public int Order { get; set; }
    }
}
