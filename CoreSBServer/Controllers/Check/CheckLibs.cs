using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Interviews;
using LINQtoObjectsCheck;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CoreSBServer.Controllers
{
    public class CheckLibs
    {

        private static readonly SemaphoreSlim _slim = new (5);
        private static readonly HttpClient _client = new HttpClient();

        public static string FakeSyncHttpCallFake()
        {
            Thread.Sleep(1000);
            return "result";
        }
        
        public static string FakeSyncHttpCall()
        {
            // Build request
            var request = new HttpRequestMessage(HttpMethod.Get, "http://localhost:8000/api/longRunning");

            // Send synchronously (blocks thread)
            using var response = _client.Send(request);
            response.EnsureSuccessStatusCode();

            // Read content synchronously
            var content = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            return content;
        }

        public static string GetThreadPoolStats()
        {
            ThreadPool.GetAvailableThreads(out int availableWorkerThreads, out int availableCompletionPortThreads);
            ThreadPool.GetMaxThreads(out int maxWorkerThreads, out int maxCompletionPortThreads);
            var usedWorkerThreads = maxWorkerThreads - availableWorkerThreads;
            var usedCompletionPortThreads = maxCompletionPortThreads - availableCompletionPortThreads;
            
            return $"Worker Threads: {usedWorkerThreads}/{maxWorkerThreads} used, " +
                   $"Completion Port Threads: {usedCompletionPortThreads}/{maxCompletionPortThreads} used, " +
                   $"Current Thread: {Thread.CurrentThread.ManagedThreadId}";
        }
        
        // This methods not usefull for real blocking I/O work
        // they all strill block thread
        // -------------------------------
        
        // blocked thread
        public static Task<string> FakeSyncFromResult()
        {
            var res = FakeSyncHttpCall();
            return Task.FromResult(res);
        }
        
        // blocks thread
        // no allocations
        public static ValueTask<string> FakeSyncValueTask()
        {
            var res = FakeSyncHttpCall();
            return new ValueTask<string>(res);
        }

        // blocked 
        // resumes on called context / scheduler
        public static async Task<string> FakeSyncYield()
        {
            await Task.Yield();
            var res = FakeSyncHttpCall();
            return res;
        }

        // -------------------------------
        
        
        public static async Task<string> FakeSyncTaskRun()
        {
            var res = await Task.Run(FakeSyncHttpCall);
            return res;
        }

        public static async Task<string> FakeSyncTaskRunThrottle()
        {
            await _slim.WaitAsync();
            try
            {
                var res = await Task.Run(FakeSyncTaskRun);
                return res;
            }
            finally
            {
                _slim.Release();
            }
        }
        
        
        
        
        

        // Blocks server thread on execution
        public static string pureSyncCPU()
        {
            long res=0;
            for (long i = 0; i < 1000000000; i++)
                res += i;
            
            return res.ToString();
        }

        // Does not block caller http thread
        public static async Task<string> AsyncOverSyncCPU()
        {
            await _slim.WaitAsync();
            try
            {
                var res = await Task.Run(pureSyncCPU);
                return res;
            }
            catch (Exception e)
            {

            }
            finally
            {
                _slim.Release();
            }

            return string.Empty;
        }


        public static List<int> LinqRnd()
        {
            var items = Enumerable.Range(0, 100).ToList();
            
            
            var rnd = new Random();
            var filter = Enumerable.Range(0, 5).Select(_ => rnd.Next(1, 10)).ToList();
                
            var selectedQr = items.Where(s=>filter.Contains(s));
            var selectedItms = selectedQr.ToList();
            return selectedItms;
        }



        public static async Task AsyncA()
        {
            Console.WriteLine("Enter A");
            await Task.Delay(2000);
            Console.WriteLine("Exit A");
        }
        public static async Task AsyncB()
        {
            Console.WriteLine("Enter B");
            await Task.Delay(2000);
            Console.WriteLine("Exit B");
        }

        public static async Task AsyncTest()
        {
            await AsyncA();
        }
    }
}
