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

    // Make it allocation-minimal
    // Make it exception-single (unwrap first failure)
    public class MultithreadingCheck
    {
        // private string urlGet = "https://api.restful-api.dev/objects";
        private string urlGet = ConstantsCheckers.testApiURl2;

        // Validate maxParallel: <= 0 will effectively deadlock.
        public async Task<IEnumerable<IndexedResp>> GO(int count, int maxParallel)
        {
            var urls = Enumerable.Range(0, count).Select(s => { return urlGet; });
            var resp = await GetParallel(urls, maxParallel);
            // var resp = await GetWhenAll(urls, maxParallel);
            return resp.Select(s=> new IndexedResp() { idx=s.Key, resp = s.Value});
        }

        // fails on first
        // Check maxparallel > 0 
        public async Task<ConcurrentDictionary<int,string>> GetWhenAllLinq(IEnumerable<string> urls, int maxParallel)
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

            await Task.WhenAll(orders);

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



    public class Parallelize
    {
        public async Task ParallelFor()
        {
            
        }
    }



    
    public class ParallelExamples
    {
        public async Task<IEnumerable<IIndexResp>> ParallelForInt (IEnumerable<string> urls)
        {
            var httpClient = new HttpClient();
            var urlsArr = urls.ToArray();
            
            var tasks = new Task<IndexedResp>[urlsArr.Length];
            
            using var cts = new CancellationTokenSource();
            using var smf = new SemaphoreSlim(2, 4);

            var ind = 0;

          
            
            // Task run not nessesary here 
            for(var i = 0; i < urls.Count(); i++) {
                int idx = i;
                var r = Task.Run(async () =>
                {
                    await smf.WaitAsync(cts.Token);
                    try
                    {
                        var resp = await HttpRequester.HttpGetSt(httpClient, urlsArr[i], cts.Token);
                        return new IndexedResp {idx = idx, resp = resp};
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
                    
                } , cts.Token);
                tasks[idx] = r;
                
                
                // without task run 
                // or => move to separate method
                var url = urlsArr[i];
                var r2 = new Func<Task<IndexedResp>>(async () => {
                    await smf.WaitAsync(cts.Token);
                    try
                    {
                        var resp = await HttpRequester.HttpGetSt(httpClient, url, cts.Token);
                        return new IndexedResp {idx = idx, resp = resp};
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
                });

                var r3 = RunHttpGet(httpClient, url, cts, smf, i);
                tasks[idx] = r3;
                
            }

            
            var response = await Task.WhenAll(tasks);
            return response.ToList();
        }

        // separate method for lambda
        public async Task<IndexedResp> RunHttpGet(HttpClient httpClient, string url, CancellationTokenSource cts, SemaphoreSlim smf, int id)
        {
            await smf.WaitAsync(cts.Token);
            try
            {
                var resp = await HttpRequester.HttpGetSt(httpClient, url, cts.Token);
                return new IndexedResp {idx = id, resp = resp};
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
   
    
    
    
    
    
    // interface to keep id to response 
    public interface IIndexResp
    {
        public int idx { get; set; }
        public string resp { get; set; }
    }
    public class IndexedResp : IIndexResp
    {
        public int idx { get; set; }
        public string resp { get; set; }
    }
    // actual method to parallelize
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
