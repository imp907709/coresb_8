using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Amazon.Runtime.Internal.Endpoints.StandardLibrary;
using Microsoft.VisualBasic;

public static class ConcurrencyTasks1_2
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
        CancellationToken cts)
    {
        using var cancelSource = CancellationTokenSource.CreateLinkedTokenSource(cts);
        using var smf = new SemaphoreSlim(maxConcurrency);

        var result = new T[uris.Count()];
        var urisArr = uris.ToArray();

        var orders = uris.Select((s, i) =>
        {
            return Task.Run(async () =>
            {
                await smf.WaitAsync(cancelSource.Token);
                try
                {
                    var res = await fetch(s, cancelSource.Token);
                    result[i] = res;
                }
                catch (Exception e)
                {
                    cancelSource.Cancel();
                }
                finally
                {
                    smf.Release();
                }
            });
        });
        await Parallel.ForEachAsync(Enumerable.Range(0, uris.Count())
            , new ParallelOptions() {MaxDegreeOfParallelism = maxConcurrency}
            , async (i, ct) =>
            {
                var content = await fetch(urisArr[i], ct);
                result[i] = content;
            });


        var ordersArr = new List<Task>();
        for (int i = 0; i < uris.ToList().Count; i++)
        {
            var res = Task.Run(async () =>
            {
                await smf.WaitAsync(cancelSource.Token);
                try
                {
                    var resp = await fetch(urisArr[i], cancelSource.Token);
                    result[i] = resp;
                }
                catch (Exception e)
                {
                    cancelSource.Cancel();
                    throw;
                }
                finally
                {
                    smf.Release();
                }
            });

            ordersArr.Add(res);
        }

        ;

        await Task.WhenAll(orders);

        // TODO: Implement. Notes:
        // - Validate arguments (maxConcurrency > 0, etc.)
        // - Use SemaphoreSlim for bounding OR Channels; no Task.Run loops needed.
        // - Use a linked CTS so that on first failure you cancel remaining work.
        // - Make sure you don't block threads (.Result/.Wait()) and you propagate ct everywhere.
        // - Keep output order matching input order.
        throw new NotImplementedException();
    }
}

public class MultithreadingTask1_2
{
    public static async Task GO()
    {
        Console.WriteLine("=== Demo 1: Success, bounded concurrency, order preserved ===");
        var uris = new[]
        {
            new Uri("mock://ok/120"), new Uri("mock://ok/20"), new Uri("mock://ok/60"), new Uri("mock://ok/10"),
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

        var resultOrdered = new ConcurrentDictionary<string, string>();
        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
        var before = Stopwatch.GetTimestamp();
        var results = await ConcurrencyTasks1_2.FetchAllAsync(uris, SimFetch, maxConcurrency: 2, cts.Token);
        var elapsedMs = (Stopwatch.GetTimestamp() - before) * 1000.0 / Stopwatch.Frequency;

        Console.WriteLine("Results (should match input order): " + string.Join(", ", results));
        Console.WriteLine(
            $"Elapsed ~ should be close to sum of the longest chains at concurrency=2, got ~{elapsedMs:F0} ms");
        Console.WriteLine();

        Console.WriteLine("=== Demo 2: Failure cancels others ===");
        var urisFail = new[]
        {
            new Uri("mock://ok/100"), new Uri("mock://fail/50"), new Uri("mock://ok/200"), new Uri("mock://ok/150"),
        };

        try
        {
            using var failCts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
            await ConcurrencyTasks1_2.FetchAllAsync(urisFail, SimFetchFail, maxConcurrency: 3, failCts.Token);
            Console.WriteLine("ERROR: should have thrown.");
        }
        catch (Exception ex)
        {
            cts.Cancel();

            Console.WriteLine("OK (caught): " + ex.GetType().Name + " - " + ex.Message);
        }
    }
}
