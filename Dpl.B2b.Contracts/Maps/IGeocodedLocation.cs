using System;
using System.Collections.Generic;
using System.Text;

namespace Dpl.B2b.Contracts.Maps
{
    public interface IGeocodedLocation
    {
        public string StateIso2Code { get; set; }
        public string CountryIso2Code { get; set; }
        public double Latitude { get; }
        public double Longitude { get; }
    }
}
