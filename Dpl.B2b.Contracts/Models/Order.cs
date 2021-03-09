using Dpl.B2b.Common.Enumerations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dpl.B2b.Contracts.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int GroupId { get; set; }
        public string OrderNumber { get; set; }
        public OrderType Type { get; set; }
        public OrderTransportType TransportType { get; set; }
        public OrderStatus Status { get; set; }
        public int PostingAccountId { get; set; }

        public int LoadCarrierId { get; set; }
        public int? BaseLoadCarrierId { get; set; }

        public int DivisionId { get; set; }
        public int? LoadingLocationId { get; set; }

        public Address Address { get; set; }

        public OrderQuantityType QuantityType { get; set; }
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

        public DateTime CreatedAt { get; set; }

        public bool HasDplNote { get; set; }

        public IEnumerable<OrderLoad> Loads { get; set; }

        public List<EmployeeNote> DplNotes { get; set; }
    }
    public class OrderSearchRequest : PaginationRequest
    {
        public string DigitalCode { get; set; }
        public List<OrderType> Type { get; set; }
        public List<OrderTransportType> TransportType { get; set; }
        public List<OrderStatus> Status { get; set; }
        public List<OrderQuantityType> QuantityType { get; set; }
        public int? LoadCarrierQuantityFrom { get; set; }
        public int? LoadCarrierQuantityTo { get; set; }
        public int? CurrentLoadCarrierQuantityFrom { get; set; }
        public int? CurrentLoadCarrierQuantityTo { get; set; }
        public List<int> DivisionId { get; set; }
        public List<int> PostingAccountId { get; set; }
        public List<int> LoadingLocationId { get; set; }
        public List<int> LoadCarrierId { get; set; }
        public List<int> BaseLoadCarrierId { get; set; }
        public DateTime? OrderDateFrom { get; set; }
        public DateTime? OrderDateTo { get; set; }
        public DateTime? FulfilmentDateFrom { get; set; }
        public DateTime? FulfilmentDateTo { get; set; }
        public DateTime? ConfirmedFulfillmentDateFrom { get; set; }
        public DateTime? ConfirmedFulfillmentDateTo { get; set; }

        public bool? HasDplNote { get; set; }
        public OrderSearchRequestSortOptions SortBy { get; set; }
        public System.ComponentModel.ListSortDirection SortDirection { get; set; }
    }

    public enum OrderSearchRequestSortOptions
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
        StackHeightMax,
        BaseLoadCarrierName,
        BaseLoadCarrierQuantity,
        EarliestFulfillmentDateTime,
        LatestFulfillmentDateTime,
        HasDplNote
    }

    public class OrderUpdateRequest
    {
        public string Code { get; set; }

        public int LoadCarrierId { get; set; }

        public int LoadCarrierQuantity { get; set; }

        public EmployeeNoteCreateRequest DplNote { get; set; }
    }

    public class OrderCancelRequest
    {
        public string Note { get; set; }
        public EmployeeNoteCreateRequest DplNote { get; set; }
    }

    public class OrderCreateSyncRequest
    {
        public Guid OrderRowGuid { get; set; }
        public string OrderNumber { get; set; }
        public int CustomerNumber { get; set; }
        public string ExternalReferenceNumber { get; set; }
        public OrderType Type { get; set; }
        public bool IsPermanent { get; set; }
        public OrderTransportType TransportType { get; set; }
        public int RefLmsLoadCarrierId { get; set; }
        public int? RefLmsBaseLoadCarrierId { get; set; }
        public int? NumberOfStacks { get; set; }
        public Address Address { get; set; }
        public string VauNumber { get; set; }
        public int StackHeightMin { get; set; }
        public int StackHeightMax { get; set; }
        public int LoadCarrierQuantity { get; set; }
        public int BaseLoadCarrierQuantity { get; set; }
        public bool SupportsRearLoading { get; set; }
        public bool SupportsSideLoading { get; set; }
        public bool SupportsJumboVehicles { get; set; }
        public int CustomerNumberLoadingLocation { get; set; }
        public int? LoadingLocationId { get; set; }
        public bool SupportsPartialMatching { get; set; }
        public Person Person { get; set; }
        public DateTime EarliestFulfillmentDateTime { get; set; }
        public DateTime LatestFulfillmentDateTime { get; set; }

        public string RefLtmsAccountNumber { get; set; }
        public int RefLtmsAccountId { get; set; }
        
        public string BusinessHoursString { get; set; }
        
        //Order Matching
        public Guid? RefLmsAvailabilityRowGuid{ get; set; }
        public Guid? RefLmsDeliveryRowGuid { get; set; }

        public string DigitalCode { get; set; }
        public Guid? RefLmsPermanentAvailabilityRowGuid { get; set; }
        public Guid? RefLmsPermanentDeliveryRowGuid { get; set; }
      }

   public class OrderUpdateSyncRequest
      {
      //public Guid? OrderRowGuid { get; set; }
      //public string ExternalReferenceNumber { get; set; }
      public OrderType Type { get; set; }
      public bool IsPermanent { get; set; }
      public OrderTransportType TransportType { get; set; }
      public int RefLmsLoadCarrierId { get; set; }
      //public int? RefLmsBaseLoadCarrierId { get; set; }
      //public int? NumberOfStacks { get; set; }
      //public Address Address { get; set; }
      //public string VauNumber { get; set; }
      //public int StackHeightMin { get; set; }
      //public int StackHeightMax { get; set; }
      public int LoadCarrierQuantity { get; set; }
      //public bool SupportsRearLoading { get; set; }
      //public bool SupportsSideLoading { get; set; }
      //public bool SupportsJumboVehicles { get; set; }
      //public Person Person { get; set; }
      public DateTime EarliestFulfillmentDateTime { get; set; }
      public DateTime LatestFulfillmentDateTime { get; set; }
      public string RefLtmsAccountNumber { get; set; }
      public int RefLtmsAccountId { get; set; }
      }
   public class OrderCancelSyncRequest
    {
        public Guid OrderRowGuid { get; set; }
        public OrderType Type { get; set; }
        public Person Person { get; set; }
        public string Note { get; set; }
        public bool IsApproved { get; set; }
    }

    public class OrderFulfillSyncRequest
    {
        public Guid? RefLmsAvailabilityRowGuid{ get; set; }
        public Guid? RefLmsDeliveryRowGuid { get; set; }
        public string DigitalCode { get; set; }
        public DateTime FulfillmentDateTime { get; set; }
    }

    public class OrdersSyncRequest
    {
        public List<OrderCreateSyncRequest> OrderCreateSyncRequests{get; set;}
        public List<OrderCancelSyncRequest> OrderCancelSyncRequests{get; set;}
        public List<OrderFulfillSyncRequest> OrderFulfillSyncRequests{get; set;}
        public List<OrderRollbackFulfillmentSyncRequest> OrderRollbackFulfillmentSyncRequests{get; set;}
    }
    
    public class OrderRollbackFulfillmentSyncRequest
    {
        public string DigitalCode { get; set; }
    }
}
