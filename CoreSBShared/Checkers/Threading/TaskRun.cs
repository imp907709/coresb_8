using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using InfrastructureCheckers;

namespace CoreSBShared.Universal.Checkers.Threading
{
    public class TaskRunCheck
    {
        private int _n = (int)(100000);
        
        public static void GO()
        {
            var t = new TaskRunCheck();
            
            var sw = new Stopwatch();
            sw.Start();

            Console.WriteLine($"Start on thread {Thread.CurrentThread.ManagedThreadId}");
            
            // ~ 22 sec on 1 ths mln
            // t.RunWithTaskSync();
            
            // ~ 4 sec 
            // t.RunCPUintenseParallel();
            
            // ~ > than several minutes
            t.CpuIntenseParallel();
            Console.WriteLine($"End on thread {Thread.CurrentThread.ManagedThreadId}");
            
            var alp = TimeSpan.FromMilliseconds(sw.ElapsedMilliseconds);
        }
        
        public static async Task GOAsync()
        {
            var t = new TaskRunCheck();
            // t.RunWithTaskSync();
            
            Console.WriteLine($"Start on thread {Thread.CurrentThread.ManagedThreadId}");
            await t.RunWithTaskAsync();
            Console.WriteLine($"End on thread {Thread.CurrentThread.ManagedThreadId}");
            
        }

        public double RunCPUintense()
        {
            var res = CpuHeavy.WorkQuadratic(_n);
            return res;
        }

        public double CpuIntenseParallelOptimized()
        {
            var res = CpuIntenseParallelOptimized(_n);
            return res;
        }
        public double CpuIntenseParallel()
        {
            var res = CpuIntenseParallel(_n);
            return res;
        }
        
        
        
        
        public double CpuIntenseParallelOptimized(int n)
        {
            double totalSum = 0;

            object syncObj = new object();

            Parallel.For(
                0, n,
                () => 0.0, // thread-local sum
                (i, loop, localSum) =>
                {
                    for (int j = 0; j < n; j++)
                    {
                        localSum += i * j * 0.0001;
                    }
                    return localSum;
                },
                localSum =>
                {
                    lock (syncObj) {
                        totalSum += localSum;
                    }
                }
            );

            return totalSum;
        }
        
        public double CpuIntenseParallel(int n)
        {
            double sum = 0;
            
            object lockObj = new object();
            
            Parallel.For(0, n, (s,i) => {

                for (int j = 0; j < n; j++)
                {
                    var val =  s * j * 0.0001;
                    lock (lockObj) {
                        sum += val; 
                    }
                }
              
            });
        
            return sum;
        }
        
    
        public void RunWithTaskSync()
        {
            var t = Task.Run(() => {
                RunCPUintense();
            });

            t.Wait();
        }

        public Task<double> RunWithTaskAsync()
        {
            return Task.Run(RunCPUintense);
        }
        
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
