using System;
using System.Collections.Generic;
using System.Text;
using Dpl.B2b.Common.Enumerations;

namespace Dpl.B2b.Contracts.Models
{
    public class TransportOffering
    {
        public int Id { get; set; } 

        public string ReferenceNumber { get; set; }

        public TransportOfferingStatus Status { get; set; }
        public TransportLoadType LoadType { get; set; }

        public TransportOfferingLoadCarrierLoad LoadCarrierLoad { get; set; }

        public TransportOfferingInfo SupplyInfo { get; set; }
        public TransportOfferingInfo DemandInfo { get; set; }

        public DateTime PlannedLatestBy { get; set; } // bid deadline
        public DateTime ConfirmedLatestBy { get; set; } // when does customers (demand/supply customres) need to be informed latest

        public long? Distance { get; set; }

        public int StackHeightMin { get; set; }
        public int StackHeightMax { get; set; }

        public bool SupportsRearLoading { get; set; }
        public bool SupportsSideLoading { get; set; }
        public bool SupportsJumboVehicles { get; set; }

        public TransportOfferingBid WinningBid { get; set; }

        public IList<TransportOfferingBid> Bids { get; set; }
    }

    public class TransportOfferingLoadCarrierLoad
    {
        public int? LoadCarrierId { get; set; }
        public int LoadCarrierTypeId { get; set; }
        public int LoadCarrierQuantity { get; set; }
    }

    public class TransportOfferingInfo
    {
        public Address Address { get; set; }

        public IEnumerable<BusinessHours> BusinessHours { get; set; }

        public DateTime EarliestFulfillmentDateTime { get; set; }
        public DateTime LatestFulfillmentDateTime { get; set; }

        public bool SupportsRearLoading { get; set; }
        public bool SupportsSideLoading { get; set; }
        public bool SupportsJumboVehicles { get; set; }
    }

    public enum TransportLoadType
    {
        LoadCarrier = 0,
        Other = 1
    }

    public enum TransportOfferingStatus
    {
        Available = 0,
        BidPlaced = 1,
        Won = 2,
        Lost = 3,
        Accepted = 4,
        Declined = 5,
        Canceled = 10
    }

    public class TransportOfferingBid
    {
        public int Id { get; set; }

        public int DivisionId { get; set; }

        public TransportBidStatus Status { get; set; }

        public DateTime SubmittedDateTime { get; set; }
        public decimal Price { get; set; }

        public DateTime PickupDate { get; set; }
        public DateTime DeliveryDate { get; set; }

        public string Note { get; set; }
    }

    public class TransportOfferingsSearchRequest : SortablePaginationRequest<TransportOfferingSortOption>
    {
        public int DivisionId { get; set; }
        public List<TransportOfferingStatus> Status { get; set; }

        public List<int> LoadCarrierType { get; set; }

        public DateTime? SupplyEarliestFulfillmentDateFrom { get; set; }
        public DateTime? SupplyEarliestFulfillmentDateTo { get; set; }

        public DateTime? DemandLatestFulfillmentDateFrom { get; set; }
        public DateTime? DemandLatestFulfillmentDateTo { get; set; }

        public string SupplyPostalCode { get; set; }
        public string DemandPostalCode { get; set; }


        public double Lat { get; set; }
        public double Lng { get; set; }
        public int Radius { get; set; }

        public int? StackHeightMin { get; set; }
        public int? StackHeightMax { get; set; }

        public bool SupportsRearLoading { get; set; }
        public bool SupportsSideLoading { get; set; }
        public bool SupportsJumboVehicles { get; set; }
    }

    public enum TransportOfferingSortOption
    {
        ReferenceNumber = 0,
        SubmittedDate = 1,
        SupplyFulfillmentDate = 2,
        DemandFulfillmentDate = 3,
        SupplyPostalCode = 4,
        DemandPostalCode = 5,
        Status = 6,
        SupplyCountry = 7,
        DemandCountry = 8,
        Distance = 9,
    }

    public class TransportOfferingBidCreateRequest
    {
        public decimal Price { get; set; }
        public DateTime PickupDate { get; set; }
        public DateTime DeliveryDate { get; set; }
        public string Note { get; set; }
        public int DivisionId { get; set; }

        // field that is filled from ressource id
        public int TransportId { get; set; }
    }

    public class TransportOfferingBidAcceptRequest
    {
        public int BidId { get; set; }
    }
}
