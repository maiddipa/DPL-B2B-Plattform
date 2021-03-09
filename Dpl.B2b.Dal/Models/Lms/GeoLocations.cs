using System;
using System.Collections.Generic;

namespace Dpl.B2b.Dal.Models.Lms
{
    public partial class GeoLocations
    {
        public GeoLocations()
        {
            GeoCoordinates = new HashSet<GeoCoordinates>();
            GeoTextdata = new HashSet<GeoTextdata>();
        }

        public int LocId { get; set; }
        public int LocType { get; set; }

        public virtual ICollection<GeoCoordinates> GeoCoordinates { get; set; }
        public virtual ICollection<GeoTextdata> GeoTextdata { get; set; }
    }
}
