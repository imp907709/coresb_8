using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Collections.Concurrent;
using System.Linq;

namespace CoreSBServer.Controllers.Check.multithreading
{
    public class ThreadingExample
    {
        public async Task GO()
        {
            await TestResponses();
        }

        public async Task ParallelExample()
        {
            var syncDelegate = new Func<string, string>(s =>
            {
                Task.Delay(s.Length * 500);
                return s;
            });
            var r = syncDelegate.Invoke("some value");
            
            
            var asyncDelegate = new Func<string, Task<string>>(async s => {
                await Task.Delay(s.Length * 500);
                return s;
            });
            var res = await asyncDelegate.Invoke("some value");
            
            
            var d = ReturnDelay("text ");
        }

        public Task ReturnDelay(string input)
        {
            return Task.Delay(input.Length * 500);
        }

        public async Task AsyncCall(string input)
        {
            await ReturnDelay(input);
        }

        
        public async Task<string> HttpResponse(string url)
        {
            using var http = new HttpClient();
            var res = await (await http?.GetAsync(url))?.Content?.ReadAsStringAsync();

            return res;
        }

   
        public async Task<List<string>> HttpResponses(int max )
        {
            var result = new List<string>();
            for (int i = 0; i < max; i++)
            {
                var resp = await HttpResponse( "https://api.restful-api.dev/objects");

                result.Add(resp);
            }

            return result;
        }
        
     

        public async Task<string> TestResponses()
        {
            var st = Stopwatch.StartNew();
            
            var urls = new List<string> {
                "https://api.restful-api.dev/objects"
            };

            var result = await HttpResponses(20);

            st.Stop();
            return $"Collections received: {result?.Count}; in {st.Elapsed}";
        }
        
        
        
        public async Task<string> TestResponsesParallel(CancellationToken ct)
        {
            var st = Stopwatch.StartNew();
            
            var urls = new List<string> {
                "https://api.restful-api.dev/objects"
            };

            var result = await HttpResponsesParallel(20, 4, "https://api.restful-api.dev/objects", ct);

            st.Stop();
            return $"Collections received: {result?.Count}; in {st.Elapsed}";
        }
        public async Task<List<string>> HttpResponsesParallel(int max, int threads, string url, CancellationToken ct)
        {
            var smf = new SemaphoreSlim(threads);
            using var http = new HttpClient();
            var cts = CancellationTokenSource.CreateLinkedTokenSource(ct);

            var orders = Enumerable.Range(0, max)
                .Select(s=> HttpResponseThread(http, url, smf, cts.Token))
                .ToList();

            try
            {
                var result = await Task.WhenAll(orders).ConfigureAwait(false);
                return result?.ToList();
            }
            catch (Exception e)
            {
                cts.Cancel();
                throw;  
            }

            return new List<string>();
        }
        public async Task<string> HttpResponseThread(HttpClient client, string url, SemaphoreSlim smf, CancellationToken ct)
        {
            await smf.WaitAsync(ct);

            try
            {
                var resp = await client.GetAsync(url, ct);
                resp.EnsureSuccessStatusCode();
                var res = await resp.Content?.ReadAsStringAsync(ct);
                return res;
            }
            finally
            {
                smf.Release();
            }
        }



        public async Task<string> GetHttp()
        {
            var sw = new Stopwatch();
            sw.Start();
            
            var urls = new List<string>()
            {
                "https://api.restful-api.dev/objects", 
                "https://api.restful-api.dev/objects",
                "https://api.restful-api.dev/objects",
                "https://api.restful-api.dev/objects",
                "https://api.restful-api.dev/objects"
            };
            var max = 4;
            var res = await GetHttpInParallel(urls, max);

            sw.Stop();

            return $"Result: {res?.Count()}; Len:{res.Sum(s=>s.Length)} in {sw.Elapsed}";
        }
        public async Task<string[]> GetHttpInParallel(IEnumerable<string> urls, int maxParallel)
        {
            var dict = new ConcurrentDictionary<string, string>();
            var smf = new SemaphoreSlim(maxParallel);
            using var client = new HttpClient();
            using var source = new CancellationTokenSource();
            var ct = source.Token;

            try
            {
                var orders = urls.Select(s => AsyncThreadSafe(HttpGet, client, s, smf, ct));

                var result = await Task.WhenAll(orders);
                return result;
            } catch {
                source?.Cancel();
                throw;
            }
        }
        // Default semaphore slim wrapper
        public async Task<TResult> AsyncThreadSafe<TClient, TParam, TResult>(
            Func<TClient, TParam, CancellationToken, Task<TResult>> work,
            TClient client, TParam param, 
            SemaphoreSlim smf, CancellationToken ct)
        {
            await smf.WaitAsync(ct);
            try
            {
                // execute work
                return await work(client, param, ct);
            }
            finally
            {
                smf.Release();
            }
        }
        
        public async Task<string> HttpGet(HttpClient client, string url, CancellationToken ct)
        {
            return await client.GetStringAsync(url, ct);
        }
    }
}
