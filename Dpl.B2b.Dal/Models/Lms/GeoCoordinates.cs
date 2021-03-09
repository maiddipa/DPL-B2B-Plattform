using System;
using System.Collections.Generic;

namespace Dpl.B2b.Dal.Models.Lms
{
    public partial class GeoCoordinates
    {
        public int LocId { get; set; }
        public int CoordType { get; set; }
        public double? Lon { get; set; }
        public double? Lat { get; set; }
        public int? CoordSubtype { get; set; }
        public DateTime? ValidSince { get; set; }
        public int? DateTypeSince { get; set; }
        public DateTime ValidUntil { get; set; }
        public int DateTypeUntil { get; set; }

        public virtual GeoLocations Loc { get; set; }
    }
}
