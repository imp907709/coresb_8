using System.Text.Json.Serialization;

namespace CoreSBShared.GeoModel
{
    /// <summary>Google Geocoding API JSON DTOs (nested under one container type).</summary>
    public class GeoApiResponse
    {
        public sealed class GeocodeResponse
        {
            [JsonPropertyName("results")]
            public List<GeocodeResult>? Results { get; set; }

            [JsonPropertyName("status")]
            public string? Status { get; set; }
        }

        public class GeocodeResult
        {
            [JsonPropertyName("address_components")]
            public List<AddressComponent>? AddressComponents { get; set; }

            [JsonPropertyName("formatted_address")]
            public string? FormattedAddress { get; set; }

            [JsonPropertyName("geometry")]
            public Geometry? Geometry { get; set; }

            [JsonPropertyName("navigation_points")]
            public List<NavigationPoint>? NavigationPoints { get; set; }

            [JsonPropertyName("place_id")]
            public string? PlaceId { get; set; }

            [JsonPropertyName("types")]
            public List<string>? Types { get; set; }
        }

        public class AddressComponent
        {
            [JsonPropertyName("long_name")]
            public string? LongName { get; set; }

            [JsonPropertyName("short_name")]
            public string? ShortName { get; set; }

            [JsonPropertyName("types")]
            public List<string>? Types { get; set; }
        }

        public class Geometry
        {
            [JsonPropertyName("bounds")]
            public LatLngBounds? Bounds { get; set; }

            [JsonPropertyName("location")]
            public LatLng? Location { get; set; }

            [JsonPropertyName("location_type")]
            public string? LocationType { get; set; }

            [JsonPropertyName("viewport")]
            public LatLngBounds? Viewport { get; set; }
        }

        /// <summary>Used for both <c>bounds</c> and <c>viewport</c> (northeast / southwest).</summary>
        public class LatLngBounds
        {
            [JsonPropertyName("northeast")]
            public LatLng? Northeast { get; set; }

            [JsonPropertyName("southwest")]
            public LatLng? Southwest { get; set; }
        }

        /// <summary><c>lat</c>/<c>lng</c> pair; reused for bounds corners, geometry location, viewport corners.</summary>
        public class LatLng
        {
            [JsonPropertyName("lat")]
            public double Lat { get; set; }

            [JsonPropertyName("lng")]
            public double Lng { get; set; }
        }

        public class NavigationPoint
        {
            [JsonPropertyName("location")]
            public NavigationLocation? Location { get; set; }

            [JsonPropertyName("restricted_travel_modes")]
            public List<string>? RestrictedTravelModes { get; set; }
        }

        /// <summary>Navigation point uses <c>latitude</c>/<c>longitude</c>, not <c>lat</c>/<c>lng</c>.</summary>
        public class NavigationLocation
        {
            [JsonPropertyName("latitude")]
            public double Latitude { get; set; }

            [JsonPropertyName("longitude")]
            public double Longitude { get; set; }
        }
    }
}
