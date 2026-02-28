using System.Collections.Concurrent;

namespace CoreSBShared.Universal.Checkers.Quizes.Review;

public class Metadata
{
    public string Key {get; set;}
    public string Value {get; set;} 
}
public class PureSyncs
{
    public async Task GO()
    {
        var dtInit = new List<Metadata>();
        
        Random rnd = new Random();

        var res = new List<string>();
        
        foreach (var k in dtInit) {
            res.Add($"{k.Key} : {k.Value}");
        }

        
        // ============================================================
        // CASE 1
        // ============================================================
        //
        // EXECUTION MODEL
        //
        // Calling Thread:
        // - Executes foreach synchronously.
        // - Each Task.Run queues work to ThreadPool immediately.
        // - resTask is fully populated on calling thread.
        // - await Task.WhenAll is reached after all tasks are created.
        //
        // Scheduler Behavior:
        // - ThreadPool receives N work items immediately.
        // - No throttling.
        // - Work stealing distributes tasks across worker threads.
        //
        // Worker Threads:
        // - Execute lambda synchronously (pure CPU string formatting).
        // - Each task completes quickly and transitions to RanToCompletion.
        //
        // Async vs Sync:
        // - Actual work is synchronous CPU work.
        // - Asynchronous behavior only due to await Task.WhenAll.
        // - Task.Run introduces parallelism.
        //
        // Return Behavior:
        // - If tasks not completed → method suspends asynchronously.
        // - If already completed → continuation may run synchronously.
        // - Deterministic lifecycle.
        //
        // Safety:
        // - No race on resTask (list populated before await).
        // - No shared mutation across threads.
        // - Correct but unthrottled parallel fan-out.
        //

        // case 1
        var resTask = new List<Task<string>>();
        foreach (var k in dtInit)
        {
            var t = Task.Run(() => {
                return $"{k.Key} : {k.Value}";
            });
            resTask.Add(t);
        }

        var tasksWaited1 = await Task.WhenAll(resTask);
        
        
        
        // ============================================================
        // CASE 2
        // ============================================================
        //
        // EXECUTION MODEL
        //
        // Calling Thread:
        // - Calls Task.Run → producer scheduled on ThreadPool.
        // - Does NOT wait for producer.
        // - Immediately executes Task.WhenAll(resTask).
        // - resTask likely empty or partially populated.
        //
        // Scheduler Behavior:
        // - Producer runs later on ThreadPool.
        // - Inside producer, Task.FromResult creates already-completed tasks.
        // - No real asynchronous work exists.
        //
        // Worker Threads:
        // - Producer runs synchronously on ThreadPool thread.
        // - No additional worker threads used for Task.FromResult.
        //
        // Async vs Sync:
        // - All work is synchronous.
        // - Fake async pattern (Task.FromResult).
        // - No real concurrency benefit.
        //
        // Major Race Condition:
        // - Task.WhenAll may observe:
        //   - Empty list
        //   - Partially populated list
        // - List<T> is not thread-safe.
        // - Concurrent modification may cause undefined behavior.
        //
        // ContinueWith:
        // - Continuation attaches to possibly empty WhenAll.
        // - Runs regardless of fault unless options specified.
        // - Uses default TaskScheduler.
        //
        // Return Behavior:
        // - await Task.WhenAll(resTask) executed without ensuring producer finished.
        // - Nondeterministic completion.
        //
        // Safety:
        // - Not thread-safe.
        // - Producer/consumer lifecycle not coordinated.
        // - Architecturally incorrect.
        //
        // case 2
        Task.Run(() => {
            foreach (var k in dtInit)
                resTask.Add(Task.FromResult($"{k.Key} : {k.Value}"));
        });

        var tasks = Task.WhenAll(resTask).ContinueWith(c=> {
            // some action
        });
        
        var tasksAwt = await Task.WhenAll(resTask);
        
        
        
        // ============================================================
        // CASE 3
        // ============================================================
        //
        // EXECUTION MODEL
        //
        // Calling Thread:
        // - Schedules producer via Task.Run.
        // - Immediately executes Task.WhenAll(resTask).
        // - resTask likely empty or partially populated.
        //
        // Producer Thread (ThreadPool):
        // - Iterates dtInit synchronously.
        // - For each item:
        //     1. Task.Delay(1) registers timer in TimerQueue.
        //     2. ContinueWith schedules continuation task.
        //     3. Continuation Task added to resTask.
        //
        // Scheduler Layers Involved:
        // - ThreadPool (producer)
        // - TimerQueue (Task.Delay)
        // - ThreadPool again (continuations)
        //
        // Async vs Sync:
        // - Producer work is synchronous.
        // - Task.Delay is true asynchronous timer-based work.
        // - ContinueWith schedules additional async continuation.
        //
        // Major Race Condition:
        // - Same race as Case 2 on resTask.
        // - WhenAll may execute before producer finishes.
        // - Delay tasks may complete before list fully populated.
        //
        // ContinueWith Issues:
        // - Does not propagate exceptions naturally.
        // - Uses TaskScheduler.Current at registration time.
        // - Harder to reason about execution order.
        //
        // Return Behavior:
        // - await Task.WhenAll(resTask) may observe:
        //     - Empty list
        //     - Partial list
        //     - Full list
        // - Completion timing nondeterministic.
        //
        // Safety:
        // - Not thread-safe.
        // - Over-layered async scheduling.
        // - Producer/consumer not synchronized.
        // - Highest nondeterminism of all cases.
        //
        // case 3
        Task.Run(() => {
            foreach (var k in dtInit)
                resTask.Add(Task.Delay(1).ContinueWith(s=>$"{k.Key} : {k.Value}"));
        });

        var tasks2 = Task.WhenAll(resTask).ContinueWith(c=> {
            // some action
        });
        
        var tasks2Awt = await Task.WhenAll(resTask);


        
        // case 4
        // ============================================================
        // EXECUTION MODEL
        //
        // Calling Thread:
        // - Executes dtInit.Select synchronously.
        // - For each element, Task.Run is called immediately.
        // - Each Task.Run queues work item to ThreadPool.
        // - An IEnumerable<Task<string>> is produced lazily.
        // - Task.WhenAll forces enumeration immediately.
        // - All tasks are created before awaiting.
        //
        // Scheduler Behavior:
        // - ThreadPool receives N work items instantly.
        // - No throttling or batching.
        // - Work stealing distributes tasks across worker threads.
        //
        // Worker Threads:
        // - Each worker executes lambda synchronously.
        // - Work is CPU-bound string formatting.
        // - Tasks complete very quickly.
        //
        // Async vs Sync:
        // - The actual work is synchronous CPU work.
        // - Async behavior exists only because of await WhenAll.
        // - Task.Run introduces parallelism, not true asynchrony.
        //
        // Await Behavior:
        // - If tasks not yet completed → method suspends asynchronously.
        // - If already completed → continuation may execute synchronously.
        // - Exceptions propagate through await.
        //
        // Return Behavior:
        // - Method completes only after ALL scheduled tasks finish.
        // - Deterministic aggregation.
        // - Result ordering preserved by WhenAll.
        //
        // Characteristics:
        // - Correct parallel fan-out.
        // - No race condition.
        // - No shared mutable state.
        // - Unthrottled ThreadPool usage.
        // - Risk of ThreadPool pressure at high cardinality.
        // ============================================================
        // correct not throttled async work
        var fnShed = await Task.WhenAll(dtInit.Select(s=> Task.Run(()=> $"{s.Key}:{s.Value}")));

        
        
        // case 5
        // ============================================================
        // CASE B
        // ============================================================
        // _ = Task.Run(() =>
        // {
        //     dtInit.ForEach(c =>
        //     {
        //         res.Add($"{c.Key}:{c.Value}");
        //     });
        // });
        //
        // EXECUTION MODEL
        //
        // Calling Thread:
        // - Calls Task.Run.
        // - Work is queued to ThreadPool.
        // - Immediately continues execution.
        // - Does NOT await the task.
        // - Fire-and-forget pattern.
        //
        // Scheduler Behavior:
        // - ThreadPool executes lambda when thread becomes available.
        // - No throttling.
        // - No lifecycle binding to caller.
        //
        // Worker Thread:
        // - Iterates dtInit synchronously.
        // - Mutates shared collection 'res'.
        //
        // Async vs Sync:
        // - Work is fully synchronous CPU work.
        // - Task.Run only moves it to background thread.
        // - No async/await involved.
        //
        // Return Behavior:
        // - Calling method returns immediately.
        // - No guarantee work has started or completed.
        // - Exceptions are unobserved unless internally handled.
        // - In ASP.NET Core, work may continue after request ends.
        //
        // Critical Risks:
        // - If 'res' is not thread-safe → race condition.
        // - Exceptions may terminate process (UnobservedTaskException).
        // - No cancellation support.
        // - No completion coordination.
        // - No backpressure.
        //
        // Characteristics:
        // - True fire-and-forget.
        // - Nondeterministic completion time.
        // - Unsafe in web request pipeline unless explicitly managed.
        // ============================================================
        // more fire and forget
        _ = Task.Run(() =>
        {
            dtInit.ForEach(c => {
                res.Add($"{c.Key} : {c.Value}");
            });
        });
    }
}
