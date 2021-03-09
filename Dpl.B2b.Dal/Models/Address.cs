using NetTopologySuite.Geometries;

namespace Dpl.B2b.Dal.Models
{
    public class Address : OlmaAuditable
    {
        public int Id { get; set; }

        public int? RefLmsAddressNumber { get; set; }
        public string Street1 { get; set; }
        public string Street2 { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
        public int? StateId { get; set; }
        public virtual CountryState State { get; set; }
        public int? CountryId { get; set; }
        public virtual Country Country { get; set; }


        public Point GeoLocation { get; set; }
        // How to create a point value
        // var geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);
        // var currentLocation = geometryFactory.CreatePoint(-122.121512, 47.6739882);
        //
        // OR
        // var seattle = new Point(-122.333056, 47.609722) { SRID = 4326 };

        // Coordinates in NTS are in terms of X and Y values. To represent longitude and latitude, use X for longitude and Y for latitude.
        // Note that this is backwards from the latitude, longitude format in which you typically see these values.
    }
}