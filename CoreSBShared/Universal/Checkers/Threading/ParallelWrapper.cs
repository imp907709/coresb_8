using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;


namespace CoreSBShared.Universal.Checkers.Threading
{
    public static class ParallelAsyncExecutor
    {
        /// <summary>
        /// Orchestrates parallel execution of a collection of items with concurrency limit, cancellation, retries, and optional logging.
        /// Returns results in the same order as input items.
        /// </summary>
        public static async Task<TResult[]> ExecuteAllAsync<TResource, TItem, TResult>(
            TResource resource,
            IList<TItem> items,
            int maxDegreeOfParallelism,
            Func<TResource, TItem, CancellationToken, Task<TResult>> operation,
            int maxRetries = 0,
            Action<TItem, Exception>? logException = null,
            CancellationToken cancellationToken = default)
        {
            if (items.Count == 0) return Array.Empty<TResult>();

            var results = new TResult[items.Count];
            using var semaphore = new SemaphoreSlim(maxDegreeOfParallelism, maxDegreeOfParallelism);
            var tasks = new Task[items.Count];

            for (int i = 0; i < items.Count; i++)
            {
                var index = i;
                var item = items[index];
                tasks[index] = RunOperationAsync(resource, item, index, results, semaphore, operation, maxRetries,
                    logException, cancellationToken);
            }

            await Task.WhenAll(tasks).ConfigureAwait(false);
            return results;
        }

        /// <summary>
        /// Executes a single operation under a semaphore, with optional retries, exception logging, and cancellation.
        /// </summary>
        public static async Task<TResult> ExecuteWithSemaphoreAsync<TResource, TItem, TResult>(
            TResource resource,
            TItem item,
            SemaphoreSlim semaphore,
            Func<TResource, TItem, CancellationToken, Task<TResult>> operation,
            int maxRetries = 0,
            Action<TItem, Exception>? logException = null,
            CancellationToken cancellationToken = default)
        {
            await semaphore.WaitAsync(cancellationToken).ConfigureAwait(false);
            try
            {
                int attempt = 0;
                while (true)
                {
                    try
                    {
                        return await operation(resource, item, cancellationToken).ConfigureAwait(false);
                    }
                    catch (Exception ex)
                    {
                        attempt++;
                        logException?.Invoke(item, ex);
                        if (attempt > maxRetries) throw;
                    }
                }
            }
            finally
            {
                semaphore.Release();
            }
        }

        /// <summary>
        /// Executes a single operation and stores the result at the correct index in a preallocated array.
        /// </summary>
        public static async Task RunOperationAsync<TResource, TItem, TResult>(
            TResource resource,
            TItem item,
            int index,
            TResult[] results,
            SemaphoreSlim semaphore,
            Func<TResource, TItem, CancellationToken, Task<TResult>> operation,
            int maxRetries = 0,
            Action<TItem, Exception>? logException = null,
            CancellationToken cancellationToken = default)
        {
            var result = await ExecuteWithSemaphoreAsync(resource, item, semaphore, operation, maxRetries, logException,
                    cancellationToken)
                .ConfigureAwait(false);
            results[index] = result;
        }
    }
}
