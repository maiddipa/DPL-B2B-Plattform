using System;
using System.Collections.Generic;

namespace Dpl.B2b.Dal.Models.Lms
{
    public partial class GeoTextdata
    {
        public int LocId { get; set; }
        public int TextType { get; set; }
        public string TextVal { get; set; }
        public string TextLocale { get; set; }
        public short? IsNativeLang { get; set; }
        public short? IsDefaultName { get; set; }
        public DateTime? ValidSince { get; set; }
        public int? DateTypeSince { get; set; }
        public DateTime ValidUntil { get; set; }
        public int DateTypeUntil { get; set; }

        public virtual GeoLocations Loc { get; set; }
    }
}
