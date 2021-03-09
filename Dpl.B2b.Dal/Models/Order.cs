using Dpl.B2b.Common.Enumerations;
using System;
using System.Collections.Generic;

namespace Dpl.B2b.Dal.Models
{
    public class Order : OlmaAuditable
    {
        public int Id { get; set; }
        public Guid RefLmsOrderRowGuid { get; set; }

        public int GroupId { get; set; }
        public virtual OrderGroup Group { get; set; }

        public string OrderNumber { get; set; }

        public virtual ICollection<OrderLoad> Loads { get; set; }

        public OrderType Type { get; set; }

        public OrderTransportType TransportType { get; set; }

        public OrderStatus Status { get; set; }

        public int PostingAccountId { get; set; }
        public virtual PostingAccount PostingAccount { get; set; }

        public int LoadCarrierId { get; set; }
        public virtual LoadCarrier LoadCarrier { get; set; }

        public int? BaseLoadCarrierId { get; set; }
        public virtual LoadCarrier BaseLoadCarrier { get; set; }

        public int DivisionId { get; set; }
        public virtual CustomerDivision Division { get; set; }

        // this is not set when creating transport type = self transport
        public int? LoadingLocationId { get; set; }
        public virtual LoadingLocation LoadingLocation { get; set; }

        // quantity related information
        public OrderQuantityType QuantityType { get; set; }
        
        public int? NumberOfStacks { get; set; }

        // TODO We need min max height per loading type (verladungsarten), Dachser has mentinoed this requierment in the past to Armine
        public int StackHeightMin { get; set; }
        public int StackHeightMax { get; set; }

        public int? LoadCarrierQuantity { get; set; }

        public bool SupportsPartialMatching { get; set; }
        // verladungsarten
        public bool SupportsRearLoading { get; set; }
        public bool SupportsSideLoading { get; set; }        

        public bool SupportsJumboVehicles { get; set; }

        public DateTime EarliestFulfillmentDateTime { get; set; }
        public DateTime LatestFulfillmentDateTime { get; set; }

        public virtual ICollection<EmployeeNote> DplNotes { get; set; }

        public DateTime? SyncDate { get; set; }
        public string SyncNote { get; set; }
    }

    //public class SupplyOrder : Order
    //{
    //}

    //public class DemandOrder : Order
    //{
    //}
}