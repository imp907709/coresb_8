using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection.Metadata.Ecma335;
using System.Threading;
using System.Threading.Tasks;
using InfrastructureCheckers;

namespace CoreSBShared.Universal.Checkers.Threading
{
    public class IndexedResp
    {
        public int idx { get; set; }
        public string resp { get; set; }
    }
    public class MultithreadingCheck
    {
        // private string urlGet = "https://api.restful-api.dev/objects";
        private string urlGet = ConstantsCheckers.testApiURl2;


        public async Task<IEnumerable<IndexedResp>> GO(int count, int maxParallel)
        {
            var urls = Enumerable.Range(0, count).Select(s => { return urlGet; });
            var resp = await GetParallel(urls, maxParallel);
            // var resp = await GetWhenAll(urls, maxParallel);
            return resp.Select(s=> new IndexedResp() { idx=s.Key, resp = s.Value});
        }

        // fails on first
        public async Task<ConcurrentDictionary<int,string>> GetWhenAll(IEnumerable<string> urls, int maxParallel)
        {
            var res = new ConcurrentDictionary<int, string>();
            
            using var _client = new HttpClient();
            using var cts = new CancellationTokenSource();

            using var smf = new SemaphoreSlim(maxParallel, maxParallel);

            var orders = urls.Select(async (url,idx) => {
                var resp = await ParallelHttpWrapper
                    .RunParallel<string>(_client, url, cts, smf, HttpRequester.HttpGetSt);
                res[idx] = resp;
            });

            try
            {
                await Task.WhenAll(orders);
            }
            catch (Exception e)
            {
                cts.Cancel();
                throw;
            }

            return res;
        }

        public async Task<ConcurrentDictionary<int,string>> GetParallel(IEnumerable<string> urls, int maxParallel)
        {
            var res = new ConcurrentDictionary<int, string>();
            
            using var _client = new HttpClient();
            using var cts = new CancellationTokenSource();

            var orders = urls.Select((url, idx) => (url, idx));

            try
            {
                await Parallel.ForEachAsync(orders,
                    new ParallelOptions() {MaxDegreeOfParallelism = maxParallel, CancellationToken = cts.Token},
                    async (s, ct) => {

                        try
                        {
                            var resp = await HttpRequester.HttpGetSt(_client, s.url, ct);
                            res[s.idx] = resp;
                        }
                        catch (Exception e)
                        {
                            cts.Cancel();
                            throw;
                        }
                    });
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e.Message}");
                throw;
            }

            return res;
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
