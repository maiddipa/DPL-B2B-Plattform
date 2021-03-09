using Dpl.B2b.Common.Enumerations;
using System;
using System.Collections.Generic;

namespace Dpl.B2b.Contracts.Models
{
    public class LoadCarrierReceipt
    {
        public int Id { get; set; }
        public LoadCarrierReceiptType Type { get; set; }
        public LoadCarrierReceiptTrigger Trigger { get; set; }

        public int? OrderLoadDetailId { get; set; }

        public string DigitalCode { get; set; }

        public int DivisionId { get; set; }
        public string DivisionName { get; set; }

        public string IssuerCompanyName { get; set; }

        public int DocumentId { get; set; }
        public string DocumentNumber { get; set; }


        public int? DepotPresetId { get; set; }
        public LoadCarrierReceiptDepotPresetCategory DepotPresetCategory { get; set; }
        public bool IsSortingRequired { get; set; }
        public bool IsSortingCompleted { get; set; }

        public DateTime IssuedDate { get; set; }

        public string TruckDriverName { get; set; }
        public string TruckDriverCompanyName { get; set; }
        public string LicensePlate { get; set; }
        public int LicensePlateCountryId { get; set; }

        public string DeliveryNoteNumber { get; set; }
        public bool DeliveryNoteShown { get; set; }
        public string PickUpNoteNumber { get; set; }
        public bool PickUpNoteShown { get; set; }
        public string CustomerReference { get; set; }
        public string ShipperCompanyName { get; set; }
        public Address ShipperAddress { get; set; }

        public IEnumerable<LoadCarrierReceiptPosition> Positions { get; set; }
        public IEnumerable<AccountingRecord> AccountingRecords { get; set; }

        public string DownloadLink { get; set; }

        public List<EmployeeNote> DplNotes { get; set; }
    }

    public class LoadCarrierReceiptPosition
    {
        public int Id { get; set; }
        public int LoadCarrierId { get; set; }
        public int Quantity { get; set; }
    }
    
    public class LoadCarrierReceiptSortingOption
    {
        public int Id { get; set; }
        public LoadCarrierReceipt LoadCarrierReceipt { get; set; }
        public IList<LoadCarrierReceiptSortingPosition> SortingPositions { get; set; }
    }
    
    public class LoadCarrierReceiptSortingPosition
    {
        public int LoadCarrierId { get; set; }
        public int Quantity { get; set; }
        public IEnumerable<LoadCarrierQuality> PossibleSortingQualities { get; set; }
    }

    public class LoadCarrierReceiptsSearchRequest : PaginationRequest
    {
        public int CustomerDivisionId { get; set; }
        public string DocumentNumber { get; set; }

        public List<LoadCarrierReceiptType> Type { get; set; }
        public List<int> LoadCarrierTypes { get; set; }

        public DateTime? IssueDateFrom { get; set; }
        public DateTime? IssueDateTo { get; set; }

        public bool IsSortingCompleted { get; set; }

        public LoadCarrierReceiptsSearchRequestSortOptions SortBy { get; set; }

        public System.ComponentModel.ListSortDirection SortDirection { get; set; }
    }

    public enum LoadCarrierReceiptsSearchRequestSortOptions
    {
        Type,
        DocumentNumber,
        IssuedBy,
        IssuanceDate,
    }

    public class LoadCarrierReceiptsCreateRequest
    {
        // reference to order match to identify posting accounts for both sides
        // optional, when not filled then one side booked to general account
        public string DigitalCode { get; set; }

        public int? DepoPresetId { get; set; }
        public bool IsSortingRequired { get; set; }

        public LoadCarrierReceiptType Type { get; set; }

        public int CustomerDivisionId { get; set; }

        public int? TargetPostingAccountId { get; set; }

        public string TruckDriverName { get; set; }
        public string TruckDriverCompanyName { get; set; }
        public string LicensePlate { get; set; }
        public int LicensePlateCountryId { get; set; }
        public int? RefLmsBusinessTypeId { get; set; }
        public Guid? RefLtmsTransactionRowGuid { get; set; }
        public string DeliveryNoteNumber { get; set; }
        public bool DeliveryNoteShown { get; set; }
        public string PickUpNoteNumber { get; set; }
        public bool PickUpNoteShown { get; set; }
        public string CustomerReference { get; set; }

        public string ShipperCompanyName { get; set; }
        public Address ShipperAddress { get; set; }

        public string Note { get; set; }

        public ICollection<LoadCarrierReceiptsCreateRequestPosition> Positions { get; set; }

        public int PrintLanguageId { get; set; }
        public int PrintCount { get; set; }
        public int PrintDateTimeOffset { get; set; }

        public EmployeeNoteCreateRequest DplNote { get; set; }
    }

    public class LoadCarrierReceiptsCreateRequestPosition
    {
        public int LoadCarrierId { get; set; }
        public int Quantity { get; set; }
    }        

    public class LoadCarrierReceiptsCancelRequest
    {
        public string Reason { get; set; }
        public EmployeeNoteCreateRequest DplNote { get; set; }
    }
    public class LoadCarrierReceiptsUpdateIsSortingRequiredRequest
    {
        public bool IsSortingRequired { get; set; }
        public EmployeeNoteCreateRequest DplNote { get; set; }
    }


}
