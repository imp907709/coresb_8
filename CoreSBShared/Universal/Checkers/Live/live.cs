using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using CoreSBShared.Universal.Checkers.Threading;
using InfrastructureCheckers;
using InfrastructureCheckers.Collections;
using LINQtoObjectsCheck;
using LiveCodingPrep;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.VisualBasic;
using MongoDB.Driver.Core.Misc;
using MongoDB.Driver.Linq;
using Nest;
using Id = Elastic.Clients.Elasticsearch.Id;

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
            Console.WriteLine($"Init arr: {string.Join(',', init)}");
            Console.WriteLine($"Result arr: {string.Join(',', res)}");
        }

        public static void GO()
        {
            var arr = new List<int>()
            {
                1,
                2,
                3,
                4,
                5
            };
            var res = arr.CustomWhere(s => s % 2 != 0);
            PrintArrRes(arr, res);
        }
    }

    public class LiveCheck
    {
        public static void GO()
        {
            var employees = new List<EmployeeCheck.Employee> {
                new EmployeeCheck.Employee { Id = 1, Name = "Alice", Department = "HR", Salary = 55000 },
                new EmployeeCheck.Employee { Id = 2, Name = "Bob", Department = "IT", Salary = 75000 },
                new EmployeeCheck.Employee { Id = 3, Name = "Charlie", Department = "IT", Salary = 80000 },
                new EmployeeCheck.Employee { Id = 4, Name = "Diana", Department = "Finance", Salary = 65000 },
                new EmployeeCheck.Employee { Id = 5, Name = "Evan", Department = "Finance", Salary = 70000 },
                new EmployeeCheck.Employee { Id = 6, Name = "Fiona", Department = "HR", Salary = 60000 },
                new EmployeeCheck.Employee { Id = 7, Name = "George", Department = "IT", Salary = 72000 },
                new EmployeeCheck.Employee { Id = 8, Name = "Hannah", Department = "Marketing", Salary = 50000 },
                new EmployeeCheck.Employee { Id = 9, Name = "Ian", Department = "Marketing", Salary = 52000 },
                new EmployeeCheck.Employee { Id = 10, Name = "Jane", Department = "Finance", Salary = 68000 }
            };
            
            // ================== Sample Data ==================
            var users = new List<UserLive>
            {
                new() {Id = 1, Name = "Alice", State = "NY", Country = "USA"},
                new() {Id = 2, Name = "Bob", State = "CA", Country = "USA"},
                new() {Id = 3, Name = "Charlie", State = "NY", Country = "USA"},
                new() {Id = 4, Name = "Diana", State = "TX", Country = "USA"},
                new() {Id = 5, Name = "Eve", State = "CA", Country = "USA"}
            };

            var products = new List<ProductLive>
            {
                new() {Id = 1, Title = "Laptop", Price = 1200},
                new() {Id = 2, Title = "Phone", Price = 800},
                new() {Id = 3, Title = "Book", Price = 20},
                new() {Id = 4, Title = "Tablet", Price = 500},
                new() {Id = 5, Title = "Headphones", Price = 150}
            };

        /*
    parallelize http call with cancellation and throttling
    IEnumerator an IEnumerable
    IEquatable
    extension where
    select n top most salary by department
    select average salary by department
    inner join
    left join
    top most frequent chars
        */

        }
    }

   
}
