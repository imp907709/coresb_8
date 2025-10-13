using System.Text.Json;
using System.Threading.Tasks;

namespace CoreSBShared.Universal.Infrastructure.HTTP
{
 
    namespace MyApp.Services.Http
    {
        public interface IHttpService
        {
            Task<string> GetAsync<TResponse>(string url);
            
            Task<string> PostAsync<TRequest, TResponse>(string url, TRequest payload);
            Task<string> PostAsync<TRequest, TResponse>(string url, TRequest payload, JsonSerializerOptions options);
            Task<string> PostJsonContentAsync<TRequest, TResponse>(string url, TRequest payload);
        
            Task<string> PutAsync<TRequest, TResponse>(string url, TRequest payload);
            Task<bool> DeleteAsync(string url);
        }
    }

}
