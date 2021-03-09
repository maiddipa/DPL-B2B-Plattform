using System;
using System.Collections.Generic;
using Dpl.B2b.Common.Enumerations;

namespace Dpl.B2b.Contracts.Models
{
    public class OrderMatch
    {
        public int Id { get; set; }
        public string OrderMatchNumber { get; set; }
        public Guid RefLmsAvailabilityRowGuid { get; set; }
        public Guid RefLmsDeliveryRowGuid { get; set; }
        public Guid? RefLmsPermanentAvailabilityRowGuid { get; set; }
        public Guid? RefLmsPermanentDeliveryRowGuid { get; set; }
        public string DigitalCode { get; set; }
        public OrderTransportType TransportType { get; set; }
        public OrderType? SelfTransportSide { get; set; }
        public OrderMatchStatus Status { get; set; }
        public OrderLoad Supply { get; set; }
        public string SupplyCompanyName { get; set; }
        public OrderLoad Demand { get; set; }
        public string DemandCompanyName { get; set; }
        public int LoadCarrierId { get; set; }
        public int? BaseLoadCarrierId { get; set; }
        public int BaseLoadCarrierQuantity { get; set; }
        public int LoadCarrierStackHeight { get; set; }
        public int LoadCarrierQuantity { get; set; }
        public bool SupportsRearLoading { get; set; }
        public bool SupportsSideLoading { get; set; }
        public bool SupportsJumboVehicles { get; set; }
    }

    public class OrderMatchesSearchRequest : PaginationRequest
    {
        public string Code { get; set; }
        public List<OrderMatchStatus> Statuses { get; set; }
        public List<int> PostingAccountId { get; set; }
    }
    public class OrderMatchesCreateRequest
    {
        public Guid SupplyOrderRowGuid { get; set; }
        public Guid DemandOrderRowGuid { get; set; }

        public OrderTransportType TransportType { get; set; }
        public OrderType? SelfTransportSide { get; set; }

        public bool SkipValidation { get; set; }

        public int LoadCarrierId { get; set; }
        public int LoadCarrierStackHeight { get; set; }
        public int LoadCarrierQuantity { get; set; }
        public int NumberOfStacks { get; set; }
        public int? BaseLoadCarrierId { get; set; }
        public int BaseLoadCarrierQuantity { get; set; }

        public bool SupportsRearLoading { get; set; }
        public bool SupportsSideLoading { get; set; }
        public bool SupportsJumboVehicles { get; set; }

        public DateTime SupplyFulfillmentDateTime { get; set; }
        public DateTime DemandFulfillmentDateTime { get; set; }
    }

    public class OrderMatchesUpdateRequest
    {
        public int SupplyOrderId { get; set; }
        public int DemandOrderId { get; set; }
        public int LoadCarrierId { get; set; }
        public int LoadCarrierQuantity { get; set; }
        public Guid? RefLmsAvailabilityRowGuid { get; set; }
        public Guid? RefLmsDeliveryRowGuid { get; set; }
        public OrderTransportType TransportType { get; set; }
        public OrderType? SelfTransportSide { get; set; }
        public OrderMatchStatus Status { get; set; }
        public int? BaseLoadCarrierId { get; set; }
        public int LoadCarrierStackHeight { get; set; }

        public bool SupportsPartialMatching { get; set; }
        public bool SupportsRearLoading { get; set; }
        public bool SupportsSideLoading { get; set; }
        public bool SupportsJumboVehicles { get; set; }
        public DateTime SupplyFulfillmentDateTime { get; set; }
        public DateTime DemandFulfillmentDateTime { get; set; }
    }

    public class OrderMatchesFulfillRequest
    {
        public int LoadCarrierId { get; set; }
        public int LoadCarrierQuantity { get; set; }

        public string TruckDriver { get; set; }
        public string LicensePlate { get; set; }
        public int LicensePlateCountryId { get; set; }
    }
}
