
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using CoreSBShared.Universal.Checkers.Threading;
using InfrastructureCheckers.Collections;
using LINQtoObjectsCheck;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.VisualBasic;

namespace Live
{
    public static class CustomLinqCheck
    {
        private class TestItem
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }

        public static void PrntArrRes<T>(IEnumerable<T> init, IEnumerable<T> res)
        {
            Console.WriteLine($"Init arr: {string.Join(',',init)}");
            Console.WriteLine($"Result arr: {string.Join(',',res)}");
        }
        public static void GO()
        {
            var arr = new List<int>(){1,2,3,4,5};
            var res = arr.CustomWhere(s => s % 2 != 0);
            PrntArrRes(arr, res);
        }

        public static void MehtodsRevice()
        {
            
        }
    }

    
    public class LiveParallelWrapper
    {
 
        private string urlGet = "https://fake-json-api.mock.beeceptor.com/users";

        public async Task<IEnumerable<string>> GO(int maxUrls,int maxParallel)
        {
            var urls = Enumerable.Range(0, maxUrls).Select(s => {
                return urlGet;
            });
            
            using var client = new HttpClient();

            return await ExecuteWrapper(client, urls, maxParallel);
        }

        public async Task<IEnumerable<string>> ExecuteWrapper(HttpClient client, IEnumerable<string> urls, int maxParallel)
        {
            using var cts = new CancellationTokenSource();
            using var smf = new SemaphoreSlim(maxParallel, maxParallel);

            var orders = urls.Select(s => {
                return ExecuteSingle<string>(client, s, cts, smf, HttpRequester.HttpGetSt);
            });

            try
            {
                return await Task.WhenAll(orders);
            }
            catch (Exception e)
            {
                cts.Cancel();
                throw;
            }
   
        }
        public async Task<T> ExecuteSingle<T>(HttpClient client, string url, CancellationTokenSource cts, SemaphoreSlim smf,
            Func<HttpClient,string,CancellationToken,Task<T>> action)
        {
            await smf.WaitAsync(cts.Token);
            try
            {
                return await action(client, url, cts.Token);
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
        }
    }
}
