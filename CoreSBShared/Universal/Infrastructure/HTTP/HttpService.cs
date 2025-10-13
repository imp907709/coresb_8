using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using CoreSBShared.Universal.Infrastructure.HTTP.MyApp.Services.Http;
using Microsoft.Extensions.Logging;

namespace CoreSBShared.Universal.Infrastructure.HTTP
{
    
    public class HttpService : IHttpService
    {
        private readonly HttpClient _httpClient;

        private static readonly JsonSerializerOptions DefaultJsonOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = false
        };

        public HttpService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> GetAsync<TResponse>(string url)
        {
            try
            {
                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var res = await response.Content.ReadAsStringAsync();
                return res;
            }
            catch (Exception ex)
            {
                return default;
            }
        }

        
        
        public async Task<string> PostAsync<TRequest, TResponse>(string url, TRequest payload)
        {
            var response = await _httpClient.PostAsJsonAsync(url, payload, DefaultJsonOptions);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
        public async Task<string> PostAsync<TRequest, TResponse>(
            string url, TRequest payload, JsonSerializerOptions options)
        {
            var response = await _httpClient.PostAsJsonAsync(url, payload, options);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
        public async Task<string> PostJsonContentAsync<TRequest, TResponse>(
            string url, TRequest payload)
        {
            var json = JsonSerializer.Serialize(payload, DefaultJsonOptions);
            using var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(url, content);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
        
        
        
        public async Task<string> PutAsync<TRequest, TResponse>(string url, TRequest payload)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync(url, payload, DefaultJsonOptions);
                response.EnsureSuccessStatusCode();

                return await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                return default;
            }
        }

        public async Task<bool> DeleteAsync(string url)
        {
            try
            {
                var response = await _httpClient.DeleteAsync(url);
                response.EnsureSuccessStatusCode();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
