
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
using LiveCodingPrep;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.VisualBasic;
using MongoDB.Driver.Linq;
using Nest;

namespace Live
{
    public static class CustomLinqCheck
    {
        private class TestItem
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }

        public static void PrintArrRes<T>(IEnumerable<T> init, IEnumerable<T> res)
        {
            Console.WriteLine($"Init arr: {string.Join(',',init)}");
            Console.WriteLine($"Result arr: {string.Join(',',res)}");
        }
        public static void GO()
        {
            var arr = new List<int>(){1,2,3,4,5};
            var res = arr.CustomWhere(s => s % 2 != 0);
            PrintArrRes(arr, res);
            
        }
    }

    public class Live
    {
        
        public static void GO()
        {
            // ================== Sample Data ==================
            var users = new List<UserLive>
            {
                new() { Id = 1, Name = "Alice", State = "NY", Country = "USA" },
                new() { Id = 2, Name = "Bob", State = "CA", Country = "USA" },
                new() { Id = 3, Name = "Charlie", State = "NY", Country = "USA" },
                new() { Id = 4, Name = "Diana", State = "TX", Country = "USA" },
                new() { Id = 5, Name = "Eve", State = "CA", Country = "USA" }
            };

            var products = new List<ProductLive>
            {
                new() { Id = 1, Title = "Laptop", Price = 1200 },
                new() { Id = 2, Title = "Phone", Price = 800 },
                new() { Id = 3, Title = "Book", Price = 20 },
                new() { Id = 4, Title = "Tablet", Price = 500 },
                new() { Id = 5, Title = "Headphones", Price = 150 }
            };
            
            

        }

    }

}
