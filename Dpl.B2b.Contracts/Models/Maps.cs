using System;
using System.Collections.Generic;
using System.Text;

namespace Dpl.B2b.Contracts.Models
{

    public class MapsRoutingResult
    {
        public long Duration { get; set; }
        public string DurationText { get; set; }

        public long Distance { get; set; }
        public string DistanceText { get; set; }

        //public LatLng StartLocation { get; set; }
        //public LatLng EndLocation { get; set; }
        public string StartAddress { get; set; }
        public string EndAddress { get; set; }
        //public Time ArrivalTime { get; set; }
        //public Time DepartureTime { get; set; }
    }
}
