
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
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
    }
    
}
