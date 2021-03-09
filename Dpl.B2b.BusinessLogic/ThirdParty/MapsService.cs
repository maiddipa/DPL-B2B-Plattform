
using System.Linq;
using Google.Maps;
using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using Google.Maps.Direction;
using Dpl.B2b.Contracts;
using Dpl.B2b.Contracts.Models;
using Dpl.B2b.Contracts.Maps;

namespace Dpl.B2b.BusinessLogic.ThirdParty
{

    public class MapsService : Contracts.IMapsService
    {
        // TODO move to appsettings.json or env variable
        private string API_KEY = "AIzaSyCnTjNBxjXRNWGNRQG5YUTHwekLu6ujogg";

        public MapsService()
        {
            GoogleSigned.AssignAllServices(new GoogleSigned(API_KEY));
        }

        public async Task<IGeocodedLocation> Geocode(Address address)
        {
            var addressString = address.Street1;

            //if(address.Street2 != null)
            //{
            //    addressString += $", {address.Street2}";
            //}

            if (address.PostalCode != null)
            {
                addressString += $", {address.PostalCode}";
            }

            if (address.City != null)
            {
                addressString += $", {address.City}";
            }

            if (address.CountryName != null)
            {
                addressString += $", {address.CountryName}";
            }

            var request = new Google.Maps.Geocoding.GeocodingRequest()
            {
                Address = new Google.Maps.Location(addressString),
                //Language = "en-US",
            };

            Google.Maps.Geocoding.GeocodeResponse response;
            try
            {
                response = await new Google.Maps.Geocoding.GeocodingService().GetResponseAsync(request);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);

                return null;
            }

            var first = response.Results.FirstOrDefault();

            if(first == null)
            {
                return null;
            }

            var country = first.AddressComponents.FirstOrDefault(i => i.Types.Contains(Google.Maps.Shared.AddressType.Country))?.ShortName;
            if(country == null)
            {
                return null;
            }

            return new GeocodedLocation()
            {
                StateIso2Code = first.AddressComponents.FirstOrDefault(i => i.Types.Contains(Google.Maps.Shared.AddressType.AdministrativeAreaLevel1))?.ShortName,
                CountryIso2Code = country,
                Latitude = first.Geometry.Location.Latitude,
                Longitude = first.Geometry.Location.Longitude
            };
        }

        public async Task<long?> GetRoutedDistance(Point from, Point to)
        {
            var directions = await GetDirections(new List<Tuple<Point, Point>>() { Tuple.Create(from, to) });
            return directions.Single()?.Distance;
        }

        public async Task<List<MapsRoutingResult>> GetDirections(List<Tuple<Point, Point>> points)
        {
            var results = points.Select(async point =>
            {
                var request = new Google.Maps.Direction.DirectionRequest()
                {
                    Origin = new Google.Maps.Location($"{point.Item1.Y.ToString(CultureInfo.InvariantCulture)}, {point.Item1.X.ToString(CultureInfo.InvariantCulture)}"),
                    Destination = new Google.Maps.Location($"{point.Item2.Y.ToString(CultureInfo.InvariantCulture)}, {point.Item2.X.ToString(CultureInfo.InvariantCulture)}"),
                    Mode = TravelMode.driving,
                    //Optimize = true,                
                };

                try
                {
                    return await new Google.Maps.Direction.DirectionService().GetResponseAsync(request);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);

                    return null;
                }

            })
            .ToArray();

            Task.WaitAll(results);

            var routingResults = results
                // TODO protect against incorrect routes
                .Select(i =>
                {
                    if (i.Result.Status != ServiceResponseStatus.Ok)
                    {
                        return null;
                    }

                    var item = i.Result.Routes.First().Legs.First();

                    return new MapsRoutingResult()
                    {
                        //ArrivalTime = item.ArrivalTime,
                        //DepartureTime = item.DepartureTime,
                        Distance = item.Distance.Value,
                        DistanceText = item.Distance.Text,
                        Duration = item.Duration.Value,
                        DurationText = item.Duration.Text,
                        StartAddress = item.StartAddress,
                        EndAddress = item.EndAddress,
                        //StartLocation = item.StartLocation
                        //EndLocation = item.EndLocation,
                    };
                }).ToList();

            return routingResults;
        }
    }

    public class GeocodedLocation : IGeocodedLocation
    {
        public string StateIso2Code { get; set; }
        public string CountryIso2Code { get; set; }
        public double Latitude { set;  get; }
        public double Longitude { set;  get; }
    }
}
