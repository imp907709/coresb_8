using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using LINQtoObjectsCheck;
using Npgsql.EntityFrameworkCore.PostgreSQL.Query.Expressions.Internal;

namespace Live
{
    
    public class ConcurentTask_
    {
        
        public static async Task<IReadOnlyList<string>> FetchUrlsAsync(
            IEnumerable<string> urls, 
            int maxConcurrency,
            CancellationToken ct)
        {

            using var client = new HttpClient();
            using var cts = CancellationTokenSource.CreateLinkedTokenSource(ct);
            using var smf = new SemaphoreSlim(maxConcurrency);

            var urlsArr = urls.ToArray(); 
            var cnt = urlsArr.Length;
            var results = new string[cnt];

            var orders = urls.Select(async (s, i) => {
                results[i] = await GetParallel(client, s, cts, smf, maxConcurrency);
            });

            await Parallel.ForEachAsync(Enumerable.Range(0,cnt), new ParallelOptions() {MaxDegreeOfParallelism = maxConcurrency}, 
            async (i,ct) => {
                using var ctsd = CancellationTokenSource.CreateLinkedTokenSource(ct);
                var url = urlsArr[i];
                results[i] = await GetParallelFor(client, url, ctsd);
            });

            await Task.WhenAll(orders);
            return results;
            
        }

        public static async Task<string> GetParallelFor(HttpClient client, string s, CancellationTokenSource ct)
        {
            try
            {
                return await GetSingleHttp(client, s, ct.Token);
            }
            catch (Exception e)
            {
                ct.Cancel();
                throw;
            }
        }
        
        public static async Task<string> GetParallel(HttpClient client, string url, CancellationTokenSource cts
        , SemaphoreSlim smf, int maxConcurrency) {

            await smf.WaitAsync(cts.Token);
            try
            {
                return await GetSingleHttp(client, url, cts.Token);
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

            return string.Empty;
        }
        
        // we dont handle errs on lowes "integration" lvl as we could want diff err flows above
        public static async Task<string> GetSingleHttp(HttpClient client, string url, CancellationToken ct)
        {
            var resp = await client.GetStringAsync(url, ct);
            return resp;
        }
    }
}
