using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Dpl.B2b.Common.Enumerations;

namespace Dpl.B2b.Contracts.Models
{
    public class LoadCarrierOffering
    {
        public Address Address { get; set; }

        public double Lat { get; set; }

        public double Lng { get; set; }

        public double Distance { get; set; }

        public IEnumerable<BusinessHours> BusinessHours { get; set; }

        public IEnumerable<LoadCarrierOfferingDetail> Details { get; set; }
    }

    public class LoadCarrierOfferingDetail
    {
        public Guid Guid { get; set; } // this is the lms order group guid
        public DateTime AvailabilityFrom { get; set; }
        public DateTime AvailabilityTo { get; set; }

        public int? BaseLoadCarrierId { get; set; }

        public int StackHeightMin { get; set; }
        public int StackHeightMax { get; set; }

        public bool SupportsRearLoading { get; set; }
        public bool SupportsSideLoading { get; set; }
        public bool SupportsJumboVehicles { get; set; }
    }

    public class LoadCarrierOfferingsSearchRequest : PaginationRequest
    {
        public int PostingAccountId { get; set; }

        public OrderType Type { get; set; }
        public OrderTransportType TransportType { get; set; } // we need to query for the reverse here

        public int LoadCarrierId { get; set; }
        public OrderQuantityType QuantityType { get; set; }
        public int LoadCarrierQuantity { get; set; }
        public List<int?> BaseLoadCarrierId { get; set; }

        public double Lat { get; set; }
        public double Lng { get; set; }
        public int Radius { get; set; }

        public List<DateTime> Date { get; set; }

        public int StackHeightMin { get; set; }
        public int StackHeightMax { get; set; }

        public bool SupportsRearLoading { get; set; }
        public bool SupportsSideLoading { get; set; }
        public bool SupportsJumboVehicles { get; set; }
    }
}
