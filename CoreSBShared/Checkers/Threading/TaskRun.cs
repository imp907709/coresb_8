using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace CoreSBShared.Universal.Checkers.Threading
{
    public class TaskRunCheck
    {
        private async Task ProcessCards(List<string> cards)
        {

            var tasks = new List<Task<HttpResponseMessage>>();

            // One ThreadPool thread runs the entire foreach
            // Task.Run does NOT make the IO async
            // It only moves the task creation loop to a ThreadPool thread
            await Task.Run(() => {
                
                // Schedules one work item onto the ThreadPool
                // One ThreadPool thread executes the lambda
                // The current async method yields
                // Caller thread is released

                // There is NO LIMIT.
                // 100 N - 100 http requests
                foreach (var card in cards) {

                    // HTTP, DB, FS

                }
            });

            // All HTTP requests are in flight
            // Threads are mostly idle
            // Kernel handles IO
            
            
            // Registers a continuation
            // The async method suspends
            // No thread is blocked
            await Task.WhenAll(tasks);
            
            // each task completes independently
            // completion threads wake continuations
            // Responses are marshaled back to the thread pool
            // each Task completes
            
        }
    }
}
