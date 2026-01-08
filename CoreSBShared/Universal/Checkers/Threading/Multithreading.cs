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
            var resp = new List<IndexedResp>();
            var urls = Enumerable.Range(0, count).Select(s => { return urlGet; });
            // var resp = await GetWhenAll(urls, maxParallel);
            return resp;
        }
    }
    
    
    
    public class ParallelExamplesAllInOne
    {
        // OPTION 1 A
        // Best for http, DB and I/O work
        // semaphoreslim + wait all
        // need batching wrapper for N > 10k
        // efficient memory
        // local function prefered over lambda 
        public async Task<IEnumerable<IndexedResp>> ParallelTaskLambda(IEnumerable<string> urls,
            CancellationToken ct, HttpClient httpClient)
        {
            if (urls?.Any() != true)
                return Array.Empty<IndexedResp>();

            var urlsArr = urls.ToArray();
            var cnt = urlsArr.Length;
            var result = new IndexedResp[cnt];

            using var smf = new SemaphoreSlim(4);
            using var cts = CancellationTokenSource.CreateLinkedTokenSource(ct);

            // lambda
            Func<int, string, Task> RunAsyncLambda = async (idx, url) =>
            {
                await smf.WaitAsync(cts.Token);
                try
                {
                    var resp = await HttpRequester.HttpGetSt(httpClient, url, cts.Token);
                    result[idx] = new IndexedResp() {idx = idx, resp = resp};
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
            };

            // OR 
            // local function
            async Task RunAsync(int idx, string url)
            {
                await smf.WaitAsync(cts.Token);
                try
                {
                    var resp = await HttpRequester.HttpGetSt(httpClient, url, cts.Token);
                    result[idx] = new IndexedResp() {idx = idx, resp = resp};
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

            var tasks = urlsArr.Select((s, i) => RunAsync(i, s));
            await Task.WhenAll(tasks);

            return result;
        }
        
        // separate method for lambda usage
        public async Task<IndexedResp> SemaphoreHttpSeparate(HttpClient httpClient, string url, CancellationTokenSource cts, SemaphoreSlim smf, int id)
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
        
        // OPTION 1 B
        // fails on first
        // Check maxparallel > 0
        // calls wrapper semaphore method
        public async Task<ConcurrentDictionary<int,string>> GetWhenAllLinq(IEnumerable<string> urls, int maxParallel)
        {
            var res = new ConcurrentDictionary<int, string>();
            
            using var _client = new HttpClient();
            using var cts = new CancellationTokenSource();

            using var smf = new SemaphoreSlim(maxParallel, maxParallel);

            var orders = urls.Select(async (url,idx) =>
            {
                var resp = await SemaphoreHttpSeparate(_client, url, cts, smf, idx);
                res[idx] = resp.resp;
            });

            await Task.WhenAll(orders);

            return res;
        }
        
        // OPTION 2
        // NOT best fit
        // with task run
        // creates tasks + state machine
        // schedulers all tasks at once
        // >10k N - high memory footprint and GC pressure
        // needs wrapper buffer
        public async Task<IEnumerable<IndexedResp>> ParallelTaskRun(IEnumerable<string> urls,
            CancellationToken ct, HttpClient httpClient)
        {
            if (urls?.Any() != true)
                return Array.Empty<IndexedResp>();

            var urlsArr = urls.ToArray();
            var cnt = urlsArr.Length;
            var result = new IndexedResp[cnt];

            using var smf = new SemaphoreSlim(4);
            using var cts = CancellationTokenSource.CreateLinkedTokenSource(ct);

           
            var tasksRunned = urlsArr
                .Select((s, i) => Task.Run(async () =>
                {
                    await smf.WaitAsync();
                    try
                    {
                        var r =  await HttpRequester.HttpGetSt(httpClient, s, cts.Token);
                        result[i] = new IndexedResp(){idx = i, resp = r};
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
                }));
            
            return result;
        }
        
        // OPTION 3
        // NOT best fit
        // parallel for
        // also not ideal - while less memory footprint
        // best for intence CPU work rather than I O  
        public async Task<IEnumerable<IndexedResp>> ParallelForEach(IEnumerable<string> urls, 
            CancellationToken ct, HttpClient httpClient)
        {
            if(urls?.Any() != true)
                return Array.Empty<IndexedResp>();

            var urlsArr = urls.ToArray();
            var cnt = urlsArr.Length;
            var result = new IndexedResp[cnt];
            
            using var smf = new SemaphoreSlim(4);
            using var cts = CancellationTokenSource.CreateLinkedTokenSource(ct);
            
       
            await Parallel.ForEachAsync(urls.Select((s,i)=>new {s,i}),
                new ParallelOptions() {MaxDegreeOfParallelism = 10, CancellationToken = cts.Token},
            async (item, ct) => {
                var resp = await HttpRequester.HttpGetSt(httpClient, item.s, ct);
                result[item.i] = new IndexedResp(){idx = item.i, resp = resp};
            });

            return result;
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
