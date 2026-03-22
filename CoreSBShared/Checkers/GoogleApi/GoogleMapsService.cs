namespace InfrastructureCheckers.GoogleApi
{
    using System.Net.Http;
    using System.Text.Json;
    using System.Threading.Tasks;

    public class GeoResult
    {
        public double Lat { get; set; }
        public double Lng { get; set; }
        public string? FormattedAddress { get; set; }
    }

    public class GoogleMapsService
    {
        private readonly HttpClient _http = new HttpClient();
        private const string BaseUrl = "https://maps.googleapis.com/maps/api/geocode/json";

        public async Task<string> GetByZipStrAsync(string zip, string apiKey)
        {
            var url = $"{BaseUrl}?components=postal_code:{zip}&key={apiKey}";

            var response = await _http.GetAsync(url);
            if (!response.IsSuccessStatusCode)
                return null;

            var json = await response.Content.ReadAsStringAsync();
            return json;
        }

        public async Task<GeoResult?> GetByZipAsync(string zip, string apiKey)
        {
            var url = $"{BaseUrl}?components=postal_code:{zip}&key={apiKey}";

            var response = await _http.GetAsync(url);
            if (!response.IsSuccessStatusCode)
                return null;

            var json = await response.Content.ReadAsStringAsync();

            using var doc = JsonDocument.Parse(json);

            var root = doc.RootElement;

            if (root.GetProperty("status").GetString() != "OK")
                return null;

            var result = root.GetProperty("results")[0];

            var location = result
                .GetProperty("geometry")
                .GetProperty("location");

            return new GeoResult
            {
                Lat = location.GetProperty("lat").GetDouble(),
                Lng = location.GetProperty("lng").GetDouble(),
                FormattedAddress = result.GetProperty("formatted_address").GetString()
            };
        }
    }
}
