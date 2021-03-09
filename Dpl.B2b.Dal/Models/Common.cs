using System;
using System.Collections.Generic;
using System.Text;

namespace Dpl.B2b.Dal.Models
{
    public interface ILoadCarrierDocument<TPosition>
    {        
        CustomerDivision CustomerDivision { get; set; }
        int CustomerDivisionId { get; set; }
        Document Document { get; set; }
        int DocumentId { get; set; }
        string LicensePlate { get; set; }
        Country LicensePlateCountry { get; set; }
        int? LicensePlateCountryId { get; set; }
        string Note { get; set; }
        ICollection<TPosition> Positions { get; set; }
        ICollection<PostingRequest> PostingRequests { get; set; }
        
        string TruckDriverName { get; set; }
        string TruckDriverCompanyName { get; set; }
    }
}
