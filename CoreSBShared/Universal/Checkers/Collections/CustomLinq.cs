using System;
using System.Collections.Generic;

namespace InfrastructureCheckers.Collections
{
    public static class CustomLinq
    {
        public static IEnumerable<T> CustomWhere<T>(this IEnumerable<T> items, Func<T, bool> predicate)
        {
            foreach (var i in items)
                if (predicate(i))
                    yield return i;
        }
    }
}
