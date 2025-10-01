using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection.Metadata.Ecma335;
using System.Threading;
using System.Threading.Tasks;
using InfrastructureCheckers;

namespace CoreSBShared.Universal.Checkers.Threading
{
    public class MultithreadingCheck
    {
        // private string urlGet = "https://api.restful-api.dev/objects";
        private string urlGet = ConstantsCheckers.testApiURl2;


        public async Task<IEnumerable<string>> GO(int count, int maxParallel)
        {
            var urls = Enumerable.Range(0, count).Select(s => { return urlGet; });
            return await ParallelHttpGet(urls, maxParallel);
        }

        // fails on first
        public async Task<IEnumerable<string>> ParallelHttpGet(IEnumerable<string> urls, int maxParallel)
        {
            using var _client = new HttpClient();
            using var cts = new CancellationTokenSource();

            using var smf = new SemaphoreSlim(maxParallel, maxParallel);

            var orders = urls.Select(s =>
            {
                var tsk = ParallelHttpWrapper
                    .RunParallel<string>(_client, s, cts, smf, HttpRequester.HttpGetSt);
                return tsk;
            });

            try
            {
                var results = await Task.WhenAll(orders);
                return results;
            }
            catch (Exception e)
            {
                cts.Cancel();
                throw;
            }
        }
    }

    public static class ParallelHttpWrapper
    {
        public static async Task<T> RunParallel<T>(HttpClient _client, string url, CancellationTokenSource cts,
            SemaphoreSlim smf, Func<HttpClient, string, CancellationToken, Task<T>> fnc)
        {
            await smf.WaitAsync(cts.Token);

            try
            {
                return await fnc(_client, url, cts.Token);
            }
            catch (Exception e)
            {
                cts.Cancel();
                throw;
            }
            finally
            {
                smf.Release();
            }
        }
    }

    public class HttpRequester
    {
        public static async Task<string> HttpGetSt(HttpClient _client, string url, CancellationToken ct)
        {
            var resp = await _client.GetStringAsync(url, ct);
            return resp;
        }

        public async Task<string> HttpGet(HttpClient _client, string url, CancellationToken ct)
        {
            var resp = await _client.GetStringAsync(url, ct);
            return resp;
        }
    }
}
