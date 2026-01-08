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



    public class ParallelExamplesAllInOne
    {
        // all in one
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
                
                // ===================
                // Option 1
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
                
                // ===================
                // Option 2
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

                var r3 = SemaphoreHttpSeparate(httpClient, url, cts, smf, i);
                tasks[idx] = r3;

            }
            
            var response = await Task.WhenAll(tasks);
            return response.ToList();
        }

        public async Task<IEnumerable<IndexedResp>> ParallelFor(IEnumerable<string> urls, CancellationTokenSource cts, HttpClient httpClient)
        {
            var parallelResp = new IndexedResp[urls?.Count() ?? 0];
            var orders = urls.Select((s, i) => new {s, i});
            await Parallel.ForEachAsync(orders,
                new ParallelOptions(){MaxDegreeOfParallelism = 10, CancellationToken = cts.Token},
                async (c,ct) => {
                    try
                    {
                        var resp = await HttpRequester.HttpGetSt(httpClient, c.s, ct);
                        parallelResp[c.i] = new IndexedResp() {idx = c.i, resp = resp};
                    }
                    catch (Exception e)
                    {
                        cts.Cancel();
                        throw;
                    }
                    
                });

            return parallelResp;

        }
        
 
       
        
        // semaphoreslim + task.run
        // semaphoreslim + whenall
        // parallel.forach
        // + batch wrap


        
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
                        result[i] = r;
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
        
        // ===================
        // Option 3
        // separate method for lambda
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
