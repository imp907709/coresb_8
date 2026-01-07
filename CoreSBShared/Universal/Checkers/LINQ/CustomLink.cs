using System;
using System.Collections.Generic;
using InfrastructureCheckers.Collections;

namespace CoreSBShared.Universal.Checkers.LINQ
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
                1, 2, 3, 4, 5
            };
            var res = arr.CustomWhere(s => s % 2 != 0);
            PrintArrRes(arr, res);
        }
    }
}
