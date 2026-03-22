using System.Text.Json;
using CoreSBShared.GeoModel;
using CoreSBShared.Registrations;
using CoreSBShared.Universal.Infrastructure.Clouds;
using Microsoft.Extensions.Options;

namespace CoreSBShared.Universal.Infrastructure.Geo
{
    public class GoogleGeoApiService : IGoogleGeoApiService
    {
        private readonly HttpClient _http;
        private readonly GoogleGeoApiOptions _options;
        private readonly GoogleCloud _googleCloud;

        public GoogleGeoApiService(
            HttpClient http,
            IOptions<GoogleGeoApiOptions> options,
            GoogleCloud googleCloud)
        {
            _http = http;
            _options = options.Value;
            _googleCloud = googleCloud;
        }

        public async Task<GeoApiResponse> GeocodeTestAsync(string address, CancellationToken ct = default)
        {
            var key = _googleCloud.GetApiKey();
            var endpoint = _options.GeocodeEndpoint.Trim();
            if (endpoint.Length == 0 || address.Length == 0)
                return null;
            var url =
                $"{endpoint}?address={Uri.EscapeDataString(address)}&key={Uri.EscapeDataString(key)}";
            
            var resp = await GeocodeTestAsyncStr(address, url, ct);
            if (string.IsNullOrEmpty(resp))
                return null;
            
            var res = JsonSerializer.Deserialize<GeoApiResponse>(resp);
            return res;
        }

        public async Task<string> GeocodeTestAsyncStr(string address, string url, CancellationToken ct = default)
        {
            using var response = await _http.GetAsync(url, ct);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync(ct);
        }
    }
}
