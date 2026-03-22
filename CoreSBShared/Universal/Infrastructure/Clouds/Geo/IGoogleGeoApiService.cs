using CoreSBShared.GeoModel;

namespace CoreSBShared.Universal.Infrastructure.Geo
{
    public interface IGoogleGeoApiService
    {
        Task<GeoApiResponse> GeocodeTestAsync(string address, CancellationToken ct = default);
        Task<string> GeocodeTestAsyncStr(string address, string url, CancellationToken ct = default);
    }
}
