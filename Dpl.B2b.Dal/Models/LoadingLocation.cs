using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dpl.B2b.Dal.Models
{
    public class LoadingLocation : OlmaAuditable
    {
        public int Id { get; set; }

        public int AddressId { get; set; }
        public virtual Address Address { get; set; }

        public virtual ICollection<BusinessHour> BusinessHours { get; set; }
        public virtual ICollection<BusinessHourException> BusinessHourExceptions { get; set; }

        public int? CustomerDivisionId { get; set; }
        public virtual CustomerDivision CustomerDivision { get; set; }

        public int? CustomerPartnerId { get; set; }
        public virtual CustomerPartner CustomerPartner { get; set; }

        public int? PartnerId { get; set; }
        public virtual Partner Partner { get; set; }

        //public virtual ICollection<LoadingLocationLoadCarrierDimensions> Dimensions { get; set; }

        public int StackHeightMin { get; set; }
        public int StackHeightMax { get; set; }

        public bool SupportsPartialMatching { get; set; }
        public bool SupportsRearLoading { get; set; }
        public bool SupportsSideLoading { get; set; }

        public bool SupportsJumboVehicles { get; set; }
    }
}