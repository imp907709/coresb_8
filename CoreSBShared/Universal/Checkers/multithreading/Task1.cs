using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace CoreSBServer.Controllers.Check.multithreading
{
    
    public static class Task1
    {
        /// <summary>
        /// Fetch all items with at most maxConcurrency running at once.
        /// - Honor CancellationToken for every awaited operation.
        /// - If any fetch fails: cancel the rest and throw (surface the first/aggregate error).
        /// - Preserve the input order in the returned list (result[i] corresponds to uris[i]).
        /// </summary>
        public static async Task<IReadOnlyList<T>> FetchAllAsync<T>(
            IEnumerable<Uri> uris,
            Func<Uri, CancellationToken, Task<T>> fetch,
            int maxConcurrency,
            CancellationToken ct)
            where T: class
        {
            // TODO: Implement. Notes:
            // - Validate arguments (maxConcurrency > 0, etc.)
            // - Use SemaphoreSlim for bounding OR Channels; no Task.Run loops needed.
            // - Use a linked CTS so that on first failure you cancel remaining work.
            // - Make sure you don't block threads (.Result/.Wait()) and you propagate ct everywhere.
            // - Keep output order matching input order.
            if (uris == null) throw new ArgumentNullException(nameof(uris));
            if (fetch == null) throw new ArgumentNullException(nameof(fetch));
            if (maxConcurrency <= 0) throw new ArgumentOutOfRangeException(nameof(maxConcurrency));

            var uriList = uris.ToList();
            if (uriList.Count == 0) return Array.Empty<T>();

            using var semaphore = new SemaphoreSlim(maxConcurrency);
            using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(ct);

            var tasks = new Task<(int index, T value)>[uriList.Count];

            for (int i = 0; i < uriList.Count; i++)
            {
                tasks[i] = FetchOneAsync(
                    i,
                    uriList[i],
                    fetch,
                    semaphore,
                    linkedCts,
                    linkedCts.Token);
            }

            var results = await Task.WhenAll(tasks).ConfigureAwait(false);

            // Preserve order
            return results
                .OrderBy(x => x.index)
                .Select(x => x.value)
                .ToList();
        }            

        public static async Task<(int index, T value)> FetchOneAsync<T>(
            int idx,
            Uri uri,
            Func<Uri, CancellationToken, Task<T>> fetch,
            SemaphoreSlim semaphore,
            CancellationTokenSource linkedCts,
            CancellationToken token)
            where T : class
        {
            await semaphore.WaitAsync(token).ConfigureAwait(false);
            try
            {
                var value = await fetch(uri, token).ConfigureAwait(false);
                return (idx, value);
            }
            catch
            {
                // Cancel all remaining work if one fails
                linkedCts.Cancel();
                throw;
            }
            finally
            {
                semaphore.Release();
            }
        }
    }

    public class Task1Prog
    {
        public static async Task Task1Main()
        {
            Console.WriteLine("=== Demo 1: Success, bounded concurrency, order preserved ===");
            var uris = new[]
            {
                new Uri("mock://ok/120"),
                new Uri("mock://ok/20"),
                new Uri("mock://ok/60"),
                new Uri("mock://ok/10"),
                new Uri("mock://ok/30"),
            };

            // Simulated fetch: delay = last path segment (ms), returns that number as string
            Task<string> SimFetch(Uri u, CancellationToken ct)
            {
                int ms = int.Parse(u.Segments.Last());
                return Task.Delay(ms, ct).ContinueWith(t =>
                {
                    ct.ThrowIfCancellationRequested();
                    return $"done:{ms}";
                }, ct);
            }

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
            var before = Stopwatch.GetTimestamp();
            var results = await Task1.FetchAllAsync(uris, SimFetch, maxConcurrency: 2, cts.Token);
            var elapsedMs = (Stopwatch.GetTimestamp() - before) * 1000.0 / Stopwatch.Frequency;

            Console.WriteLine("Results (should match input order): " + string.Join(", ", results));
            Console.WriteLine($"Elapsed ~ should be close to sum of the longest chains at concurrency=2, got ~{elapsedMs:F0} ms");
            Console.WriteLine();

            Console.WriteLine("=== Demo 2: Failure cancels others ===");
            var urisFail = new[]
            {
                new Uri("mock://ok/100"),
                new Uri("mock://fail/50"),
                new Uri("mock://ok/200"),
                new Uri("mock://ok/150"),
            };

            Task<string> SimFetchFail(Uri u, CancellationToken ct)
            {
                int ms = int.Parse(u.Segments.Last());
                return Task.Delay(ms, ct).ContinueWith<string>(t =>
                {
                    ct.ThrowIfCancellationRequested();
                    if (u.Host == "fail") throw new HttpRequestException("boom");
                    return $"done:{ms}";
                }, ct);
            }

            try
            {
                using var failCts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
                await Task1.FetchAllAsync(urisFail, SimFetchFail, maxConcurrency: 3, failCts.Token);
                Console.WriteLine("ERROR: should have thrown.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("OK (caught): " + ex.GetType().Name + " - " + ex.Message);
            }
        }
    }

}
