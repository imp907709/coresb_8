using System.Diagnostics;

namespace CoreSBShared.Universal.Checkers.Threading;

public class WaitTask
{
    public static void GO()
    {
        var sw = new Stopwatch();
        sw.Start();
        
        CancellationTokenSource ts = new CancellationTokenSource();
        Thread thread = new Thread(o => { CancelToken(null, sw); });
        thread.Start(ts);
        Console.WriteLine($"0 {sw.Elapsed} Thread started for {thread.ManagedThreadId} on thread: {Thread.CurrentThread.ManagedThreadId}");

        Task t = Task.Run( () => { 
            Task.Delay(5000).Wait();
            Console.WriteLine($"2 {sw.Elapsed} Actual work end. on thread: {Thread.CurrentThread.ManagedThreadId}");
        });

        try
        {
            Console.WriteLine($"1 {sw.Elapsed} Work enter on Task {t.Id} Thread : {Thread.CurrentThread.ManagedThreadId}");
            bool result = t.Wait(1510, ts.Token);
            Console.WriteLine($"3 {sw.Elapsed} Work ended normally. With status {t.Status} on Task {t.Id} Thread : {Thread.CurrentThread.ManagedThreadId}");
            
        }
        // with a lot of timings it would not be hit at all
        catch (OperationCanceledException e) {
            Console.WriteLine($"6 {sw.Elapsed} The wait has been canceled for :{ e.GetType().Name}. With status {t.Status} on Task {t.Id} Thread : {Thread.CurrentThread.ManagedThreadId}");
            Thread.Sleep(4000);
            Console.WriteLine($"7 {sw.Elapsed} After sleeping, the task status:  With status {t.Status} on Task {t.Id} Thread : {Thread.CurrentThread.ManagedThreadId}");
            ts.Dispose();
        }
    }

    private static void CancelToken(object? obj, Stopwatch sw)
    {
        Thread.Sleep(1500);
        Console.WriteLine($"5 {sw.Elapsed} Canceling the cancellation token from thread  Thread : {Thread.CurrentThread.ManagedThreadId}");
        CancellationTokenSource source = obj as CancellationTokenSource;
        if (source != null) source.Cancel();
    }
}
