using Dpl.B2b.Common.Enumerations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dpl.B2b.Dal.Models
{

    public class LoadCarrierReceipt : OlmaAuditable, ILoadCarrierDocument<LoadCarrierReceiptPosition>
    {
        public int Id { get; set; }

        public LoadCarrierReceiptType Type { get; set; }

        public LoadCarrierReceiptTrigger Trigger { get; set; }

        public string DigitalCode { get; set; }

        public int? DepoPresetId { get; set; }
        public virtual LoadCarrierReceiptDepotPreset DepotPreset { get; set; }
        public bool IsSortingRequired { get; set; }

        public bool IsSortingCompleted { get; set; }

        public int? OrderLoadDetailId { get; set; }
        public virtual OrderLoadDetail OrderLoadDetail { get; set; }

        public int? RefLmsAvail2DeliId { get; set; }

        // TODO discuss if this should be nullable for load carrier receipts
        public int? PostingAccountId { get; set; }
        public virtual PostingAccount PostingAccount { get; set; }

        public int? TargetPostingAccountId { get; set; }
        public virtual PostingAccount TargetPostingAccount { get; set; }

        // ------ ILoadCarrierDocument props
        public int DocumentId { get; set; }
        public virtual Document Document { get; set; }

        public int CustomerDivisionId { get; set; }
        public virtual CustomerDivision CustomerDivision { get; set; }

        public string Note { get; set; }
       
        public virtual ICollection<PostingRequest> PostingRequests { get; set; }
       
        public string TruckDriverName { get; set; }
        public string TruckDriverCompanyName { get; set; }
        public string LicensePlate { get; set; }
        public int? LicensePlateCountryId { get; set; }
        public virtual Country LicensePlateCountry { get; set; }

        public string? DeliveryNoteNumber { get; set; }
        public bool? DeliveryNoteShown { get; set; }
        public string? PickUpNoteNumber { get; set; }
        public bool? PickUpNoteShown { get; set; }
        public string? CustomerReference { get; set; }

        public string? ShipperCompanyName { get; set; }
        public int? ShipperAddressId { get; set; }
        public virtual Address ShipperAddress { get; set; }

        public virtual ICollection<LoadCarrierReceiptPosition> Positions { get; set; }

        public virtual ICollection<EmployeeNote> DplNotes { get; set; }
    }

    public class LoadCarrierReceiptPosition : OlmaAuditable
    {
        public int Id { get; set; }

        public int LoadCarrierId { get; set; }
        public virtual LoadCarrier LoadCarrier { get; set; }
        public int LoadCarrierQuantity { get; set; }
    
    
        public int ReceiptId { get; set; }
        public virtual LoadCarrierReceipt LoadCarrierReceipt { get; set; }
    }
}
