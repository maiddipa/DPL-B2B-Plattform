using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Dpl.B2b.Common.Enumerations;

namespace Dpl.B2b.Contracts.Models
{
    public class OrderGroup
    {
        public int Id { get; set; }
        public IList<int> OrderIds { get; set; }
        public IList<Order> Orders { get; set; }
    }

    public class OrderGroupsSearchRequest
    {
        public List<int> Id { get; set; }
        public List<int> OrderId { get; set; }
        public List<int> OrderLoadId { get; set; }
        public List<int> LoadCarrierReceiptId { get; set; }
    }

    public class OrderGroupsCreateRequest
    {
        public int DivisionId { get; set; }
        public OrderType Type { get; set; }
        public OrderTransportType TransportType { get; set; }
        public int PostingAccountId { get; set; }
        public int LoadCarrierId { get; set; }
        public int? BaseLoadCarrierId { get; set; }
        public int? LoadingLocationId { get; set; }
        public OrderQuantityType QuantityType { get; set; }
        public int NumberOfLoads { get; set; }
        public int? NumberOfStacks { get; set; }
        public int StackHeightMin { get; set; }
        public int StackHeightMax { get; set; }
        public int LoadCarrierQuantity { get; set; }

        public bool SupportsPartialMatching { get; set; }
        public bool SupportsRearLoading { get; set; }
        public bool SupportsSideLoading { get; set; }
        public bool SupportsJumboVehicles { get; set; }

        public DateTime EarliestFulfillmentDateTime { get; set; }
        public DateTime LatestFulfillmentDateTime { get; set; }

        public Guid? MatchLmsOrderGroupRowGuid { get; set; }

        public string Note { get; set; }
        public EmployeeNoteCreateRequest DplNote { get; set; }
    }

    public class OrderGroupsUpdateRequest
    {
        public int PostingAccountId { get; set; }
        public OrderType Type { get; set; }
        public OrderTransportType TransportType { get; set; }
        public int LoadCarrierId { get; set; }
        public int? BaseLoadCarrierId { get; set; }
        public int LoadingLocationId { get; set; }
        public OrderQuantityType QuantityType { get; set; }
        public int NumberOfLoads { get; set; }
        public int? NumberOfStacks { get; set; }
        public int StackHeightMin { get; set; }
        public int StackHeightMax { get; set; }
        public int LoadCarrierQuantity { get; set; }

        public bool SupportsPartialMatching { get; set; }
        public bool SupportsRearLoading { get; set; }
        public bool SupportsSideLoading { get; set; }
        public bool SupportsJumboVehicles { get; set; }

        public DateTime EarliestFulfillmentDateTime { get; set; }
        public DateTime LatestFulfillmentDateTime { get; set; }
        public EmployeeNoteCreateRequest DplNote { get; set; }
    }

    public class OrderGroupCancelRequest
    {
        public EmployeeNoteCreateRequest DplNote { get; set; }
    }
}
