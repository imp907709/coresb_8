using System.Collections.Concurrent;

namespace CoreSBShared.Universal.Checkers.Threading;

public class ParallelCorrect
{
    // Service method performing throttled parallel processing
    // - Limits concurrency
    // - Supports cancellation
    // - Propagates exceptions correctly
    // - Avoids Task.Run unless CPU-bound
    public async Task<List<string>> ProcessAsync(
        IReadOnlyCollection<KeyValuePair<string, string>> dtInit,
        int maxDegreeOfParallelism,
        CancellationToken cancellationToken)
    {
        if (dtInit == null)
            throw new ArgumentNullException(nameof(dtInit));

        if (maxDegreeOfParallelism <= 0)
            throw new ArgumentOutOfRangeException(nameof(maxDegreeOfParallelism));

        var results = new ConcurrentBag<string>();

        await Parallel.ForEachAsync(
            dtInit,
            new ParallelOptions
            {
                MaxDegreeOfParallelism = maxDegreeOfParallelism,
                CancellationToken = cancellationToken
            },
            async (item, ct) =>
            {
                // Simulate CPU or IO bound operation
                var computed = $"{item.Key}:{item.Value}";

                results.Add(computed);

                await ValueTask.CompletedTask;
            });

        return results.ToList();
    }
    
    // Manual throttling implementation
    public async Task<List<string>> ProcessWithSemaphoreAsync(
        IEnumerable<KeyValuePair<string, string>> dtInit,
        int maxConcurrency,
        CancellationToken cancellationToken)
    {
        if (dtInit == null)
            throw new ArgumentNullException(nameof(dtInit));

        using var semaphore = new SemaphoreSlim(maxConcurrency);
        var tasks = new List<Task<string>>();

        foreach (var item in dtInit)
        {
            await semaphore.WaitAsync(cancellationToken);

            tasks.Add(Task.Run(async () =>
            {
                try
                {
                    return $"{item.Key}:{item.Value}";
                }
                finally
                {
                    semaphore.Release();
                }
            }, cancellationToken));
        }

        return (await Task.WhenAll(tasks)).ToList();
    }
}
