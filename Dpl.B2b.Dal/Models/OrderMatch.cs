using System;
using System.Collections.Generic;
using Dpl.B2b.Common.Enumerations;

namespace Dpl.B2b.Dal.Models
{
    public class OrderMatch : OlmaAuditable
    {
        public int Id { get; set; }
        public string OrderMatchNumber { get; set; }

        public Guid RefLmsAvailabilityRowGuid { get; set; }
        public Guid RefLmsDeliveryRowGuid { get; set; }

        public Guid? RefLmsPermanentAvailabilityRowGuid { get; set; }
        public Guid? RefLmsPermanentDeliveryRowGuid { get; set; }

        //TODO Add [Index(IsUnique = true)]
        // TODO discuss if this field should be on here since only the transporter should see it
        // only exception is selbstabholung
        public string DigitalCode { get; set; }

        public OrderTransportType TransportType { get; set; }
        public OrderType? SelfTransportSide { get; set; }
        public int? TransportId { get; set; }
        public virtual Transport Transport { get; set; }

        public OrderMatchStatus Status { get; set; }

        public int LoadCarrierId { get; set; }
        public virtual LoadCarrier LoadCarrier { get; set; }

        public int? BaseLoadCarrierId { get; set; }
        public virtual LoadCarrier BaseLoadCarrier { get; set; }

        public int? DocumentId { get; set; }

        public virtual Document Document { get; set; }
        
        public int LoadCarrierStackHeight { get; set; }
        public int LoadCarrierQuantity { get; set; }
        public int NumberOfStacks { get; set; }
        public int BaseLoadCarrierQuantity { get; set; }

        public bool SupportsRearLoading { get; set; }
        public bool SupportsSideLoading { get; set; }
        public bool SupportsJumboVehicles { get; set; }

        public int? DemandId { get; set; }
        public virtual OrderLoad Demand { get; set; }

        public int? SupplyId { get; set; }
        public virtual OrderLoad Supply { get; set; }
    }
}