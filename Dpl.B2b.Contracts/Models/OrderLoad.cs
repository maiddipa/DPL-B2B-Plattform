using Dpl.B2b.Common.Enumerations;
using System;
using System.Collections.Generic;

namespace Dpl.B2b.Contracts.Models
{
    public class OrderLoad
    {
        public int Id { get; set; }

        public int OrderId { get; set; }
        public Order Order { get; set; }

        public int OrderMatchId { get; set; }

        public string DigitalCode { get; set; }

        public OrderType Type { get; set; }
        public OrderTransportType TransportType { get; set; }
        public OrderLoadStatus Status { get; set; }
        public int PostingAccountId { get; set; }

        public int LoadCarrierId { get; set; }
        public int? BaseLoadCarrierId { get; set; }

        public int? LoadingLocationId { get; set; }

        public Address Address { get; set; }

        public int LoadCarrierStackHeight { get; set; }
        public int LoadCarrierQuantity { get; set; }
        public int NumberOfStacks { get; set; }
        public int BaseLoadCarrierQuantity { get; set; }

        public bool SupportsRearLoading { get; set; }
        public bool SupportsSideLoading { get; set; }

        public bool SupportsJumboVehicles { get; set; }

        public DateTime PlannedFulfillmentDateTime { get; set; }
        public DateTime? ActualFulfillmentDateTime { get; set; }

        public DateTime CreatedAt { get; set; }

        public LoadCarrierReceipt LoadCarrierReceipt { get; set; }

        public List<EmployeeNote> DplNotes { get; set; }
    }

    public class OrderLoadSearchRequest : SortablePaginationRequest<OrderLoadSearchRequestSortOptions>
    {        
        public List<int> DivisionId { get; set; }
        public string DigitalCode { get; set; }
        public List<OrderType> Type { get; set; }
        public List<OrderTransportType> TransportType { get; set; }
        public List<OrderLoadStatus> Status { get; set; }
        public int? LoadCarrierQuantityFrom { get; set; }
        public int? LoadCarrierQuantityTo { get; set; }
        public List<int> PostingAccountId { get; set; }
        public List<int> LoadingLocationId { get; set; }
        public List<int> LoadCarrierId { get; set; }
        public List<int> BaseLoadCarrierId { get; set; }
        public DateTime? PlannedFulfilmentDateFrom { get; set; }
        public DateTime? PlannedFulfilmentDateTo { get; set; }
        public DateTime? ActualFulfillmentDateFrom { get; set; }
        public DateTime? ActualFulfillmentDateTo { get; set; }

        public bool? HasDplNote { get; set; }
    }

    public enum OrderLoadSearchRequestSortOptions
    {
        Id,
        CreatedAt,
        ConfirmedFulfillmentDate,
        NumberOfLoads,
        Status,
        PostalCode,
        LoadCarrierName,
        LoadCarrierQuantity,
        NumberOfStacks,
        StackHeight,
        BaseLoadCarrierName,
        BaseLoadCarrierQuantity,
        PlannedFulfillmentDateTime,
        ActualFulfillmentDateTime
    }    

    public class OrderLoadCancelRequest
    {
        public string Note { get; set; }
        public EmployeeNoteCreateRequest DplNote { get; set; }
    }
}
